using Algorithms;
using NUnit.Framework;
using UnityEngine;

namespace DRL
{
    [TestFixture]
    public class ExploredMapTests
    {
        private ExploredMap _map;

        [SetUp]
        public void Initialize()
        {
            _map = new ExploredMap(new Vector2Int(3, 3), new Vector2Int(1, 1));
        }

        //Test for GetSensorRobotPosition
        [Test]
        public void GetSensorRobotPosition_Should_Return_OneOne_For_ThreeByThreeSensor()
        {
            ExploredMap exploredMap = new ExploredMap(new Vector2Int(30, 30), new Vector2Int(1, 1));
            int[,] sensorData = {{0, 0, 0}, {0, 2, 0}, {0, 0, 0}};
            Vector2Int result = exploredMap.GetSensorRobotPosition(sensorData);
            Vector2Int expected = new Vector2Int(1, 1);
            Assert.That(result, Is.EqualTo(expected));
        }

        //Test for GetSensorRobotPosition
        [Test]
        public void GetSensorRobotPosition_Should_Return_OneOne_For_FiveByFiveSensor()
        {
            ExploredMap exploredMap = new ExploredMap(new Vector2Int(30, 30), new Vector2Int(1, 1));
            int[,] sensorData = {{0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 2, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}};
            Vector2Int result = exploredMap.GetSensorRobotPosition(sensorData);
            Vector2Int expected = new Vector2Int(2, 2);
            Assert.That(result, Is.EqualTo(expected));
        }

        //Test for ProcessSensor
        [Test]
        public void ProcessSensor_Should_Add_GivenSensorThreeByThreeSensorData_To_ExploredMap()
        {
            ExploredMap exploredMap = new ExploredMap(new Vector2Int(30, 30), new Vector2Int(1, 1));
            int[,] sensorData = {{-1, 1, 0}, {0, 2, 1}, {-1, -1, 1}};
            exploredMap.ProcessSensor(sensorData);
            var mazeArray = exploredMap.GetMazeArray();
            var saveData = new int[3, 3];
            for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                saveData[i, j] = mazeArray[i, j];
            Assert.That(sensorData, Is.EqualTo(saveData));
        }

        //Test for ProcessSensor
        [Test]
        public void ProcessSensor_Should_Add_GivenSensorFiveByFiveSensorData_To_ExploredMap()
        {
            ExploredMap exploredMap = new ExploredMap(new Vector2Int(30, 30), new Vector2Int(2, 2));
            int[,] sensorData =
            {
                {-1, -1, -1, -1, -1},
                {-1, 1, 0, 1, -1},
                {-1, 0, 2, 1, -1},
                {-1, 0, 0, 1, -1},
                {-1, 1, 0, 1, 1}
            };
            exploredMap.ProcessSensor(sensorData);
            int[,] expectedData =
            {
                {-1, -1, -1, -1, -1},
                {-1, 1, 0, 1, -1},
                {-1, 0, 2, 1, -1},
                {-1, 0, 0, 1, -1},
                {-1, 1, 0, 1, 1}
            };
            var mazeArray = exploredMap.GetMazeArray();
            var saveData = new int[5, 5];
            for (var i = 0; i < 5; i++)
            for (var j = 0; j < 5; j++)
                saveData[i, j] = mazeArray[i, j];
            Assert.That(expectedData, Is.EqualTo(saveData));
        }
        
        [Test]
        public void ProcessSensor_Should_Add_GivenSensorFiveByFiveSensorData_To_ExploredMap_WhenRobotOnCorner()
        {
            ExploredMap exploredMap = new ExploredMap(new Vector2Int(30, 30), new Vector2Int(1, 1));
            int[,] sensorData =
            {
                {-1, -1, -1, -1, -1},
                {-1, 1, 0, 1, -1},
                {-1, 0, 2, 1, -1},
                {-1, 0, 0, 1, -1},
                {-1, 1, 0, 1, 1}
            };
            exploredMap.ProcessSensor(sensorData);
            int[,] expectedData =
            {
                {1, 0, 1, -1},
                {0, 2, 1, -1},
                {0, 0, 1, -1},
                {1, 0, 1, 1}
            };
            var mazeArray = exploredMap.GetMazeArray();
            var saveData = new int[4, 4];
            for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
                saveData[i, j] = mazeArray[i, j];
            Assert.That(expectedData, Is.EqualTo(saveData));
        }

        [Test]
        public void RobotValidMove()
        {
            _map.MoveRelative(new Vector2Int(1, 0));
            Assert.That(_map.GetCurrentPosition(), Is.EqualTo(new Vector2Int(2, 1)));
            _map.MoveRelative(new Vector2Int(0, -1));
            Assert.That(_map.GetCurrentPosition(), Is.EqualTo(new Vector2Int(2, 0)));
        }

        [Test]
        public void RobotInvalidMove()
        {
            _map.MoveRelative(new Vector2Int(1, 0));
            Assert.That(_map.GetCurrentPosition(), Is.EqualTo(new Vector2Int(2, 1)));
            bool returnCode = _map.MoveRelative(new Vector2Int(1, 0));
            Assert.That(_map.GetCurrentPosition(), Is.EqualTo(new Vector2Int(2, 1)));
            Assert.That(returnCode, Is.EqualTo(false));
        }
        
        //Test for GetSensorRobotPosition
        [Test]
        public void GetSensorRobotPosition_Should_Return_Position_For_ProximitySensor()
        {
            var exploredMap = new ExploredMap(new Vector2Int(30,30),new Vector2Int(1,1) );
            var array = new int[3, 3] {{-1, 1, -1}, {1, 2, 1}, {-1, 0, -1}}; //Data like proximity sensor
            var result = exploredMap.GetSensorRobotPosition(array);
            Assert.That(result,Is.EqualTo(new Vector2Int(1,1)));
        }
    }
}
