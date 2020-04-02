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
Currently, **FOUR** APIs are provided for accessing database (mocking data for now).
Please see inside the documentation folder for more detail. 
1. 
```
public string[][] GetMazeById(int id)
```

2. 
```
public int InsertMazeRecord(int id, int[] nodes, string[, ] edges)
```

3. 
```
public int UpdateMazeDirection(int id, string[] edges)
```

4. 
```
public int DeleteMazeById(int id)
```

### Demo
Please take a look at UnitTest.cs file or go our own GitHub Repo for the demo.<br>
https://github.com/vin20777/Drone-Data-Layer


