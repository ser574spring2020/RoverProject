using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using Algorithms;
using System.Collections.Generic;
using SensorsComponent;

public class AlgorithmsSimulation : MonoBehaviour
{
    GameObject robot, exploringRobot;
    float xStart= 0, yStart = 1;
    float xSpace= 3.5f, ySpace = 3.5f;
    private float placementThreshold;
    public Text sensorData;
    public GameObject wallPrefab, robotPrefab, floorPrefab, visitedFloorPrefab;
    public Button createMaze, backButton, manualButton, automaticButton;
    private int mazeHeight, mazeWidth;
    GameObject[] mazeObjects, exploredMazeObjects;
    int counter= 0;
    int currentX= 1,currentY = 1;

    int[,] maze, experimentalMaze;
    ExploredMap exploredMaze;
    System.Random rand= new System.Random();
    bool mazeCreated= false;
    MazeGenerator mazeGenerator= new MazeGenerator();
    Exploration exploration;
    private static SensorsComponent.Sensors sensor;
    private String startTime, endTime, runTime;
    private int mazeCoverage, batteryLife= 3600, pointsScored;
    private String algorithmSelected, mazeSize, sensorSelected, pointsScoredStr, mazeCoverageStr;
    private float mazeOffset= 140;
    private static database expDB;
    private bool isSimulationComplete= false;
    private GameObject robotMain;
    private enum SensorType{
        Proximity = 1,
        Range = 2,
        Lidar = 3,
        Radar= 4,
        Bumper=5
    }
    int currentSensor = (int)SensorType.Proximity;
    String robotDirection = "East";

    void Start()
    {
        sensor= SensorsComponent.SensorFactory.GetInstance(currentSensor, robotPrefab);
        createMaze.onClick.AddListener(createMazeButtonListener);
        expDB= new database();
        manualButton.onClick.AddListener(manualButtonListener);
        automaticButton.onClick.AddListener(automaticButtonListener);
        backButton.interactable= false;
    }


    void createMazeButtonListener()
    {
        UpdateParameters();
        mazeObjects= new GameObject[mazeHeight * mazeWidth];
        exploredMazeObjects= new GameObject[mazeHeight * mazeWidth];
        exploration= new Exploration(mazeHeight, mazeWidth);
        if (mazeCreated== false)
        {
            maze= mazeGenerator.GenerateMaze(mazeHeight, mazeWidth, placementThreshold);
            maze[currentX, currentY]= 2;
            updateMaze();
            mazeCreated= true;
        }
    }

    void getNextCommand()
    {
        mazeCoverageStr= exploration.GetCoverage().ToString();
        pointsScoredStr= exploration.GetPoints().ToString();
        mazeCoverage= Int32.Parse(mazeCoverageStr);
        pointsScored= Int32.Parse(pointsScoredStr);
        if(checkRunTimeStatus()){
            int matrixSize = (currentSensor==1 || currentSensor == 5)? 3:5;
            sensor.Update_Maze_Data(getMazeData(matrixSize),robotDirection);
            int[,] sensorReading = sensor.Get_Obstacle_Matrix();
            updateUISensorData(sensorReading);  
            String robotCommand = exploration.GetNextCommand(sensorReading);
            moveInDirection(robotCommand);
        }

        else{
            CancelInvoke("getNextCommand");
            isSimulationComplete= true;
            endTime= DateTime.Now.ToString(@"hh\:mm\:ss");
            runTime= calculateRunTime();
            backButton.interactable= true;
            Debug.Log("END TIME : " + runTime);
            Debug.Log("Battery Life : " + batteryLife);
            Debug.Log("Points : " + pointsScored);
            sendDateToExpDesign();
            Debug.Log("Maze Coverage : " + mazeCoverage);
        }
    }

    //get maze data around robot with a diameter ${size
    int[,] getMazeData(int size)
    {
        int[,] result= new int[size, size];
        for (int i= 0; i < size; i++)
            for (int j= 0; j < size; j++)
                result[i,j] = -1;

        int position = (size==3)?1:2;
        for (int i= 0; i < size; i++)
            for (int j= 0; j < size; j++){
                int x= currentX - position + i;
                int y= currentY - position + j;
                if(x<0 || x>mazeHeight || y<0 || y>mazeWidth)
                    continue;
                if (maze[x,y]== 1)
                    result[i, j]= 1;
                else
                    result[i, j]= 0;
            }
        result[position,position]=2;
        return result;
    }

    public SensorsComponent.Sensors GetSensorsFromAlgorithmsSimulation(){
        return sensor;
    }

    public GameObject getRoverInstanceFromAlgorithmSimulation(){
        return robotMain;
    }

    void manualButtonListener(){
        getNextCommand();
    }

    void automaticButtonListener(){
        startTime= DateTime.Now.ToString(@"hh\:mm\:ss");
        InvokeRepeating("getNextCommand", 0.1f, 0.1f);
    }

    private bool checkRunTimeStatus(){
        return !(mazeCoverage >= 80 || batteryLife <= 0);
    }

    private String calculateRunTime(){
        TimeSpan duration= DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
        return duration.ToString(@"hh\:mm\:ss");
    }

