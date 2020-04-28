using Random = UnityEngine.Random;
using RandomSystem = System.Random;

namespace Algorithms
{
    public class MazeGenerator
    {
        //Returns an integer array maze
        // @param sizeRows - Number of rows in the maze
        // @param sizeCols - Number of cols in the maze
        // @param placementThreshold - Threshold of wall placement
        public int[,] GenerateMaze(int sizeRows, int sizeCols, float placementThreshold)
        {
            if (sizeCols < 3 || sizeRows < 3 || placementThreshold <= 0f || placementThreshold >= 1f)
                return null;
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