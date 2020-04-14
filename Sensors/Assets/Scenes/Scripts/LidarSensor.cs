/*
    Author   : Aneesh Dalvi
    Function : Implements Functionality for Lidar Sensor
    Version  : V1
    Email    : adalvi1@asu.edu | Arizona State University.
*/

using UnityEngine;
class LidarSensor : Sensors
{
    public LidarSensor(){
        sensorLength = 5f;
        obstacle_matrix = new int[,] {{ -1, -1, -1, -1, -1 }, 
                                  { -1, -1, -1, -1, -1 }, 
                                  { -1, -1,  2, -1, -1 }, 
                                  { -1, -1, -1, -1, -1 }, 
                                  { -1, -1, -1, -1, -1 }};
    }
    public override void update_Obstacles(GameObject gObj){
        /* initial position for every step before checking for potential collisions
            5X5 matrix is taken for this sensor
            -1 : don't know; setting distance of collision to object in range of (1f-2f) */

        checkObstacle(gObj.transform.position, 
                      Vector3.forward, 
                      gObj, 0, "Front", 
                      new int[] { 0, 2 }, 
                      new int[] { 1, 2 });

        checkObstacle(gObj.transform.position, 
                      Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward, 
                      gObj, 45, "45 Degrees Front Right",
                      new int[] { 0, 4 }, 
                      new int[] { 1, 3 });

        checkObstacle(gObj.transform.position, 
                      Quaternion.AngleAxis(25, Vector3.up) * gObj.transform.forward, 
                      gObj, 25, "25 Degrees Front Right",
                      new int[] { 0, 3 },
                      new int[] { 1, 3 });

        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward,
                      gObj, -45, "45 Degrees Front Left",
                      new int[] { 0, 0 },
                      new int[] { 1, 1 });

        checkObstacle(gObj.transform.position, 
                      Quaternion.AngleAxis(-25, Vector3.up) * gObj.transform.forward, 
                      gObj, -25, "25 Degrees Front Left",
                      new int[] { 0, 1 },
                      new int[] { 1, 1 });
     }
}