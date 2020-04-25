using Algorithms;
using NUnit.Framework;
using UnityEngine;

namespace DRL
{
    [TestFixture]
    public class ExplorationTests
    {
        [Test]
        public void Exploration_Initializes_ExploredMap_When_RowsColsGreaterEqualThree()
        {
            Exploration exploration = new Exploration(3, 3);
            Vector2Int robotPosition = exploration.GetExploredMap().GetCurrentPosition();
            exploration.GetExploredMap().GetMazeArray();
            Assert.That(robotPosition, Is.EqualTo(new Vector2Int(1, 1)));
            
        }
    }
}