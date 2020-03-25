using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class updatemessage : MonoBehaviour
{
	public TMPro.TMP_Dropdown InputAlgorithm;
	public TMPro.TMP_Dropdown MazeSize;
	public TMPro.TMP_Dropdown SensorType;
	public TMPro.TMP_InputField Thresholdvalue;
	public Text Result;
	
    // Start is called before the first frame update
    public void updateText()
    {
	InputAlgorithm = GameObject.Find("Algorithm").GetComponent<TMPro.TMP_Dropdown>();
    string InputAlgorithmValue = InputAlgorithm.captionText.text;
	MazeSize = GameObject.Find("MazeSize").GetComponent<TMPro.TMP_Dropdown>();
    string MazeSizeValue = MazeSize.captionText.text;
	string value = Thresholdvalue.text;
	float thresholdval = float.Parse(value);
	SensorType = GameObject.Find("SensorType").GetComponent<TMPro.TMP_Dropdown>();
    string SensorTypeValue = SensorType.captionText.text;
	

	  if (!string.IsNullOrEmpty(value)&&(thresholdval >= 0 && thresholdval <=1))
   {
    string updatedmessage="Simulation is running with " + InputAlgorithmValue + " as Algorithm Type, " + value  + " as Threshold Value, " + MazeSizeValue + " as Maze Size, " + SensorTypeValue + " as sensors.";
	Result.text=updatedmessage;
   }
   else
   {
      string alertmessage = "Kindly enter the value of Placement Threshold or Enter values in range 0 to 1";
	  Result.text=alertmessage;
   }    
}
	//string updatedmessage="Simulation is running with " + InputAlgorithmValue + " as Algorithm Type, " + value  + " as Threshold Value, " + MazeSizeValue + " as Maze Size, " + SensorTypeValue + " as sensors.";
	//Result.text=updatedmessage;

   
}
