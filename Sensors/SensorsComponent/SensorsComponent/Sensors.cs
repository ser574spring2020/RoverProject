using System;
using UnityEngine;

namespace SensorsComponent
{
    public class Sensors : MonoBehaviour
    {
        protected static Sensors sensors;
        protected static GameObject Rover;
        protected float Distance;
        protected float oldTime;
        protected float newTime;
        protected float newVelocity;
        protected float oldVelocity;

        [Header("Sensors")]
        protected float sensorLength;
        protected float sideSensorPos;
        protected float frontSensorAngle = 45;

        protected static int sensorType;

        protected static int[,] obstacle_matrix;
        private static String currentSensor;

        //Interface for updating matrix     
        public virtual void Update_Obstacles(GameObject gameObj)
        {

        }

        public void Update()
        {
            sensors.Update_Obstacles(Rover);
        }


        //Singleton Pattern. Only one instance allowed.
        protected Sensors()
        {
        }

        public int[,] Get_Obstacle_Matrix()
        {
            return obstacle_matrix;
        }

        public void SetSensorType(int value)
        {
            sensorType = value;
        }

        public int GetSensorType()
        {
            return sensorType;
        }

        // gets the string for current running Sensor
        public String GetCurrentSensor()
        {
            return currentSensor;
        }

        protected static void SetCurrentSensor(String value)
        {
            currentSensor = value;
        }

        protected void TestProximityMatrix(int[,] matrix)
        {
            Debug.Log("-- Printing Matrix : -- ");

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Debug.Log(matrix.GetValue(i, j));
                }
                Debug.Log("-- row done -- ");
            }
            Debug.Log("-- Printing Matrix done -- ");
        }

        protected void DrawCircle(Vector3 position, float radius, Color color)
        {
            var increment = 10;
            for (int angle = 0; angle < 360; angle = angle + increment)
            {
                var heading = Vector3.forward - position;
                var direction = heading / heading.magnitude;
                var point = position + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
                var point2 = position + Quaternion.Euler(0, angle + increment, 0) * Vector3.forward * radius;
                Debug.DrawLine(point, point2, color);
            }
        }

        // Draw rotating line over Radar
        protected void DrawRotatingLine(GameObject gObj, Color color)
        {
            Quaternion q = Quaternion.AngleAxis(100 * Time.time, Vector3.up);
            Vector3 direction = q * gObj.transform.forward * sensorLength;
            Debug.DrawLine(gObj.transform.position,
            gObj.transform.position + direction);

        }

        protected void CheckObstacle(Vector3 origin, Vector3 direction, GameObject gObj,
                                   int angle, String obstacleToBeFound, int[] outRangeIndexes,
                                   int[] inRangeIndexes)
        {
            RaycastHit hit;
            Ray ray;

            if (angle != 0)
            {
                ray = new Ray(origin, direction);
            }
            else // for 0
            {
                ray = new Ray(origin, gObj.transform.TransformDirection(direction));
            }

            int result = 1;

            if (Physics.Raycast(ray, out hit, sensorLength))
            {
                // commented for algo testing
                Debug.Log("found " + obstacleToBeFound + " obstacle");

                if (GetSensorType() == 1)
                {
                    DrawRayOnRover(ray, hit, "");
                    obstacle_matrix[outRangeIndexes[0], outRangeIndexes[1]] = result;
                }
                else
                {
                    // for LiDAR and Radar don't need this for now.
                    //if (GetSensorType() == 3 || GetSensorType() == 4) result = (int)hit.distance;

                    if (hit.distance > 2.5f)
                    {
                        DrawRayOnRover(ray, hit, "out");
                        obstacle_matrix[outRangeIndexes[0], outRangeIndexes[1]] = result;
                    }
                    else
                    {
                        DrawRayOnRover(ray, hit, "in");
                        obstacle_matrix[inRangeIndexes[0], inRangeIndexes[1]] = result;
                    }
                }
            }

        }

        /* Function description for Drawing Ray on Rover. */
        protected void DrawRayOnRover(Ray ray, RaycastHit hit, String range)
        {
            if (GetSensorType() == 3) // LiDAR
            {
                if (range.Equals("out"))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.green);
                }
                else
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                }
            }
            else if (GetSensorType() == 4) // Radar
            {
                // No rays required
            }
            else if (GetSensorType() == 2) // Range
            {
                if (range.Equals("out"))
                {
                    Debug.DrawLine(ray.origin, hit.point);
                }
                else
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                }
            }
            else if (GetSensorType() == 5) // Bumper
            {
                // no rays required
            }
            else if (GetSensorType() == 1)  // Proximity
            {
                Debug.DrawLine(ray.origin, hit.point);
            }
            else
            {
                // no rays
            }

        }

    }
}
