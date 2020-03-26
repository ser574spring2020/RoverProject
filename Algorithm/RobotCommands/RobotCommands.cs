using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
@author: Mayank Batra
Desciption: This class is reponsible for making the decision and 
hence generating the commands for robot's next movement 
*/
public class RobotCommands
{
    public static void Main()
    {
        int[,] sensorData = new int[3, 3];
        sensorData[1, 2] = 0;
        sensorData[0, 0] = 1;
        sensorData[0, 1] = 1;
        sensorData[0, 2] = 1;
        sensorData[1, 0] = 1;
        sensorData[1, 1] = 2;
        sensorData[2, 1] = 1;
        sensorData[2, 2] = 1;
        sensorData[2, 0] = 1;
        RobotCommands obj = new RobotCommands();

        foreach (var item in obj.computeDirection(sensorData))
        {
            Console.WriteLine("The nodes are:" + item);
        }
    }

    public List<String> computeDirection(int[,] sensorData)
    {
		Vector2Int East =1,2;
        Vector2Int West =1,0;
        Vector2Int North =0,1;
        Vector2Int South =2,1;

		MazeCell dataBaseMatrix=new MazeCell();
        List<String> nextCommandList = new List<String>();

        if ((sensorData[1, 2] == 0) && (dataBaseMatrix.GetCell(East).isVisited()) == false)
        {
            nextCommandList.Add("East");
            saveSensorData(sensorData, 1, 2);
        }
        else if ((sensorData[1, 0] == 0) && (dataBaseMatrix.GetCell(West).isVisited()) == false)
        {
            nextCommandList.Add("West");
            saveSensorData(sensorData, 1, 0);
        }
        else if ((sensorData[0, 1] == 0) && (dataBaseMatrix.GetCell(North).isVisited()) == false)
        {
            nextCommandList.Add("North");
            saveSensorData(sensorData, 0, 1);
        }
        else if ((sensorData[2, 1] == 0) && (dataBaseMatrix.GetCell(South).isVisited()) == false)
        {
            nextCommandList.Add("South");
            saveSensorData(sensorData, 2, 1);
        }
        else
        {
            DepthFirstSearch findPath = new DepthFirstSearch();
            nextCommandList = findPath.findPathAdapter();
        }
        return nextCommandList;
    }

    public void saveSensorData(int[,] sensorData, int rowPosition, int columnPosition)
    {
        MazeExplorationMap saveSensorData = new MazeExplorationMap();
        // Current movement is updated and then sensor data stored 
        sensorData[rowPosition, columnPosition] = 4;
        saveSensorData.ProcessSensor(sensorData);
    }
}