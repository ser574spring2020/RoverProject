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
    private int mazeUid;

    void Start() {
        dbm = new DataBaseManager();
        dbm.ConnectToDB("Rover.db");
        // Algorithm
        TestCreateExploredMaze();
        TestUpdateMaze();
        TestGetMazeById();
        TestUpdateCoverage();
        TestUpdateTimeTaken();
        TestUpdateMoveHistory();
        TestUpdatePoints();
        // Sensor
        TestInsertSensor();
        TestGetSensor();
        
    }

    void TestCreateExploredMaze() {
        //int[] nodes = new int[4]{1, 2, 3, 4};
        //string[, ] edges = new string[4, 3]{
        //    {"1", "3", "S"}, {"2", "3", "N"}, {"2", "4", "W"}, {"3", "4", "E"}};
        //mazeUid = provideUid();
        //int resultCode = dbm.InsertMazeRecord(mazeUid, edges);
        //Debug.Log("Insert Maze Result:" +
        //          (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
        //           : "Failure"));
    }

    void TestUpdateMaze() {
        //string[] edges = new string[3]{"1", "3", "E"};
        //int resultCode = dbm.UpdateMazeDirection(mazeUid, edges);
        //Debug.Log("Update Maze Result:" +
        //          (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
        //           : "Failure"));
    }

    void TestGetMazeById() {
        //mazeUid = 37887891;
        //var arr = dbm.GetMazeById(mazeUid);

        //int rowLength = arr.GetLength(0);
        //int colLength = arr[0].Length;

        //Debug.Log("Get Maze Result:");

        //for (int i = 0; i < rowLength; i++) {
        //    string str = String.Empty;
        //    for (int j = 0; j < colLength; j++) {
        //        str += arr [i]
        //               [j] +
        //               " ";
        //    }
        //    Debug.Log(str);
        //}
    }

    void TestUpdateCoverage() {
        //int resultCode = dbm.DeleteMazeById(mazeUid);
        //Debug.Log("Delete Maze Result:" +
        //          (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
        //           : "Failure"));
    }

    void TestUpdateTimeTaken()
    {
        
    }

    void TestUpdateMoveHistory()
    {

    }

    void TestUpdatePoints()
    {

    }

    void TestInsertSensor()
    {
        int[,] matrix = new int[4, 4] { { 1, 2, 3, 4 }, { 2, 3, 4, 5 }, { 3, 4, 5, 6 }, { 5, 6, 7, 8 } };

        int resultCode = dbm.SetSensorMatrixById(20200420, 10, matrix);
        Debug.Log("Insert Sensor Result:" +
                  (resultCode == Constants.RESPONSE_CODE_SUCCESS ? "Success"
                   : "Failure"));
    }

    void TestGetSensor()
    {
        int[,] matrix = dbm.GetSensorMatrixById(10, 20200420);
        for (int i = 0; i <= matrix.GetUpperBound(0); i++)
        {
            for(int j = 0; j <= matrix.GetUpperBound(1); j++)
            {
                Debug.Log(matrix[i, j] + ",");
            }
        }
    }

    private int provideUid() {
        var now = DateTime.Now;
        var zeroDate = DateTime.MinValue.AddHours(now.Hour)
                           .AddMinutes(now.Minute)
                           .AddSeconds(now.Second)
                           .AddMilliseconds(now.Millisecond);
        int uniqueId = (int)(zeroDate.Ticks / 10000) % 10000;
        return uniqueId;
    }
}
