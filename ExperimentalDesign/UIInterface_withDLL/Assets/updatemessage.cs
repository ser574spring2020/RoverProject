using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using SampleDLL;
using ExperimentalDesign;
public class updatemessage : MonoBehaviour
{
	public TMPro.TMP_Dropdown InputAlgorithm;
	public TMPro.TMP_Dropdown MazeType;
	public TMPro.TMP_Dropdown MazeSize;
	public TMPro.TMP_Dropdown SensorType;
	public Text Result;


	// Start is called before the first frame update

	public void Start()
	{
		InputAlgorithm.options.Clear();
		MazeType.options.Clear();
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
		options_list = experimentalDesign.setMazeType();
		foreach (string maze_type in options_list)
		{
			MazeType.options.Add(new TMP_Dropdown.OptionData() { text = maze_type });
		}
		MazeType.value = 1;
		MazeType.value = 0;

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
