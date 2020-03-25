using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class updatemessage : MonoBehaviour
{
	public TMPro.TMP_Dropdown InputAlgorithm;
	public TMPro.TMP_Dropdown MazeType;
	public TMPro.TMP_Dropdown MazeSize;
	public TMPro.TMP_Dropdown SensorType;
	public Text Result;
	
    // Start is called before the first frame update
    public void updateText()
    {
	InputAlgorithm = GameObject.Find("Algorithm").GetComponent<TMPro.TMP_Dropdown>();
    string InputAlgorithmValue = InputAlgorithm.captionText.text;
	MazeType = GameObject.Find("MazeType").GetComponent<TMPro.TMP_Dropdown>();
    string MazeTypeValue = MazeType.captionText.text;
	MazeSize = GameObject.Find("MazeSize").GetComponent<TMPro.TMP_Dropdown>();
    string MazeSizeValue = MazeSize.captionText.text;
	SensorType = GameObject.Find("SensorType").GetComponent<TMPro.TMP_Dropdown>();
    string SensorTypeValue = SensorType.captionText.text;
	// result  =  GameObject.Find("Status_text").GetComponent<InputField>();
	string updatedmessage="Simulation is running with " + InputAlgorithmValue + " as Algorithm Type, " + MazeTypeValue  + " as Maze Type, " + MazeSizeValue + " as Maze Size, " + SensorTypeValue + " as sensors.";
	Result.text=updatedmessage;

    }

   
   
}
