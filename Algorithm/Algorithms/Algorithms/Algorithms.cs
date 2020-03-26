using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Algorithms
{
    public class Exploration
    {
        private int[,] exploredMaze;

        public Exploration(int sizeRows, int sizeCols)
        {
            exploredMaze = new int[sizeRows, sizeCols];
        }

        public string GetNextCommand(int[,] sensorData)
        {
            int north = sensorData[0, 1];
            int east = sensorData[1, 2];
            int south = sensorData[2, 1];
            int west = sensorData[1, 0];
            if (east == 0)
            {
                return "East";
            }
            else if (south == 0)
            {
                return "South";
            }
            else if (west == 0)
            {
                return "West";
            }
            else
            {
                return "North";
            }
        }

        private void SaveSensorData(int[,] sensorData)
        {
            
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
                            //3
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