    private void UpdateParameters()
    {

        placementThreshold= float.Parse(GameObject.Find("MazeButton").GetComponentInChildren<Text>().text);
        String[] size= GameObject.Find("SizeButton").GetComponentInChildren<Text>().text.ToString().Split('X');
        mazeWidth= Int32.Parse(size[0].Trim());
        mazeHeight= Int32.Parse(size[1].Trim());
    }


    private float getTimeInSeconds(String time){
        String[] timeSplit= time.Split(':');
        int timeTaken= Int32.Parse(timeSplit[0].Trim())*3600
            + Int32.Parse(timeSplit[1].Trim())*60
            + Int32.Parse(timeSplit[2].Trim());
        return (float) timeTaken;
    }

    private void sendDateToExpDesign(){
        expDB.UpdatePointsScored((float) pointsScored);
        expDB.UpdateMazeCoverage((float) mazeCoverage);
        expDB.UpdateDroneLife((float) batteryLife);

        expDB.UpdateTimeTaken(getTimeInSeconds(runTime));
    }

    //Update original maze on UI
    void updateMaze()
    {

        for (int i= 0; i < mazeObjects.Length; i++)
            Destroy(mazeObjects[i]);
        counter= 0;

        for (int i= 0; i < mazeHeight; i++)
            for (int j= 0; j < mazeWidth; j++)
            {
                Vector3 tempVector= new Vector3(xStart + (xSpace * j), 0, yStart - (ySpace * i));
                if (maze[i, j]== 0)
                    mazeObjects[counter++]= Instantiate(floorPrefab, tempVector, Quaternion.identity);
                if (maze[i, j]== 1)
                    mazeObjects[counter++]= Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (maze[i, j]== 2){
                    mazeObjects[counter]= Instantiate(robotPrefab, tempVector, Quaternion.identity);
                    robot= mazeObjects[counter++];
                    robotMain= robot;
                }
                else if (maze[i, j]== 4)
                    mazeObjects[counter++]= Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
    }

    //update explored maze on UI
    void updateExplored()
    {
        if(mazeWidth== 50){
            mazeOffset= 190;
        }
        exploredMaze= exploration.GetExploredMap();
        for (int i= 0; i < exploredMazeObjects.Length; i++)
            Destroy(exploredMazeObjects[i]);
        counter= 0;


        for (int i= 0; i < mazeHeight; i++)
            for (int j= 0; j < mazeWidth; j++)
            {
                Vector3 tempVector= new Vector3(xStart + (xSpace * j)+mazeOffset, 0, yStart - (ySpace * i));
                MazeCell mazeCell= exploredMaze.GetCell(new Vector2Int(i,j));
                if(mazeCell== null)
                    continue;
                if (mazeCell.IsWallCell()== true)
                    exploredMazeObjects[counter++]= Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (mazeCell.IsVisited()== false)
                    exploredMazeObjects[counter++]= Instantiate(floorPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze.GetCell(new Vector2Int(i,j)).IsVisited())
                    exploredMazeObjects[counter++]= Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
        Vector2Int vector= exploredMaze.GetCurrentPosition();
        Vector3 robotPosition= new Vector3(xStart + (xSpace * vector.y)+mazeOffset,0, yStart - (ySpace * vector.x));
        exploredMazeObjects[counter]= Instantiate(robotPrefab, robotPosition, Quaternion.identity);
        exploringRobot= exploredMazeObjects[counter++];
    }

    public enum CellType : int
    {
        floor,
        wall,
        robot,
        endPoint,
        visitedFloor
    }

    //Update sensor data on UI
    void updateUISensorData(int[,] tempData)
    {
        sensorData.text= "";
        for (int i= 0; i <tempData.GetLength(0); i++){
            for (int j= 0; j < tempData.GetLength(1); j++){
                if(tempData[i, j]==-1)
                    sensorData.text += "  ";
                else
                    sensorData.text += tempData[i, j] + " ";
            }
            sensorData.text += "\n";
        }
    }

    

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W)) moveInDirection("North");
        if (Input.GetKeyDown(KeyCode.D)) moveInDirection("East");
        if (Input.GetKeyDown(KeyCode.A)) moveInDirection("West");
        if (Input.GetKeyDown(KeyCode.S)) moveInDirection("South");
    }

    void moveInDirection(string direction)
    {
        batteryLife--;
        if (direction== "North")
        {
            if (maze[currentX - 1, currentY +0 ]== 1) return;
            move(-1, 0);
            robot.transform.Rotate(0.0f, 270f, 0.0f, Space.Self);
            exploringRobot.transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
        }
        else if (direction== "East")
        {
            if (maze[currentX, currentY +1 ]== 1) return;
            move(0, 1);
            robot.transform.Rotate(0.0f, 0f, 0.0f, Space.Self);
            exploringRobot.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
        else if (direction== "West")
        {
            if (maze[currentX, currentY -1 ]== 1) return;
            move(0, -1);
            robot.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
            exploringRobot.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
        }
        else if (direction== "South"){
            if (maze[currentX + 1, currentY +0 ]== 1) return;
            move(1, 0);
            robot.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
            exploringRobot.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
        robotDirection = direction;
    }

    void move(int x, int y)
    {
        if (maze[currentX + x, currentY + y]== 1) return;
        maze[currentX, currentY]= 4;
        currentX += x;
        currentY += y;
        maze[currentX, currentY]= 2;
        updateMaze();
        updateExplored();
    }

    public bool getIsSimulationComplete(){
        return isSimulationComplete;
    }
}