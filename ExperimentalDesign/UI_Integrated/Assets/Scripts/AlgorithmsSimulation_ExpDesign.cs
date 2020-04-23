using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using Algorithms;
using System.Collections.Generic;


public class AlgorithmsSimulation_ExpDesign : MonoBehaviour
{
    GameObject robot, exploringRobot;
    float xStart = 0, yStart = 1;
    float xSpace = 3.5f, ySpace = 3.5f;
    private float placementThreshold;
    public Text sensorData;//, sensorTeamData;
    public GameObject wallPrefab, robotPrefab, floorPrefab, visitedFloorPrefab;
    public Button createMaze, backButton, manualButton, automaticButton;
    private int mazeHeight, mazeWidth;
    GameObject[]  exploredMazeObjects;
    List<GameObject[]> arrayOfGameObjects = new List<GameObject[]>();
    int arrayCounter = 0;
    int counter = 0;
    int expCounter = 0;
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
    private String startTime, endTime, runTime;
    private int mazeCoverage, batteryLife = 3600, pointsScored;
    private String algorithmSelected, mazeSize, sensorSelected, pointsScoredStr, mazeCoverageStr;
    private float mazeOffset = 140;
    private static database expDB;
    // private DataBaseManager dbm;
    private bool isSimulationComplete = false;
    public Slider healthBar;
    public Text statusText;

    void Start()
    {

        expDB = new database();
        /*sensor = SensorsComponent.SensorFactory.GetInstance(1, robotPrefab);
        createMaze.onClick.AddListener(createMazeButtonListener);
        expDB = new database();
        manualButton.onClick.AddListener(manualButtonListener);
        automaticButton.onClick.AddListener(automaticButtonListener);
        backButton.interactable = false;*/

        // dbm = new DataBaseManager();
        // dbm.ConnectToDB("Rover.db");
    }

    public void resetValues()
    {
        expCounter = 0;
    }
    public void Begin()
    {
        int expc = expCounter + 1;
        statusText.text = "Experiment " + expc  + " is Running.";
        sensor = SensorsComponent.SensorFactory.GetInstance(1, robotPrefab);
        createMazeButtonListener();
        
        // manualButton.onClick.AddListener(manualButtonListener);
        automaticButtonListener();
        backButton.interactable = false;
        /*for (int i = 0; i < 3; i++)
        {
            Debug.Log(i);
            
        }*/
        expCounter++;
    }
    //Create the initial maze
    public void createMazeButtonListener()
    {

        
       /* if (exploredMazeObjects != null)
        {
            for (int i = 0; i < exploredMazeObjects.Length; i++)
                Destroy(exploredMazeObjects[i]);
        }*/


       
        UpdateParameters();
        arrayOfGameObjects.Add(new GameObject[mazeHeight * mazeWidth]);
        

        //Debug.Log(arrayOfGameObjects[i]);
       
        //mazeObjects = new GameObject[mazeHeight * mazeWidth];
        //exploredMazeObjects = new GameObject[mazeHeight * mazeWidth];
        exploration = new Exploration(mazeHeight, mazeWidth);
        if (mazeCreated == false)
        {
            if(arrayCounter == 0)
                maze = mazeGenerator.GenerateMaze(mazeHeight, mazeWidth, placementThreshold);
            maze[currentX, currentY] = 2;
           // updateMaze();
            mazeCreated = true;
        }
        //arrayOfGameObjects.Add(mazeObjects);
    }

    void manualButtonListener(){
        getNextCommand();
    }

    void automaticButtonListener(){
        startTime = DateTime.Now.ToString(@"hh\:mm\:ss");
        InvokeRepeating("getNextCommand", 0.1f, 0.1f);
    }

    private bool checkRunTimeStatus(){
        return !(mazeCoverage >= PlayerPrefs.GetInt("MazeCoverage") || batteryLife <= 0);
    }

    private String calculateRunTime(){
        TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
        return duration.ToString(@"hh\:mm\:ss");
    }

    private void UpdateParameters()
    {
        //Debug.Log("ALGORITHM SELECTED : " + GameObject.Find("AlgoButton").GetComponentInChildren<Text>().text);
        
        placementThreshold = float.Parse(GameObject.Find("MazeButton").GetComponentInChildren<Text>().text);
        String[] size = GameObject.Find("SizeButton").GetComponentInChildren<Text>().text.ToString().Split('X');
        mazeWidth = Int32.Parse(size[0].Trim());
        mazeHeight = Int32.Parse(size[1].Trim());
        //Debug.Log("SENSOR SELECTED : " + GameObject.Find("SensorButton").GetComponentInChildren<Text>().text);
        expDB.Insert(PlayerPrefs.GetString("Algo"), PlayerPrefs.GetString("Size"), Math.Round(float.Parse(PlayerPrefs.GetString("Maze")), 2), PlayerPrefs.GetString("Sensor"), PlayerPrefs.GetString("Experiment"));
    }

