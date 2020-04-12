using UnityEngine;
class ProximitySensor : Sensors
{
    public override void update_Obstacles(GameObject gObj){
        this.obstacle_matrix = new int[,] {{ 1, 0, 1 }, 
                                      { 0, 2, 0 }, 
                                      { 1, 0, 1 }};

        RaycastHit hit;
        Vector3 origin = gObj.transform.position;

        //Checks obstacles in 4 directions.
        checkObstacle(gObj.transform.position,
                      Vector3.forward,
                      gObj, 0, "Front",
                      new int[] { 0, 1 },
                      new int[] { 0, 1 },
                      this.obstacle_matrix);

        checkObstacle(gObj.transform.position,
                      Vector3.right,
                      gObj, 0, "Right",
                      new int[] { 1, 2},
                      new int[] { 1, 2},
                      this.obstacle_matrix);

        checkObstacle(gObj.transform.position,
                      -Vector3.right,
                      gObj, 0, "Left",
                      new int[] { 1, 0 },
                      new int[] { 1, 0 },
                      this.obstacle_matrix);

        checkObstacle(gObj.transform.position,
                      -Vector3.forward,
                      gObj, 0, "Back",
                      new int[] { 2, 1 },
                      new int[] { 2, 1 },
                      this.obstacle_matrix);
    }
}