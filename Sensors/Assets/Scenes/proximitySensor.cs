using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximitySensor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Cube;
    public GameObject Cube1;
    public float Distance;
    private float oldTime;
    private float newTime;
    private float newVelocity;
    private float oldVelocity;
    void Start()
    {

        
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

    // Update is called once per frame
    void Update()
    {
        float speed = 10f;
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Rigidbody cube = GameObject.Find("Cube").GetComponent<Rigidbody>();        
        float acceleration = getAcceleration(cube);

        Debug.Log(string.Format("Acceration of {0} is {1}", Cube, acceleration));

        if (Input.GetKey(KeyCode.W))
            Cube.transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.S))
            Cube.transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.A))
            Cube.transform.Rotate(0, -1, 0);

        if (Input.GetKey(KeyCode.D))
            Cube.transform.Rotate(0, 1, 0);

        if (Cube != null && Cube1 != null)
        {
            Distance = Vector3.Distance(Cube.transform.position, Cube1.transform.position);
            Debug.Log(string.Format("Distance between {0} and {1} is: {2}", Cube, Cube1, Distance));            
        }
        
    }
}
