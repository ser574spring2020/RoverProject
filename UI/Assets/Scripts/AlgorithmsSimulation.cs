using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using Algorithms;
using System.Collections.Generic;

public class AlgorithmsSimulation : MonoBehaviour
{
    GameObject robot, exploringRobot;
    float xStart = 0, yStart = 1;
    float xSpace = 3.5f, ySpace = 3.5f;
    private float placementThreshold;
    public Text sensorData;//, sensorTeamData;
    public GameObject wallPrefab, robotPrefab;//;, floorPrefab, visitedFloorPrefab;
    public Button createMaze, backButton;
    public int mazeHeight, mazeWidth;
    GameObject[] mazeObjects, exploredMazeObjects;
    int counter = 0;
    int currentX=1,currentY=1;
    // string path = @"/home/lisa/new.csv";
    int[,] maze, experimentalMaze;
    ExploredMap exploredMaze;
    System.Random rand = new System.Random();
    bool mazeCreated = false;
    MazeGenerator mazeGenerator = new MazeGenerator();
    Exploration exploration;
    // Sensors1.Sensors sensor;
    private static SensorsComponent.Sensors sensor;
    private String startTime, endTime;
    private int batteryLife, mazeCoverage, allowedRunTime = 3600;
    private String algorithmSelected, mazeSize, sensorSelected;

    void Start()
    {
        sensor = SensorsComponent.SensorFactory.GetInstance(1, robotPrefab);
        mazeObjects = new GameObject[mazeHeight * mazeWidth];
        exploredMazeObjects = new GameObject[mazeHeight * mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
        backButton.interactable = false;
    }

    //Create the initial maze
    void createMazeButtonListener()
    {
        startTime = DateTime.Now.ToString("mm:ss");
        UpdateParameters();
        exploration = new Exploration(mazeHeight, mazeWidth);
        if (mazeCreated == false)
        {
            maze = mazeGenerator.GenerateMaze(mazeHeight, mazeWidth, placementThreshold);
            maze[currentX, currentY] = 2;
            updateMaze();
            mazeCreated = true;
            InvokeRepeating("getNextCommand", 0.1f, 0.1f);
        }
    }

    private bool checkRunTimeStatus(){
        if(mazeCoverage >= 80 || batteryLife <= 0 || allowedRunTime <= 0){
            //enable back button
            backButton.interactable = true;
            // get explored maze from algo dll

            // calculate end time
            endTime = DateTime.Now.ToString("mm:ss");
            String runTime = calculateRunTime();
            // store the info in DB
            return false;
        }
        return true;
    }

    private String calculateRunTime(){
        TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
        return duration.ToString(@"mm\:ss");
    }

    private void UpdateParameters()
    {
        // Debug.Log("VALUE IN METHOD : " + GameObject.Find("MazeButton").GetComponentInChildren<Text>().text);
        // GameObject.Find("AlgoButton").GetComponentInChildren<Text>().text
        placementThreshold = float.Parse(GameObject.Find("MazeButton").GetComponentInChildren<Text>().text);
        // GameObject.Find("SizeButton").GetComponentInChildren<Text>().text.ToString();
        // GameObject.Find("SensorButton").GetComponentInChildren<Text>().text
    }

    void getNextCommand()
    {
        updateSensorsData(getSensorsData());
        // updateSensorsTeamData();
        int[,] sensorMatrix = sensor.Get_Obstacle_Matrix();
        int[,] matrix = getSensorsData();
        updateSensorMaze(sensorMatrix, matrix);
        String robotCommand = exploration.GetNextCommand(sensorMatrix);
        moveInDirection(robotCommand);
        // if(checkRunTimeStatus()){
        //     updateSensorsData(getSensorsData());
        //     // updateSensorsTeamData();
        //     int[,] sensorMatrix = sensor.Get_Obstacle_Matrix();
        //     int[,] matrix = getSensorsData();
        //     updateSensorMaze(sensorMatrix, matrix);
        //     String robotCommand = exploration.GetNextCommand(sensorMatrix);
        //     moveInDirection(robotCommand);
        // }
        // else{
        //     CancelInvoke("getNextCommand");
        // }
    }

    // proximity, bumper - 3x3
    // range, radar - 5x5
    // lidar 3x5
    void updateSensorMaze(int[,] sensorMatrix, int[,] matrix){
        for (int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                if(sensorMatrix[i, j] != matrix[i, j]){
                    sensorMatrix[i, j] = matrix[i, j];
                }
            }
        }
    }

