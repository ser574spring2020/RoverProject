using System.Collections.Generic;
using UnityEngine;
using RandomSystem = System.Random;

namespace Algorithms
{
    //Contains all the methods used to access the explored map
    public class ExploredMap
    {
        private readonly List<MazeCell> _cells;
        private readonly MazeCell[,] _mazeMap;
        private readonly List<Vector2Int> _moveHistory;
        public Vector2Int _robotPosition;

        public ExploredMap(Vector2Int mazeDimension, Vector2Int robotPosition)
        {
            _mazeMap = new MazeCell[mazeDimension.x, mazeDimension.y];
            _robotPosition = new Vector2Int(robotPosition.x, robotPosition.y);
            _cells = new List<MazeCell>();
            _moveHistory = new List<Vector2Int>();
        }

        public Vector2Int GetCurrentPosition()
        {
            return new Vector2Int(_robotPosition.x, _robotPosition.y);
        }

        //Returns the position of the robot in a given sensor data matrix
        public Vector2Int GetSensorRobotPosition(int[,] sensorReading)
        {
            for (var i = 0; i < sensorReading.GetLength(0); i++)
            for (var j = 0; j < sensorReading.GetLength(1); j++)
                if (sensorReading[i, j] == 2)
                    return new Vector2Int(i, j);
            return new Vector2Int(0, 0);
        }

        //saves the sensor reading to the explored map
        public void ProcessSensor(int[,] sensorReading)
        {
            Vector2Int localRobotPosition = GetSensorRobotPosition(sensorReading);
            for (var x = 0; x < sensorReading.GetLength(0); x++)
            for (var y = 0; y < sensorReading.GetLength(1); y++)
            {
                var xMaze = _robotPosition.x + x - localRobotPosition.x;
                var yMaze = _robotPosition.y + y - localRobotPosition.y;
                if (sensorReading[x, y] == -1) continue;
                if (_mazeMap[xMaze, yMaze] != null) continue;
                var neighbor = new MazeCell(xMaze, yMaze); // create 
                _mazeMap[xMaze, yMaze] = neighbor;
                if (sensorReading[x, y] == 1) neighbor.MakeWall();
            }
            _mazeMap[_robotPosition.x, _robotPosition.y].Visit();
        }

        //Returns the integer array for the explored maze
        public int[,] GetMazeArray()
        {
            var intArray = new int[_mazeMap.GetLength(0), _mazeMap.GetLength(1)];
            for (var i = 0; i < _mazeMap.GetLength(0); i++)
            for (var j = 0; j < _mazeMap.GetLength(1); j++)
                if (_mazeMap[i, j] == null)
                    intArray[i, j] = -1;            // unexplored
                else if (_mazeMap[i, j].IsWallCell())
                    intArray[i, j] = 1;             // wall
                else
                    intArray[i, j] = 0;             // open

            intArray[_robotPosition.x, _robotPosition.y] = 2; // robot position
            return intArray;
        }

        //Checks if the position is valid or not
        private bool CheckAbsolutePosition(Vector2Int position)
        {
            return position.x >= 0
                   && position.x < _mazeMap.GetLength(0)
                   && position.y >= 0
                   && position.y < _mazeMap.GetLength(1);
        }

        //checks if the robot can move in the given direction
        private bool CheckMoveBounds(Vector2Int relativeMove)
        {
            var newPosition = _robotPosition + relativeMove;
            return CheckAbsolutePosition(newPosition);
        }

        //checks if the robot can move in the given direction
        public bool CheckOpening(Vector2Int relativeMove)
        {
            if (!CheckMoveBounds(relativeMove)) return false;
            var newPosition = _robotPosition + relativeMove;
            return _mazeMap[newPosition.x, newPosition.y] != null;
        }

        //checks if the cell in the given direction is visited or not
        public bool CheckVisited(Vector2Int relativeMove)
        {
            if (!CheckMoveBounds(relativeMove)) return false;
            var newPosition = _robotPosition + relativeMove;
            return _mazeMap[newPosition.x, newPosition.y].IsVisited();
        }

        //move the robot in the given relative direction
        public bool MoveRelative(Vector2Int relativeMove)
        {
            if (!CheckMoveBounds(relativeMove)) return false;
            var newPosition = _robotPosition + relativeMove;
            _robotPosition = newPosition;
            _moveHistory.Add(relativeMove);
            return true;
        }

        //Returns the cell at the given position
        public MazeCell GetCell(Vector2Int absolutePosition)
        {
            return CheckAbsolutePosition(absolutePosition) ? _mazeMap[absolutePosition.x, absolutePosition.y] : null;
        }

        //returns the move history of the robot
        public Vector2Int[] GetMoveHistoryArray()
        {
            var moves = new Vector2Int[_moveHistory.Count];
            _moveHistory.CopyTo(moves);
            return moves;
        }

        //Returns the unvisited cells of the explored maze
        public List<MazeCell> GetUnvisitedCells()
        {
            var unvisited = new List<MazeCell>();
            foreach (var cell in _cells)
                if (!cell.IsVisited())
                    unvisited.Add(cell);

            return unvisited;
        }
    }
}