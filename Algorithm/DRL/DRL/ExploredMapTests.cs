using Algorithms;
using NUnit.Framework;
using UnityEngine;

namespace DRL
{
    [TestFixture]
    public class ExploredMapTests
    {
        private ExploredMap map;

        [SetUp]
        public void Initialize()
        {
            map = new ExploredMap(new Vector2Int(3, 3), new Vector2Int(1, 1));
        }

        [Test]
        public void RobotValidMove()
        {
            map.MoveRelative(new Vector2Int(1, 0));
            Assert.That(map.GetCurrentPosition(), Is.EqualTo(new Vector2Int(2, 1)));
            map.MoveRelative(new Vector2Int(0, -1));
            Assert.That(map.GetCurrentPosition(), Is.EqualTo(new Vector2Int(2, 0)));
        }

        [Test]
        public void RobotInvalidMove()
        {
            map.MoveRelative(new Vector2Int(1, 0));
            Assert.That(map.GetCurrentPosition(), Is.EqualTo(new Vector2Int(2, 1)));
            bool returnCode = map.MoveRelative(new Vector2Int(1, 0));
            Assert.That(map.GetCurrentPosition(), Is.EqualTo(new Vector2Int(2, 1)));
            Assert.That(returnCode, Is.EqualTo(false));
        }
    }
}