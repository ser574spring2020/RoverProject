using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Mono.Data.Sqlite;

public class TestSuiteDatabase : MonoBehaviour
{
    // Update is called once per frame
    public void TestSelectValuesfromDB(string YAxisValue)
    {
        Debug.Log(" In Y Axis Value Testcase");
        Assert.IsNotNull(YAxisValue, "Y Axis Value Is Null");
        Debug.Assert(YAxisValue.Contains("TimeTaken") || YAxisValue.Contains("PointsScored") || YAxisValue.Contains("MazeCoverage") || YAxisValue.Contains("DroneLife"), "Invalid YAxis paramter passed");
    }

    
    public bool testUpdateTimeTaken(SqliteConnection dbconnection, string query)
    {
        Debug.Log(query);

        dbconnection.Open();
  
        SqliteCommand cmd = new SqliteCommand( query, dbconnection);

       int count = int.Parse(cmd.ExecuteScalar() + "");

        dbconnection.Close();

        Debug.Log(count);

        Assert.AreEqual(0, count, "More than 1 rows found");

        return true;

    }

    public bool testUpdatePointsScored(SqliteConnection dbconnection, string query)
    {
        Debug.Log(query);

        dbconnection.Open();

        SqliteCommand cmd = new SqliteCommand(query, dbconnection);

        int count = int.Parse(cmd.ExecuteScalar() + "");

        dbconnection.Close();

        Debug.Log(count);

        Assert.AreEqual(0, count, "More than 1 rows found");

        return true;

    }
    public bool testUpdateMazeCoverage(SqliteConnection dbconnection, string query)
    {
        Debug.Log(query);

        dbconnection.Open();

        SqliteCommand cmd = new SqliteCommand(query, dbconnection);

        int count = int.Parse(cmd.ExecuteScalar() + "");

        dbconnection.Close();

        Debug.Log(count);

        Assert.AreEqual(0, count, "More than 1 rows found");

        return true;

    }

    public bool testUpdateDroneLife(SqliteConnection dbconnection, string query)
    {
        Debug.Log(query);

        dbconnection.Open();

        SqliteCommand cmd = new SqliteCommand(query, dbconnection);

        int count = int.Parse(cmd.ExecuteScalar() + "");

        dbconnection.Close();

        Debug.Log(count);

        Assert.AreEqual(0, count, "More than 1 rows found");

        return true;

    }



    
    
}
