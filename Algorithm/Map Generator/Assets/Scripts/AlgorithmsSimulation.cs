using UnityEngine;
using UnityEngine.UI;
using System;

using System.Collections.Generic;
using Algorithms;

public class AlgorithmsSimulation : MonoBehaviour
{
    float xStart = 0, yStart = 0;
    float xSpace = 0.5f, ySpace = 0.5f;
    public float placementThreshold;
    public Text sensorData;
    public GameObject wallPrefab, endPointPrefab, robotPrefab, floorPrefab, flagPrefab, visitedFloorPrefab;
    public GameObject camera;
    public Button createMaze, sensorDataButton, nextCommandButton, exploredMazeButton, mazeButton;
    public int mazeHeight, mazeWidth;
    GameObject[] mazeObjects, exploredMazeObjects;
    int counter = 0;
    int currentX=1,currentY=1;
    // string path = @"/home/lisa/new.csv";
    int[,] maze, exploredMaze;
    System.Random rand = new System.Random();
    bool mazeCreated = false;

    MazeGenerator mazeGenerator = new MazeGenerator();
    // MazeExplorationMap explorationMap;
    Exploration exploration;

    void Start()
    {
        mazeObjects = new GameObject[mazeHeight * mazeWidth];
        exploredMazeObjects = new GameObject[mazeHeight * mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
        sensorDataButton.onClick.AddListener(updateSensorsData);
        nextCommandButton.onClick.AddListener(getNextCommand);
        exploration = new Exploration(mazeHeight, mazeWidth);
        exploredMazeButton.onClick.AddListener(moveToExploredMaze);
        mazeButton.onClick.AddListener(moveToOriginalMaze);
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
        }
    }

    void getNextCommand()
    {
        // CALL TO SAVE SENSOR DATA
        // string[] nextCommands = exploration.GetNextCommand(getSensorsData());
        // updateExplored();

        List<String> robotCommand = exploration.GetNextCommand(getSensorsData());

        // MAYANK RAWAT
        // List<String> final_list = exploration.FindPath(tempMaze, currentX,currentY, 20, 20);
        foreach (String x in robotCommand)
        {
            moveInDirection(x);
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
                Vector3 tempVector = new Vector3(xStart + (xSpace * j), 0, yStart - (ySpace * i));
                if (maze[i, j] == 0)
                    mazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                else if (maze[i, j] == 1)
                    mazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (maze[i, j] == 2)
                    mazeObjects[counter++] = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                else if (maze[i, j] == 4)
                    mazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
    }

    void updateExplored()
    {
        exploredMaze = exploration.GetExploredMaze();
        for (int i = 0; i < exploredMazeObjects.Length; i++)
            Destroy(exploredMazeObjects[i]);
        counter = 0;

        //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(xStart + (xSpace * j), 100, yStart - (ySpace * i));
                if (exploredMaze[i, j] == 0)
                    exploredMazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze[i, j] == 1)
                    exploredMazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze[i, j] == 2)
                    exploredMazeObjects[counter++] = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze[i, j] == 4)
                    exploredMazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
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
        for (int i = 2; i >= 0; i--)
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
        if (Input.GetKeyDown(KeyCode.W)) moveInDirection("North");        //North - W Key
        if (Input.GetKeyDown(KeyCode.D)) moveInDirection("East");        //East  - D Key
        if (Input.GetKeyDown(KeyCode.A)) moveInDirection("West");       //West  - A Key
        if (Input.GetKeyDown(KeyCode.S)) moveInDirection("South");       //South - S Key

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
        currentX += x;
        currentY += y;
        maze[currentX, currentY] = 2;
        exploration.setPosition(currentX, currentY);
        updateMaze();
    }


    //Move camera to explore maze
    void moveToExploredMaze()
    {
        updateExplored();
        Vector3 position = camera.transform.position;
        position.y = 109;
        camera.transform.position = position;
    }

    //move camera to default maze
    void moveToOriginalMaze()
    {
        Vector3 position = camera.transform.position;
        position.y = 9;
        camera.transform.position = position;
    }
}