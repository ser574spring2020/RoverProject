using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class BreadthFirstSearch
    {
        public List<String> FindPath(int [,] grid, int start_x, int start_y, int end_x, int end_y)
        {
            
            
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            

            List<Tuple<Tuple<int,int>, Tuple<int,int>>> prev = new List<Tuple<Tuple<int,int>, Tuple<int, int>>>();

            Tuple<int, int>[] directions = { Tuple.Create(-1,0), Tuple.Create(0,1), Tuple.Create(1,0), Tuple.Create(0,-1)};

            Queue<Tuple<int,int>> visited = new Queue<Tuple<int,int>>();
            visited.Enqueue(Tuple.Create(start_x,start_y));
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
                                        direc.Add("east");
                                    if (x == 0 && y == -1)
                                        direc.Add("west");
                                    if (x == 1 && y == 0)
                                        direc.Add("south");
                                    if (x == -1 && y == 0)
                                        direc.Add("north");
                                }
                                return direc;
                            }

                            //Backtracking path from 'prev' tuple list.
                            foreach (Tuple<Tuple<int, int>, Tuple<int, int>> tup in prev)
                            {
                                //Console.WriteLine(tup.Item1);
                                //Console.WriteLine(rev_path[len_rev_path - 1]);
                                if (tup.Item1.Item1 == rev_path[len_rev_path - 1].Item1 && tup.Item1.Item2 == rev_path[len_rev_path - 1].Item2)
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

        public static void Main(string[] args)
        {
            int[,] grid = new int[7, 9] { { 1,1,1,1,1,1,1,1,1},
                                          { 1,0,1,0,1,0,1,2,1},
                                          { 1,0,1,0,0,0,0,0,1},
                                          { 1,1,1,1,0,1,1,1,1},
                                          { 1,3,1,0,0,0,1,0,1},
                                          { 1,0,0,0,0,0,1,0,1},
                                          { 1,1,1,1,1,1,1,1,1} };
            BreadthFirstSearch obj = new BreadthFirstSearch();
            //Console.WriteLine(0);
            List<String> final_list = obj.FindPath(grid, 4, 1, 0, 4);
            //Console.WriteLine(1);
            foreach (String x in final_list)
                Console.WriteLine(x);
        }
    }
}
