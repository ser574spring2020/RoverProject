## Drone-Data-Layer
## SER 574: Advanced Software Design

### Team Member
* Bingrui Feng (rheafeng)
* Huijing Liang (MollyLiang)
* Jiayan Wang (jywang0)
* Meng-Ze Chen (meng-ze)
* Xinkai Wang (w546296781)
* Yu-Ting Tsao (vin20777)

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

#### Sensor Team
1. 
```
public int SetSensorMatrixById(int timestamp, int sensorId, int[,] matrix)
```
For example:<br>
```
int[,] matrix = new int[4, 4] { 
{ 1, 1, 1, 1 }, 
{ 1, 0, 0, 1 }, 
{ 1, 0, 0, 1 }, 
{ 1, 1, 1, 1 } 
};
int resultCode = dbm.SetSensorMatrixById(20200420, 2, matrix);
```

2. 
```
public int[,] GetSensorMatrixById(int sensorId, int timestamp)
```
For example:<br>
```
int[,] matrix = dbm.GetSensorMatrixById(2, 20200420);
```

#### Algorithm Team
3. **Unfinish**
```
public int CreateExploredMaze(int mazeId, int[,] exploredMaze)
```

4. **Unfinish**
```
public string[][] GetMazeById(int mazeId)
```

5. **Unfinish**
```
public int UpdateMaze(int[,] updatedMaze)
```

6. **Unfinish**
```
public int UpdateCoverage(float mazeCoverage)
```

7. **Unfinish**
```
public int UpdateTimeTaken(int second)
```

8. **Unfinish**
```
public int UpdateMoveHistory(String[] path)
```

9. **Unfinish**
```
public int UpdatePoints(int points)
```

### Demo
Please take a look at UnitTest.cs file or go our own GitHub Repo for the demo.<br>
https://github.com/vin20777/Drone-Data-Layer


