using System;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using Sql;
using Assets.Scripts.Database;

/// <summary>
/// This is database manager class that can access the database.
/// Author: Jiayan Wang, Bingrui Feng, Xinkai Wang
/// </summary>
public class DataBaseManager {
    private SqliteConnection dbConnection;
    private SqliteCommand dbCommand;
    private SqliteDataReader dataReader;
    private int actived_mazeId = -999999;
    public string mazeId { get; private set; }

    /// <summary>
    /// Connect to the database.
    /// </summary>
    public void ConnectToDB(string filename) {
        string filePath = Application.streamingAssetsPath + "/" + filename;
        try {
            dbConnection = new SqliteConnection("Data Source = " + filePath);
            dbConnection.Open();
            Debug.Log("success to connect " + filename);
        } catch (System.Exception e) {
            Debug.Log(e.Message);
        }
    }

    #region Algorithm Team
    /// <summary>
    /// This method is to get back maze matrix by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int[,] GetMazeById(int id) {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add(Constants.MAZE_MATRIX);
        string tableName = Constants.TABLE_MAZE;
        Dictionary<string, string> condition = new Dictionary<string, string>();
        condition.Add(Constants.COLUMN_ID, id.ToString());
        dataReader =
            ExecuteQuery(sql.Select(selectvalue, tableName, condition));
        string res = "";
        if (dataReader.Read())
        {
            res = dataReader.GetString(0);
        }

