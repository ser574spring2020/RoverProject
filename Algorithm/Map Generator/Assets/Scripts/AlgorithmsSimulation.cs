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
    public Button createMaze, sensorDataButton, nextCommandButton;
    public int mazeHeight, mazeWidth;
    GameObject[] mazeObjects;
    int counter = 0;
    // string path = @"/home/lisa/new.csv";
    int[,] maze;
    Vector2Int currentPosition;
    System.Random rand = new System.Random();
    bool mazeCreated = false;

    static MazeGenerator mazeGenerator = new MazeGenerator();
    //static Exploration exploration;
    static MazeExplorationMap explorationMap;

    void Start()
    {
        mazeObjects = new GameObject[mazeHeight * mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
        sensorDataButton.onClick.AddListener(updateSensorsData);
        nextCommandButton.onClick.AddListener(getNextCommand);
        //exploration = new Exploration(mazeHeight,mazeWidth);
        explorationMap = new MazeExplorationMap(new Vector2Int(mazeWidth, mazeHeight), new Vector2Int(1, 1));
        currentPosition = explorationMap.GetCurrentPosition();
    }

    //Create the initial maze
    void createMazeButtonListener()
    {
        if (mazeCreated == false)
        {
            maze = mazeGenerator.GenerateMaze(mazeHeight, mazeWidth, placementThreshold);
            maze[currentPosition.x, currentPosition.y] = 2;
            updateUI();
            mazeCreated = true;
        }
    }

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
        }
    }

    //update the maze in the UI
    void updateUI()
    {
        //Destroy UI
        for (int i = 0; i < counter; i++)
        {
            Destroy(mazeObjects[i]);
        }
        counter = 0;
        //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
        {
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(xStart + (xSpace * i), 0, yStart + (j * ySpace));
                if (maze[i, j] == 0)
                {
                    mazeObjects[counter++] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                }
                else if (maze[i, j] == 1)
                {
                    mazeObjects[counter++] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                }
                else if (maze[i, j] == 2)
                {
                    mazeObjects[counter++] = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                }
                else if (maze[i, j] == 3)
                {
                    mazeObjects[counter++] = Instantiate(endPointPrefab, tempVector, Quaternion.identity);
                }
                else if (maze[i, j] == 4)
                {
                    mazeObjects[counter++] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
                }
            }
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
            for (int x = 0; x < 3; x++)
            {
                sensorData.text += (tempData[x, y] ? "T" : "F") + " ";
            }
            sensorData.text += "\n";
        }
    }

    bool[,] getSensorsData()
    {
        bool[,] result = new bool[3, 3];
        Vector2Int current = explorationMap.GetCurrentPosition();

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                result[x, y] = maze[current.x + x - 1, current.y + y - 1] != 1;
            }
        }

        return result;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) move(Vector2Int.up);        //North - W Key
        if (Input.GetKeyDown(KeyCode.D)) move(Vector2Int.left);        //East  - D Key
        if (Input.GetKeyDown(KeyCode.A)) move(Vector2Int.right);       //West  - A Key
        if (Input.GetKeyDown(KeyCode.S)) move(Vector2Int.down);       //South - S Key
    }


    /*void saveAsCSV(int[,] arr)
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    File.AppendAllText(path, arr[j, i].ToString() + ",", Encoding.UTF8);
                }
                File.AppendAllText(path, Environment.NewLine);
            }
            File.AppendAllText(path, Environment.NewLine);
        }*/
}
