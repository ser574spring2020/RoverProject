using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sensors1;
using System;

public class SensorController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Cube;    
    private float Distance;
    private float oldTime;
    private float newTime;
    private float newVelocity;
    private float oldVelocity;

    [Header("Sensors")]
    public float sensorLength;
    public float sideSensorPos;
    public float frontSensorAngle = 45;
    private static int[,] proximityMatrix;
    private static int[,] rangeMatrix;
    private static int[,] lidarMatrix;
    private static int[,] radarMatrix;
    private static int[,] bumperMatrix;
    private static int sensorType;    

    private static Sensors sensor;


    public void setSensorType(int value)
    {
        sensorType = value;
    }

    private int getSensorType()
    {
        return sensorType;
    }

    private void testProximityMatrix(int[,] matrix)
    {
        Debug.Log("-- Printing Matrix : -- ");
        
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Debug.Log(matrix.GetValue(i,j));
            }
            Debug.Log("-- row done -- ");
        }


        Debug.Log("-- Printing Matrix done -- ");

    }

    void Start(){
         sensor = SensorFactory.getInstance(sensorType, Cube);
        //sensor = Sensors.getInstance(sensorType, Cube);
    }
    private void FixedUpdate()
    {
        //int[,] proximityMatrix = getMatrixFromProximitySensor(Cube);

        //int[,] rangeMatrix = getMatrixFromRangeSensor(Cube);

        //int[,] lidarMatrix = getMatrixFromLiDARSensor(Cube);

        //int[,] radarMatrix = getMatrixFromRadarSensor(Cube);

        //int[,] bumperMatrix = getMatrixFromBumperSensor(Cube);


        // this how you call our API component
        /*        
                Sensors1.Sensors sensor = new Sensors1.Sensors();
                Debug.Log(sensor.chooseSensor(getSensorType()));
                int[,] matrix = sensor.getSensorData(Cube);
                testProximityMatrix(matrix);
        */
    }

    // Update is called once per frame
    void Update()
    {
        changePosRover();
        sensor.update_Obstacles(Cube);
        int[,] matrix = sensor.get_Obstacle_Matrix();
        //int[,] matrix = Sensors.obstacle_matrix;
        Debug.Log("Getting matrix of Size:"+matrix.Length);
    }

    private float getAcceleration(Rigidbody cube)
    {
        newTime = Time.time;
        newVelocity = cube.velocity.magnitude;
        float acceleration = ((newVelocity - oldVelocity) / (newTime - oldTime));
        oldTime = newTime;
        oldVelocity = newVelocity;

        return acceleration;
    }


    private float getDataFromAccelerometer()
    {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Rigidbody cube = GameObject.Find("Cube").GetComponent<Rigidbody>();
        return getAcceleration(cube);
    }

    private void changePosRover()
    {
        float speed = 10f;
        if (Input.GetKey(KeyCode.W))
            Cube.transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.S))
            Cube.transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.A))
            Cube.transform.Rotate(0, -1, 0);

        if (Input.GetKey(KeyCode.D))
            Cube.transform.Rotate(0, 1, 0);
    }

    
    /*From here will be the implementations for diferent sensors functions
    <summary>
    This function sets lidar matrix which has a 5 X 5 dimensions
    It detects object only in front of it. It detects the distance 
    from the potential collision to rover        
    </summary>
    <param name="gObj">requires rover game object</param>
    <returns>Returns a 5X5 matrix of the surrounding of rover</returns>*/
    private int[,] getMatrixFromLiDARSensor(GameObject gObj)
    {
        // initial position for every step before checking for potential collisions
        // 5X5 matrix is taken for this sensor
        // -1 : don't know; setting distance of collision to object in range of (1f-2f)
        lidarMatrix = new int[,] {{ -1, -1, -1, -1, -1 }, 
                                  { -1, -1, -1, -1, -1 }, 
                                  { -1, -1,  2, -1, -1 }, 
                                  { -1, -1, -1, -1, -1 }, 
                                  { -1, -1, -1, -1, -1 }};

        checkObstacle(gObj.transform.position, 
                      Vector3.forward, 
                      gObj, 0, "Front", 
                      new int[] { 0, 2 }, 
                      new int[] { 1, 2 },
                      lidarMatrix);

        checkObstacle(gObj.transform.position, 
                      Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward, 
                      gObj, 45, "45 Degrees Front Right",
                      new int[] { 0, 4 }, 
                      new int[] { 1, 3 },
                      lidarMatrix);

        checkObstacle(gObj.transform.position, 
                      Quaternion.AngleAxis(25, Vector3.up) * gObj.transform.forward, 
                      gObj, 25, "25 Degrees Front Right",
                      new int[] { 0, 3 },
                      new int[] { 1, 3 },
                      lidarMatrix);

        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward,
                      gObj, -45, "45 Degrees Front Left",
                      new int[] { 0, 0 },
                      new int[] { 1, 1 },
                      lidarMatrix);

        checkObstacle(gObj.transform.position, 
                      Quaternion.AngleAxis(-25, Vector3.up) * gObj.transform.forward, 
                      gObj, -25, "25 Degrees Front Left",
                      new int[] { 0, 1 },
                      new int[] { 1, 1 },
                      lidarMatrix);

        return lidarMatrix;
    }



    /// <summary>
    /// This function sets lidar matrix which has a 5 X 5 dimensions
    /// It detects object only in front of it. It detects the distance 
    /// from the potential collision to rover        
    /// </summary>
    /// <param name="gObj">requires rover game object</param>
    /// <returns>Returns a 5X5 matrix of the surrounding of rover</returns>
    private int[,] getMatrixFromBumperSensor(GameObject gObj)
    {
        sensorLength = 1f;

        bumperMatrix = new int[,] {{ 0, 0, 0 },
                                   { 0, 2, 0 },
                                   { 0, 0, 0 }};


        checkObstacle(gObj.transform.position,
                      Vector3.forward,
                      gObj, 0, "Front",
                      new int[] { 0, 1 },
                      new int[] { 0, 1 },
                      bumperMatrix);

        checkObstacle(gObj.transform.position,
                      Vector3.right,
                      gObj, 0, "Right",
                      new int[] { 1, 2 },
                      new int[] { 1, 2 },
                      bumperMatrix);

        checkObstacle(gObj.transform.position,
                      -Vector3.right,
                      gObj, 0, "Left",
                      new int[] { 1, 0 },
                      new int[] { 1, 0 },
                      bumperMatrix);

        checkObstacle(gObj.transform.position,
                      -Vector3.forward,
                      gObj, 0, "Back",
                      new int[] { 2, 1 },
                      new int[] { 2, 1 },
                      bumperMatrix);


        // 45 Degree Angle Lines
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward,
                      gObj, 45, "45 Degrees Front Right",
                      new int[] { 0, 0 },
                      new int[] { 0, 0 },
                      bumperMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward,
                      gObj, -45, "45 Degrees Front Left",
                      new int[] { 0, 2 },
                      new int[] { 0, 2 },
                      bumperMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-45, Vector3.up) * (-gObj.transform.forward),
                      gObj, -45, "45 Degrees Back Left",
                      new int[] { 2, 2 },
                      new int[] { 2, 2 },
                      bumperMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(45, Vector3.up) * (-gObj.transform.forward),
                      gObj, 45, "45 Degrees Back Right",
                      new int[] { 2, 0 },
                      new int[] { 2, 0 },
                      bumperMatrix);


        return bumperMatrix;
    }

        // Draw circle on XZ plane
        private void DrawCircle(Vector3 position, float radius, Color color)
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
    private void DrawRotatingLine(GameObject gObj, Color red)
    {        
        Quaternion q = Quaternion.AngleAxis(100 * Time.time, Vector3.up);        
        Vector3 direction = q * gObj.transform.forward * sensorLength;
        Debug.DrawLine(gObj.transform.position,
        gObj.transform.position + direction);
        
    }

    /// <summary>
    /// This function sets radar matrix which has a 5 X 5 outer radius and 3x3 inner radius
    /// It detects object only in circle within given radius. It detects the distance 
    /// from the potential collision to rover        
    /// </summary>
    /// <param name="gObj">requires rover game object</param>
    /// <returns>Returns a 5X5 matrix of the surrounding of rover</returns>
    private int[,] getMatrixFromRadarSensor(GameObject gObj)
    {
        //.......change
        // initial position for every step before checking for potential collisions
        // 5X5 matrix is taken for this sensor
        // -1 : don't know; setting distance of collision to object in range of (1f-2f)
        sensorLength = 4f;
        DrawCircle(gObj.transform.position, sensorLength, Color.blue);
        DrawCircle(gObj.transform.position, sensorLength-2, Color.red);

        DrawRotatingLine(gObj, Color.red); 
        

        radarMatrix = new int[,] {{ -1, -1, -1, -1, -1 },
                                  { -1, -1, -1, -1, -1 },
                                  { -1, -1,  2, -1, -1 },
                                  { -1, -1, -1, -1, -1 },
                                  { -1, -1, -1, -1, -1 }};

        // Straight Lines
        checkObstacle(gObj.transform.position,
                      Vector3.forward,
                      gObj, 0, "Front",
                      new int[] { 0, 2 },
                      new int[] { 1, 2 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      -Vector3.forward,
                      gObj, 0, "Back",
                      new int[] { 4, 2 },
                      new int[] { 3, 2 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Vector3.right,
                      gObj, 0, "Right",
                      new int[] { 2, 4 },
                      new int[] { 2, 3 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      -Vector3.right,
                      gObj, 0, "Left",
                      new int[] { 2, 0 },
                      new int[] { 2, 1 },
                      radarMatrix);


        // 45 Degree Angle Lines
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward,
                      gObj, 45, "45 Degrees Front Right",
                      new int[] { 1, 3 },
                      new int[] { 1, 3 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward,
                      gObj, -45, "45 Degrees Front Left",
                      new int[] { 1, 1 },
                      new int[] { 1, 1 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-45, Vector3.up) * (-gObj.transform.forward),
                      gObj, -45, "45 Degrees Back Left",
                      new int[] { 3, 1 },
                      new int[] { 3, 1 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(45, Vector3.up) * (-gObj.transform.forward),
                      gObj, 45, "45 Degrees Back Right",
                      new int[] { 3, 3 },
                      new int[] { 3, 3 },
                      radarMatrix);


        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(25, Vector3.up) * gObj.transform.forward,
                      gObj, 25, "25 Degrees Front Right",
                      new int[] { 0, 3 },
                      new int[] { 0, 3 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-25, Vector3.up) * gObj.transform.forward,
                      gObj, -25, "25 Degrees Front Left",
                      new int[] { 0, 1 },
                      new int[] { 0, 1 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-25, Vector3.up) * gObj.transform.right,
                      gObj, -25, "25 Degrees Right Up",
                      new int[] { 1, 4 },
                      new int[] { 1, 4 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(25, Vector3.up) * gObj.transform.right,
                      gObj, 25, "25 Degrees Right Down",
                      new int[] { 3, 4 },
                      new int[] { 3, 4 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(25, Vector3.up) * (-gObj.transform.right),
                      gObj, 25, "25 Degrees left Up",
                      new int[] { 1, 0 },
                      new int[] { 1, 0 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-25, Vector3.up) * (-gObj.transform.right),
                      gObj, -25, "25 Degrees left Down",
                      new int[] { 3, 0 },
                      new int[] { 3, 0 },
                      radarMatrix);

        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(25, Vector3.up) * (-gObj.transform.forward),
                      gObj, -25, "25 Degrees Back Right",
                      new int[] { 4, 3 },
                      new int[] { 4, 3 },
                      radarMatrix);
        checkObstacle(gObj.transform.position,
                      Quaternion.AngleAxis(-25, Vector3.up) * (-gObj.transform.forward),
                      gObj, 25, "25 Degrees Back Left",
                      new int[] { 1, 3 },
                      new int[] { 1, 3 },
                      radarMatrix);

        return radarMatrix;
    }

    
    /// <summary>
    /// This function gets a range matrix which has a 5 X 5 dimensions
    /// It detects object at twice the range of proximity matrix        
    /// </summary>
    /// <param name="gObj">requires rover game object</param>
    /// <returns>Returns a 5X5 matrix of the surrounding of rover</returns>
    private int[,] getMatrixFromRangeSensor(GameObject gObj)
    {
        sensorLength = 4f;
        // initial position for every step before checking for potential collisions
        // 5X5 matrix is taken for this sensor
        rangeMatrix = new int[,] { { -1, -1, -1, -1, -1 }, 
                                   { -1, -1, -1, -1, -1 }, 
                                   { -1, -1,  2, -1, -1 }, 
                                   { -1, -1, -1, -1, -1 }, 
                                   { -1, -1, -1, -1, -1 }};
        Vector3 origin = gObj.transform.position;

        checkObstacle(origin,
                      Vector3.right,
                      gObj, 0, "Right",
                      new int[] { 2, 4 },
                      new int[] { 2, 3 },
                      rangeMatrix);

        checkObstacle(origin,
                      Vector3.forward,
                      gObj, 0, "Front",
                      new int[] { 0, 2 },
                      new int[] { 1, 2 },
                      rangeMatrix);

        checkObstacle(origin,
                      -Vector3.forward,
                      gObj, 0, "Back",
                      new int[] { 4, 2 },
                      new int[] { 3, 2 },
                      rangeMatrix);
        
        checkObstacle(origin,
                      -Vector3.right,
                      gObj, 0, "Left",
                      new int[] { 2, 0 },
                      new int[] { 2, 1 },
                      rangeMatrix);

        checkObstacle(origin,
                      Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward,
                      gObj, 45, "45 Degrees Front Right",
                      new int[] { 0, 4 },
                      new int[] { 1, 3 },
                      rangeMatrix);

        
        checkObstacle(origin,
                      Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward,
                      gObj, -45, "45 Degrees Front Left",
                      new int[] { 0, 0 },
                      new int[] { 1, 1 },
                      rangeMatrix);

        checkObstacle(origin,
                      Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.right,
                      gObj, 45, "45 Degrees Back Right",
                      new int[] { 4, 4 },
                      new int[] { 3, 3 },
                      rangeMatrix);

        checkObstacle(origin,
                      Quaternion.AngleAxis(-45, Vector3.up) * -gObj.transform.right,
                      gObj, -45,"45 Degrees Back Left",
                      new int[] { 4, 0 }, 
                      new int[] { 3, 1 },
                      rangeMatrix);

        return rangeMatrix;
    }

    /// <summary>
    /// This function calculates the 3x3 proximity matrix by finding potential obstacles
    /// in four directions
    /// </summary>
    /// <param name="gObj"> requires rover game object</param>
    /// <returns>returns a 2d 3X3 matrix which consists of obstacles set in it</returns>
    private int[,] getMatrixFromProximitySensor(GameObject gObj)
    {
        sensorLength = 2f;
        // initial position for every step before checking for potential collisions
        // 3X3 matrix is passed here
        proximityMatrix = new int[,] {{ 1, 0, 1 }, 
                                      { 0, 2, 0 }, 
                                      { 1, 0, 1 }};

        
        checkObstacle(gObj.transform.position,
                      Vector3.forward,
                      gObj, 0, "Front",
                      new int[] { 0, 1 },
                      new int[] { 0, 1 },
                      proximityMatrix);

        checkObstacle(gObj.transform.position,
                      Vector3.right,
                      gObj, 0, "Right",
                      new int[] { 1, 2},
                      new int[] { 1, 2},
                      proximityMatrix);

        checkObstacle(gObj.transform.position,
                      -Vector3.right,
                      gObj, 0, "Left",
                      new int[] { 1, 0 },
                      new int[] { 1, 0 },
                      proximityMatrix);

        checkObstacle(gObj.transform.position,
                      -Vector3.forward,
                      gObj, 0, "Back",
                      new int[] { 2, 1 },
                      new int[] { 2, 1 },
                      proximityMatrix);

        return proximityMatrix;
    }

    /// <summary>
    /// This is a helper function to check Obstacle in the direction given
    /// by using a concept called Raycasting
    /// </summary>
    /// <params>
    /// requires rover game object, direction of ray, origin of gameobject, String which is displayed on console
    /// and the indexes which are set with that particular ray
    /// </params>
    /// <returns></returns>
    private void checkObstacle(Vector3 origin, Vector3 direction, GameObject gObj, 
                               int angle, String obstacleToBeFound, int[] outRangeIndexes, 
                               int[] inRangeIndexes, int[,] resultantMatrix)
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
            Debug.Log("found " + obstacleToBeFound + " obstacle");

            // for Proximity, Bumper
            if (getSensorType() == 1 || getSensorType() == 5)
            {
                drawRayOnRover(ray, hit, "");
                resultantMatrix[outRangeIndexes[0], outRangeIndexes[1]] = result;
            }
            else
            {                
                // for LiDAR and Radar
                if (getSensorType() == 3 || getSensorType() == 4) result = (int)hit.distance;

                // for LiDAR, Radar, Range
                if (hit.distance > 2.0f)
                {                    
                    drawRayOnRover(ray, hit, "out");
                    resultantMatrix[outRangeIndexes[0], outRangeIndexes[1]] = result;                    
                }
                else
                {
                    
                    drawRayOnRover(ray, hit, "in");
                    resultantMatrix[inRangeIndexes[0], inRangeIndexes[1]] = result;
                }
            }
        }
        
    }

    
    /// <summary>
    /// This is a helper function to draw a ray
    /// </summary>
    /// <param>
    /// requires ray, hit RaycastHit object and String for distinguishing range   
    /// </param>
    /// <returns></returns>
    private void drawRayOnRover(Ray ray, RaycastHit hit, String range)
    {
        if (getSensorType() == 3) // LiDAR
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
        else if (getSensorType() == 4) // Radar
        {
            // no ray required to show. 
        }
        else if (getSensorType() == 2) // Range
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
        else if (getSensorType() == 5) // Bumper
        {
            // no ray required to show.
        }
        else  // Proximity
        {
            Debug.DrawLine(ray.origin, hit.point);
        }

    }
    
    /*private void highlightCollider(GameObject gObj)
    {
        Renderer r = gObj.GetComponent(typeof(Renderer)) as Renderer;
        oldColor = r.material.GetColor("_Color");
        Color newColor = new Color(oldColor.r + 0.2f, oldColor.g + 0.2f, oldColor.b + 0.2f, oldColor.a);
        r.material.SetColor("_Color", newColor);
    }

    private void restoreColor(GameObject gObj)
    {
        Renderer r = gObj.GetComponent(typeof(Renderer)) as Renderer;
        r.material.SetColor("_Color", oldColor);
    }*/
}
