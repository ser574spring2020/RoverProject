using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Algorithms
{
    public class Exploration
    {
        private int[,] exploredMaze;
        private int currentX = 1, currentY = 1;

        public Exploration(int sizeRows, int sizeCols)
        {
            exploredMaze = new int[sizeRows, sizeCols];
            for (int i = 0; i < sizeRows; i++)
            for (int j = 0; j < sizeCols; j++)
                exploredMaze[i, j] = -1;
        }

        public int[,] GetExploredMaze()
        {
            return exploredMaze;
        }

        public string[] GetNextCommand(int[,] sensorData)
        {
            string[] result = new string[100];
            SaveSensorData(sensorData);
            return result;
        }

        private void SaveSensorData(int[,] sensorData)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    exploredMaze[currentX-1+j,currentY-1+i]=sensorData[i, j];
                }
            }
        }

        public void setPosition(int x, int y)
        {
            currentX += x;
            currentY += y;
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