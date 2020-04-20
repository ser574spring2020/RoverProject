using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetParamters : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateParameters();
    }

    private void UpdateParameters()
    {
        GameObject.Find("AlgoButton").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("Algo");
        GameObject.Find("MazeButton").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("Maze");
        GameObject.Find("SizeButton").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("Size");
        GameObject.Find("SensorButton").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("Sensor");

    }
    // Update is called once per frame
    void Update()
    {
       
        
    }
}
