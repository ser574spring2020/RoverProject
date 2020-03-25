using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
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
    // string path = @"/home/lisa/new.csv";
    int[,] maze, exploredMaze;
    System.Random rand = new System.Random();
    bool mazeCreated = false;
    int currentX = 1, currentY = 1;
    static MazeGenerator mazeGenerator = new MazeGenerator();
    static Exploration exploration;
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
        string[] nextCommands = exploration.GetNextCommand(getSensorsData());
        updateExplored();
        // int tempCounter =0;
        // while (nextCommands[tempCounter]!=null)
        // {
        //     moveInDirection(nextCommands[tempCounter]);
        //     tempCounter++;
        // }
    }


    //move the robot by 'x' steps west and 'y' steps north

    //update the maze in the UI
    void updateMaze()
    {
        //Destroy UI
        for (int i = 0; i < counter; i++)
            Destroy(mazeObjects[i]);
        counter = 0;

        //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(xStart + (xSpace * i), 0, yStart - (j * ySpace));
                if (maze[i, j] == 0)
                    mazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                else if (maze[i, j] == 1)
                    mazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (maze[i, j] == 2)
                    mazeObjects[counter++] = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                else if (maze[i, j] == 3)
                    mazeObjects[counter++] = Instantiate(endPointPrefab, tempVector, Quaternion.identity);
                else if (maze[i, j] == 4)
                    mazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
    }

    void updateExplored()
    {
        exploredMaze = exploration.GetExploredMaze();
        for (int i = 0; i < counter; i++)
            Destroy(exploredMazeObjects[i]);
        counter = 0;

        //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(xStart + (xSpace * i), 100, yStart - (j * ySpace));
                if (exploredMaze[i, j] == 0)
                    exploredMazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze[i, j] == 1)
                    exploredMazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze[i, j] == 2)
                    exploredMazeObjects[counter++] = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze[i, j] == 3)
                    exploredMazeObjects[counter++] = Instantiate(endPointPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze[i, j] == 4)
                    exploredMazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
    }

    // Update the sensors data text on the screen
    void updateSensorsData()
    {
        int[,] tempData = getSensorsData();
        sensorData.text = "";
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                sensorData.text += tempData[i, j] + " ";
        sensorData.text += "\n";
    }

    int[,] getSensorsData()
    {
        int[,] result = new int[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (maze[currentX - 1 + j, currentY - 1 + i] == 1)
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
        if (direction == "North") move(0, -1);
        if (direction == "East") move(1, 0);
        if (direction == "West") move(-1, 0);
        if (direction == "South") move(0, 1);
    }

    void move(int x, int y)
    {
        maze[currentX, currentY] = 4;
        if (maze[currentX + x, currentY + y] == 1) return;
        currentX += x;
        currentY += y;
        maze[currentX, currentY] = 2;
        exploration.setPosition(x, y);
        updateMaze();
    }


    //Move camera to explore maze
    void moveToExploredMaze()
    {
        updateExplored();
        Vector3 position = camera.transform.position;
        position.y += 100;
        camera.transform.position = position;
    }

    //move camera to default maze
    void moveToOriginalMaze()
    {
        Vector3 position = camera.transform.position;
        position.y -= 100;
        camera.transform.position = position;
    }
}
