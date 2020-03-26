using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximitySensor : MonoBehaviour
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
    public float sensorLength = 3f;
    //public float frontSensorPos = 0.5f;
    public float sideSensorPos = 0.2f;
    public float frontSensorAngle = 45;
    private static int[,] proximityMatrix;

    void Start()
    {
        proximityMatrix = new int[,] { { 1, 0, 1 }, { 0, -1, 0 }, { 1, 0, 1 } };

    } 

    private int[,] getProximityMatrixFromSensor()
    {
        RaycastHit hit;

        Ray leftRay = new Ray(Cube.transform.position, Cube.transform.TransformDirection(transform.forward));        

        if (Physics.Raycast(leftRay, out hit, sensorLength))
        {
            Debug.Log("found forward obstacle");
            Debug.DrawLine(leftRay.origin, hit.point);
            proximityMatrix[1, 0] = 1;
            testProximityMatrix(proximityMatrix);
        }
        else
        {
            proximityMatrix[1, 0] = 0;            
        }


        Ray frontRay = new Ray(Cube.transform.position, Cube.transform.TransformDirection(transform.right));
        if (Physics.Raycast(frontRay, out hit, sensorLength))
        {
            Debug.Log("found front obstacle");
            Debug.DrawLine(frontRay.origin, hit.point);
            proximityMatrix[0, 1] = 1;
            testProximityMatrix(proximityMatrix);
        }
        else
        {
            proximityMatrix[0, 1] = 0;            
        }

        var direction = Quaternion.AngleAxis(45, transform.up) * transform.forward;
        Ray frontleftRay = new Ray(Cube.transform.position, Cube.transform.TransformDirection(direction));
        if (Physics.Raycast(frontleftRay, out hit, sensorLength))
        {
            Debug.Log("found frontleftRay obstacle");
            Debug.DrawLine(frontleftRay.origin, hit.point);
            //proximityMatrix[0, 1] = 1;
            //testProximityMatrix(proximityMatrix);
        }
        else
        {
            //proximityMatrix[0, 1] = 0;
        }


        Ray backRay = new Ray(Cube.transform.position, Cube.transform.TransformDirection(-transform.right));
        if (Physics.Raycast(backRay, out hit, sensorLength))
        {
            Debug.Log("found back obstacle");
            Debug.DrawLine(backRay.origin, hit.point);
            proximityMatrix[2, 1] = 1;
            testProximityMatrix(proximityMatrix);
        }
        else
        {
            proximityMatrix[2, 1] = 0;            
        }


        Ray rightRay = new Ray(Cube.transform.position, Cube.transform.TransformDirection(-transform.forward));
       if (Physics.Raycast(rightRay, out hit, sensorLength))
       {
            Debug.Log("found right obstacle");            
           Debug.DrawLine(rightRay.origin, hit.point);
            proximityMatrix[1, 2] = 1;
            testProximityMatrix(proximityMatrix);
        }
        else
        {
            proximityMatrix[1, 2] = 0;
            
        }

        // print matrix
        //testProximityMatrix(proximityMatrix);

        return proximityMatrix;
       
    }

    private void testProximityMatrix(int[,] matrix)
    {

        
         foreach(var eachrow in matrix)
        {
            Debug.Log(eachrow);
        }

    }

    private void FixedUpdate()
    {
        getProximityMatrixFromSensor();
    }


    // Update is called once per frame
    void Update()
    {

        // changes the position of Rover
        changePosRover();


        // calculate acceleration of the gameobject
        float acceleration = getDataFromAccelerometer();       
        Debug.Log(string.Format("Acceration of {0} is {1}", Cube, acceleration));
        

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
}
