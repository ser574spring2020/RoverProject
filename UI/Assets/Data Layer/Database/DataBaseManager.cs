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
    /// This method is to get back maze record by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string[][] GetMazeById(int id) {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add(Constants.COLUMN_NODE);
        selectvalue.Add(Constants.COLUMN_CONNECTTO);
        selectvalue.Add(Constants.COLUMN_DIRECTION);

        Dictionary<string, string> condition = new Dictionary<string, string>();
        condition.Add(Constants.COLUMN_ID, id.ToString());

        List<string[]> res = new List<string[]>();
        dataReader = ExecuteQuery(sql.Select(selectvalue, Constants.TABLE_MAZE, condition));
        while (dataReader.HasRows)
        {
            if (dataReader.Read())
            {
                res.Add(new string[3] { dataReader[Constants.COLUMN_NODE].ToString(), dataReader[Constants.COLUMN_CONNECTTO].ToString(), dataReader[Constants.COLUMN_DIRECTION].ToString() });
            }
        }
        return res.ToArray();
    }

    // TODO: NEW requirements from algorithm team
    public int CreateExploredMaze(int mazeId, int[,] exploredMaze)
    {
        throw new NotImplementedException();
    }

    public int UpdateMaze(int[,] updatedMaze)
    {
        throw new NotImplementedException();
    }

    public int UpdateCoverage(float mazeCoverage)
    {
        throw new NotImplementedException();
    }

    public int UpdateTimeTaken(int second)
    {
        throw new NotImplementedException();
    }

    public int UpdateMoveHistory(String[] path)
    {
        throw new NotImplementedException();
    }

    public int UpdatePoints(int points)
    {
        throw new NotImplementedException();
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
            //Debug.Log(str);

            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText =
                sql.Insert(Constants.TABLE_SENSOR, columnName, value);
            //Debug.Log(dbCommand.CommandText);
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

    public string[][] GetAllMazeRecord()
    {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add(Constants.COLUMN_ID);
        selectvalue.Add(Constants.COLUMN_NODE);
        selectvalue.Add(Constants.COLUMN_CONNECTTO);
        selectvalue.Add(Constants.COLUMN_DIRECTION);

        Dictionary<string, string> condition = new Dictionary<string, string>();

        List<string[]> res = new List<string[]>();
        dataReader = ExecuteQuery(sql.Select(selectvalue, Constants.TABLE_MAZE, condition));
        while (dataReader.HasRows)
        {
            if (dataReader.Read())
            {
                res.Add(new string[4] { dataReader[Constants.COLUMN_ID].ToString(), dataReader[Constants.COLUMN_NODE].ToString(), dataReader[Constants.COLUMN_CONNECTTO].ToString(), dataReader[Constants.COLUMN_DIRECTION].ToString() });
            }
        }
        return res.ToArray();
    }

    public int[, ] getPathSize(int id) {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add("count(Step)");
        string tableName = "Path";
        Dictionary<string, string> condition = new Dictionary<string, string>();
        condition.Add("SolutionID", id.ToString());

        dataReader =
            ExecuteQuery(sql.Select(selectvalue, tableName, condition));
        int step = 0;
        if (dataReader.Read()) {
            step = dataReader.GetInt32(0);
        }
        return new int[step, 2];
    }

    /// <summary>
    /// return the sepecific path according to id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int[, ] getPathByID(int id) {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add("Step");
        selectvalue.Add("X");
        selectvalue.Add("Y");
        string tableName = "Path";
        Dictionary<string, string> condition = new Dictionary<string, string>();
        condition.Add("SolutionID", id.ToString());

        int[, ] res = getPathSize(id);
        dataReader =
            ExecuteQuery(sql.Select(selectvalue, tableName, condition));
        while (dataReader.HasRows) {
            if (dataReader.Read()) {
                int step = dataReader.GetInt32(0) - 1;
                int x = dataReader.GetInt32(1);
                int y = dataReader.GetInt32(2);
                res[step, 0] = x;
                res[step, 1] = y;
            }
        }
        return res;
    }

    /// <summary>
    /// return the size of the commands list
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string[] getCommandsSize(int id) {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add("count(Step)");
        string tableName = "Commands";
        Dictionary<string, string> condition = new Dictionary<string, string>();
        condition.Add("ID", id.ToString());
        dataReader =
            ExecuteQuery(sql.Select(selectvalue, tableName, condition));
        string[] res = new string[0];
        if (dataReader.Read()) {
            int size = dataReader.GetInt32(0);
            res = new string[size];
        }
        return res;
    }

    /// <summary>
    /// return the sepecific Command according to id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string[] getCommandByID(int id)
    {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add("Step");
        selectvalue.Add("Command");
        string tableName = "Commands";
        Dictionary<string, string> condition = new Dictionary<string, string>();
        condition.Add("ID", id.ToString());

        string[] res = getCommandsSize(id);
        dataReader =
            ExecuteQuery(sql.Select(selectvalue, tableName, condition));
        // Debug.Log(dataReader.Read());
        while (dataReader.HasRows)
        {
            if (dataReader.Read())
            {
                int index = dataReader.GetInt32(0) - 1;
                res[index] = dataReader.GetString(1);
            }
        }
        return res;
    }

    /// <summary>
    /// close the connection with the database
    /// </summary>
    public void CloseConnection() {
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

    /// <summary>
    /// Return the size of the maze according to id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private int[, ] getMazeSize(int id) {
        SqlEncap sql = new SqlEncap();
        List<string> selectvalue = new List<string>();
        selectvalue.Add("max(X)");
        selectvalue.Add("max(Y)");
        string tableName = "Maze";
        Dictionary<string, string> condition = new Dictionary<string, string>();
        condition.Add("ID", id.ToString());

        dataReader =
            ExecuteQuery(sql.Select(selectvalue, tableName, condition));
        int x = 0;
        int y = 0;
        if (dataReader.Read()) {
            x = dataReader.GetInt32(0) + 1;
            y = dataReader.GetInt32(1) + 1;
        }
        return new int[x, y];
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
