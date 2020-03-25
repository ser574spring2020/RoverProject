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
    void Start()
    {

        
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
            Debug.Log(string.Format("Distance between {0} and {1} is: {2}", Cube, Cube1, Distance));
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
