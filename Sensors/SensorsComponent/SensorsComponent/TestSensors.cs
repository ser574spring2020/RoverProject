using NUnit.Framework;
using UnityEngine;
using SensorsComponent;

namespace TestSensors
{
     
        [TestFixture]
        public class TestSensors
        {

            public GameObject Rover;
            public static Sensors sensor;            

            public TestSensors()
            {
                sensor = SensorFactory.GetInstance(1, Rover);
            }

            [Test]            
            public void CheckSensorTypeForProximity()
            {
                sensor.SetSensorType(1);
                int sensorType = sensor.GetSensorType();
                Assert.That(sensorType, Is.EqualTo(1));
            }

            [Test]
            public void CheckSensorTypeForRange()
            {
                sensor.SetSensorType(2);
                int sensorType = sensor.GetSensorType();
                Assert.That(sensorType, Is.EqualTo(1));
            }

            [Test]
            public void CheckSensorTypeForLiDAR()
            {
                sensor.SetSensorType(3);
                int sensorType = sensor.GetSensorType();
                Assert.That(sensorType, Is.EqualTo(1));
            }

            [Test]
            public void CheckSensorTypeForRadar()
            {
                sensor.SetSensorType(4);
                int sensorType = sensor.GetSensorType();
                Assert.That(sensorType, Is.EqualTo(1));
            }

            [Test]
            public void CheckSensorTypeForBumper()
            {
                sensor.SetSensorType(5);
                int sensorType = sensor.GetSensorType();
                Assert.That(sensorType, Is.EqualTo(1));
            }

                [Test]            
                public void CheckProximityMatrixDimensions()
                {
                    int[,] proximityMatrix = new int[,] { { -1, 0, -1 }, { 0, 2, 0 }, { -1, 0, -1 } };
                    Assert.That(proximityMatrix.GetLength(0), Is.EqualTo(3));
                    Assert.That(proximityMatrix.GetLength(1), Is.EqualTo(3));
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
                public void CheckLiDARMatrixDimensions()
                {
                    int[,] liDARMatrix = new int[,] {{ -1, -1, -1, -1, -1 },
                                                     { -1, -1, -1, -1, -1 },
                                                     { -1, -1,  2, -1, -1 },
                                                     { -1, -1, -1, -1, -1 },
                                                     { -1, -1, -1, -1, -1 }};

                    Assert.That(liDARMatrix.GetLength(0), Is.EqualTo(5));
                    Assert.That(liDARMatrix.GetLength(1), Is.EqualTo(5));
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

                [Test]
                public void CheckBumperMatrixDimensions()
                {
                    int[,] bumperMatrix = new int[,] { { 0, 0, 0 }, { 0, 2, 0 }, { 0, 0, 0 } };
                    Assert.That(bumperMatrix.GetLength(0), Is.EqualTo(3));
                    Assert.That(bumperMatrix.GetLength(1), Is.EqualTo(3));
                }



                [Test]
                public void CheckRoverInstanceAsNull()
                {
                    sensor = SensorFactory.GetInstance(1, Rover);
                    Assert.That(sensor, Is.Null);
                }


    }


}


