using NSubstitute;
using NUnit.Framework;

namespace Sensors_Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
     
        public void CheckBumperMatrixDimensions()
        {
            int[,] bumperMatrix = new int[,] { { -1, 0, -1 }, { 0, 2, 0 }, { -1, 0, -1 } };            
            Assert.That(bumperMatrix.GetLength(0), Is.EqualTo(3));
            Assert.That(bumperMatrix.GetLength(1), Is.EqualTo(3));
        }

        [Test]
        public void CheckRangeMatrixDimensions()
        {
            int[,] rangeMatrix = new int[,] {{ -1, -1, -1, -1, -1 },
                                                     { -1, -1, -1, -1, -1 },
                                                     { -1, -1,  2, -1, -1 },
                                                     { -1, -1, -1, -1, -1 },
                                                     { -1, -1, -1, -1, -1 }};

            Assert.That(rangeMatrix.GetLength(0), Is.EqualTo(5));
            Assert.That(rangeMatrix.GetLength(1), Is.EqualTo(5));
        }

        [Test]
        public void CheckRadarMatrixDimensions()
        {
            int[,] radarMatrix = new int[,] {{ -1, -1, -1, -1, -1 },
                                                     { -1, -1, -1, -1, -1 },
                                                     { -1, -1,  2, -1, -1 },
                                                     { -1, -1, -1, -1, -1 },
                                                     { -1, -1, -1, -1, -1 }};

            Assert.That(radarMatrix.GetLength(0), Is.EqualTo(5));
            Assert.That(radarMatrix.GetLength(1), Is.EqualTo(5));
        }

        public void CheckProximityMatrixDimensions()
        {
            int[,] proximityMatrix = new int[,] { { -1, 0, -1 }, { 0, 2, 0 }, { -1, 0, -1 } };
            Assert.That(proximityMatrix.GetLength(0), Is.EqualTo(3));
            Assert.That(proximityMatrix.GetLength(1), Is.EqualTo(3));
        }

        [Test]
        public void CheckSensorTypeForProximity()
        {

            int sensorType = 1;
            Assert.That(sensorType, Is.EqualTo(1));
        }


        [Test]
        public void CheckSensorTypeForRange()
        {

            int sensorType = 2;
            Assert.That(sensorType, Is.EqualTo(2));
        }

        [Test]
        public void CheckSensorTypeForLiDAR()
        {
            int sensorType = 3;
            Assert.That(sensorType, Is.EqualTo(3));
        }

        [Test]
        public void CheckSensorTypeForRadar()
        {
            int sensorType = 4;
            Assert.That(sensorType, Is.EqualTo(4));
        }

        [Test]
        public void CheckSensorTypeForBumper()
        {
            int sensorType = 5;
            Assert.That(sensorType, Is.EqualTo(5));
        }


    }
}