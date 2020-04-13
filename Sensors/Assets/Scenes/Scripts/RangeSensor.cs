
/*
    Author   : Aneesh Dalvi
    Function : Implements Functionality for Range Sensor
    Version  : V1
    Email    : adalvi1@asu.edu | Arizona State University.
*/

using UnityEngine;
class RangeSensor : Sensors
{
    public override void update_Obstacles(GameObject gObj){
        sensorLength = 4f;
        // initial position for every step before checking for potential collisions
        // 5X5 matrix is taken for this sensor
        this.obstacle_matrix = new int[,] { { -1, -1, -1, -1, -1 }, 
                                   { -1, -1, -1, -1, -1 }, 
                                   { -1, -1,  2, -1, -1 }, 
                                   { -1, -1, -1, -1, -1 }, 
                                   { -1, -1, -1, -1, -1 }};
        Vector3 origin = gObj.transform.position;

        //Checks obstacles in 8 directions.
        checkObstacle(origin,
                      Vector3.right,
                      gObj, 0, "Right",
                      new int[] { 2, 4 },
                      new int[] { 2, 3 },
                      this.obstacle_matrix);

        checkObstacle(origin,
                      Vector3.forward,
                      gObj, 0, "Front",
                      new int[] { 0, 2 },
                      new int[] { 1, 2 },
                      this.obstacle_matrix);

        checkObstacle(origin,
                      -Vector3.forward,
                      gObj, 0, "Back",
                      new int[] { 4, 2 },
                      new int[] { 3, 2 },
                      this.obstacle_matrix);
        
        checkObstacle(origin,
                      -Vector3.right,
                      gObj, 0, "Left",
                      new int[] { 2, 0 },
                      new int[] { 2, 1 },
                      this.obstacle_matrix);

        checkObstacle(origin,
                      Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward,
                      gObj, 45, "45 Degrees Front Right",
                      new int[] { 0, 4 },
                      new int[] { 1, 3 },
                      this.obstacle_matrix);

        
        checkObstacle(origin,
                      Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward,
                      gObj, -45, "45 Degrees Front Left",
                      new int[] { 0, 0 },
                      new int[] { 1, 1 },
                      this.obstacle_matrix);

        checkObstacle(origin,
                      Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.right,
                      gObj, 45, "45 Degrees Back Right",
                      new int[] { 4, 4 },
                      new int[] { 3, 3 },
                      this.obstacle_matrix);

        checkObstacle(origin,
                      Quaternion.AngleAxis(-45, Vector3.up) * -gObj.transform.right,
                      gObj, -45,"45 Degrees Back Left",
                      new int[] { 4, 0 }, 
                      new int[] { 3, 1 },
                      this.obstacle_matrix);
    }
}