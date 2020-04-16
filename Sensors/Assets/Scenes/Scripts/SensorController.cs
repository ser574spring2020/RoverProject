/*
    Author   : Aneesh Dalvi, Sumanth Paranjape
    Function : Implements Functionality for Proximity Sensor
    Version  : V1
    Email    : adalvi1@asu.edu | Arizona State University.
*/

using System;
using UnityEngine;

/// <summary>
/// This class is the controller class for all the sensors.
/// </summary>

public class SensorController : MonoBehaviour
{
    // for dll; for normal execution comment this
    private static SensorsComponent.Sensors sensor;

    // for normal unity execution uncomment this
    //private static Sensors sensor;

    private static int sensorType;
    public GameObject Cube;
    void Start() {
        sensor = SensorsComponent.SensorFactory.GetInstance(getSensorType(), Cube);
        Debug.Log(sensor.GetCurrentSensor());
        
    }

    private void testProximityMatrix(int[,] matrix)
    {
        {
            Debug.Log("Printing Matrix : -- ");             
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Debug.Log(string.Format("{0} ", matrix[i, j]));
                }                
                Debug.Log("-- row done -- ");
            }            
        }
    }

        // Update is called once per frame
        void Update()
        {
        changePosRover();
        sensor.Update_Obstacles(Cube);
        int[,] matrix = sensor.Get_Obstacle_Matrix();
        testProximityMatrix(matrix);

        //Debug.Log("Getting matrix of Size:"+matrix.Length);

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

        public void setSensorType(int value)
        {
            sensorType = value;
        }

        private int getSensorType()
        {
            return sensorType;
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

