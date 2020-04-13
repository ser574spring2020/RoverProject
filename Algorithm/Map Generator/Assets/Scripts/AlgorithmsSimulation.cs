using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
// using Algorithms;
using Random = UnityEngine.Random;
using RandomSystem = System.Random;

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
            InvokeRepeating("getNextCommand", 1f, 0.2f);
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
        exploredMaze = exploration.GetExploredMap();
        for (int i = 0; i < exploredMazeObjects.Length; i++)
            Destroy(exploredMazeObjects[i]);
        counter = 0;

        // // //Recreate UI
        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
            {
                Vector3 tempVector = new Vector3(xStart + (xSpace * j), 100, yStart - (ySpace * i));
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
        Vector3 robotPosition = new Vector3(xStart + (xSpace * vector.y), 100, yStart - (ySpace * vector.x));
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
        currentX += x;
        currentY += y;
        maze[currentX, currentY] = 2;
        // exploration.setPosition(currentX, currentY);
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
//*************************************************************************************************************************



























































































public class Exploration : MonoBehaviour
    {
        private List<String> commands = new List<string>() {"West", "North", "East", "South"};
        private List<Vector2Int> vectorCommands = new List<Vector2Int>(){Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.right};
        ExploredMap exploredMap;

        public String GetNextCommand(int[,] sensorData)
        {
            String robotCommand;
            RandomSystem r = new RandomSystem();
            exploredMap.ProcessSensor(sensorData);
            List<String> possibleDirections = GetAvailableDirections(sensorData);
            int x = r.Next(0, possibleDirections.Count);
            robotCommand = possibleDirections[x];
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(robotCommand)]);
            return robotCommand;
        }

        public Exploration(int sizeRows, int sizeCols)
        {
            exploredMap = new ExploredMap(new Vector2Int(30,30),new Vector2Int(1,1) );
        }

        public ExploredMap GetExploredMap(){
            return exploredMap;
        }

        private List<String> GetAvailableDirections(int[,] sensorData)
        {
            List<string> possibleDirections = new List<string>();
            if (sensorData[0, 1] == 0)
                possibleDirections.Add("North");
            if (sensorData[1, 2] == 0)
                possibleDirections.Add("East");
            if (sensorData[2, 1] == 0)
                possibleDirections.Add("South");
            if (sensorData[1,0] == 0)
                possibleDirections.Add("West");
            return possibleDirections;
        }
    }














    public class ExploredMap
    {
        List<MazeCell> cells;
        MazeCell[,] mazeMap;
        List<Vector2Int> moveHistory;
        Vector2Int robotPosition;

        public ExploredMap(Vector2Int mazeDimension, Vector2Int robotPosition)
        {
            mazeMap = new MazeCell[mazeDimension.x, mazeDimension.y];
            this.robotPosition = new Vector2Int(robotPosition.x, robotPosition.y);
            this.cells = new List<MazeCell>();
            this.moveHistory = new List<Vector2Int>();
        }

        public Vector2Int GetCurrentPosition()
        {
            return new Vector2Int(robotPosition.x, robotPosition.y);
        }

        public bool ProcessSensor(int[,] sensorReading)
        {
            if (sensorReading.GetLength(0) == 3 && sensorReading.GetLength(1) == 3)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        int xMaze = robotPosition.x + x -1 ;
                        int yMaze = robotPosition.y + y -1;
                        if(mazeMap[xMaze,yMaze]!=null) {
                            if (x == 1 && y == 1)
                            {
                                mazeMap[x,y].Visit();   
                            }
                            continue;
                        }                      
                        MazeCell neighbor = new MazeCell(xMaze, yMaze); // create 
                        mazeMap[xMaze, yMaze] = neighbor;
                        cells.Add(neighbor);
                        if (sensorReading[x, y]==1)
                        {
                            neighbor.makeWall();
                        }
                    }
                }
                mazeMap[robotPosition.x, robotPosition.y].Visit();
                return true;
            }

            return false;
        }

        private bool CheckAbsolutePosition(Vector2Int position)
        {
            return position.x >= 0
                   && position.x < mazeMap.GetLength(0)
                   && position.y >= 0
                   && position.y < mazeMap.GetLength(1);
        }

        private bool CheckMoveBounds(Vector2Int relativeMove)
        {
            Vector2Int newPosition = robotPosition + relativeMove;
            return CheckAbsolutePosition(newPosition);
        }

        public bool CheckOpening(Vector2Int relativeMove)
        {
            if (!CheckMoveBounds(relativeMove)) return false;
            Vector2Int newPosition = robotPosition + relativeMove;
            return mazeMap[newPosition.x, newPosition.y] != null;
        }

        public bool CheckVisited(Vector2Int relativeMove)
        {
            if (!CheckMoveBounds(relativeMove)) return false;
            Vector2Int newPosition = robotPosition + relativeMove;
            return mazeMap[newPosition.x, newPosition.y].IsVisited();
        }

        public bool MoveRelative(Vector2Int relativeMove)
        {
            if (!CheckMoveBounds(relativeMove)) return false;
            Vector2Int newPosition = robotPosition + relativeMove;
            robotPosition = newPosition;
            moveHistory.Add(relativeMove);
            return true;
        }

        public MazeCell GetCell(Vector2Int absolutePosition)
        {
            return CheckAbsolutePosition(absolutePosition) ? mazeMap[absolutePosition.x, absolutePosition.y] : null;
        }

        public Vector2Int[] GetMoveHistoryArray()
        {
            Vector2Int[] moves = new Vector2Int[this.moveHistory.Count];
            this.moveHistory.CopyTo(moves);
            return moves;
        }

        public List<MazeCell> GetUnvisitedCells()
        {
            List<MazeCell> unvisited = new List<MazeCell>();
            foreach (MazeCell cell in this.cells)
            {
                if (!cell.IsVisited())
                {
                    unvisited.Add(cell);
                }
            }

            return unvisited;
        }
    }

    public class MazeCell
    {
        Vector2Int position;
        bool visited;
        bool isWall = false;

        public MazeCell(int x, int y)
        {
            this.position = new Vector2Int(x, y);
            this.visited = false;
        }

        public void makeWall(){
            this.isWall = true;
        }

        public bool isWallCell(){
            return this.isWall;
        }

        public void Visit()
        {
            this.visited = true;
        }

        public bool IsVisited()
        {
            return this.visited;
        }

        public Vector2Int GetPosition()
        {
            return new Vector2Int(position.x, position.y);
        }
    }




















    public class MazeGenerator
    {
        public int[,] GenerateMaze(int sizeRows, int sizeCols, float placementThreshold)
        {
            int[,] maze = new int[sizeRows, sizeCols];

            for (int i = 0; i < sizeRows; i++)
            {
                for (int j = 0; j < sizeCols; j++)
                {
                    if (i == 0 || j == 0 || i == sizeRows - 1 || j == sizeCols - 1)
                    {
                        maze[i, j] = 1;
                    }

                    else if (i % 2 == 0 && j % 2 == 0)
                    {
                        if (Random.value > placementThreshold)
                        {
                            maze[i, j] = 1;

                            int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                            int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                            maze[i + a, j + b] = 1;
                        }
                    }
                }
            }
            return maze;
        }
    }