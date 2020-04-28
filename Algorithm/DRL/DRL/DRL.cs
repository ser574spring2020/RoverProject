using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using DRL;
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

        private String direction = "East";
        public ExploredMap exploredMap;

        public Exploration(int rows, int cols)
        {
            this._rows = rows;
            this._cols = cols;
            exploredMap = new ExploredMap(new Vector2Int(rows, cols), new Vector2Int(1, 1));
        }

        private enum SensorType
        {
            Proximity = 1,
            Range = 2,
            Lidar = 3,
            Radar = 4,
            Bumper = 5
        }

        private enum AlgorithmType
        {
            BackPropagation = 1,
            FeedForward = 2,
            DeepLearning = 3,
            RandomDirection = 4
        }

        //Returns the next command for the robot
        // @param SensorData - Used to compute the next command
        public String GetNextCommand(int[,] sensorData, int sensorType, int algorithmType)
        {
            int[,] dataToBeSaved = sensorData;
            if (sensorType == 3)
                sensorData = RotateSensorData(sensorData, direction);
            exploredMap.ProcessSensor(sensorData);
            String robotCommand;
            switch (algorithmType)
            {
                case (int) AlgorithmType.BackPropagation:
                    robotCommand = getCommandFromBackPropagation(sensorData, direction, sensorType);
                    break;
                case (int) AlgorithmType.RandomDirection:
                    robotCommand = getCommandFromRandomDirectionAlgorithm(sensorData, direction, sensorType);
                    break;
                case (int) AlgorithmType.FeedForward:
                    robotCommand = getCommandFromRandomDirectionAlgorithm(sensorData, direction, sensorType);
                    break;
                default:
                    robotCommand = getCommandFromRandomDirectionAlgorithm(sensorData, direction, sensorType);
                    Debug.Log("Unknown Algorithm Type: Switched to Random Direction Algorithm");
                    break;
            }

            ManagePoints(vectorCommands[commands.IndexOf(robotCommand)]);
            WriteSensorDataToCsv(dataToBeSaved, robotCommand, sensorType);
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(robotCommand)]);
            direction = robotCommand;
            return robotCommand;
        }

        String getCommandFromRandomDirectionAlgorithm(int[,] sensorData, String direection, int sensorType)
        {
            RandomSystem r = new RandomSystem();
            List<String> possibleDirections = GetAvailableDirections(sensorData);
            int x = r.Next(0, possibleDirections.Count);
            var robotCommand = possibleDirections[x];
            return robotCommand;
        }

        String getCommandFromFeedForward(int[,] sensorData, String direction, int sensorType)
        {
            String sensorTypeString = "";
            switch (sensorType)
            {
                case (int) SensorType.Proximity:
                    sensorTypeString = "Proximity";
                    break;
                case (int) SensorType.Range:
                    sensorTypeString = "Range";
                    break;
                case (int) SensorType.Lidar:
                    sensorTypeString = "Lidar";
                    break;
                case (int) SensorType.Radar:
                    sensorTypeString = "Radar";
                    break;
                case (int) SensorType.Bumper:
                    sensorTypeString = "Bumper";
                    break;
            }

            int[,] processedSensorData = getProcessedSensorData(sensorData, direction, sensorType);
            FeedForwardManager forwardManager = new FeedForwardManager();
            var robotCommand =
                forwardManager.GetDirectionFromFeedForward(sensorTypeString,
                    convertToOneDimensional(processedSensorData));
            return robotCommand;
        }

        String getCommandFromBackPropagation(int[,] sensorData, String direction, int sensorType)
        {
            String sensorTypeString = "";
            switch (sensorType)
            {
                case (int) SensorType.Proximity:
                    sensorTypeString = "Proximity";
                    break;
                case (int) SensorType.Range:
                    sensorTypeString = "Range";
                    break;
                case (int) SensorType.Lidar:
                    sensorTypeString = "Lidar";
                    break;
                case (int) SensorType.Radar:
                    sensorTypeString = "Radar";
                    break;
                case (int) SensorType.Bumper:
                    sensorTypeString = "Bumper";
                    break;
            }

            int[,] processedSensorData = getProcessedSensorData(sensorData, direction, sensorType);
            var robotCommand =
                BackPropagation.Driver("Command", sensorTypeString, convertToOneDimensionalDouble(processedSensorData));
            return robotCommand;
        }

        public double[] convertToOneDimensionalDouble(int[,] array)
        {
            double[] result = new double[array.GetLength(0) * array.GetLength(1)];
            int i = 0;
            foreach (int VARIABLE in array)
            {
                result[i++] = Convert.ToDouble(VARIABLE);
            }

            return result;
        }

        public float[] convertToOneDimensional(int[,] array)
        {
            float[] result = new float[array.GetLength(0) * array.GetLength(1)];
            int i = 0;
            foreach (int VARIABLE in array)
            {
                result[i++] = VARIABLE;
            }

            return result;
        }

        public void GenerateDataset(int[,] sensorData, string resultDirection, int sensorType)
        {
            int[,] dataToBeSaved = sensorData;
            if (sensorType == 3)
                sensorData = RotateSensorData(sensorData, direction);
            exploredMap.ProcessSensor(sensorData);
            var robotCommand = resultDirection;
            WriteSensorDataToCsv(dataToBeSaved, robotCommand, sensorType);
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(robotCommand)]);
            direction = robotCommand;
        }

        // Rotates the array clockwise and returns the rotated array
        // @param direction - affects the number of times that array will be rotated
        public int[,] RotateSensorData(int[,] sensorData, string direction)
        {
            int counter = 0;
            switch (direction)
            {
                case "West":
                    counter = 3;
                    break;
                case "South":
                    counter = 2;
                    break;
                case "East":
                    counter = 1;
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

        public static void printSensorData(int[,] sensorData)
        {
            string sensorDataString = "";
            for (int i = 0; i < sensorData.GetLength(0); i++)
            {
                for (int j = 0; j < sensorData.GetLength(1); j++)
                {
                    sensorDataString += sensorData[i, j] + " ";
                }

                sensorDataString += " ,,, ";
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

        public int[,] getProcessedSensorData(int[,] sensorData, string direction, int sensorType)
        {
            int[,] processedSensorData = sensorData;
            Vector2Int localRobotPosition = exploredMap.GetSensorRobotPosition(sensorData);
            Vector2Int _robotPosition = exploredMap._robotPosition;
            for (var x = 0; x < processedSensorData.GetLength(0); x++)
            for (var y = 0; y < processedSensorData.GetLength(1); y++)
            {
                var xMaze = _robotPosition.x + x - localRobotPosition.x;
                var yMaze = _robotPosition.y + y - localRobotPosition.y;
                if (exploredMap.GetCell(new Vector2Int(xMaze, yMaze)) != null &&
                    exploredMap.GetCell(new Vector2Int(xMaze, yMaze)).IsVisited())
                    processedSensorData[x, y] = 4;
                processedSensorData[localRobotPosition.x, localRobotPosition.y] = 2;
            }

            return processedSensorData;
        }

        public void WriteSensorDataToCsv(int[,] sensorData, string direction, int sensorType)
        {
            int[,] dataToBeSaved = sensorData;
            Vector2Int localRobotPosition = exploredMap.GetSensorRobotPosition(sensorData);
            Vector2Int _robotPosition = exploredMap._robotPosition;
            for (var x = 0; x < dataToBeSaved.GetLength(0); x++)
            for (var y = 0; y < dataToBeSaved.GetLength(1); y++)
            {
                var xMaze = _robotPosition.x + x - localRobotPosition.x;
                var yMaze = _robotPosition.y + y - localRobotPosition.y;
                if (exploredMap.GetCell(new Vector2Int(xMaze, yMaze)) != null &&
                    exploredMap.GetCell(new Vector2Int(xMaze, yMaze)).IsVisited())
                    dataToBeSaved[x, y] = 4;
                dataToBeSaved[localRobotPosition.x, localRobotPosition.y] = 2;
            }

            var path = Directory.GetCurrentDirectory();
            string filePath = "/Datasets/Dataset.csv";
            if (sensorType == 1)
                filePath = path + "/Datasets/Proximity.csv";
            if (sensorType == 2)
                filePath = path + "/Datasets/Range.csv";
            if (sensorType == 3)
                filePath = path + "/Datasets/Lidar.csv";
            if (sensorType == 4)
                filePath = path + "/Datasets/Radar.csv";
            if (sensorType == 5)
                filePath = path + "/Datasets/Bumper.csv";
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