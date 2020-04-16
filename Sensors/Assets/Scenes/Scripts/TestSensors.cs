/*
    Author   : Aneesh Dalvi, Sumanth Paranjape
    Function : Implements Functionality for Proximity Sensor
    Version  : V1
    Email    : adalvi1@asu.edu | Arizona State University.
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is associated with the User Interface dropdown
/// and chooses the sensor and passes on to the Controller class.
/// </summary>
public class TestSensors : MonoBehaviour
{

    public Button play;
    public Dropdown sensorInput;

    // Start is called before the first frame update
    void Start()
    {
        play.onClick.AddListener(callSensorLibrary);
    }

   
    public void callSensorLibrary()
    {        
        int val = sensorInput.value;
        int sensorType = val + 1;
        Debug.Log("Chosen value from drop down : "+sensorType);

        SensorController sc = new SensorController();
        sc.setSensorType(sensorType);        
        SceneManager.LoadScene("SensorTest");
    }
}

