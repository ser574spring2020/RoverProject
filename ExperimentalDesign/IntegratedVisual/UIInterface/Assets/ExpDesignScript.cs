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
    public TMPro.TMP_Dropdown ExperimentType;
    public TMPro.TMP_InputField NumberOfExperiments;
    public Text Result;
    public String[] inputs;

    // Start is called before the first frame update

    public void Start()
    {
        InputAlgorithm.options.Clear();
        MazeSize.options.Clear();
        SensorType.options.Clear();
        ExperimentType.options.Clear();
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

        options_list = experimentalDesign.setExperimentType();
        foreach (string experimentType in options_list)
        {
            ExperimentType.options.Add(new TMP_Dropdown.OptionData() { text = experimentType });
        }
        ExperimentType.value = 1;
        ExperimentType.value = 0;
    }
    public void updateText()
    {

        string alertmessage = "";
        string value1 = "";
        string value = "";
        double thresholdval = 2.0F;
        double experimentVal = 0.0F;
        InputAlgorithm = GameObject.Find("Algorithm").GetComponent<TMPro.TMP_Dropdown>();
        string InputAlgorithmValue = InputAlgorithm.captionText.text;
        MazeSize = GameObject.Find("MazeSize").GetComponent<TMPro.TMP_Dropdown>();
        string MazeSizeValue = MazeSize.captionText.text;
        TestSuite ts = new TestSuite();

        try
        {
            value1 = NumberOfExperiments.text;
            experimentVal = float.Parse(value1);
        }
        catch (FormatException fe)
        {
            alertmessage += "Kindly enter the value of Number of Experiments or Enter values in range 1 to 100";
            Result.text = alertmessage;
        }

        try
        {
            value = Thresholdvalue.text;
            thresholdval = float.Parse(value);

        }
        catch (FormatException fe)
        {
            alertmessage += "Kindly enter the value of Placement Threshold or Enter values in range 0 to 1";
            Result.text = alertmessage;
        }
        SensorType = GameObject.Find("SensorType").GetComponent<TMPro.TMP_Dropdown>();
        string SensorTypeValue = SensorType.captionText.text;


        if ((!string.IsNullOrEmpty(value) && (thresholdval >= 0 && thresholdval <= 1)) && !string.IsNullOrEmpty(value1) && (experimentVal >= 1 && experimentVal <= 100))
        {

            string updatedmessage = "Simulation is running with " + InputAlgorithmValue + " as Algorithm Type, " + value + " as Threshold Value, " + MazeSizeValue + " as Maze Size, " + SensorTypeValue + " as sensors on" + ExperimentType.captionText.text + " with " + experimentVal + " Experiments";
            Result.text = updatedmessage;
            database db = new database();
            thresholdval = Math.Round(thresholdval, 2);
            inputs = new string[] { InputAlgorithmValue, MazeSizeValue, SensorTypeValue, ExperimentType.captionText.text };
            ts.testUpdateText(inputs);
            db.Insert(InputAlgorithmValue, MazeSizeValue, thresholdval, SensorTypeValue, ExperimentType.captionText.text);
        }
        else
        {
            alertmessage = "Kindly enter the valid Values";
            Result.text = alertmessage;
        }


    }

    public void changeScene(String sceneName)
    {
        Application.LoadLevel(sceneName);
    }


}
