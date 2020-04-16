using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using Algorithms;
using System.Collections.Generic;
using Sensors1;

public class AlgorithmsSimulation : MonoBehaviour
{
    float xStart = -3, yStart = 0;
    float xStart1 = -3, yStart1 = -10;
    float xSpace = 0.5f, ySpace = 0.5f;
    public float placementThreshold;
    public Text sensorData;
    public GameObject wallPrefab, endPointPrefab, robotPrefab, floorPrefab, flagPrefab, visitedFloorPrefab;
    public Button createMaze, sensorDataButton, nextCommandButton;
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
    Sensors1.Sensors sensor;

    void Start()
    {
        mazeObjects = new GameObject[mazeHeight * mazeWidth];
        exploredMazeObjects = new GameObject[mazeHeight * mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
        sensorDataButton.onClick.AddListener(updateSensorsData);
        nextCommandButton.onClick.AddListener(getNextCommand);
        exploration = new Exploration(mazeHeight, mazeWidth);
        sensor = new Sensors1.Sensors();
        String sensorChosen = sensor.chooseSensor(1);
        Debug.Log(sensorChosen);
    }


    //Create the initial maze
    void createMazeButtonListener()
    {
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
        String robotCommand = exploration.GetNextCommand(getSensorsData());
        moveInDirection(robotCommand);
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
                Vector3 tempVector = new Vector3(xStart1 + ((xSpace-0.4f) * j), -0.4f, yStart1 - ((ySpace-0.4f) * i));
                if (maze[i, j] == 0){
                	GameObject tempFloor = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                    tempFloor.transform.localScale += new Vector3(-0.4f, -0.4f, -0.4f);
                    mazeObjects[counter++] = tempFloor;
                }
                else if (maze[i, j] == 1){
                	GameObject tempWall = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                    tempWall.transform.localScale += new Vector3(-0.4f, -0.4f, -0.4f);
                    mazeObjects[counter++] = tempWall;
                }
                // else if (maze[i, j] == 2){
                // 	GameObject temp = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                //     temp.transform.localScale += new Vector3(-0.4f, -0.4f, -0.4f);
                //     mazeObjects[counter++] = temp;
                // }
                else if (maze[i, j] == 4){
                	GameObject tempVisitedFloor = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
                    tempVisitedFloor.transform.localScale += new Vector3(-0.4f, -0.4f, -0.4f);
                    mazeObjects[counter++] = tempVisitedFloor;
                }
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
                Vector3 tempVector = new Vector3(xStart + (xSpace * j)+18, 0, yStart - (ySpace * i));
                MazeCell mazeCell = exploredMaze.GetCell(new Vector2Int(i,j));
                if(mazeCell == null)
                    continue;
                if (mazeCell.isWallCell()==true)
                    exploredMazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (mazeCell.IsVisited()==false)
                    exploredMazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze.GetCell(new Vector2Int(i,j)).IsVisited())
                    exploredMazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
        Vector2Int vector = exploredMaze.GetCurrentPosition();
        Vector3 robotPosition = new Vector3(xStart + (xSpace * vector.y)+18,0, yStart - (ySpace * vector.x));
        exploredMazeObjects[counter++] = Instantiate(robotPrefab, robotPosition, Quaternion.identity);
    }

    public enum CellType : int
    {
        floor,
        wall,
        robot,
        endPoint,
        visitedFloor
    }

    // Update the sensors data text on the screen
    void updateSensorsData()
    {
        int[,] tempData = getSensorsData();
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
        return sensor.getSensorData(maze, currentX, currentY);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) moveInDirection("North");          //North - W Key
        if (Input.GetKeyDown(KeyCode.D)) moveInDirection("East");           //East  - D Key
        if (Input.GetKeyDown(KeyCode.A)) moveInDirection("West");           //West  - A Key
        if (Input.GetKeyDown(KeyCode.S)) moveInDirection("South");          //South - S Key
    }

    void moveInDirection(string direction)
    {
        if (direction == "North") move(-1, 0);
        else if (direction == "East") move(0, 1);
        else if (direction == "West") move(0, -1);
        else if (direction == "South") move(1, 0);
    }

    void move(int x, int y)
    {
        if (maze[currentX + x, currentY + y] == 1) return;
        maze[currentX, currentY] = 4;
        // exploredMaze[currentX, currentY] = 4;
        currentX += x;
        currentY += y;
        maze[currentX, currentY] = 2;
        // exploredMaze[currentX, currentY] = 2;
        updateMaze();
        updateExplored();
    }
}
