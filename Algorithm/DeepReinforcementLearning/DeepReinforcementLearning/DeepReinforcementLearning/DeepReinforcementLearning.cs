using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using RandomSystem = System.Random;


namespace Algorithms
{
    public class Exploration : MonoBehaviour
    {
        private List<String> commands = new List<string>() {"West", "North", "East", "South"};

        public List<String> GetNextCommand(int[,] sensorData)
        {
            List<String> commands = new List<string>();
            RandomSystem r = new RandomSystem();
            List<String> possibleDirections = GetAvailableDirections(sensorData);
            int x = r.Next(0, possibleDirections.Count+1);
            String command = possibleDirections[x];
            Debug.Log(command);
            commands.Add(command);
            return commands;
        }

        public Exploration(int sizeRows, int sizeCols)
        {
            ExploredMap exploredMap = new ExploredMap(new Vector2Int(30,30),new Vector2Int(1,1) );
            
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

        public bool ProcessSensor(bool[,] sensorReading)
        {
            if (sensorReading.GetLength(0) == 3 && sensorReading.GetLength(1) == 3)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (x == 1 && y == 1)
                        {
                            continue; // skip the the center, which is this cell
                        }

                        int xMaze = robotPosition.x + x - 1;
                        int yMaze = robotPosition.y + y - 1;
                        if (sensorReading[x, y] && mazeMap[xMaze, yMaze] == null)
                        {
                            MazeCell neighbor = new MazeCell(xMaze, yMaze); // create 
                            mazeMap[xMaze, yMaze] = neighbor;
                            cells.Add(neighbor);
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

        public MazeCell(int x, int y)
        {
            this.position = new Vector2Int(x, y);
            this.visited = false;
        }

        public void Visit()
        {
            visited = true;
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
}