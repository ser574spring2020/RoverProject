using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using RandomSystem = System.Random;

namespace Algorithms
{
    /*
        Main Class for Exploration
        Contains all the methods related to Exploration
     */
    public class Exploration
    {
        private List<String> commands = new List<string>() {"West", "North", "East", "South"};

        private List<Vector2Int> vectorCommands = new List<Vector2Int>()
            {Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.right};

        private int _points, _rows, _cols;

        public ExploredMap exploredMap;

        public Exploration(int rows, int cols)
        {
            this._rows = rows;
            this._cols = cols;
            exploredMap = new ExploredMap(new Vector2Int(rows, cols), new Vector2Int(1, 1));
        }

        //Returns the next command for the robot
        // @param SensorData - Used to compute the next command
        public String GetNextCommand(int[,] sensorData)
        {
            String robotCommand;
            exploredMap.ProcessSensor(sensorData);
            RandomSystem r = new RandomSystem();
            // sensorData = RotateSensorData(sensorData, _direction);
            List<String> possibleDirections = GetAvailableDirections(sensorData);
            int x = r.Next(0, possibleDirections.Count);
            robotCommand = possibleDirections[x];
            ManagePoints(vectorCommands[commands.IndexOf(robotCommand)]);
            // robotCommand = "South";
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(robotCommand)]);

            return robotCommand;
        }

        // Rotates the array anti-clockwise and returns the rotated array
        // @param direction - affects the number of times that array will be rotated
        public int[,] RotateSensorData(int[,] sensorData, string direction)
        {
            int counter = 0;
            switch (direction)
            {
                case "West":
                    counter = 1;
                    break;
                case "South":
                    counter = 2;
                    break;
                case "East":
                    counter = 3;
                    break;
            }

            int[,] output = sensorData;
            for (int n = 0; n < counter; n++)
            {
                int cols = sensorData.GetLength(0);
                int rows = sensorData.GetLength(1);
                output = new int [rows, cols];

                for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    output[j, cols - 1 - i] = sensorData[i, j];
                sensorData = output;
            }

            return output;
        }

        void printSensorData(int[,] sensorData)
        {
            string sensorDataString = "";
            for (int i = 0; i < sensorData.GetLength(0); i++)
            {
                for (int j = 0; j < sensorData.GetLength(1); j++)
                {
                    sensorDataString += sensorData[i, j] + " ";
                }

                sensorDataString += ",";
            }

            Debug.Log(sensorDataString);
        }

        public void MoveRobot(int[,] sensorData, string direction)
        {
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(direction)]);
        }

        private void ManagePoints(Vector2Int direction)
        {
            var futurePosition = GetExploredMap().GetCurrentPosition() + direction;
            if (exploredMap.GetCell(futurePosition).IsVisited() == false)
            {
                _points += 10;
            }
            else
            {
                _points -= 2;
            }
        }

        public int GetPoints()
        {
            return _points;
        }

        public int GetCoverage()
        {
            float coverage = 0, total = _cols * _rows;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (exploredMap.GetCell(new Vector2Int(i, j)) != null)
                        coverage += 1;
                }
            }

            coverage = coverage / total * 100;
            return (int) coverage;
        }

        //Returns the explored map
        public ExploredMap GetExploredMap()
        {
            return exploredMap;
        }

        //Computes all the available directions
        private List<String> GetAvailableDirections(int[,] sensorData)
        {
            List<string> possibleDirections = new List<string>();
            Vector2Int robotPosition = exploredMap.GetCurrentPosition();

            var mazeCellPosition = new Vector2Int(robotPosition.x - 1, robotPosition.y);
            var mazeCell = exploredMap.GetCell(mazeCellPosition);
            if (mazeCell != null)
                if (!mazeCell.IsWallCell() && !mazeCell.IsVisited())
                    possibleDirections.Add("North");

            mazeCellPosition = new Vector2Int(robotPosition.x, robotPosition.y + 1);
            mazeCell = exploredMap.GetCell(mazeCellPosition);
            if (mazeCell != null)
                if (!mazeCell.IsWallCell() && !mazeCell.IsVisited())
                    possibleDirections.Add("East");


            mazeCellPosition = new Vector2Int(robotPosition.x + 1, robotPosition.y);
            mazeCell = exploredMap.GetCell(mazeCellPosition);
            if (mazeCell != null)
                if (!mazeCell.IsWallCell() && !mazeCell.IsVisited())
                    possibleDirections.Add("South");


            mazeCellPosition = new Vector2Int(robotPosition.x, robotPosition.y - 1);
            mazeCell = exploredMap.GetCell(mazeCellPosition);
            if (mazeCell != null)
                if (!mazeCell.IsWallCell() && !mazeCell.IsVisited())
                    possibleDirections.Add("West");
            if (possibleDirections.Count == 0)
            {
                mazeCellPosition = new Vector2Int(robotPosition.x - 1, robotPosition.y);
                mazeCell = exploredMap.GetCell(mazeCellPosition);
                if (mazeCell != null)
                    if (!mazeCell.IsWallCell())
                        possibleDirections.Add("North");

                mazeCellPosition = new Vector2Int(robotPosition.x, robotPosition.y + 1);
                mazeCell = exploredMap.GetCell(mazeCellPosition);
                if (mazeCell != null)
                    if (!mazeCell.IsWallCell())
                        possibleDirections.Add("East");


                mazeCellPosition = new Vector2Int(robotPosition.x + 1, robotPosition.y);
                mazeCell = exploredMap.GetCell(mazeCellPosition);
                if (mazeCell != null)
                    if (!mazeCell.IsWallCell())
                        possibleDirections.Add("South");


                mazeCellPosition = new Vector2Int(robotPosition.x, robotPosition.y - 1);
                mazeCell = exploredMap.GetCell(mazeCellPosition);
                if (mazeCell != null)
                    if (!mazeCell.IsWallCell())
                        possibleDirections.Add("West");
            }

            return possibleDirections;
        }

        public void WriteSensorDataToCsv(int[,] sensorData, string direction)
        {
            var path = Directory.GetCurrentDirectory();
            string filePath = path + "/Datasets/Dataset.csv";
            foreach (var item in sensorData)
            {
                File.AppendAllText(filePath, item.ToString(), Encoding.UTF8);
                File.AppendAllText(filePath, ",", Encoding.UTF8);
            }

            File.AppendAllText(filePath, direction + ",", Encoding.UTF8);
            File.AppendAllText(filePath, Environment.NewLine, Encoding.UTF8);
        }
    }
}