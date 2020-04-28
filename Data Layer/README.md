## Drone-Data-Layer
## SER 574: Advanced Software Design

### Purpose
**Provide a data layer library for the other team to use.**

### Installation
Step 1. Pull the whole **Data layer** folder under the project assets folder.

Step 2. Create an instance of DataBaseManager and connect to Rover.db, for example:
```
private DataBaseManager dbm;
dbm = new DataBaseManager();
dbm.ConnectToDB("Rover.db");
```

Step 3. Refer the belows functions for further usage.

### Docs
Currently, **NINE**(7 Undone) APIs are provided for accessing database.
Please see inside the documentation folder for more detail.

#### Sensor
1. 
```C#
public int SetSensorMatrixById(int timestamp, int sensorId, int[,] matrix)
```
For example:<br>
```C#
int[,] matrix = new int[4, 4] { 
{ 1, 1, 1, 1 }, 
{ 1, 0, 0, 1 }, 
{ 1, 0, 0, 1 }, 
{ 1, 1, 1, 1 } 
};
int resultCode = dbm.SetSensorMatrixById(20200420, 2, matrix);
```

2. 
```C#
public int[,] GetSensorMatrixById(int sensorId, int timestamp)
```
For example:<br>
```C#
int[,] matrix = dbm.GetSensorMatrixById(2, 20200420);
```

#### Algorithm
3.
```C#
public int CreateExploredMaze(int mazeId, int[,] exploredMaze)
```
For example:<br>
```C#
int mazeId = 3;
int[,] exploredMaze = new int[4, 4] { 
{ 1, 1, -1, -1 }, 
{ 1, 0, 0, -1 }, 
{ 1, 0, 0, 1 }, 
{ 1, 1, 1, 1 } 
};
int resultCode = dbm.CreateExploredMaze(mazeId, exploredMaze);
```

4.
```C#
public string[][] GetMazeById(int mazeId)
```
For example:<br>
```C#
int mazeId = 3;
int[,] storedMaze = dbm.GetMazeById(mazeId);
```

5.
```C#
public int UpdateMaze(int[,] updatedMaze)
```
For example:<br>
```C#
int mazeId = 3;
int[,] updatedMaze = new int[4, 4] { 
{ 1, 1, 1, 1 }, 
{ 1, 0, 0, 1 }, 
{ 1, 0, 0, 1 }, 
{ 1, 1, 1, 1 } 
};
int resultCode = dbm.UpdateMaze(mazeId, updatedMaze);
```

6.
```C#
public int UpdateCoverage(float mazeCoverage)
```
For example:<br>
```C#
float mazeCoverage = 0.4F;
int resultCode = dbm.UpdateCoverage(mazeCoverage);
```

7.
```C#
public int UpdateTimeTaken(int second)
```
For example:<br>
```C#
int second = 101;
int resultCode = dbm.UpdateTimeTaken(second);
```

8.
```C#
public int UpdateMoveHistory(String[] path)
```
For example:<br>
```C#
String[] path = new String[5] { "East", "East", "North", "East", "South" };
int resultCode = dbm.UpdateMoveHistory(path);
```

9.
```C#
public int UpdatePoints(int points)
```
For example:<br>
```C#
int points = 999;
int resultCode = dbm.UpdatePoints(points);
```

### Team Member
* Bingrui Feng (rheafeng)
* Huijing Liang (MollyLiang)
* Jiayan Wang (jywang0)
* Meng-Ze Chen (meng-ze)
* Xinkai Wang (w546296781)
* Yu-Ting Tsao (vin20777)

### Demo
Please take a look at UnitTest.cs file or go our own GitHub Repo for the demo version.<br>
https://github.com/vin20777/Drone-Data-Layer


