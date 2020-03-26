using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
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
    // string path = @"/home/lisa/new.csv";
<<<<<<< HEAD
    int[,] maze;
    Vector2Int currentPosition;
=======
    int[,] maze, exploredMaze;
>>>>>>> Algorithms
    System.Random rand = new System.Random();
    bool mazeCreated = false;

    static MazeGenerator mazeGenerator = new MazeGenerator();
    //static Exploration exploration;
    static MazeExplorationMap explorationMap;

    void Start()
    {
        mazeObjects = new GameObject[mazeHeight * mazeWidth];
        exploredMazeObjects = new GameObject[mazeHeight * mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
        sensorDataButton.onClick.AddListener(updateSensorsData);
        nextCommandButton.onClick.AddListener(getNextCommand);
<<<<<<< HEAD
        //exploration = new Exploration(mazeHeight,mazeWidth);
        explorationMap = new MazeExplorationMap(new Vector2Int(mazeWidth, mazeHeight), new Vector2Int(1, 1));
        currentPosition = explorationMap.GetCurrentPosition();
=======
        exploration = new Exploration(mazeHeight, mazeWidth);
        exploredMazeButton.onClick.AddListener(moveToExploredMaze);
        mazeButton.onClick.AddListener(moveToOriginalMaze);
>>>>>>> Algorithms
    }


    //Create the initial maze
    void createMazeButtonListener()
    {
        if (mazeCreated == false)
        {
            maze = mazeGenerator.GenerateMaze(mazeHeight, mazeWidth, placementThreshold);
<<<<<<< HEAD
            maze[currentPosition.x, currentPosition.y] = 2;
            updateUI();
=======
            maze[currentX, currentY] = 2;
            updateMaze();
>>>>>>> Algorithms
            mazeCreated = true;
        }
    }

<<<<<<< HEAD
    void getNextCommand(){
        string nextCommand;
        bool[,] sensorData = getSensorsData();
        explorationMap.ProcessSensor(sensorData);
        if (sensorData[1, 2])
        {
            nextCommand = "South";
        }
        else if (sensorData[2, 1])
        {
            nextCommand = "East";
        }
        else if (sensorData[1, 0])
        {
            nextCommand = "North";
        }
        else
        {
            nextCommand = "West";
        }
        moveInDirection(nextCommand);
    }

    void moveInDirection(string direction){
        if(direction=="North"){
            move(Vector2Int.down);
        }
        if(direction=="East"){
            move(Vector2Int.right);
        }
        if(direction=="West"){
            move(Vector2Int.left);
        }
        if(direction=="South"){
            move(Vector2Int.up);
        }
    }

    //move the robot by 'x' steps west and 'y' steps north
    void move(Vector2Int command)
    {
        bool success = explorationMap.CheckOpening(command);
        if (success)
        {
            maze[currentPosition.x, currentPosition.y] = 4;
            explorationMap.MoveRelative(command);
            currentPosition = explorationMap.GetCurrentPosition();

            maze[currentPosition.x, currentPosition.y] = 2;
            updateUI();
        } else
        {
            print("Move failed: " + command);
            print(explorationMap);
=======
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
>>>>>>> Algorithms
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
        bool[,] tempData = getSensorsData();
        sensorData.text = "";
        for (int y = 2; y >= 0; y--)
        {
<<<<<<< HEAD
            for (int x = 0; x < 3; x++)
            {
                sensorData.text += (tempData[x, y] ? "T" : "F") + " ";
            }
=======
            for (int j = 0; j < 3; j++)
                sensorData.text += tempData[i, j] + " ";
>>>>>>> Algorithms
            sensorData.text += "\n";
        }
    }

    bool[,] getSensorsData()
    {
<<<<<<< HEAD
        bool[,] result = new bool[3, 3];
        Vector2Int current = explorationMap.GetCurrentPosition();

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                result[x, y] = maze[current.x + x - 1, current.y + y - 1] != 1;
            }
        }

=======
        int[,] result = new int[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (maze[currentX - 1 + i, currentY - 1 + j] == 1)
                    result[i, j] = 1;
                else
                    result[i, j] = 0;
>>>>>>> Algorithms
        return result;
    }

    void Update()
    {
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.W)) move(Vector2Int.up);        //North - W Key
        if (Input.GetKeyDown(KeyCode.D)) move(Vector2Int.left);        //East  - D Key
        if (Input.GetKeyDown(KeyCode.A)) move(Vector2Int.right);       //West  - A Key
        if (Input.GetKeyDown(KeyCode.S)) move(Vector2Int.down);       //South - S Key
=======
        if (Input.GetKeyDown(KeyCode.W)) moveInDirection("North");        //North - W Key
        if (Input.GetKeyDown(KeyCode.D)) moveInDirection("East");        //East  - D Key
        if (Input.GetKeyDown(KeyCode.A)) moveInDirection("West");       //West  - A Key
        if (Input.GetKeyDown(KeyCode.S)) moveInDirection("South");       //South - S Key
>>>>>>> Algorithms
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
