using System.Collections;
using System.Collections.Generic;

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
		RobotCommands obj=new RobotCommands();
        foreach (var item in obj.computeDirection(sensorData))
        {
            Console.WriteLine("The nodes are:" + item);
        }
    }

    public List<String> computeDirection(int[,] sensorData)
    {
        // For unit testing
        int[] dataBaseMatrix = new int[1];
        int Left, North, South;
        int right = Left = North = South = 0;
        dataBaseMatrix[right] = 0;

        List<String> nextCommandList = new List<String>();

        if ((sensorData[1, 2] == 0) && (dataBaseMatrix[right] != 4))
        {
            nextCommandList.Add("East");
            // update the current movement in DB and then save the sensor data 
            saveSensorData(sensorData, 1, 2);
        }
        else if ((sensorData[1, 0] == 0) && (dataBaseMatrix[Left] != 4))
        {
            nextCommandList.Add("West");
            // update the current movement in DB and then save the sensor data 
            saveSensorData(sensorData, 1, 0);
        }
        else if ((sensorData[0, 1] == 0) && (dataBaseMatrix[North] != 4))
        {
            nextCommandList.Add("North");
            // update the current movement in DB and then save the sensor data 
            saveSensorData(sensorData, 0, 1);
        }
        else if ((sensorData[2, 1] == 0) && (dataBaseMatrix[South] != 4))
        {
            nextCommandList.Add("South");
            // update the current movement in DB and then save the sensor data 
            saveSensorData(sensorData, 2, 1);
        }
        else
        {
            // DepthFirstSearch findPath=new DepthFirstSearch();
            // nextCommandList=findPath.findPathAdapter();
        }
        return nextCommandList;
    }

    public void saveSensorData(int[,] sensorData, int rowPosition, int columnPosition)
    {
        //	save sensor data as it is
        // update the rowPosition and columnPosition with 4 
    }
}