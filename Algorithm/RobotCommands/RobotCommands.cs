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
 public List<String> computeDirection(int[,] sensorData)
    {
		List<String> nextCommandList = new List<String>();

        if ((sensorData[1,2])] == 0) && (dataBaseMatrix[right] != 4))
        {
			nextCommandList.Add("East");
			saveSensorData(sensorData);
		
        }
        else if ((sensorData[1,2])] == 0) && (dataBaseMatrix[Left] != 4))
        {
            nextCommandList.Add("West");
			saveSensorData(sensorData);
        }
        else if ((sensorData[1,2])] == 0) && (dataBaseMatrix[North] != 4))
        {
			nextCommandList.Add("North");
			saveSensorData(sensorData);
        }
        else if ((sensorData[1,2])] == 0) && (dataBaseMatrix[South] != 4))
        {
			nextCommandList.Add("South");
			saveSensorData(sensorData);
        }
        else
        {
			DepthFirstSearch findPath=new DepthFirstSearch();
			nextCommandList=findPath.findPathAdapter();
        }
		return nextCommandList;
	}
	
	public void saveSensorData(int[,] sensorData, int currentPosition)
	{
		
		
	}
}