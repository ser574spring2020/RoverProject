using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using RandomSystem = System.Random;


namespace Algorithms
{
    public class Exploration : MonoBehaviour
    {
        private List<String> commands = new List<string>() {"West","North","East","West"};
        public List<String> GetNextCommand(int[,] sensorData)
        {
            List<String> commands = new List<string>();
            RandomSystem r = new RandomSystem();
            List<String> possibleDirections = GetAvailableDirections(sensorData);
            int x = r.Next(0, possibleDirections.Count+1);
            commands.Add(possibleDirections[x]);
            return commands;
        }

        public Exploration(int sizeRows, int sizeCols)
        {
        }

        private List<String> GetAvailableDirections(int[,] sensorData)
        {
            int[,] directions = new int[,] {{-1, 0}, {0, 1}, {1, 0}, {0, -1}};
            Vector2Int robotPosition = new Vector2Int(1, 1);
            List<string> possibleDirections = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                if (sensorData[robotPosition.x+directions[i,0],robotPosition.x+directions[i,0]]==0)
                    possibleDirections.Add(commands[i]);
                    
            }
            return possibleDirections;
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
    }
}