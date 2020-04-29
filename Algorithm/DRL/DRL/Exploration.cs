using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using DRL;
using RandomSystem = System.Random;

namespace Algorithms
{
    /*Main Class for Exploration.*/
    public class Exploration
    {
        private List<String> commands = new List<string>() {"West", "North", "East", "South"};

        private List<Vector2Int> vectorCommands = new List<Vector2Int>()
            {Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.right};

        private String _direction = "East";
        private int _points, _rows, _cols;
        public ExploredMap exploredMap;

        private enum AlgorithmType
        {
            BackPropagation = 0,
            FeedForward = 1,
            DeepLearning = 2,
            RandomDirection = 3
        }

        private enum SensorType
        {
            Proximity = 0,
            Range = 1,
            Lidar = 2,
            Radar = 3,
            Bumper = 4
        }

        private enum ExperimentType
        {
            Training = 0,
            Testing = 1,
        }

        public Exploration(int rows, int cols)
        {
            this._rows = rows;
            this._cols = cols;
            exploredMap = new ExploredMap(new Vector2Int(rows, cols), new Vector2Int(1, 1));
        }


        /*Returns the next command for the robot
        @param SensorData - Used to compute the next command*/
        public string GetNextCommand(int[,] sensorData, int sensorType, int algorithmType, int experimentType)
        {
            if (experimentType == (int) ExperimentType.Training) return "";
            int[,] dataToBeSaved = sensorData;
            if (sensorType == 3)
                sensorData = RotateSensorData(sensorData, _direction);
            exploredMap.ProcessSensor(sensorData);
            String robotCommand;
            switch (algorithmType)
            {
                case (int) AlgorithmType.BackPropagation:
                    robotCommand = GetCommandFromBackPropagation(sensorData, sensorType);
                    break;
                case (int) AlgorithmType.RandomDirection:
                    robotCommand = GetCommandFromRandomDirectionAlgorithm(sensorData);
                    break;
                case (int) AlgorithmType.FeedForward:
                    robotCommand = GetCommandFromFeedForward(sensorData, sensorType);
                    break;
                default:
                    robotCommand = GetCommandFromRandomDirectionAlgorithm(sensorData);
                    Debug.Log("Unknown Algorithm Type: Switched to Random Direction Algorithm");
                    break;
            }

            ManagePoints(vectorCommands[commands.IndexOf(robotCommand)]);
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(robotCommand)]);
            _direction = robotCommand;
            return robotCommand;
        }

        String GetCommandFromBackPropagation(int[,] sensorData, int sensorType)
        {
            String sensorTypeString = "";
            if (sensorType == (int) SensorType.Proximity)
                sensorTypeString = "Proximity";
            else if (sensorType == (int) SensorType.Range)
                sensorTypeString = "Range";
            else if (sensorType == (int) SensorType.Lidar)
                sensorTypeString = "Lidar";
            else if (sensorType == (int) SensorType.Radar)
                sensorTypeString = "Radar";
            else if (sensorType == (int) SensorType.Bumper) sensorTypeString = "Bumper";

            int[,] processedSensorData = GetProcessedSensorData(sensorData);
            var robotCommand =
                BackPropagation.Driver("Command", sensorTypeString, convertToOneDimensionalDouble(processedSensorData));
            return robotCommand;
        }

        String GetCommandFromFeedForward(int[,] sensorData, int sensorType)
        {
            String sensorTypeString = "";
            if (sensorType == (int) SensorType.Proximity)
                sensorTypeString = "Proximity";
            else if (sensorType == (int) SensorType.Range)
                sensorTypeString = "Range";
            else if (sensorType == (int) SensorType.Lidar)
                sensorTypeString = "Lidar";
            else if (sensorType == (int) SensorType.Radar)
                sensorTypeString = "Radar";
            else if (sensorType == (int) SensorType.Bumper) sensorTypeString = "Bumper";
            int[,] processedSensorData = GetProcessedSensorData(sensorData);
            FeedForwardManager forwardManager = new FeedForwardManager();
            var robotCommand =
                forwardManager.GetDirectionFromFeedForward(sensorTypeString,
                    ConvertToOneDimensionalFloat(processedSensorData));
            return robotCommand;
        }

        String GetCommandFromRandomDirectionAlgorithm(int[,] sensorData)
        {
            RandomSystem r = new RandomSystem();
            List<String> possibleDirections = GetAvailableDirections(sensorData);
            int x = r.Next(0, possibleDirections.Count);
            var robotCommand = possibleDirections[x];
            return robotCommand;
        }

        //Computes all the available directions
        private List<String> GetAvailableDirections(int[,] sensorData)
        {
            List<string> possibleDirections = new List<string>();
            Vector2Int robotPosition = exploredMap.GetCurrentPosition();

            Vector2Int mazeCellPosition = new Vector2Int(robotPosition.x - 1, robotPosition.y);
            MazeCell mazeCell = exploredMap.GetCell(mazeCellPosition);
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
            if (possibleDirections.Count != 0) return possibleDirections;
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
            if (mazeCell == null) return possibleDirections;
            if (!mazeCell.IsWallCell())
                possibleDirections.Add("West");

            return possibleDirections;
        }

        //Save movement and sensor data to csv file
        public void GenerateDataset(int[,] sensorData, string resultDirection, int sensorType)
        {
            int[,] dataToBeSaved = sensorData;
            if (sensorType == 3)
                sensorData = RotateSensorData(sensorData, _direction);
            exploredMap.ProcessSensor(sensorData);
            var robotCommand = resultDirection;
            WriteSensorDataToCsv(dataToBeSaved, robotCommand, sensorType);
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(robotCommand)]);
            _direction = robotCommand;
        }

        //Move the robot in given direction
        public void MoveRobot(string direction)
        {
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(direction)]);
        }

        //Calculates the points of the robot
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

        //Returns the total coverage of the robot.
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

        public int GetPoints()
        {
            return _points;
        }

        public ExploredMap GetExploredMap()
        {
            return exploredMap;
        }

        //Returns the sensor data with the information about the visited cells
        private int[,] GetProcessedSensorData(int[,] sensorData)
        {
            int[,] processedSensorData = sensorData;
            Vector2Int localRobotPosition = exploredMap.GetSensorRobotPosition(sensorData);
            Vector2Int robotPosition = exploredMap._robotPosition;
            for (var x = 0; x < processedSensorData.GetLength(0); x++)
            for (var y = 0; y < processedSensorData.GetLength(1); y++)
            {
                var xMaze = robotPosition.x + x - localRobotPosition.x;
                var yMaze = robotPosition.y + y - localRobotPosition.y;
                if (exploredMap.GetCell(new Vector2Int(xMaze, yMaze)) != null &&
                    exploredMap.GetCell(new Vector2Int(xMaze, yMaze)).IsVisited())
                    processedSensorData[x, y] = 4;
                processedSensorData[localRobotPosition.x, localRobotPosition.y] = 2;
            }

            return processedSensorData;
        }

        //Converts int[,] to int[]
        public double[] convertToOneDimensionalDouble(int[,] array)
        {
            double[] result = new double[array.GetLength(0) * array.GetLength(1)];
            int i = 0;
            foreach (int variable in array)
            {
                result[i++] = Convert.ToDouble(variable);
            }

            return result;
        }

        //Converts int[,] to float[]
        public float[] ConvertToOneDimensionalFloat(int[,] array)
        {
            float[] result = new float[array.GetLength(0) * array.GetLength(1)];
            int i = 0;
            foreach (int variable in array)
            {
                result[i++] = variable;
            }

            return result;
        }

        //Prints the sensorData array
        public static void PrintSensorData(int[,] sensorData)
        {
            string sensorDataString = "";
            for (var i = 0; i < sensorData.GetLength(0); i++)
            {
                for (var j = 0; j < sensorData.GetLength(1); j++)
                {
                    sensorDataString += sensorData[i, j] + " ";
                }

                sensorDataString += " ,,, ";
            }

            Debug.Log(sensorDataString);
        }
        
        // Rotates the array clockwise and returns the rotated array
        // @param direction - affects the number of times that array will be rotated
        public static int[,] RotateSensorData(int[,] sensorData, string direction)
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

        //Write sensor data and the given direction to csv file
        private void WriteSensorDataToCsv(int[,] sensorData, string direction, int sensorType)
        {
            int[,] dataToBeSaved = sensorData;
            Vector2Int localRobotPosition = exploredMap.GetSensorRobotPosition(sensorData);
            Vector2Int robotPosition = exploredMap._robotPosition;
            for (var x = 0; x < dataToBeSaved.GetLength(0); x++)
            for (var y = 0; y < dataToBeSaved.GetLength(1); y++)
            {
                var xMaze = robotPosition.x + x - localRobotPosition.x;
                var yMaze = robotPosition.y + y - localRobotPosition.y;
                if (exploredMap.GetCell(new Vector2Int(xMaze, yMaze)) != null &&
                    exploredMap.GetCell(new Vector2Int(xMaze, yMaze)).IsVisited())
                    dataToBeSaved[x, y] = 4;
                dataToBeSaved[localRobotPosition.x, localRobotPosition.y] = 2;
            }

            var path = Directory.GetCurrentDirectory();
            string filePath = "";
            if (sensorType == (int)SensorType.Proximity)
                filePath = path + "/Datasets/Proximity.csv";
            if (sensorType == (int)SensorType.Range)
                filePath = path + "/Datasets/Range.csv";
            if (sensorType == (int)SensorType.Lidar)
                filePath = path + "/Datasets/Lidar.csv";
            if (sensorType == (int)SensorType.Radar)
                filePath = path + "/Datasets/Radar.csv";
            if (sensorType == (int)SensorType.Bumper)
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