using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using ExperimentalDesign;


public class ExpDesignScript : MonoBehaviour
{
	public TMPro.TMP_Dropdown InputAlgorithm;
	public TMPro.TMP_Dropdown MazeSize;
	public TMPro.TMP_Dropdown SensorType;
	public TMPro.TMP_InputField Thresholdvalue;
	public Text Result;

	// Start is called before the first frame update

	public void Start()
	{
		InputAlgorithm.options.Clear();
		MazeSize.options.Clear();
		SensorType.options.Clear();
		ExperimentalDesignClass experimentalDesign = new ExperimentalDesignClass();
		List<string> options_list;

		// Setting algorithm dropdown values
		options_list = experimentalDesign.setAlgorithm();
		foreach (string algorithm in options_list)
		{
			InputAlgorithm.options.Add(new TMP_Dropdown.OptionData() { text = algorithm });
		}
		InputAlgorithm.value = 1;
		InputAlgorithm.value = 0;

		// Setting Maze Type dropdown values


		// Setting Maze size dropdown values
		options_list = experimentalDesign.setMazeSize();
		foreach (string maze_size in options_list)
		{
			MazeSize.options.Add(new TMP_Dropdown.OptionData() { text = maze_size });
		}
		MazeSize.value = 1;
		MazeSize.value = 0;

		// Setting Sensor Type dropdown values
		options_list = experimentalDesign.setSensors();
		foreach (string sensor in options_list)
		{
			SensorType.options.Add(new TMP_Dropdown.OptionData() { text = sensor });
		}
		SensorType.value = 1;
		SensorType.value = 0;
	}
	public void updateText()
	{
		string value = "";
		double thresholdval = 2.0F;
		InputAlgorithm = GameObject.Find("Algorithm").GetComponent<TMPro.TMP_Dropdown>();
		string InputAlgorithmValue = InputAlgorithm.captionText.text;
		MazeSize = GameObject.Find("MazeSize").GetComponent<TMPro.TMP_Dropdown>();
		string MazeSizeValue = MazeSize.captionText.text;



		try
		{
			value = Thresholdvalue.text;
			thresholdval = float.Parse(value);
		}
		catch (FormatException fe)
		{
			string alertmessage = "Kindly enter the value of Placement Threshold or Enter values in range 0 to 1";
			Result.text = alertmessage;
		}
		SensorType = GameObject.Find("SensorType").GetComponent<TMPro.TMP_Dropdown>();
		string SensorTypeValue = SensorType.captionText.text;


		if (!string.IsNullOrEmpty(value) && (thresholdval >= 0 && thresholdval <= 1))
		{

			string updatedmessage = "Simulation is running with " + InputAlgorithmValue + " as Algorithm Type, " + value + " as Threshold Value, " + MazeSizeValue + " as Maze Size, " + SensorTypeValue + " as sensors.";
			Result.text = updatedmessage;
			database db = new database();
			db.Insert(InputAlgorithmValue, MazeSizeValue, thresholdval, SensorTypeValue);
		}
		else
		{
			string alertmessage = "Kindly enter the value of Placement Threshold or Enter values in range 0 to 1";
			Result.text = alertmessage;
		}


	}

	public void changeScene(String sceneName)
	{
		Application.LoadLevel(sceneName);
	}
	//string updatedmessage="Simulation is running with " + InputAlgorithmValue + " as Algorithm Type, " + value  + " as Threshold Value, " + MazeSizeValue + " as Maze Size, " + SensorTypeValue + " as sensors.";
	//Result.text=updatedmessage;


}
