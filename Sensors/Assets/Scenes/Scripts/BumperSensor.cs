/*
    Author   : Aneesh Dalvi
    Function : Implements Functionality for Bumper Sensor
    Version  : V1
    Email    : adalvi1@asu.edu | Arizona State University.
*/

using UnityEngine;
public class BumperSensor : Sensors
{
    // Start is called before the first frame update
    public BumperSensor()
    {
        sensorLength = 1f;
        obstacle_matrix = new int[,] {{ 0, 0, 0 },
                                   { 0, 2, 0 },
                                   { 0, 0, 0 }};
    }

    public override void update_Obstacles(GameObject gObj)
    {
        checkObstacle(gObj.transform.position,
                      Vector3.forward,
                      gObj, 0, "Front",
                      new int[] { 0, 1 },
                      new int[] { 0, 1 }
                      );

        checkObstacle(gObj.transform.position,
                      Vector3.right,
                      gObj, 0, "Right",
                      new int[] { 1, 2 },
                      new int[] { 1, 2 }
                      );

        checkObstacle(gObj.transform.position,
                      -Vector3.right,
                      gObj, 0, "Left",
                      new int[] { 1, 0 },
                      new int[] { 1, 0 }
                      );

        checkObstacle(gObj.transform.position,
                      -Vector3.forward,
                      gObj, 0, "Back",
                      new int[] { 2, 1 },
                      new int[] { 2, 1 }
                      );


        // 45 Degree Angle Lines
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward,
                      gObj, 45, "45 Degrees Front Right",
                      new int[] { 0, 0 },
                      new int[] { 0, 0 }
                      );
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward,
                      gObj, -45, "45 Degrees Front Left",
                      new int[] { 0, 2 },
                      new int[] { 0, 2 }
                      );
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-45, Vector3.up) * (-gObj.transform.forward),
                      gObj, -45, "45 Degrees Back Left",
                      new int[] { 2, 2 },
                      new int[] { 2, 2 }
                      );
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(45, Vector3.up) * (-gObj.transform.forward),
                      gObj, 45, "45 Degrees Back Right",
                      new int[] { 2, 0 },
                      new int[] { 2, 0 }
                      );
    }
}
