using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExperimentalDesignDatabase;


public class database : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        expdb.Initialize();   
        //print(expdb.OpenConnection());
    }

    public void Insert(string algorithm, string mazetype, double thresholdvalue, string sensor) { 
    
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();

        expdb.Initialize();

        //print("INSERT INTO experimental_results(algorithmtype, mazesize, tresholdfrequency,sensortype) VALUES ('" + algorithm + "','" + mazetype + "'," + thresholdvalue + ",'" + sensor + "');");

        expdb.Insert("INSERT INTO experimental_results(AlgorithmType, MazeSize, ThresholdFrequency,SensorType) VALUES ('" + algorithm + "','"+mazetype + "',"+ thresholdvalue+",'"+ sensor+"');");


    }

    public void selectValuesfromDB(string yAxisValue, string InputAlgorithmValue, string MazeSizeValue,float Threshold, string SensorTypeValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();

        expdb.Initialize();
        List<string> range = expdb.Select();
        foreach (string item in range)
        {
            print(item);
        }

    }

    // Update is called once per frame

    public void UpdateAlgorithm(string InputAlgorithmvalue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();

        expdb.Initialize();
        //print(expdb.OpenConnection());
        expdb.Update("UPDATE experimental_results SET AlgorithmType='" + InputAlgorithmvalue + "' WHERE ID IN (SELECT Max(ID) FROM experimental_results);;");
        //print(expdb.UpdateAlgo());

    }

    public void UpdateSensor(string SensorValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();

        expdb.Initialize();
        expdb.Update("UPDATE experimental_results SET SensorType='" + SensorValue + "' WHERE ID IN (SELECT Max(ID) FROM experimental_results);;");

    }

    public void UpdateMazeSize(string MazeSize)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();

        expdb.Initialize();
        expdb.Update("UPDATE experimental_results SET MazeSize='" + MazeSize + "' WHERE ID IN (SELECT Max(ID) FROM experimental_results);;");

    }

    public void UpdateThreshold(float threshold)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();

        expdb.Initialize();
        expdb.Update("UPDATE experimental_results SET ThresholdFrequency=" + threshold + " WHERE ID IN (SELECT Max(ID) FROM experimental_results);;");

    }
    void Update()
    {

    }
}
