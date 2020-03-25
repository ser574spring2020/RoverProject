using UnityEngine;

namespace Algorithms
{
    public class MazeGenerator : MonoBehaviour
    {
        public static int[,] GenerateMaze(int sizeRows, int sizeCols, float placementThreshold)
        {
            var maze = new int[sizeRows, sizeCols];

            for (var i = 0; i < sizeRows; i++)
                    for (var j = 0; j < sizeCols; j++)
                        if (i == 0 || j == 0 || i == sizeRows - 1 || j == sizeCols - 1)
                            maze[i, j] = 1;
                        else if (i % 2 == 0 && j % 2 == 0)
                            if (Random.value > placementThreshold)
                            {
                                maze[i, j] = 1;
                                var a = Random.value < .5 ? 0 : Random.value < .5 ? -1 : 1;
                                var b = a != 0 ? 0 : Random.value < .5 ? -1 : 1;
                                maze[i + a, j + b] = 1;
                            }
            return maze;
        }
    }
}