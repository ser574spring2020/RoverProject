using NUnit.Framework;
using Algorithms;
using UnityEngine;

namespace DRL
{
    [TestFixture]
    public class TestsExploredMaze
    {
        [Test]
        public void GetSensorRobotPosition_Should_Return_Position_For_ProximitySensor()
        {
            ExploredMap exploredMap = new ExploredMap(new Vector2Int(30,30),new Vector2Int(1,1) );
            int[,] array = new int[3, 3] {{-1, 1, -1}, {1, 2, 1}, {-1, 0, -1}}; //Data like proximity sensor
            Vector2Int result = exploredMap.GetSensorRobotPosition(array);
            Assert.That(result,Is.EqualTo(new Vector2Int(1,1)));
        }
        
        
    }
}