        string[] split1 = res.Split(';');
        int num1 = split1.Length;
        int num2 = split1[0].Split(',').Length;
        int[,] result = new int[num1, num2];
        for (int i = 0; i < split1.Length; i++)
        {
            string[] split2 = split1[i].Split(',');
            for (int j = 0; j < split2.Length; j++)
            {
                result[i, j] = Convert.ToInt32(split2[j]);
            }
        }
        return result;
    }

    /// <summary>
    /// Create a new maze by id and matrix
    /// </summary>
    /// <param name="mazeId"></param>
    /// <param name="exploredMaze"></param>
    /// <returns></returns>
    public int CreateExploredMaze(int mazeId, int[,] exploredMaze)
    {
        actived_mazeId = mazeId;
        SqlEncap sql = new SqlEncap();
        int result = Constants.RESPONSE_CODE_SUCCESS;

        List<string> columnName = new List<string>();
        List<string> value = new List<string>();

        try
        {
            columnName.Add(Constants.COLUMN_ID);
            columnName.Add(Constants.MAZE_MATRIX);

            string str = "'";
            for (int i = 0; i <= exploredMaze.GetUpperBound(0); i++)
            {
                str += "";
                for (int j = 0; j <= exploredMaze.GetUpperBound(1); j++)
                {
                    str += exploredMaze[i, j];
                    if (j != exploredMaze.GetUpperBound(1))
                    {
                        str += ",";
                    }
                }
                str += "";
                if (i != exploredMaze.GetUpperBound(0))
                {
                    str += ";";
                }
            }
            str += "'";

            value.Clear();
            value.Add(mazeId.ToString());
            value.Add(str);

            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                sql.Insert(Constants.TABLE_MAZE, columnName, value);
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException sqlEx)
        {
            result = Constants.RESPONSE_CODE_FAILURE;
            Debug.LogError(sqlEx);
        }
        return result;
    }

    /// <summary>
    /// Update maze matrix
    /// </summary>
    /// <param name="updatedMaze"></param>
    /// <returns></returns>
    public int UpdateMaze(int[,] updatedMaze)
    {
        SqlEncap sql = new SqlEncap();
        int result = Constants.RESPONSE_CODE_SUCCESS;

        Dictionary<string, string> value = new Dictionary<string, string>();
        Dictionary<string, string> condition = new Dictionary<string, string>();

        try
        {
            string str = "'";
            for (int i = 0; i <= updatedMaze.GetUpperBound(0); i++)
            {
                str += "";
                for (int j = 0; j <= updatedMaze.GetUpperBound(1); j++)
                {
                    str += updatedMaze[i, j];
                    if (j != updatedMaze.GetUpperBound(1))
                    {
                        str += ",";
                    }
                }
                str += "";
                if (i != updatedMaze.GetUpperBound(0))
                {
                    str += ";";
                }
            }
            str += "'";

            value.Clear();
            value.Add(Constants.MAZE_MATRIX, str);

            condition.Add(Constants.COLUMN_ID, actived_mazeId.ToString());

            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                sql.Update(Constants.TABLE_MAZE, value, condition);
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException sqlEx)
        {
            result = Constants.RESPONSE_CODE_FAILURE;
            Debug.LogError(sqlEx);
        }
        return result;
    }

    /// <summary>
    /// Update maze coverage
    /// </summary>
    /// <param name="mazeCoverage"></param>
    /// <returns></returns>
    public int UpdateCoverage(float mazeCoverage)
    {
        SqlEncap sql = new SqlEncap();
        int result = Constants.RESPONSE_CODE_SUCCESS;

        Dictionary<string, string> value = new Dictionary<string, string>();
        Dictionary<string, string> condition = new Dictionary<string, string>();

        try
        {
            value.Clear();
            value.Add(Constants.MAZE_COVERAGE, mazeCoverage.ToString());

            condition.Add(Constants.COLUMN_ID, actived_mazeId.ToString());

            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                sql.Update(Constants.TABLE_MAZE, value, condition);
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException sqlEx)
        {
            result = Constants.RESPONSE_CODE_FAILURE;
            Debug.LogError(sqlEx);
        }
        return result;
    }

    /// <summary>
    /// Update maze time taken
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public int UpdateTimeTaken(int second)
    {
        SqlEncap sql = new SqlEncap();
        int result = Constants.RESPONSE_CODE_SUCCESS;

        Dictionary<string, string> value = new Dictionary<string, string>();
        Dictionary<string, string> condition = new Dictionary<string, string>();

        try
        {
            value.Clear();
            value.Add(Constants.MAZE_TIMETAKEN, second.ToString());

            condition.Add(Constants.COLUMN_ID, actived_mazeId.ToString());

            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                sql.Update(Constants.TABLE_MAZE, value, condition);
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException sqlEx)
        {
            result = Constants.RESPONSE_CODE_FAILURE;
            Debug.LogError(sqlEx);
        }
        return result;
    }

    /// <summary>
    /// Update maze move history
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public int UpdateMoveHistory(String[] path)
    {
        SqlEncap sql = new SqlEncap();
        int result = Constants.RESPONSE_CODE_SUCCESS;

        Dictionary<string, string> value = new Dictionary<string, string>();
        Dictionary<string, string> condition = new Dictionary<string, string>();

        try
        {
            string str = "'";
            for(int i = 0; i < path.Length; i++)
            {
                if(i != 0)
                {
                    str += ",";
                }
                str += path[i];
            }
            str += "'";

            value.Clear();
            value.Add(Constants.MAZE_HISTORY, str);

            condition.Add(Constants.COLUMN_ID, actived_mazeId.ToString());

            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                sql.Update(Constants.TABLE_MAZE, value, condition);
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException sqlEx)
        {
            result = Constants.RESPONSE_CODE_FAILURE;
            Debug.LogError(sqlEx);
        }
        return result;
    }

    /// <summary>
    /// Update maze points
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public int UpdatePoints(int points)
    {
        SqlEncap sql = new SqlEncap();
        int result = Constants.RESPONSE_CODE_SUCCESS;

        Dictionary<string, string> value = new Dictionary<string, string>();
        Dictionary<string, string> condition = new Dictionary<string, string>();

        try
        {
            value.Clear();
            value.Add(Constants.MAZE_POINTS, points.ToString());

            condition.Add(Constants.COLUMN_ID, actived_mazeId.ToString());

            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                sql.Update(Constants.TABLE_MAZE, value, condition);
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException sqlEx)
        {
            result = Constants.RESPONSE_CODE_FAILURE;
            Debug.LogError(sqlEx);
        }
        return result;
    }
    #endregion

    #region Sensor Team
    /// SetSensorMatrixById
    /// <summary>
    /// Set the environment record by the sensor.
    /// Could be any size.
    /// </summary>
    /// <param name="sensorId"></param>
    /// /// <param name="matrix"></param>
    /// <returns></returns>
    public int SetSensorMatrixById(int timestamp, int sensorId, int[,] matrix)
    {
        SqlEncap sql = new SqlEncap();
        int result = Constants.RESPONSE_CODE_SUCCESS;

        List<string> columnName = new List<string>();
        List<string> value = new List<string>();

        try
        {
            columnName.Add(Constants.SENSOR_TIMESTAMP);
            columnName.Add(Constants.COLUMN_ID);
            columnName.Add(Constants.SENSOR_CONTENT);

            string str = "'";
            for (int i = 0; i <= matrix.GetUpperBound(0); i++)
            {
                str += "";
                for (int j = 0; j <= matrix.GetUpperBound(1); j++)
                {
                    str += matrix[i, j];
                    if (j != matrix.GetUpperBound(1))
                    {
                        str += ",";
                    }
                }
                str += "";
                if (i != matrix.GetUpperBound(0))
                {
                    str += ";";
                }
            }
            str += "'";

            value.Clear();
            value.Add(timestamp.ToString());
            value.Add(sensorId.ToString());
            value.Add(str);

            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                sql.Insert(Constants.TABLE_SENSOR, columnName, value);
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException sqlEx)
        {
            result = Constants.RESPONSE_CODE_FAILURE;
            Debug.LogError(sqlEx);
        }
        return result;
    }

    /// GetSensorMatrixById
    /// <summary>
    /// return the sensor according to id
    /// </summary>
    /// <param name="sensorId"></param>
    /// <returns></returns>
    public int[,] GetSensorMatrixById(int sensorId, int timestamp)
    {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add("Content");
        string tableName = Constants.TABLE_SENSOR;
        Dictionary<string, string> condition = new Dictionary<string, string>();
        condition.Add(Constants.COLUMN_ID, sensorId.ToString());
        condition.Add(Constants.SENSOR_TIMESTAMP, timestamp.ToString());
        dataReader =
            ExecuteQuery(sql.Select(selectvalue, tableName, condition));
        string res = "";
        if (dataReader.Read())
        {
            res = dataReader.GetString(0);
        }

        string[] split1 = res.Split(';');
        int num1 = split1.Length;
        int num2 = split1[0].Split(',').Length;
        int[,] result = new int[num1, num2];
        for (int i = 0; i < split1.Length; i++)
        {
            string[] split2 = split1[i].Split(',');
            for (int j = 0; j < split2.Length; j++)
            {
                result[i, j] = Convert.ToInt32(split2[j]);
            }
        }
        return result;
    }
    #endregion

    public int provideUid()
    {
        var now = DateTime.Now;
        var zeroDate = DateTime.MinValue.AddHours(now.Hour)
                           .AddMinutes(now.Minute)
                           .AddSeconds(now.Second)
                           .AddMilliseconds(now.Millisecond);
        int uniqueId = (int)(zeroDate.Ticks / 10000) % 10000;
        return uniqueId;
    }

    /// <summary>
    /// close the connection with the database
    /// </summary>
    public void CloseConnection()
    {
        if (dbCommand != null) {
            dbCommand.Cancel();
        }
        dbCommand = null;

        if (dataReader != null) {
            dataReader.Close();
        }
        dataReader = null;

        if (dbConnection != null) {
            dbConnection.Close();
        }
        dbConnection = null;
    }

