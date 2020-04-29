/*
    Author   : Aneesh Dalvi, Vaibhav Singhal
    Function : Implements Functionality for Radar Sensor
    Version  : V1
    Email    : adalvi1@asu.edu | Arizona State University.
*/

using UnityEngine;

namespace SensorsComponent
{
    class RadarSensor : Sensors
    {
        public RadarSensor()
        {
            sensorLength = 4f;
            obstacle_matrix = new int[,] {{ -1, -1, -1, -1, -1 },
                                          { -1, -1, -1, -1, -1 },
                                          { -1, -1,  2, -1, -1 },
                                          { -1, -1, -1, -1, -1 },
                                          { -1, -1, -1, -1, -1 }};
        }
        public override void Update_Obstacles(GameObject gObj, int[,] mazeData, string Direction)
        {
            /*
            initial position for every step before checking for potential collisions
            5X5 matrix is taken for this sensor
            -1 : don't know; setting distance of collision to object in range of (1f-2f) */

            base.Update_Obstacles(gObj, mazeData, Direction);

            Update_radar_matrix(mazeData, Direction);

            DrawCircle(gObj.transform.position, sensorLength, Color.blue);
            DrawCircle(gObj.transform.position, sensorLength - 2, Color.red);

            DrawRotatingLine(gObj, Color.white);

            // Straight Lines
            CheckObstacle(gObj.transform.position,
                          Vector3.forward,
                          gObj, 0, "Front",
                          new int[] { 0, 2 },
                          new int[] { 1, 2 });
            CheckObstacle(gObj.transform.position,
                          -Vector3.forward,
                          gObj, 0, "Back",
                          new int[] { 4, 2 },
                          new int[] { 3, 2 });
            CheckObstacle(gObj.transform.position,
                          Vector3.right,
                          gObj, 0, "Right",
                          new int[] { 2, 4 },
                          new int[] { 2, 3 });
            CheckObstacle(gObj.transform.position,
                          -Vector3.right,
                          gObj, 0, "Left",
                          new int[] { 2, 0 },
                          new int[] { 2, 1 });

            // 45 Degree Angle Lines
            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward,
                          gObj, 45, "45 Degrees Front Right",
                          new int[] { 1, 3 },
                          new int[] { 1, 3 });
            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward,
                          gObj, -45, "45 Degrees Front Left",
                          new int[] { 1, 1 },
                          new int[] { 1, 1 });
            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(-45, Vector3.up) * (-gObj.transform.forward),
                          gObj, -45, "45 Degrees Back Left",
                          new int[] { 3, 1 },
                          new int[] { 3, 1 });
            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(45, Vector3.up) * (-gObj.transform.forward),
                          gObj, 45, "45 Degrees Back Right",
                          new int[] { 3, 3 },
                          new int[] { 3, 3 });
            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(25, Vector3.up) * gObj.transform.forward,
                          gObj, 25, "25 Degrees Front Right",
                          new int[] { 0, 3 },
                          new int[] { 0, 3 });
            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(-25, Vector3.up) * gObj.transform.forward,
                          gObj, -25, "25 Degrees Front Left",
                          new int[] { 0, 1 },
                          new int[] { 0, 1 });


            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(-25, Vector3.up) * gObj.transform.right,
                          gObj, -25, "25 Degrees Right Up",
                          new int[] { 1, 4 },
                          new int[] { 1, 4 });
            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(25, Vector3.up) * gObj.transform.right,
                          gObj, 25, "25 Degrees Right Down",
                          new int[] { 3, 4 },
                          new int[] { 3, 4 });

            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(25, Vector3.up) * (-gObj.transform.right),
                          gObj, 25, "25 Degrees left Up",
                          new int[] { 1, 0 },
                          new int[] { 1, 0 });
            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(-25, Vector3.up) * (-gObj.transform.right),
                          gObj, -25, "25 Degrees left Down",
                          new int[] { 3, 0 },
                          new int[] { 3, 0 });

            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(25, Vector3.up) * (-gObj.transform.forward),
                          gObj, -25, "25 Degrees Back Right",
                          new int[] { 4, 3 },
                          new int[] { 4, 3 });
            CheckObstacle(gObj.transform.position,
                          Quaternion.AngleAxis(-25, Vector3.up) * (-gObj.transform.forward),
                          gObj, 25, "25 Degrees Back Left",
                          new int[] { 1, 3 },
                          new int[] { 1, 3 });
        }
    }
}

