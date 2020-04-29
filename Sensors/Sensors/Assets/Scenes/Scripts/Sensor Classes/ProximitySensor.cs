/*
    Author   : Aneesh Dalvi, Sumanth Paranjape
    Function : Implements Functionality for Proximity Sensor
    Version  : V1
    Email    : adalvi1@asu.edu | Arizona State University.
*/

using UnityEngine;

/// <summary>
/// This class implements functionality for Proximity Sensor
/// </summary>
class ProximitySensor : Sensors
{
    public ProximitySensor(){
        obstacle_matrix = new int[,] {{ 1, 0, 1 }, 
                                      { 0, 2, 0 }, 
                                      { 1, 0, 1 }};
        sensorLength = 2f;
    }
    public override void Update_Obstacles(GameObject gObj){

        //Checks obstacles in 4 directions.
        CheckObstacle(gObj.transform.position,
                      Vector3.forward,
                      gObj, 0, "Front",
                      new int[] { 0, 1 },
                      new int[] { 0, 1 });

        CheckObstacle(gObj.transform.position,
                      Vector3.right,
                      gObj, 0, "Right",
                      new int[] { 1, 2},
                      new int[] { 1, 2});

        CheckObstacle(gObj.transform.position,
                      -Vector3.right,
                      gObj, 0, "Left",
                      new int[] { 1, 0 },
                      new int[] { 1, 0 });

        CheckObstacle(gObj.transform.position,
                      -Vector3.forward,
                      gObj, 0, "Back",
                      new int[] { 2, 1 },
                      new int[] { 2, 1 });
    }
}