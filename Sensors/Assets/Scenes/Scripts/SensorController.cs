using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sensors1;
using System;

public class SensorController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Cube;
    public GameObject Cube1;
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
    private static int sensorType;

    void Start()
    {
        
    }

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

    private void FixedUpdate()
    {
        //int[,] proximityMatrix = getMatrixFromProximitySensor(Cube);

        //int[,] rangeMatrix = getMatrixFromRangeSensor(Cube);

        //int[,] rangeMatrix = getMatrixFromLiDARSensor(Cube);
        //testProximityMatrix(rangeMatrix);

        // this how you call our API component
        Sensors1.Sensors sensor = new Sensors1.Sensors();
        Debug.Log(sensor.chooseSensor(getSensorType()));
        int[,] matrix = sensor.getSensorData(Cube);
        testProximityMatrix(matrix);

    }

    


    // Update is called once per frame
    void Update()
    {

        
        // changes the position of Rover
        changePosRover();


        // calculate acceleration of the gameobject
        float acceleration = getDataFromAccelerometer();       
        //Debug.Log(string.Format("Acceration of {0} is {1}", Cube, acceleration));
        

        //proximity sensor
        if (Cube != null && Cube1 != null)
        {
            Distance = getDataFromProximitySensor();
            //Debug.Log(string.Format("Distance between {0} and {1} is: {2}", Cube, Cube1, Distance));
        }

    }

    

    private float getDataFromProximitySensor()
    { 
          return Vector3.Distance(Cube.transform.position, Cube1.transform.position);
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


    // from here will be the implementations for diferent sensors functions


    /// <summary>
    /// This function sets lidar matrix which has a 5 X 5 dimensions
    /// It detects object only in front of it. It detects the distance 
    /// from the potential collision to rover        
    /// </summary>
    /// <param name="gObj">requires rover game object</param>
    /// <returns>Returns a 5X5 matrix of the surrounding of rover</returns>
    private int[,] getMatrixFromLiDARSensor(GameObject gObj)
    {
        // initial position for every step before checking for potential collisions
        // 5X5 matrix is taken for this sensor
        // -1 : don't know; setting distance of collision to object in range of (1f-2f)
        lidarMatrix = new int[,] { { -1, -1, -1, -1, -1 }, { -1, -1, -1, -1, -1 }, { -1, -1, 2, -1, -1 }, { -1, -1, -1, -1, -1 }, { -1, -1, -1, -1, -1 } };

        RaycastHit hit;
        Vector3 origin = gObj.transform.position;


        Ray frontRay = new Ray(origin, gObj.transform.TransformDirection(Vector3.forward));
        if (Physics.Raycast(frontRay, out hit, sensorLength))
        {
            Debug.Log("found front obstacle");


            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(frontRay.origin, hit.point, Color.green);
                lidarMatrix[0, 2] = (int)hit.distance;
            }
            else
            {
                Debug.DrawLine(frontRay.origin, hit.point, Color.red);
                lidarMatrix[1, 2] = (int)hit.distance;
            }
        }


        // for angled degrees directions 
        var right_45_direction = Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward;
        Ray frontRight_45_Ray = new Ray(origin, right_45_direction);
        if (Physics.Raycast(frontRight_45_Ray, out hit, sensorLength))
        {
            Debug.Log("found frontRightRay obstacle at 45 deg");

            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(frontRight_45_Ray.origin, hit.point, Color.green);
                lidarMatrix[0, 4] = (int)hit.distance;
            }
            else
            {
                Debug.DrawLine(frontRight_45_Ray.origin, hit.point, Color.red);
                lidarMatrix[1, 3] = (int)hit.distance;
            }
        }

        var right_25_direction = Quaternion.AngleAxis(25, Vector3.up) * gObj.transform.forward;
        Ray frontRight_25_Ray = new Ray(origin, right_25_direction);
        if (Physics.Raycast(frontRight_25_Ray, out hit, sensorLength))
        {
            Debug.Log("found frontRightRay obstacle at 25 deg");

            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(frontRight_25_Ray.origin, hit.point, Color.green);
                lidarMatrix[0, 4] = (int)hit.distance;
            }
            else
            {
                Debug.DrawLine(frontRight_25_Ray.origin, hit.point, Color.red);
                lidarMatrix[1, 3] = (int)hit.distance;
            }
        }

        var left_45_direction = Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward;
        Ray frontLeft_45_Ray = new Ray(origin, left_45_direction);
        if (Physics.Raycast(frontLeft_45_Ray, out hit, sensorLength))
        {
            Debug.Log("found frontLeftRay obstacle at 45 deg");

            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(frontLeft_45_Ray.origin, hit.point, Color.green);
                lidarMatrix[0, 0] = (int)hit.distance;
            }
            else
            {
                Debug.DrawLine(frontLeft_45_Ray.origin, hit.point, Color.red);
                lidarMatrix[1, 1] = (int)hit.distance;
            }
        }

        var left_25_direction = Quaternion.AngleAxis(-25, Vector3.up) * gObj.transform.forward;
        Ray frontLeft_25_Ray = new Ray(origin, left_25_direction);
        if (Physics.Raycast(frontLeft_25_Ray, out hit, sensorLength))
        {
            Debug.Log("found frontLeftRay obstacle at 25 deg");

            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(frontLeft_25_Ray.origin, hit.point, Color.green);
                lidarMatrix[0, 0] = (int)hit.distance;
            }
            else
            {
                Debug.DrawLine(frontLeft_25_Ray.origin, hit.point, Color.red);
                lidarMatrix[1, 1] = (int)hit.distance;
            }
        }

        return lidarMatrix;
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
        rangeMatrix = new int[,] { { -1, -1, -1, -1, -1 }, { -1, -1, -1, -1, -1 }, { -1, -1, 2, -1, -1 }, { -1, -1, -1, -1, -1 }, { -1, -1, -1, -1, -1 } };

        RaycastHit hit;
        Vector3 origin = gObj.transform.position;

        Ray rightRay = new Ray(origin, gObj.transform.TransformDirection(Vector3.right));
        if (Physics.Raycast(rightRay, out hit, sensorLength))
        {
            Debug.Log("found right obstacle");

            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(rightRay.origin, hit.point);
                rangeMatrix[2, 4] = 1;
            }
            else
            {
                Debug.DrawLine(rightRay.origin, hit.point, Color.red);
                rangeMatrix[2, 3] = 1;
            }
        }

        Ray frontRay = new Ray(origin, gObj.transform.TransformDirection(Vector3.forward));
        if (Physics.Raycast(frontRay, out hit, sensorLength))
        {
            Debug.Log("found front obstacle");


            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(frontRay.origin, hit.point);
                rangeMatrix[0, 2] = 1;
            }
            else
            {
                Debug.DrawLine(frontRay.origin, hit.point, Color.red);
                rangeMatrix[1, 2] = 1;
            }
        }

        Ray backRay = new Ray(origin, gObj.transform.TransformDirection(-Vector3.forward));
        if (Physics.Raycast(backRay, out hit, sensorLength))
        {
            Debug.Log("found back obstacle");


            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(backRay.origin, hit.point);
                rangeMatrix[4, 2] = 1;
            }
            else
            {
                Debug.DrawLine(backRay.origin, hit.point, Color.red);
                rangeMatrix[3, 2] = 1;
            }
        }


        Ray leftRay = new Ray(origin, gObj.transform.TransformDirection(-Vector3.right));
        if (Physics.Raycast(leftRay, out hit, sensorLength))
        {
            Debug.Log("found left obstacle");


            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(leftRay.origin, hit.point);
                rangeMatrix[2, 0] = 1;
            }
            else
            {
                Debug.DrawLine(leftRay.origin, hit.point, Color.red);
                rangeMatrix[2, 1] = 1;
            }
        }

        // for four 45 degrees directions 
        var rightdirection = Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.forward;
        Ray frontRightRay = new Ray(origin, rightdirection);
        if (Physics.Raycast(frontRightRay, out hit, sensorLength))
        {
            Debug.Log("found frontRightRay obstacle");

            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(frontRightRay.origin, hit.point);
                rangeMatrix[0, 4] = 1;
            }
            else
            {
                Debug.DrawLine(frontRightRay.origin, hit.point, Color.red);
                rangeMatrix[1, 3] = 1;
            }
        }

        var leftdirection = Quaternion.AngleAxis(-45, Vector3.up) * gObj.transform.forward;
        Ray frontLeftRay = new Ray(origin, leftdirection);
        if (Physics.Raycast(frontLeftRay, out hit, sensorLength))
        {
            Debug.Log("found frontLeftRay obstacle");

            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(frontLeftRay.origin, hit.point);
                rangeMatrix[0, 0] = 1;
            }
            else
            {
                Debug.DrawLine(frontLeftRay.origin, hit.point, Color.red);
                rangeMatrix[1, 1] = 1;
            }
        }

        var backrightdirection = Quaternion.AngleAxis(45, Vector3.up) * gObj.transform.right;
        Ray backRightRay = new Ray(origin, backrightdirection);

        if (Physics.Raycast(backRightRay, out hit, sensorLength))
        {
            Debug.Log("found backRightRay obstacle");

            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(backRightRay.origin, hit.point);
                rangeMatrix[4, 4] = 1;
            }
            else
            {
                Debug.DrawLine(backRightRay.origin, hit.point, Color.red);
                rangeMatrix[3, 3] = 1;
            }
        }

        var backleftdirection = Quaternion.AngleAxis(-45, Vector3.up) * -gObj.transform.right;
        Ray backLeftRay = new Ray(origin, backleftdirection);

        if (Physics.Raycast(backLeftRay, out hit, sensorLength))
        {
            Debug.Log("found backLeftRay obstacle");

            if (hit.distance > 2.0f)
            {
                Debug.DrawLine(backLeftRay.origin, hit.point);
                rangeMatrix[4, 0] = 1;
            }
            else
            {
                Debug.DrawLine(backLeftRay.origin, hit.point, Color.red);
                rangeMatrix[3, 1] = 1;
            }
        }

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
        sensorLength = 1.5f;
        // initial position for every step before checking for potential collisions
        // 3X3 matrix is passed here
        proximityMatrix = new int[,] { { 1, 0, 1 }, { 0, 2, 0 }, { 1, 0, 1 } };

        RaycastHit hit;
        Vector3 origin = gObj.transform.position;

        Debug.DrawLine(origin, origin + Vector3.forward * sensorLength, Color.red);
        Ray frontRay = new Ray(origin, gObj.transform.TransformDirection(Vector3.forward));
        if (Physics.Raycast(frontRay, out hit, sensorLength))
        {
            //Debug.Log("found forward obstacle" + leftRay.origin + " " + hit.point);
            Debug.Log("found front obstacle");
            Debug.DrawLine(frontRay.origin, hit.point);
            proximityMatrix[1, 0] = 1;
        }
        else
        {
            proximityMatrix[1, 0] = 0;
        }

        Debug.DrawLine(origin, origin + Vector3.right * sensorLength, Color.red);
        Ray rightRay = new Ray(origin, gObj.transform.TransformDirection(Vector3.right));
        if (Physics.Raycast(rightRay, out hit, sensorLength))
        {
            Debug.Log("found right obstacle");
            Debug.DrawLine(rightRay.origin, hit.point);
            proximityMatrix[0, 1] = 1;
        }
        else
        {
            proximityMatrix[0, 1] = 0;
        }

        Debug.DrawLine(origin, origin + (-Vector3.right) * sensorLength, Color.red);
        Ray leftRay = new Ray(origin, gObj.transform.TransformDirection(-Vector3.right));
        if (Physics.Raycast(leftRay, out hit, sensorLength))
        {
            Debug.Log("found left obstacle");
            Debug.DrawLine(leftRay.origin, hit.point);
            proximityMatrix[2, 1] = 1;
        }
        else
        {
            proximityMatrix[2, 1] = 0;
        }

        Debug.DrawLine(origin, origin + (-Vector3.forward) * sensorLength, Color.red);
        Ray backRay = new Ray(origin, gObj.transform.TransformDirection(-Vector3.forward));
        if (Physics.Raycast(backRay, out hit, sensorLength))
        {
            Debug.Log("found right obstacle");
            Debug.DrawLine(backRay.origin, hit.point);
            proximityMatrix[1, 2] = 1;
        }
        else
        {
            proximityMatrix[1, 2] = 0;
        }

        return proximityMatrix;

    }




}