#region private functions
    /// <summary>
    /// Execute SQL sentences.
    /// </summary>
    /// <param name="queryString"></param>
    /// <returns></returns>
    private SqliteDataReader ExecuteQuery(string queryString)
    {
        Debug.Log(queryString);
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryString;
        Debug.Log(queryString);
        dataReader = dbCommand.ExecuteReader();
        return dataReader;
    }

    public string[][] GetAllMazeRecord()
    {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add(Constants.COLUMN_ID);
        selectvalue.Add(Constants.MAZE_MATRIX);
        selectvalue.Add(Constants.MAZE_COVERAGE);
        selectvalue.Add(Constants.MAZE_TIMETAKEN);
        selectvalue.Add(Constants.MAZE_HISTORY);
        selectvalue.Add(Constants.MAZE_POINTS);

        Dictionary<string, string> condition = new Dictionary<string, string>();

        List<string[]> res = new List<string[]>();
        dataReader = ExecuteQuery(sql.Select(selectvalue, Constants.TABLE_MAZE, condition));
        while (dataReader.HasRows)
        {
            if (dataReader.Read())
            {
                res.Add(new string[6] { dataReader[Constants.COLUMN_ID].ToString(), dataReader[Constants.MAZE_MATRIX].ToString(), dataReader[Constants.MAZE_COVERAGE].ToString(), dataReader[Constants.MAZE_TIMETAKEN].ToString(), dataReader[Constants.MAZE_HISTORY].ToString(), dataReader[Constants.MAZE_POINTS].ToString() });
            }
        }
        return res.ToArray();
    }

    #endregion

    #region error check
    /// <summary>
    /// Check all input data for insert or update maze table
    /// </summary>
    /// <param name="id"></param>
    /// <param name="nodes"></param>
    /// <param name="edges"></param>
    /// <returns></returns>
    private bool errorCheckMaze(int id, string[,] edges)
    {
        if (id < 0)                                      //Check ID
            return true;
        List<string> directionList = new List<string>();
        directionList.Add("N");
        directionList.Add("S");
        directionList.Add("W");
        directionList.Add("E");

        for (int i = 0; i < edges.GetLength(0); i++) {
            if (edges.GetLength(1) > 3)
                return true;
            else if (System.Convert.ToInt32(edges[i, 0]) < 0 ||
                     System.Convert.ToInt32(edges[i, 1]) < 0)
                return true;
            else if (!directionList.Contains(edges[i, 2]))
                return true;
        }
        return false;
    }
#endregion
}