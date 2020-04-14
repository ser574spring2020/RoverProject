using NUnit.Framework;
using UnityEngine;

namespace Algorithms
{
    [TestFixture]
    public class MazeGeneratorTests
    {
        private MazeGenerator _mazeGenerator = new MazeGenerator();

        [Test]
        public void GenerateMaze_ReturnMaze_WhenRowsColsThreshPositive()
        {
            var maze = _mazeGenerator.GenerateMaze(30, 30, 0.5f);
            Assert.That(maze, !Is.Null);
        }

        [Test]
        public void GenerateMaze_ReturnNull_WhenRowsLessThanThree()
        {
            var maze = _mazeGenerator.GenerateMaze(2, 30, 0.5f);
            Assert.That(maze, Is.Null);
        }

        [Test]
        public void GenerateMaze_ReturnNull_WhenColsLessThanThree()
        {
            var maze = _mazeGenerator.GenerateMaze(30, 2, 0.5f);
            Assert.That(maze, Is.Null);
        }

        [Test]
        public void GenerateMaze_ReturnNull_WhenThreshNegative()
        {
            var maze = _mazeGenerator.GenerateMaze(30, 30, -0.5f);
            Assert.That(maze, Is.Null);
        }

        [Test]
        public void GenerateMaze_ReturnNull_WhenRowsColsThreshNegative()
        {
            var maze = _mazeGenerator.GenerateMaze(-30, -30, -0.5f);
            Assert.That(maze, Is.Null);
        }
    }
}