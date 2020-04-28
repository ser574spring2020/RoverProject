
/*
    Author   : Aneesh Dalvi
    Function : Implements Functionality for Range Sensor
    Version  : V1
    Email    : adalvi1@asu.edu | Arizona State University.
*/

using UnityEngine;

namespace SensorsComponent
{
    class RangeSensor : Sensors
    {
        public RangeSensor()
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
            /* initial position for every step before checking for potential collisions
               5X5 matrix is taken for this sensor */

            Update_range_matrix(mazeData, Direction);

            Vector3 origin = gObj.transform.position;

            //Checks obstacles in 8 directions.
            CheckObstacle(origin,
                          Vector3.right,
                          gObj, 0, "Right",
                          new int[] { 2, 4 },
                          new int[] { 2, 3 });

            CheckObstacle(origin,
                          Vector3.forward,
                          gObj, 0, "Front",
                          new int[] { 0, 2 },
                          new int[] { 1, 2 });

            CheckObstacle(origin,
                          -Vector3.forward,
                          gObj, 0, "Back",
                          new int[] { 4, 2 },
                          new int[] { 3, 2 });

            CheckObstacle(origin,
                          -Vector3.right,
                          gObj, 0, "Left",
                          new int[] { 2, 0 },
                          new int[] { 2, 1 });

            CheckObstacle(origin,
                          Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward,
                          gObj, 45, "45 Degrees Front Right",
                          new int[] { 0, 4 },
                          new int[] { 1, 3 });


            CheckObstacle(origin,
                          Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward,
                          gObj, -45, "45 Degrees Front Left",
                          new int[] { 0, 0 },
                          new int[] { 1, 1 });

            CheckObstacle(origin,
                          Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.right,
                          gObj, 45, "45 Degrees Back Right",
                          new int[] { 4, 4 },
                          new int[] { 3, 3 });

            CheckObstacle(origin,
                          Quaternion.AngleAxis(-45, Vector3.up) * -gObj.transform.right,
                          gObj, -45, "45 Degrees Back Left",
                          new int[] { 4, 0 },
                          new int[] { 3, 1 });
        }
    }
}
