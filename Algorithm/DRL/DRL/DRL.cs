using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
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

        private int points, rows, cols;

        ExploredMap exploredMap;
        
        public Exploration(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            exploredMap = new ExploredMap(new Vector2Int(rows, cols), new Vector2Int(1, 1));
        }

        //Returns the next command for the robot
        // @param SensorData - Used to compute the next command
        public String GetNextCommand(int[,] sensorData)
        {
            String robotCommand;
            RandomSystem r = new RandomSystem();
            exploredMap.ProcessRangeSensor(sensorData);
            List<String> possibleDirections = GetAvailableDirections(sensorData);
            int x = r.Next(0, possibleDirections.Count);
            robotCommand = possibleDirections[x];
            ManagePoints(vectorCommands[commands.IndexOf(robotCommand)]);
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(robotCommand)]);
            return robotCommand;
        }

        public void MoveRobot(int[,] sensorData, string direction)
        {
            exploredMap.ProcessRangeSensor(sensorData);
            exploredMap.MoveRelative(vectorCommands[commands.IndexOf(direction)]);
        }

        private void ManagePoints(Vector2Int direction)
        {
            var futurePosition = GetExploredMap().GetCurrentPosition() + direction;
            if (exploredMap.GetCell(futurePosition).IsVisited() == false)
            {
                points += 10;
            }
            else
            {
                points -=2;
            }
        }

        public int GetPoints()
        {
            return points;
        }

        public int GetCoverage()
        {
            float coverage = 0, total = cols * rows;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (exploredMap.GetCell(new Vector2Int(i, j)) != null)
                        coverage+=1;
                }
            }
            coverage = coverage/total * 100;
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
            if (sensorData[0, 1] == 0 &&
                exploredMap.GetCell(new Vector2Int(robotPosition.x - 1, robotPosition.y)).IsVisited() == false)
                possibleDirections.Add("North");
            if (sensorData[1, 2] == 0 &&
                exploredMap.GetCell(new Vector2Int(robotPosition.x, robotPosition.y + 1)).IsVisited() == false)
                possibleDirections.Add("East");
            if (sensorData[2, 1] == 0 &&
                exploredMap.GetCell(new Vector2Int(robotPosition.x + 1, robotPosition.y)).IsVisited() == false)
                possibleDirections.Add("South");
            if (sensorData[1, 0] == 0 &&
                exploredMap.GetCell(new Vector2Int(robotPosition.x, robotPosition.y - 1)).IsVisited() == false)
                possibleDirections.Add("West");
            if (possibleDirections.Count == 0)
            {
                if (sensorData[0, 1] == 0)
                    possibleDirections.Add("North");
                if (sensorData[1, 2] == 0)
                    possibleDirections.Add("East");
                if (sensorData[2, 1] == 0)
                    possibleDirections.Add("South");
                if (sensorData[1, 0] == 0)
                    possibleDirections.Add("West");
            }
            return possibleDirections;
        }
        
        public void WriteSensorDataToCsv(int[,] sensorData, string direction){
            var path = Directory.GetCurrentDirectory();
            string filePath = path + "/Datasets/Dataset.csv";
            foreach (var item in sensorData)
            {
                File.AppendAllText(filePath,item.ToString(),Encoding.UTF8);
                File.AppendAllText(filePath,",",Encoding.UTF8);
            }
            File.AppendAllText(filePath,direction+",",Encoding.UTF8);
            File.AppendAllText(filePath, Environment.NewLine, Encoding.UTF8);
        }
    }
}