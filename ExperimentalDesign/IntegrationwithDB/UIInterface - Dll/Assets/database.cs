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
    }

    public void Insert(string algorithm, string mazetype, double thresholdvalue, string sensor)
    {

        ExperimentalDesignDb expdb = new ExperimentalDesignDb();

        expdb.Initialize();

        expdb.Insert("INSERT INTO experimental_results(AlgorithmType, MazeSize, ThresholdFrequency,SensorType) VALUES ('" + algorithm + "','" + mazetype + "'," + thresholdvalue + ",'" + sensor + "');");


    }

    public List<float> selectValuesfromDB(string yAxisValue, string InputAlgorithmValue, string MazeSizeValue, float Threshold, string SensorTypeValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        List<float> range= new List<float>();
        expdb.Initialize();
            range = expdb.Select(yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
            return range;

    }

    public double minvalue(string yAxisValue, string InputAlgorithmValue, string MazeSizeValue, float Threshold, string SensorTypeValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        expdb.Initialize();
        double range = expdb.minimumvalue(yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
        return range;

    }
    public double maxvalue(string yAxisValue, string InputAlgorithmValue, string MazeSizeValue, float Threshold, string SensorTypeValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        expdb.Initialize();
        double range = expdb.maximumvalue(yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
        return range;

    }
    public double averagevalue(string yAxisValue, string InputAlgorithmValue, string MazeSizeValue, float Threshold, string SensorTypeValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        expdb.Initialize();
        double range = expdb.averagevalue(yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
        return range;

    }
    // Update is called once per frame

    public void UpdateAlgorithm(string InputAlgorithmvalue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        expdb.Initialize();
        expdb.Update("UPDATE experimental_results SET AlgorithmType='" + InputAlgorithmvalue + "' WHERE ID IN (SELECT Max(ID) FROM experimental_results);;");

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