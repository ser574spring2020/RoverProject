using Algorithms;
using RandomSystem = System.Random;
using NUnit.Framework;

namespace DRL
{
    [TestFixture]
    public class MazeGeneratorTests
    {
        private MazeGenerator _mazeGenerator;

        public MazeGeneratorTests()
        {
            _mazeGenerator = new MazeGenerator();
        }

        private RandomSystem _randomNumber = new RandomSystem();

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
                        if (Random() > placementThreshold)
                        {
                            maze[i, j] = 1;
                            int a = Random() < .5 ? 0 : (Random() < .5 ? -1 : 1);
                            int b = a != 0 ? 0 : (Random() < .5 ? -1 : 1);
                            maze[i + a, j + b] = 1;
                        }
                    }
                }
            }

            return maze;
        }

        private double Random()
        {
            return _randomNumber.NextDouble();
        }

        //Test for GenerateMaze
        [Test]
        public void GenerateMaze_Return_Null_WhenRowsLessThanThree()
        {
            var maze = _mazeGenerator.GenerateMaze(2, 30, 0.5f);
            Assert.That(maze, Is.Null);
        }

        //Test for GenerateMaze
        [Test]
        public void GenerateMaze_Return_Null_WhenColsLessThanThree()
        {
            var maze = _mazeGenerator.GenerateMaze(30, 2, 0.5f);
            Assert.That(maze, Is.Null);
        }

        //Test for GenerateMaze
        [Test]
        public void GenerateMaze_Return_Null_WhenThreshNegative()
        {
            var maze = _mazeGenerator.GenerateMaze(30, 30, -0.5f);
            Assert.That(maze, Is.Null);
        }

        //Test for GenerateMaze
        [Test]
        public void GenerateMaze_Return_Null_WhenRowsColsThreshNegative()
        {
            var maze = _mazeGenerator.GenerateMaze(-30, -30, -0.5f);
            Assert.That(maze, Is.Null);
        }
    }
}
