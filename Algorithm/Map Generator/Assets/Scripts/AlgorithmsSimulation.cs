using UnityEngine;
using System.Text;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using Algorithms;
using NeuralNet;

public class AlgorithmsSimulation : MonoBehaviour
{
    float xStart = 0, yStart = 0;
    float xSpace = 0.5f, ySpace = 0.5f;
    public float placementThreshold;
    public Text sensorData, points, coverage;
    public GameObject wallPrefab, endPointPrefab, robotPrefab, floorPrefab, flagPrefab, visitedFloorPrefab;

    GameObject robot, exploringRobot;
    public Button createMaze, automateButton, manualButton;
    public int mazeHeight, mazeWidth;
    GameObject[] mazeObjects, exploredMazeObjects;
    int counter = 0;
    int currentX = 1, currentY = 1;
    int[,] maze;
    ExploredMap exploredMaze;
    System.Random rand = new System.Random();
    bool mazeCreated = false;
    MazeGenerator mazeGenerator = new MazeGenerator();
    Exploration exploration;

    void Start()
    {
        mazeObjects = new GameObject[mazeHeight * mazeWidth];
        exploredMazeObjects = new GameObject[mazeHeight * mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
        automateButton.onClick.AddListener(automate);
      //  manualButton.onClick.AddListener(getNextCommand);

        manualButton.onClick.AddListener(getNextCommandFromNeuralNetwork);
        // Debug.Log(AlgorithmsSimulation.getNextCommandFromNeuralNetwork());
    }

    void automate() {
        InvokeRepeating("getNextCommand", 0.1f, 0.1f);
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
        }
    }

    void getNextCommand()
    {
        String robotCommand = exploration.GetNextCommand(getSensorsData());
        moveInDirection(robotCommand);
    }

     void getNextCommandFromNeuralNetwork()
    {
        NeuralNetwork NN= new NeuralNetwork();
        moveInDirection(NN.getNextCommands(getSensorsData()));
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
        exploredMaze = exploration.GetExploredMap();
        for (int i = 0; i < exploredMazeObjects.Length; i++)
            Destroy(exploredMazeObjects[i]);
        counter = 0;

        // // //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(xStart + (xSpace * j)+20, 0, yStart - (ySpace * i));
                MazeCell mazeCell = exploredMaze.GetCell(new Vector2Int(i,j));
                if(mazeCell == null)
                    continue;
                if (mazeCell.IsWallCell()==true)
                    exploredMazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                else if (mazeCell.IsVisited()==false)
                    exploredMazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                else if (exploredMaze.GetCell(new Vector2Int(i,j)).IsVisited())
                    exploredMazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
            }
        Vector2Int vector = exploredMaze.GetCurrentPosition();
        Vector3 robotPosition = new Vector3(xStart + (xSpace * vector.y)+20,0, yStart - (ySpace * vector.x));
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

   public int[,] getSensorsData()
    {
        int[,] result = new int[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (maze[currentX - 1 + i, currentY - 1 + j] == 1)
                    result[i, j] = 1;
                else
                    result[i, j] = 0;
        result[1,1]=2;
        return result;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)){
            
            moveInDirection("North");          //North - W Key
        }
        if (Input.GetKeyDown(KeyCode.D)){
             moveInDirection("East");          //East  - D Key
        }
        if (Input.GetKeyDown(KeyCode.A)){
            moveInDirection("West");           //West  - A Key
        }
        if (Input.GetKeyDown(KeyCode.S)){
            moveInDirection("South");          //South - S Key
        } 
    }

    void moveInDirection(string direction)
    {
        exploration.WriteSensorDataToCsv(getSensorsData(), direction);
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
        updateSensorsData();
        points.text = "Points: " + exploration.GetPoints().ToString();
        coverage.text = "Coverage: " + exploration.GetCoverage().ToString();
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
