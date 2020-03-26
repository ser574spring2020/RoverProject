using System;
using System.Collections.Generic;
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

        public List<String> GetNextCommand(int[,] sensorData)
        {
            List<String> commands = new List<string>();
            SaveSensorData((sensorData));
            int north = sensorData[0, 1];
            int south = sensorData[2, 1];
            int east = sensorData[1, 2];
            int west = sensorData[1, 0];
            
            if (south == 0)
            {
                commands.Add("South");
                Debug.Log("Moving South from" + currentX + ", " + currentY);
                currentX += 1;
            }
            else if (east == 0)
            {
                commands.Add("East");
                Debug.Log("Moving East from" + currentX + ", " + currentY);
                currentY += 1;
            }
            
            else if(west == 0)
            {
                commands.Add("West");
                Debug.Log("Moving West from" + currentX + ", " + currentY);
                currentY -= 1;
            }
            else if (north == 0)
            {
                commands.Add("North");
                Debug.Log("Moving North from" + currentX + ", " + currentY);
                currentX -= 1;
            }
            else
            {
                
            }
            

            Debug.Log(currentX + ", " + currentY);
            return commands;
        }

        public void addFlags()
        {
        }

        private void SaveSensorData(int[,] sensorData)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (exploredMaze[currentX - 1 + i, currentY - 1 + j] == -1)
                    {
                        exploredMaze[currentX - 1 + i, currentY - 1 + j] = sensorData[i, j];
                    }
                }
            }
        }

        public void setPosition(int x, int y)
        {
            currentX = x;
            currentY = y;
        }

        public List<String> FindPath(int[,] grid, int start_x, int start_y, int end_x, int end_y)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);


            List<Tuple<Tuple<int, int>, Tuple<int, int>>> prev = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();

            Tuple<int, int>[] directions =
                {Tuple.Create(-1, 0), Tuple.Create(0, 1), Tuple.Create(1, 0), Tuple.Create(0, -1)};

            Queue<Tuple<int, int>> visited = new Queue<Tuple<int, int>>();
            visited.Enqueue(Tuple.Create(start_x, start_y));
            while (visited.Count != 0)
            {
                Tuple<int, int> point = visited.Dequeue();
                foreach (Tuple<int, int> direction in directions)
                {
                    int new_x = direction.Item1 + point.Item1;
                    int new_y = direction.Item2 + point.Item2;
                    //Console.WriteLine("{0},{1}", new_x, new_y);

                    if (new_x == end_x && new_y == end_y)
                    {
                        Console.WriteLine("Reached Destination");
                        prev.Add(Tuple.Create(Tuple.Create(new_x, new_y), Tuple.Create(point.Item1, point.Item2)));
                        List<Tuple<int, int>> rev_path = new List<Tuple<int, int>>();
                        rev_path.Add(Tuple.Create(new_x, new_y));
                        while (true)
                        {
                            int len_rev_path = rev_path.Count;
                            Tuple<int, int> cell = rev_path[len_rev_path - 1];

                            //Checking if we have reached the source
                            if (cell.Item1 == start_x && cell.Item2 == start_y)
                            {
                                rev_path.Reverse();

                                //Initializing Direction List
                                List<String> direc = new List<String>();

                                //Populating Direction List
                                for (int i = 1; i < len_rev_path; i++)
                                {
                                    int x = rev_path[i].Item1 - rev_path[i - 1].Item1;
                                    int y = rev_path[i].Item2 - rev_path[i - 1].Item2;
                                    if (x == 0 && y == 1)
                                        direc.Add("South");
                                    if (x == 0 && y == -1)
                                        direc.Add("North");
                                    if (x == 1 && y == 0)
                                        direc.Add("East");
                                    if (x == -1 && y == 0)
                                        direc.Add("West");
                                }

                                return direc;
                            }

                            //Backtracking path from 'prev' tuple list.
                            foreach (Tuple<Tuple<int, int>, Tuple<int, int>> tup in prev)
                            {
                                //Console.WriteLine(tup.Item1);
                                //Console.WriteLine(rev_path[len_rev_path - 1]);
                                if (tup.Item1.Item1 == rev_path[len_rev_path - 1].Item1 &&
                                    tup.Item1.Item2 == rev_path[len_rev_path - 1].Item2)
                                {
                                    //Console.WriteLine("Adding");
                                    rev_path.Add(tup.Item2);
                                    break;
                                }
                            }
                        }
                    }
                    else if (new_x >= 0 && new_x < rows && new_y >= 0 && new_y < cols && grid[new_x, new_y] == 0)
                    {
                        //Console.WriteLine("{0},{1} {2},{3}", new_x,new_y, end_x,end_y);
                        grid[new_x, new_y] = 1;
                        visited.Enqueue(Tuple.Create(new_x, new_y));
                        prev.Add(Tuple.Create(Tuple.Create(new_x, new_y), Tuple.Create(point.Item1, point.Item2)));
                    }
                }
            }

            //Returning empty list when there is no path
            List<String> lst = new List<String>();
            lst.Add("");
            return lst;
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