    //update the maze in the UI
    void updateMaze()
    {
        //Destroy UI
        for (int i = 0; i < mazeObjects.Length; i++)
            Destroy(mazeObjects[i]);
        counter = 0;
        //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(yStart - (ySpace * i)+10, 0f, xStart + (xSpace * j));
                // if (maze[i, j] == 0){
                //     mazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                // }
                if (maze[i, j] == 1){
                    mazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                }
                else if (maze[i, j] == 2){
                    mazeObjects[counter] = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                    robot = mazeObjects[counter++];
                }
                // else if (maze[i, j] == 4){
                //     mazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
                // }
            }
    }

    void updateExplored()
    {
        exploredMaze = exploration.GetExploredMap();
        for (int i = 0; i < exploredMazeObjects.Length; i++)
            Destroy(exploredMazeObjects[i]);
        counter = 0;

        // // //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(yStart - (ySpace * i)-100, 0, xStart + (xSpace * j));
                MazeCell mazeCell = exploredMaze.GetCell(new Vector2Int(i,j));
                if(mazeCell == null)
                    continue;
                if (mazeCell.IsWallCell()==true)
                    exploredMazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                // else if (mazeCell.IsVisited()==false)
                //     exploredMazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                // else if (exploredMaze.GetCell(new Vector2Int(i,j)).IsVisited())
                //     exploredMazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
        Vector2Int vector = exploredMaze.GetCurrentPosition();
        Vector3 robotPosition = new Vector3(yStart - (ySpace * vector.x)-100, 1.2f, xStart + (xSpace * vector.y));
        exploredMazeObjects[counter] = Instantiate(robotPrefab, robotPosition, Quaternion.identity);
        exploringRobot = exploredMazeObjects[counter++];  
    }

    public enum CellType : int
    {
        floor,
        wall,
        robot,
        endPoint,
        visitedFloor
    }

    // // Update the sensors data text on the screen
    void updateSensorsData(int[,] tempData)
    {
        // int[,] tempData = getSensorsData();
        sensorData.text = "";
        for (int i = 0; i <3; i++)
        {
            for (int j = 0; j < 3; j++)
                sensorData.text += tempData[i, j] + " ";
            sensorData.text += "\n";
        }
    }

    int[,] getSensorsData()
    {
        int[,] result = new int[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (maze[currentX - 1 + i, currentY - 1 + j] == 1)
                    result[i, j] = 1;
                else
                    result[i, j] = 0;
        return result;
    }

    void Update()
    {
        // sensor.Update_Obstacles(robotPrefab);
        if (Input.GetKeyDown(KeyCode.A)) moveInDirection("North");          //North - W Key
        if (Input.GetKeyDown(KeyCode.S)) moveInDirection("East");           //East  - D Key
        if (Input.GetKeyDown(KeyCode.W)) moveInDirection("West");           //West  - A Key
        if (Input.GetKeyDown(KeyCode.D)) moveInDirection("South");          //South - S Key
    }

    void moveInDirection(string direction)
    {
        allowedRunTime--;
        if (direction == "North")
        {
            move(-1, 0);
            robot.transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
            exploringRobot.transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
        }
        else if (direction == "East") 
        {
            move(0, 1);
            robot.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
            exploringRobot.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
        else if (direction == "West")
        {
            move(0, -1);
            robot.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
            exploringRobot.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
        }
        else if (direction == "South"){
            move(1, 0);
            robot.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
            exploringRobot.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
    }

    void move(int x, int y)
    {
        if (maze[currentX + x, currentY + y] == 1) return;
        maze[currentX, currentY] = 4;
        currentX += x;
        currentY += y;
        maze[currentX, currentY] = 2;
        updateMaze();
        updateExplored();
    }
}
