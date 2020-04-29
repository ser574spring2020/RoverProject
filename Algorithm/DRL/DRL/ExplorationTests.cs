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

        [Test]
        public void RotateSensorData_Should_RotateArrayThrice_When_Direction_Is_West()
        {
            var array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            array = Exploration.RotateSensorData(array, "West");
            var result = new int[3, 3] {{3, 6, 9}, {2, 5, 8}, {1, 4, 7}};
            Assert.That(array, Is.EqualTo(result));
        }

        [Test]
        public void RotateSensorData_Should_RotateArrayOnce_When_Direction_Is_East()
        {
            var array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            array = Exploration.RotateSensorData(array, "East");
            var result = new int[3, 3] {{7, 4, 1}, {8, 5, 2}, {9, 6, 3}};
            Assert.That(array, Is.EqualTo(result));
        }

        [Test]
        public void RotateSensorData_ShouldNot_RotateArray_When_Direction_Is_North()
        {
            var array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            array = Exploration.RotateSensorData(array, "North");
            var result = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            Assert.That(array, Is.EqualTo(result));
        }

        [Test]
        public void RotateSensorData_Should_RotateArrayTwice_When_Direction_Is_South()
        {
            var array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            array = Exploration.RotateSensorData(array, "South");
            var result = new int[3, 3] {{9, 8, 7}, {6, 5, 4}, {3, 2, 1}};
            Assert.That(array, Is.EqualTo(result));
        }

        [Test]
        public void ConvertToOneDimensionalDouble_Should_Convert_Int2d_To_Double1d()
        {
            var exploration = new Exploration(30, 30);
            var array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            var converted = exploration.convertToOneDimensionalDouble(array);
            var expected = new double[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
            Assert.That(converted, Is.EqualTo(expected));
        }

        [Test]
        public void ConvertToOneDimensionalFloat_Should_Convert_Int2d_To_Double1d()
        {
            var exploration = new Exploration(30, 30);
            var array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            var converted = exploration.ConvertToOneDimensionalFloat(array);
            var expected = new float[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
            Assert.That(converted, Is.EqualTo(expected));
        }
    }
}