    void getNextCommand()
    {
        healthBar.value = batteryLife;
        mazeCoverageStr = exploration.GetCoverage().ToString();
        pointsScoredStr = exploration.GetPoints().ToString();
        mazeCoverage = Int32.Parse(mazeCoverageStr);
        pointsScored = Int32.Parse(pointsScoredStr);
        if(checkRunTimeStatus()){
            updateSensorsData(getSensorsData());
            // updateSensorsTeamData();
            int[,] sensorMatrix = sensor.Get_Obstacle_Matrix();
            int[,] matrix = getSensorsData();
            updateSensorMaze(sensorMatrix, matrix);
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;


            // update SensorType here
            //dbm.SetSensorMatrixById(unixTimestamp, 1, sensorMatrix);


            String robotCommand = exploration.GetNextCommand(sensorMatrix);
            moveInDirection(robotCommand);
        }
        else{
            CancelInvoke("getNextCommand");
            isSimulationComplete = true;
            endTime = DateTime.Now.ToString(@"hh\:mm\:ss");
            runTime = calculateRunTime();
            // Debug.Log("RUN TIME : " + calculateRunTime());
            //enable back button
            backButton.interactable = true;
            // calculate end time
            Debug.Log("END TIME : " + runTime);
            // store the info in DB
            Debug.Log("Battery Life : " + batteryLife);
            Debug.Log("Points : " + pointsScored);
            sendDateToExpDesign();
            Debug.Log("Maze Coverage : " + mazeCoverage);
            mazeCreated = false;
            currentX = 1;
            currentY = 1;
            mazeCoverage = 0;
            batteryLife = 3600;
            pointsScored = 0;
            
            GameObject[] mazeObjects = arrayOfGameObjects[arrayCounter];
            //Destroy UI
            for (int i = 0; i < mazeObjects.Length; i++)
                Destroy(mazeObjects[i]);
            arrayCounter++;
            if (expCounter < PlayerPrefs.GetInt("Iteration"))
                Begin();
           /* if (expCounter == PlayerPrefs.GetInt("Iteration"))
                expCounter = 0;*/
        }
    }

    private float getTimeInSeconds(String time){
        String[] timeSplit = time.Split(':');
        int timeTaken = Int32.Parse(timeSplit[0].Trim())*3600
            + Int32.Parse(timeSplit[1].Trim())*60
            + Int32.Parse(timeSplit[2].Trim());
        return (float) timeTaken;        
    }

    private void sendDateToExpDesign(){
        expDB.UpdatePointsScored((float) pointsScored);
        expDB.UpdateMazeCoverage((float) mazeCoverage);
        expDB.UpdateDroneLife((float) batteryLife);
        // getTimeInSeconds(runTime);
        expDB.UpdateTimeTaken(getTimeInSeconds(runTime));
    }

    // proximity, bumper - 3x3
    // range, radar - 5x5
    // lidar 3x5
    void updateSensorMaze(int[,] sensorMatrix, int[,] matrix){
        
        // int rows = sensorMatrix.GetLength(0);
        // int cols = sensorMatrix.GetLength(1);
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
        GameObject[] mazeObjects = arrayOfGameObjects[arrayCounter];
        //Destroy UI
        for (int i = 0; i < mazeObjects.Length; i++)
            Destroy(mazeObjects[i]);
        counter = 0;
        //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(xStart + (xSpace * j), 0, yStart - (ySpace * i));
                if (maze[i, j] == 0)
                    mazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                if (maze[i, j] == 1)
                    mazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (maze[i, j] == 2){
                    mazeObjects[counter] = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                    robot = mazeObjects[counter++];
                }
                else if (maze[i, j] == 4)
                    mazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
    }

    void updateExplored()
    {
        if(mazeWidth == 50){
            mazeOffset = 190;
        }
        exploredMaze = exploration.GetExploredMap();
        for (int i = 0; i < exploredMazeObjects.Length; i++)
            Destroy(exploredMazeObjects[i]);
        counter = 0;

        // // //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(xStart + (xSpace * j)+mazeOffset, 0, yStart - (ySpace * i));
                MazeCell mazeCell = exploredMaze.GetCell(new Vector2Int(i,j));
                if(mazeCell == null)
                    continue;
                if (mazeCell.IsWallCell() == true)
                    exploredMazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (mazeCell.IsVisited() == false)
                    exploredMazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze.GetCell(new Vector2Int(i,j)).IsVisited())
                    exploredMazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
        Vector2Int vector = exploredMaze.GetCurrentPosition();
        Vector3 robotPosition = new Vector3(xStart + (xSpace * vector.y)+mazeOffset,0, yStart - (ySpace * vector.x));
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
        if (Input.GetKeyDown(KeyCode.W)) moveInDirection("North");          //North - W Key
        if (Input.GetKeyDown(KeyCode.D)) moveInDirection("East");           //East  - D Key
        if (Input.GetKeyDown(KeyCode.A)) moveInDirection("West");           //West  - A Key
        if (Input.GetKeyDown(KeyCode.S)) moveInDirection("South");          //South - S Key
    }

    void moveInDirection(string direction)
    {
        batteryLife-=10;
        if (direction == "North")
        {
            move(-1, 0);
           //robot.transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
            //exploringRobot.transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
        }
        else if (direction == "East")
        {
            move(0, 1);
          // robot.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
            //exploringRobot.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
        else if (direction == "West")
        {
            move(0, -1);
          // robot.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
           // exploringRobot.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
        }
        else if (direction == "South"){
            move(1, 0);
          //robot.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
          //  exploringRobot.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
    }

    void move(int x, int y)
    {
        if (maze[currentX + x, currentY + y] == 1) return;
        maze[currentX, currentY] = 4;
        currentX += x;
        currentY += y;
        maze[currentX, currentY] = 2;
       // updateMaze();
       // updateExplored();
    }

    public bool getIsSimulationComplete(){
        return isSimulationComplete;
    }
}
