using Algorithms;
using NUnit.Framework;

namespace DRL
{
    [TestFixture(0, 0, 0, 0, "East")]
    [TestFixture(0, 0, 0, 0, "North")]
    [TestFixture(0, 0, 0, 0, "South")]
    [TestFixture(0, 0, 0, 0, "West")]
    [TestFixture(0, 0, 0, 1, "East")]
    [TestFixture(0, 0, 0, 1, "North")]
    [TestFixture(0, 0, 0, 1, "South")]
    [TestFixture(0, 0, 1, 0, "North")]
    [TestFixture(0, 0, 1, 0, "South")]
    [TestFixture(0, 0, 1, 0, "West")]
    [TestFixture(0, 0, 1, 1, "North")]
    [TestFixture(0, 0, 1, 1, "South")]
    [TestFixture(0, 1, 0, 0, "East")]
    [TestFixture(0, 1, 0, 0, "North")]
    [TestFixture(0, 1, 0, 0, "West")]
    [TestFixture(0, 1, 0, 1, "East")]
    [TestFixture(0, 1, 0, 1, "North")]
    [TestFixture(0, 1, 1, 0, "North")]
    [TestFixture(0, 1, 1, 0, "West")]
    [TestFixture(0, 1, 1, 1, "North")]
    [TestFixture(1, 0, 0, 0, "East")]
    [TestFixture(1, 0, 0, 0, "South")]
    [TestFixture(1, 0, 0, 0, "West")]
    [TestFixture(1, 0, 0, 1, "East")]
    [TestFixture(1, 0, 0, 1, "South")]
    [TestFixture(1, 0, 1, 0, "South")]
    [TestFixture(1, 0, 1, 0, "West")]
    [TestFixture(1, 0, 1, 1, "South")]
    [TestFixture(1, 1, 0, 0, "East")]
    [TestFixture(1, 1, 0, 0, "West")]
    [TestFixture(1, 1, 0, 1, "East")]
    [TestFixture(1, 1, 1, 0, "West")]
    public class ExplorationRandomDirectionTests
    {
        private int[,] _sensorData;
        private string _direction;

        public ExplorationRandomDirectionTests(int north, int south, int east, int west, string direction)
        {
            _sensorData = new int[,] {{-1, north, -1}, {west, 2, east}, {-1, south, -1}};
            _direction = direction;
        }
        
        [Test]
        public void TestArguments()
        {
            Exploration exploration = new Exploration(30, 30);
            exploration.exploredMap.ProcessSensor(_sensorData);
            var directions = exploration.GetAvailableDirections(_sensorData);
            Assert.That(directions.Contains(_direction));
        }
    }
}
