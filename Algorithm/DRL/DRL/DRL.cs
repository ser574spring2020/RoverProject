using System.Collections.Generic;
using UnityEngine;
using System;
using RandomSystem = System.Random;

namespace Algorithms
{
    public class Exploration
    {
        private List<String> commands = new List<string>() {"West", "North", "East", "South"};

        private List<Vector2Int> vectorCommands = new List<Vector2Int>()
            {Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.right};

        ExploredMap exploredMap;

        public Exploration(int rows, int cols)
        {
            exploredMap = new ExploredMap(new Vector2Int(rows, cols), new Vector2Int(1, 1));
        }

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

        public ExploredMap GetExploredMap()
        {
            return exploredMap;
        }

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
    }
}