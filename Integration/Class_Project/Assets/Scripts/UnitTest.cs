using System;
using Assets.Scripts.Database;
using UnityEngine;

/// <summary>
/// This Demo is used to show the functionality of the DataBaseManager,
/// Please run the demo in Unity and see console for details.
///
/// Author: Bingrui Feng, Xinkai Wang, Jiayan Wang,
/// Yu-Ting Tsao, Huijing Liang, Meng-Ze Chen
/// </summary>
public class UnitTest : MonoBehaviour {
    private DataBaseManager dbm;
    private int mazeUid = 3;
    private int distanceSensorId = 2;
    private int fooTimeStamp = 20200420;

    void Start() {
        dbm = new DataBaseManager();
        dbm.ConnectToDB("Rover.db");
        // Sensor
        TestSetSensorMatrixById();
        TestGetSensorMatrixById();
        // Algorithm
        TestCreateExploredMaze();
        TestGetMazeById();
        TestUpdateMaze();
        TestUpdateCoverage();
        TestUpdateTimeTaken();
        TestUpdateMoveHistory();
        TestUpdatePoints();
    }

    #region Sensor Team
    void TestSetSensorMatrixById()
    {
        int[,] matrix = new int[4, 4] {
            { 1, 1, 1, 1 },
            { 1, 0, 0, 1 },
            { 1, 0, 0, 1 },
            { 1, 1, 1, 1 }
        };

        int resultCode = dbm.SetSensorMatrixById(fooTimeStamp, distanceSensorId, matrix);
        Debug.Log("Set Sensor Result:" +
                  (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
                   : "Failure"));
    }

    void TestGetSensorMatrixById()
    {
        int[,] matrix = dbm.GetSensorMatrixById(distanceSensorId, fooTimeStamp);
        string str = "\n";
        for (int i = 0; i <= matrix.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= matrix.GetUpperBound(1); j++)
            {
                str += matrix[i, j];
                if (j != matrix.GetUpperBound(1))
                {
                    str += ",";
                }
            }
            str += "\n";
        }
        Debug.Log(str);
    }
    #endregion

    #region Algorithm Team
    void TestCreateExploredMaze() {
        int[,] exploredMaze = new int[4, 4] {
            { 1, 1, -1, -1 },
            { 1, 0, 0, -1 },
            { 1, 0, 0, 1 },
            { 1, 1, 1, 1 }
         };
        int resultCode = dbm.CreateExploredMaze(mazeUid, exploredMaze);
        Debug.Log("Create Explored Maze Result:" +
                  (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
                   : "Failure"));
    }

    void TestGetMazeById()
    {
        int[,] storedMaze = dbm.GetMazeById(mazeUid);
        string str = "\n";
        for (int i = 0; i <= storedMaze.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= storedMaze.GetUpperBound(1); j++)
            {
                str += storedMaze[i, j];
                if (j != storedMaze.GetUpperBound(1))
                {
                    str += ",";
                }
            }
            str += "\n";
        }
        Debug.Log(str);
    }

    void TestUpdateMaze() {
        int[,] updatedMaze = new int[4, 4] {
            { 1, 1, 1, 2 },
            { 1, 0, 0, 1 },
            { 1, 0, 0, 1 },
            { 1, 1, 1, 1 }
        };
        int resultCode = dbm.UpdateMaze(updatedMaze);
        Debug.Log("Update Maze Result:" +
                  (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
                   : "Failure"));
    }

    void TestUpdateCoverage() {
        float mazeCoverage = 0.4F;
        int resultCode = dbm.UpdateCoverage(mazeCoverage);
        Debug.Log("Update Coverage Result:" +
                  (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
                   : "Failure"));
    }

    void TestUpdateTimeTaken()
    {
        int second = 101;
        int resultCode = dbm.UpdateTimeTaken(second);
        Debug.Log("Update Time Taken Result:" +
                  (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
                   : "Failure"));
    }

    void TestUpdateMoveHistory()
    {
        String[] path = new String[5] { "East", "East", "North", "East", "South" };
        int resultCode = dbm.UpdateMoveHistory(path);
        Debug.Log("Update Move History Result:" +
                  (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
                   : "Failure"));
    }

    void TestUpdatePoints()
    {
        int points = 999;
        int resultCode = dbm.UpdatePoints(points);
        Debug.Log("Update Points Result:" +
                  (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
                   : "Failure"));
    }
    #endregion
}