using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExperimentalDesignDatabase;
using Mono.Data.Sqlite;


public class database : MonoBehaviour
{
    public SqliteConnection dbConnection;


    // Start is called before the first frame update
    void Start()
    {
        try
        {
            string filePath = Application.streamingAssetsPath + "/" + "ExperimentalDatabase.db";
            dbConnection = new SqliteConnection("Data Source = " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Insert(string algorithm, string mazetype, double thresholdvalue, string sensor, string experimentType,int ExperimentalID)
    {

        ExperimentalDesignDb expdb = new ExperimentalDesignDb();

        Start();

        expdb.Insert(dbConnection, "INSERT INTO experimental_results(AlgorithmType, MazeSize, ThresholdFrequency,SensorType,ExperimentType,ExperimentID) VALUES ('" + algorithm + "','" + mazetype + "'," + thresholdvalue + ",'" + sensor + "','"+ experimentType+"','" + ExperimentalID + "');");


    }

    public List<float> selectValuesfromDB(string yAxisValue, string InputAlgorithmValue, string MazeSizeValue, float Threshold, string SensorTypeValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        Start();
        List<float> range= new List<float>();
            range = expdb.Select(dbConnection, yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
            return range;

    }
    public List<float> selectValuesfromDB(string yAxisValue, int Expid)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        Start();
        List<float> range = new List<float>();
        range = expdb.Select(dbConnection, yAxisValue,Expid);
        return range;

    }
    public int get()
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        Start();
        expdb.maxexpid(dbConnection);
        return expdb.maxexpid(dbConnection);
    }

    public float minvalue(string yAxisValue, string InputAlgorithmValue, string MazeSizeValue, float Threshold, string SensorTypeValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        Start();
        float range = expdb.minimumvalue(dbConnection,yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
        return range;

    }
    public float minvalue(string yAxisValue, int Expid)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        Start();
        float range = expdb.minimumvalue(dbConnection, yAxisValue, Expid);
        return range;

    }
    public float maxvalue(string yAxisValue, string InputAlgorithmValue, string MazeSizeValue, float Threshold, string SensorTypeValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        Start();
        float range = expdb.maximumvalue(dbConnection,yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
        return range;

    }
    public float maxvalue(string yAxisValue, int Expid)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        Start();
        float range = expdb.maximumvalue(dbConnection, yAxisValue, Expid);
        return range;

    }
    public float averagevalue(string yAxisValue, string InputAlgorithmValue, string MazeSizeValue, float Threshold, string SensorTypeValue)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        Start();
        float range = expdb.averagevalue(dbConnection,yAxisValue, InputAlgorithmValue, MazeSizeValue, Threshold, SensorTypeValue);
        return range;

    }
    public float averagevalue(string yAxisValue, int Expid)
    {
        ExperimentalDesignDb expdb = new ExperimentalDesignDb();
        Start();
        float range = expdb.averagevalue(dbConnection, yAxisValue, Expid);
        return range;

    }
    // Update is called once per frame

    public int UpdateTimeTaken(float TimeTaken)
    {
        int statusCode = 0;
        try
        {
            ExperimentalDesignDb expdb = new ExperimentalDesignDb();
            Start();
            expdb.Update(dbConnection, "UPDATE experimental_results SET TimeTaken='" + TimeTaken + "' WHERE ID IN (SELECT Max(ID) FROM experimental_results);");
            statusCode = 200;
    }
        catch (SqliteException sqlEx)
        {
            statusCode = 400;
            Debug.LogError(sqlEx);
        }

        return statusCode;

    }

    public int UpdatePointsScored(float PointsScored)
    {
        int statusCode = 0;
        try
        {
            ExperimentalDesignDb expdb = new ExperimentalDesignDb();
            Start();
            expdb.Update(dbConnection, "UPDATE experimental_results SET PointsScored='" + PointsScored + "' WHERE ID IN (SELECT Max(ID) FROM experimental_results);");
            statusCode = 200;
        }
        catch (SqliteException sqlEx)
        {
            statusCode = 400;
            Debug.LogError(sqlEx);
        }

        return statusCode;

    }

    public int UpdateMazeCoverage(float MazeCoverage)
    {
        int statusCode = 0;
        try
        {
            ExperimentalDesignDb expdb = new ExperimentalDesignDb();
            Start();
            expdb.Update(dbConnection, "UPDATE experimental_results SET MazeCoverage='" + MazeCoverage + "' WHERE ID IN (SELECT Max(ID) FROM experimental_results);");
            statusCode = 200;
        }
        catch (SqliteException sqlEx)
        {
            statusCode = 400;
            Debug.LogError(sqlEx);
        }

        return statusCode;

    }

    public int UpdateDroneLife(float DroneLife)
    {
        int statusCode = 0;
        try
        {
            ExperimentalDesignDb expdb = new ExperimentalDesignDb();
            Start();
            expdb.Update(dbConnection,"UPDATE experimental_results SET DroneLife=" + DroneLife + " WHERE ID IN (SELECT Max(ID) FROM experimental_results);");
            statusCode = 200;
        }
        catch (SqliteException sqlEx)
        {
            statusCode = 400;
            Debug.LogError(sqlEx);
        }

        return statusCode;

    }
}