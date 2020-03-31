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
    float xStart = 14, yStart = 0;
    float xStart1 = -2, yStart1 = 0;
    float xSpace = 0.5f, ySpace = 0.5f;
    float xSpace1 = 0.5f, ySpace1 = 0.5f;
    public float placementThreshold;
    public Text sensorData;
    public GameObject wallPrefab, endPointPrefab, robotPrefab, floorPrefab, flagPrefab, visitedFloorPrefab;
    public Button createMaze, sensorDataButton, nextCommandButton;
    public int mazeHeight, mazeWidth;
    GameObject[] mazeObjects;
    int counter = 0;
    // string path = @"/home/lisa/new.csv";
    int[,] mazeUser, mazeAlgo;
    System.Random rand = new System.Random();
    bool mazeCreated = false;
    int currentX = 1, currentY = 1, currentX1 = 1, currentY1 = 1;
    static MazeGenerator mazeGenerator = new MazeGenerator();
    static Exploration exploration;
    static Sensors1.Sensors sensor;

    void Start()
    {
        mazeObjects = new GameObject[mazeHeight * mazeWidth];
        //mazeObjects1 = new GameObject[mazeHeight * mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
        sensorDataButton.onClick.AddListener(updateSensorsData);
        nextCommandButton.onClick.AddListener(getNextCommand);
        exploration = new Exploration(mazeHeight,mazeWidth);
        sensor = new Sensors1.Sensors();
        String sensorChosen = sensor.chooseSensor(1);
        Debug.Log(sensorChosen);
        InvokeRepeating("getNextCommand", 1f, 2f);
    }

    //Create the initial maze
    void createMazeButtonListener()
    {
        if (mazeCreated == false)
        {
            mazeAlgo = mazeGenerator.GenerateMaze(mazeHeight, mazeWidth, placementThreshold);
            mazeUser = new int[mazeHeight, mazeWidth];
            for(int i = 0; i < mazeHeight; i++){
                for(int j = 0; j < mazeWidth; j++){
                    mazeUser[i, j] = mazeAlgo[i, j];
                }
            }

            mazeAlgo[currentX, currentY] = 2;
            mazeUser[currentX1, currentY1] = 2;
            updateUI();
            mazeCreated = true;
            InvokeRepeating("getNextCommand", 1f, 2f);
        }
    }

    void getNextCommand(){
        List<String> robotCommand = exploration.GetNextCommand(getSensorsData());
        foreach (String x in robotCommand)
        {
            moveInDirection(x);
        }
    }

    void moveInDirection(string direction){
        if(direction=="North"){
            move(0,1);
        }
        if(direction=="East"){
            move(1,0);
        }
        if(direction=="West"){
            move(-1,0);
        }
        if(direction=="South"){
            move(0,-1);
        }
    }

    //move the robot by 'x' steps west and 'y' steps north
    void move(int x, int y)
    {
        mazeAlgo[currentX, currentY] = 4;
        if (mazeAlgo[currentX + x, currentY + y] == 1) return;
        currentX += x;
        currentY += y;
        mazeAlgo[currentX, currentY] = 2;
        updateUI();
    }

    void moveUser(int x, int y)
    {
        mazeUser[currentX1, currentY1] = 4;
        if (mazeUser[currentX1 + x, currentY1 + y] == 1) return;
        currentX1 += x;
        currentY1 += y;
        mazeUser[currentX1, currentY1] = 2;
        updateUI();
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
                Vector3 tempVector = new Vector3(xStart + (xSpace * j), 0, yStart - (i * ySpace));
                //Vector3 tempVector1 = new Vector3(xStart1 + (xSpace * j), 0, yStart1 - (i * ySpace));
                if (mazeAlgo[i, j] == 0)
                {
                    mazeObjects[counter] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                    //mazeObjects[counter] = Instantiate(floorPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
                else if (mazeAlgo[i, j] == 1)
                {
                    mazeObjects[counter] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                    //mazeObjects[counter] = Instantiate(wallPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
                else if (mazeAlgo[i, j] == 2)
                {
                    mazeObjects[counter] = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                    //mazeObjects[counter] = Instantiate(robotPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
                else if (mazeAlgo[i, j] == 3)
                {
                    mazeObjects[counter] = Instantiate(endPointPrefab, tempVector, Quaternion.identity);
                    //mazeObjects[counter] = Instantiate(endPointPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
                else if (mazeAlgo[i, j] == 4)
                {
                    mazeObjects[counter] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
                    //mazeObjects[counter] = Instantiate(visitedFloorPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
            }
        }
        counter = 0;
        for (int i = 0; i < mazeHeight; i++)
        {
            for (int j = 0; j < mazeWidth; j++)
            {
                //Vector3 tempVector = new Vector3(xStart1 + (xSpace * j), 0, yStart1 - (i * ySpace));
                Vector3 tempVector1 = new Vector3(xStart1 + (xSpace * j), 0, yStart1 - (i * ySpace));
                if (mazeUser[i, j] == 0)
                {
                    //mazeObjects[counter] = Instantiate(floorPrefab, tempVector, Quaternion.identity);
                    mazeObjects[counter] = Instantiate(floorPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
                else if (mazeUser[i, j] == 1)
                {
                    //mazeObjects[counter] = Instantiate(wallPrefab, tempVector, Quaternion.identity);
                    mazeObjects[counter] = Instantiate(wallPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
                else if (mazeUser[i, j] == 2)
                {
                    //mazeObjects[counter] = Instantiate(robotPrefab, tempVector, Quaternion.identity);
                    mazeObjects[counter] = Instantiate(robotPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
                else if (mazeUser[i, j] == 3)
                {
                    //mazeObjects[counter] = Instantiate(endPointPrefab, tempVector, Quaternion.identity);
                    mazeObjects[counter] = Instantiate(endPointPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
                else if (mazeUser[i, j] == 4)
                {
                    //mazeObjects[counter] = Instantiate(visitedFloorPrefab, tempVector, Quaternion.identity);
                    mazeObjects[counter] = Instantiate(visitedFloorPrefab, tempVector1, Quaternion.identity);
                    counter++;
                }
            }
        }
    }

    // Update the sensors data text on the screen
    void updateSensorsData()
    {
        int[,] tempData = getSensorsData();
        sensorData.text = "";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                sensorData.text += tempData[i, j] + " ";
            }
            sensorData.text += "\n";
        }
    }

    int[,] getSensorsData()
    {
        return sensor.getSensorData(mazeAlgo, currentX, currentY);
        /*
        int[,] result = new int[3, 3];

        //fetching the array as a 1D array of length 9
        int[] tempArray = new int[9];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                tempArray[i * 3 + j] = maze[currentX - 1 + i, currentY - 1 + j];

        //adjusting the fetched data (rotating the array anti-clockwise if you think of that as a 3x3 array)
        int[] tempArrayClone = new int[9];
        int a = 0;
        for (int j = 2; j >= 0; j--)
            for (int i = 0; i < 3; i++)
                tempArrayClone[a++] = tempArray[j + 3 * i];

        //creating a 3x3 2D array from the 1D array
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // result[i, j] = tempArrayClone[i * 3 + j];
                if (tempArrayClone[i * 3 + j] == 1)
                    result[i, j] = 1;
                else
                    result[i, j] = 0;
            }
        }
        return result;
        */
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) moveUser(0, 1);        //North - W Key
        if (Input.GetKeyDown(KeyCode.S)) moveUser(1, 0);        //East  - D Key
        if (Input.GetKeyDown(KeyCode.W)) moveUser(-1, 0);       //West  - A Key
        if (Input.GetKeyDown(KeyCode.A)) moveUser(0, -1);       //South - S Key
    }
}
