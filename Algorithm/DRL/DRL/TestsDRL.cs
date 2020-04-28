using Algorithms;
using NUnit.Framework;
using UnityEngine.Rendering;

namespace DRL
{
    [TestFixture]
    public class DrlTests
    {
        [Test]
        public void RotateSensorData_Should_RotateArrayThrice_When_Direction_Is_East()
        {
            Exploration exploration = new Exploration(30, 30);
            int[,] array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            array = exploration.RotateSensorData(array, "East");
            int[,] result = new int[3, 3] {{3, 6, 9}, {2, 5, 8}, {1, 4, 7}};
            Assert.That(array, Is.EqualTo(result));
        }

        [Test]
        public void RotateSensorData_Should_RotateArrayOnce_When_Direction_Is_West()
        {
            Exploration exploration = new Exploration(30, 30);
            int[,] array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            array = exploration.RotateSensorData(array, "West");
            int[,] result = new int[3, 3] {{7, 4, 1}, {8, 5, 2}, {9, 6, 3}};
            Assert.That(array, Is.EqualTo(result));
        }

        [Test]
        public void RotateSensorData_ShouldNot_RotateArray_When_Direction_Is_North()
        {
            Exploration exploration = new Exploration(30, 30);
            int[,] array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            array = exploration.RotateSensorData(array, "North");
            int[,] result = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            Assert.That(array, Is.EqualTo(result));
        }

        [Test]
        public void RotateSensorData_Should_RotateArrayTwice_When_Direction_Is_South()
        {
            Exploration exploration = new Exploration(30, 30);
            int[,] array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            array = exploration.RotateSensorData(array, "South");
            int[,] result = new int[3, 3] {{9, 8, 7}, {6, 5, 4}, {3, 2, 1}};
            Assert.That(array, Is.EqualTo(result));
        }

        [Test]
        public void ConvertToOneDimensional_Should_Convert_Int2d_To_Double1d()
        {
            Exploration exploration = new Exploration(30,30);
            int[,] array = new int[3, 3] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            double[] converted = exploration.convertToOneDimensionalDouble(array);
            double[] expected = new double[]{1,2,3,4,5,6,7,8,9};
            Assert.That(converted, Is.EqualTo(expected));
        }
    }
}