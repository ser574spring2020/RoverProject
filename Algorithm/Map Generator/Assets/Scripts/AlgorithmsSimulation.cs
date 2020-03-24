using UnityEngine;
using UnityEngine.UI;

public class AlgorithmsSimulation : MonoBehaviour
{
    float xStart = 0, yStart = 0;
    float xSpace = 0.5f, ySpace = 0.5f;
    public float placementThreshold;
    public GameObject wallPrefab, endPointPrefab, startPointPrefab, floorPrefab, flagPrefab, visitedFloorPrefab;
    public Button createMaze, moveNorthButton, moveEastButton, moveWestButton, moveSouthButton;
    public int mazeHeight, mazeWidth;
    GameObject[] mazeObjects;
    int counter = 0;
    int[,] maze;
    System.Random rand = new System.Random();
    bool mazeCreated = false;
    int currentX = 1, currentY = 1;
    void Start()
    {
        mazeObjects = new GameObject[mazeHeight * mazeWidth];
        createMaze.onClick.AddListener(createMazeButtonListener);
        moveNorthButton.onClick.AddListener(() => move(0, 1));
        moveEastButton.onClick.AddListener(() => move(-1, 0));
        moveWestButton.onClick.AddListener(() => move(1, 0));
        moveSouthButton.onClick.AddListener(() => move(0, -1));

    }

    void createMazeButtonListener()
    {
        if (mazeCreated == false)
        {
            maze = Algorithms.MazeGenerator.GenerateMaze(mazeHeight, mazeWidth, placementThreshold);
            maze[currentX, currentY] = 2;
            updateUI();
            mazeCreated = true;
        }
    }

    void move(int x, int y)
    {
        maze[currentX, currentY] = 4;
        currentX += x;
        currentY += y;
        maze[currentX, currentY] = 2;
        updateUI();
    }


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
                    mazeObjects[counter++] = Instantiate(startPointPrefab, tempVector, Quaternion.identity);
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) move(0, 1);        //North - W Key
        if (Input.GetKeyDown(KeyCode.D)) move(1, 0);        //East  - D Key
        if (Input.GetKeyDown(KeyCode.A)) move(-1, 0);       //West  - A Key
        if (Input.GetKeyDown(KeyCode.S)) move(0, -1);       //South - S Key
    }
}
