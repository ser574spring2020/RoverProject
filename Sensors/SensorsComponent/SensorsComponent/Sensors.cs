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

        /// <summary>Updates the obstacles.</summary>
        /// <param name="gameObj">The game object of Rover.</param>
        /// <param name="mazeData">The maze matrix chosen.</param>
        /// <param name="Direction">The direction in which the rover starts.</param>
        public virtual void Update_Obstacles(GameObject gameObj, int[,] mazeData, string Direction)
        {
            Rover = gameObj;
        }
        
        void Update()
        {
            //sensors.Update_Obstacles(Rover);
        }


        //Singleton Pattern. Only one instance allowed.
        protected Sensors()
        {
        }

        /// <summary>Gets the rover object.</summary>
        /// <returns>Retrurns the instance of the Rover</returns>
        public GameObject GetRoverObject()
        {
            return Rover;
        }

        /// <summary>Gets the obstacle matrix.</summary>
        /// <returns>Returns the obstacle matrix at the current position of the rover</returns>
        public int[,] Get_Obstacle_Matrix()
        {
            return obstacle_matrix;
        }
        
        protected void SetSensorType(int value)
        {
            sensorType = value;
        }

        /// <summary>Gets the type of the sensor.</summary>
        /// <returns>Returns SensorType value</returns>
        public int GetSensorType()
        {
            return sensorType;
        }

        /// <summary>Gets the current sensor.</summary>
        /// <returns>Returns string informing which sensor is currently been used</returns>
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
                    //obstacle_matrix[outRangeIndexes[0], outRangeIndexes[1]] = result;
                }
                else if (GetSensorType() > 1 || GetSensorType() < 5)
                {
                    // for LiDAR and Radar don't need this for now.
                    //if (GetSensorType() == 3 || GetSensorType() == 4) result = (int)hit.distance;

                    if (hit.distance > 2.5f)
                    {
                        DrawRayOnRover(ray, hit, "out");
                      //  obstacle_matrix[outRangeIndexes[0], outRangeIndexes[1]] = result;
                    }
                    else
                    {
                        DrawRayOnRover(ray, hit, "in");
                        //obstacle_matrix[inRangeIndexes[0], inRangeIndexes[1]] = result;
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
        
        protected void Update_liDAR_matrix(int[,] mazeData, string direction)
        {            
            obstacle_matrix = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 2, 0, 0 } };

            mazeData = RotateSensorData(mazeData, direction);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    obstacle_matrix[i, j] = mazeData[i, j];
                }
            }

            if (obstacle_matrix[1,1] == 1)
            {
                obstacle_matrix[0, 0] = -1;
                obstacle_matrix[0, 1] = -1;
            }

            if (obstacle_matrix[1, 3] == 1)
            {
                obstacle_matrix[0, 4] = -1;
                obstacle_matrix[0, 3] = -1;
            }

            if (obstacle_matrix[1, 2] == 1)
            {
                obstacle_matrix[0, 2] = -1;                
            }

            obstacle_matrix[1, 0] = -1;
            obstacle_matrix[2, 0] = -1;
            obstacle_matrix[2, 1] = -1;

            obstacle_matrix[1, 4] = -1;
            obstacle_matrix[2, 3] = -1;
            obstacle_matrix[2, 4] = -1;

        }

        private int[,] RotateSensorData(int[,] sensorData, string direction)
        {
            int counter = 0;
            switch (direction)
            {
                case "West":
                    counter = 1;
                    break;
                case "South":
                    counter = 2;
                    break;
                case "East":
                    counter = 3;
                    break;
            }

            int[,] output = sensorData;
            for (int n = 0; n < counter; n++)
            {
                int cols = sensorData.GetLength(0);
                int rows = sensorData.GetLength(1);
                output = new int[rows, cols];

                for (int i = 0; i < cols; i++)
                    for (int j = 0; j < rows; j++)
                        output[j, cols - 1 - i] = sensorData[i, j];
                sensorData = output;
            }

            return output;
        }


        protected void Update_range_matrix(int[,] mazeData, string Direction)
        {
            update_matrix(mazeData);

            Update_radar_matrix(obstacle_matrix, Direction);

            obstacle_matrix[0, 1] = -1;
            obstacle_matrix[0, 3] = -1;
            obstacle_matrix[1, 0] = -1;
            obstacle_matrix[1, 4] = -1;
            obstacle_matrix[3, 0] = -1;
            obstacle_matrix[3, 4] = -1;
            obstacle_matrix[4, 1] = -1;
            obstacle_matrix[4, 3] = -1;            

        }

        protected void Update_bumper_matrix(int[,] mazeData, string direction)
        {
            refractor_bumper_matrix(mazeData);

            if (direction.Equals("West"))
            {                
                obstacle_matrix[1, 0] = (mazeData[1, 0] == 0) ? 0 : 1;
            }
            else if (direction.Equals("East"))
            {                
                obstacle_matrix[1, 2] = (mazeData[1, 2] == 0) ? 0 : 1;
            }
            else if (direction.Equals("North"))
            {
                obstacle_matrix[0, 1] = (mazeData[0, 1] == 0) ? 0 : 1;                
            }
            else if (direction.Equals("South"))
            {
                obstacle_matrix[2, 1] = (mazeData[2, 1] == 0)? 0 : 1;
            }

            obstacle_matrix[1, 1] = 2;

        }

        private void refractor_bumper_matrix(int[,] mazeData)
        {
            for (int i = 0; i < obstacle_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < obstacle_matrix.GetLength(1); j++)
                {                  
                        obstacle_matrix[i, j] = -1;                                        
                }
            }
        }

        protected void Update_proximity_matrix(int[,] mazeData)
        {
            update_matrix(mazeData);

            obstacle_matrix[0, 0] = -1;
            obstacle_matrix[0, 2] = -1;
            obstacle_matrix[2, 0] = -1;
            obstacle_matrix[2, 2] = -1;
            
        }

        private void update_matrix(int[,] mazeData)
        {
            for (int i = 0; i < mazeData.GetLength(0); i++)
            {
                for (int j = 0; j < mazeData.GetLength(1); j++)
                {
                    obstacle_matrix[i, j] = mazeData[i, j];
                }
            }
        }

        protected void Update_radar_matrix(int[,] mazeData, string Direction)
        {
            update_matrix(mazeData);
            
            if (mazeData[1, 1] == 1) 
            {
                obstacle_matrix[0, 0] = -1;
                obstacle_matrix[0, 1] = -1;
                obstacle_matrix[1, 0] = -1;
            }
            if (mazeData[1, 2] == 1)
            {
                obstacle_matrix[0, 2] = -1;
            }
            if (mazeData[1, 3] == 1)
            {
                obstacle_matrix[1, 4] = -1;
                obstacle_matrix[0, 4] = -1;
                obstacle_matrix[0, 3] = -1;
            }            
            if (mazeData[2, 3] == 1)
            {
                obstacle_matrix[2, 4] = -1;
            }
            if (mazeData[3, 3] == 1)
            {
                obstacle_matrix[3, 4] = -1;
                obstacle_matrix[4, 4] = -1;
                obstacle_matrix[4, 3] = -1;
            }
            
            if (mazeData[3, 2] == 1)
            {
                obstacle_matrix[4, 2] = -1;
            }
            if (mazeData[3, 1] == 1)
            {
                obstacle_matrix[3, 0] = -1;
                obstacle_matrix[4, 0] = -1;
                obstacle_matrix[4, 1] = -1;
            }
            
            if (mazeData[2, 1] == 1)
            {
                obstacle_matrix[2, 0] = -1;
            }            
        }
    }
}
