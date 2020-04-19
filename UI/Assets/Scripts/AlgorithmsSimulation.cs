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
    public float placementThreshold;
    public Text sensorData;//, sensorTeamData;
    public GameObject wallPrefab, robotPrefab;//;, floorPrefab, visitedFloorPrefab;
    public Button createMaze, sensorDataButton;
    public int mazeHeight, mazeWidth;
    GameObject[] mazeObjects, exploredMazeObjects;
    int counter = 0;
    int currentX=1,currentY=1;
    // string path = @"/home/lisa/new.csv";
    int[,] maze;
    ExploredMap exploredMaze;
    System.Random rand = new System.Random();
    bool mazeCreated = false;
    MazeGenerator mazeGenerator = new MazeGenerator();
    Exploration exploration;
    // Sensors1.Sensors sensor;
    private static SensorsComponent.Sensors sensor;

    void Start()
    {
        sensor = SensorsComponent.SensorFactory.GetInstance(1, robotPrefab);
        mazeObjects = new GameObject[mazeHeight * mazeWidth];
        exploredMazeObjects = new GameObject[mazeHeight * mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
    }


    //Create the initial maze
    void createMazeButtonListener()
    {
        exploration = new Exploration(mazeHeight, mazeWidth);
        if (mazeCreated == false)
        {
            maze = mazeGenerator.GenerateMaze(mazeHeight, mazeWidth, placementThreshold);
            maze[currentX, currentY] = 2;
            updateMaze();
            mazeCreated = true;
            InvokeRepeating("getNextCommand", 0.5f, 0.5f);
        }
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
    }

    // proximity, bumper
    //range, radar - 5x5
    //lidar 3x5
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
        Vector3 robotPosition = new Vector3(yStart - (ySpace * vector.x)-100, 0, xStart + (xSpace * vector.y));
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
