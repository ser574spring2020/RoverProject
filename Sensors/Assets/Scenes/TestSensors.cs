using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sensors1;
using System;
using UnityEngine.SceneManagement;

public class TestSensors : MonoBehaviour
{

    public Button play;
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {

        play.onClick.AddListener(callSensorLibrary);
    }

    public void callSensorLibrary()
    {
        int SensorType = int.Parse(input.text);
        Sensors1.Sensors sensor = new Sensors1.Sensors();
        Debug.Log(sensor.chooseSensor(SensorType));
        SceneManager.LoadScene("SensorTest");
    }




}

