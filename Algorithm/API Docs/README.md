# API Doc: Algorithms Team

## Exploration : Class

Namespace	:	Algorithms

Assembly 	:	DRL.dll

Class for the Exploration Algorithm


<table>
  <tr>
   <td><strong>Class</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>ExploredMap exploredMap;
   </td>
   <td>Map of the explored maze. (See ExploredMap)
   </td>
  </tr>
  <tr>
   <td>Exploration.GetNextCommand()
   </td>
   <td>Return the next command for the robot.
   </td>
  </tr>
  <tr>
   <td>Exploration.GetExploredMap()
   </td>
   <td>Returns the exploredMap object.
   </td>
  </tr>
</table>



### Exploration.MazeCell : Constructor

Creates an instance of the ExploredMap (See ExploredMap)


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >public Exploration(int rows, int cols)
   </td>
  </tr>
  <tr>
   <td><strong>Parameters</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>rows : <em>int</em>
   </td>
   <td>Number of rows in the original maze that is being explored
   </td>
  </tr>
  <tr>
   <td>cols : <em>int</em>
   </td>
   <td>Number of columns in the original maze that is being explored
   </td>
  </tr>
  <tr>
   <td colspan="2" >Exploration exploration = new Exploration(30, 40);
   </td>
  </tr>
</table>



### Exploration.GetNextCommand : Method

Returns the next command for the robot based on the sensor data and the explored map.


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >public String GetNextCommand(int[,] sensorData)
   </td>
  </tr>
  <tr>
   <td><strong>Example</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >Exploration exploration = new Exploration(30, 40);
<p>
... \
exploration.GetNextCommand(sensorData);
   </td>
  </tr>
  <tr>
   <td><strong>Return Value</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>“North”
   </td>
   <td>Move the robot in the<strong> north </strong>direction.
   </td>
  </tr>
  <tr>
   <td>“West”
   </td>
   <td>Move the robot in the <strong>west </strong>direction.
   </td>
  </tr>
  <tr>
   <td>“East”
   </td>
   <td>Move the robot in the <strong>east</strong> direction.
   </td>
  </tr>
  <tr>
   <td>“South”
   </td>
   <td>Move the robot in the <strong>south</strong> direction.
   </td>
  </tr>
</table>



### Exploration.GetExploredMap : Method

Returns the exploredMap object.


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >public ExploredMap GetExploredMap()
   </td>
  </tr>
  <tr>
   <td><strong>Example</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >Exploration exploration = new Exploration(30, 40); \
ExploredMap exploredMap;
<p>
... \
exploredMap = exploration.GetExploredMap();
   </td>
  </tr>
  <tr>
   <td><strong>Return Value</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>ExploredMap
   </td>
   <td>Contains the information about the map explored so far. (See ExploredMap)
   </td>
  </tr>
</table>

***

## ExploredMap : Class

Namespace	:	Algorithms

Assembly 	:	DRL.dll

Class for the ExplorationMap.

Used to store all the data about the maze explored by the robot.


<table>
  <tr>
   <td colspan="2" ><strong>Class</strong>
   </td>
  </tr>
  <tr>
   <td>MazeCell[,] mazeMap
   </td>
   <td>Contains the different cells in the maze.
   </td>
  </tr>
  <tr>
   <td>List&lt;Vector2Int> moveHistory
   </td>
   <td>Returns the exploredMap object.
   </td>
  </tr>
  <tr>
   <td>Vector2Int robotPosition
   </td>
   <td>Stores the current position of the robot.
   </td>
  </tr>
</table>



### ExploredMap.GetCurrentPosition : Method

Returns the current position of the robot.


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >public Vector2Int GetCurrentPosition()
   </td>
  </tr>
  <tr>
   <td><strong>Example</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >ExploredMap exploredMap;
<p>
... \
Debug.Log(exploredMap.GetCurrentPosition();
   </td>
  </tr>
  <tr>
   <td><strong>Return Value</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>eg. Vector2Int(1,2)
   </td>
   <td>Position of the maze cell is (1, 2) \
1 is the position of the robot on x-axis
<p>
2 is the position of the robot on y-axis
   </td>
  </tr>
</table>



### ExploredMap.MoveRelative : Method

Moves the robot in the specified direction.


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >public bool MoveRelative(Vector2Int relativeMove)
   </td>
  </tr>
  <tr>
   <td><strong>Parameters</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>relativeMove : Vector2Int
<p>
               Vector2Int.up
<p>
               Vector2Int.down
<p>
               Vector2Int.left
<p>
               Vector2Int.right
   </td>
   <td>The direction in which you want to move the robot.
   </td>
  </tr>
  <tr>
   <td><strong>Example</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >ExploredMap exploredMap;
<p>
…
<p>
Debug.Log(exploredMap.GetCurrentPosition);    // Suppose (1,2) \
exploredMap.MoveRelative(new Vector2Int.Up)
<p>
Debug.Log(exploredMap.GetCurrentPosition);    // Prints  (1,1)
   </td>
  </tr>
</table>



### ExploredMap.GetMoveHistory : Method

Moves the robot in the specified direction.


<table>
  <tr>
   <td colspan="3" ><strong>Usage</strong>
   </td>
  </tr>
  <tr>
   <td colspan="3" >public Vector2Int[] GetMoveHistoryArray()
   </td>
  </tr>
  <tr>
   <td colspan="3" ><strong>Example</strong>
   </td>
  </tr>
  <tr>
   <td colspan="3" >ExploredMap exploredMap;
<p>
...
<p>
Vector2Int[] moveHistory = exploredMap.GetMoveHistory();
   </td>
  </tr>
  <tr>
   <td colspan="3" ><strong>Return Value</strong>
   </td>
  </tr>
  <tr>
   <td colspan="2" >eg. [Vector2Int.up, Vector2.left]
   </td>
   <td>Robot went up(North) and then took a left(West).
   </td>
  </tr>
</table>

***

## MazeCell : Class

Namespace	:	Algorithms

Assembly 	:	DRL.dll

Class for the cells of explored maze used by Algorithms


<table>
  <tr>
   <td><strong>Class</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>Vector2Int position
   </td>
   <td>Position of the MazeCell.
   </td>
  </tr>
  <tr>
   <td>bool visited
   </td>
   <td>True if MazeCell is visited. (Default: False)
   </td>
  </tr>
  <tr>
   <td>bool isWall
   </td>
   <td>True if MazeCell is a wall. (Default: False)
   </td>
  </tr>
  <tr>
   <td>MazeCell.IsWallCell()
   </td>
   <td>Returns the status of the MazeCell if it is a wall cell or not.
   </td>
  </tr>
  <tr>
   <td>MazeCell.IsVisited()
   </td>
   <td>Returns the status of the MazeCell if it is visited or not.
   </td>
  </tr>
  <tr>
   <td>MazeCell.Visit()
   </td>
   <td>Changes the visited status of the maze cell.
   </td>
  </tr>
</table>



### MazeCell.IsWallCell : Method

Returns the status of the MazeCell if it is a wall cell or not.


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >public bool isWallCell()
   </td>
  </tr>
  <tr>
   <td><strong>Example</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >MazeCell mazeCell = exploredMap.GetCell(new Vector2Int(1,2));    //See ExploredMap
<p>
... \
bool isMazeCellWall = mazeCell.isWallCell();
   </td>
  </tr>
  <tr>
   <td><strong>Return Value</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>True
   </td>
   <td>MazeCell is a wall
   </td>
  </tr>
  <tr>
   <td>False
   </td>
   <td>MazeCell is not a wall
   </td>
  </tr>
</table>



### MazeCell.IsVisited : Method

Returns the status of the MazeCell if it is visited or not.


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >public bool IsVisited()
   </td>
  </tr>
  <tr>
   <td><strong>Example</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >MazeCell mazeCell = new MazeCell(1, 1);
<p>
... \
bool isMazeCellVisted = mazeCell.isVisited();
   </td>
  </tr>
  <tr>
   <td><strong>Return Value</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>True
   </td>
   <td>MazeCell is visited.
   </td>
  </tr>
  <tr>
   <td>False
   </td>
   <td>MazeCell is not visited.
   </td>
  </tr>
</table>



### MazeCell.GetPosition : Method

Returns the position of the MazeCell.


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >public Vector2Int GetPosition()
   </td>
  </tr>
  <tr>
   <td><strong>Example</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >MazeCell mazeCell;
<p>
... \
bool mazeCellStatus = mazeCell.isVisited();
   </td>
  </tr>
  <tr>
   <td><strong>Return Value</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>eg. Vector2Int(1,2)
   </td>
   <td>Position of the maze cell is (1, 2) \
1 is the position of the robot on x-axis
<p>
2 is the position of the robot on y-axis
   </td>
  </tr>
</table>



### MazeCell.Visit : Method

Changes the visited status (MazeCell.isVisited) of the maze cell.


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
  </tr>
  <tr>
   <td>public void Visit()
   </td>
  </tr>
  <tr>
   <td><strong>Example</strong>
   </td>
  </tr>
  <tr>
   <td>MazeCell mazeCell;
<p>
…
<p>
Debug.Log(mazeCell.IsVisited());    //prints False \
mazeCell.Visit();
<p>
Debug.Log(mazeCell.IsVisited());    //prints True
   </td>
  </tr>
  <tr>
   <td><strong>Return Value</strong>
   </td>
  </tr>
  <tr>
   <td>void
   </td>
  </tr>
</table>

***

## MazeGenerator : Class

Namespace	:	Algorithms

Assembly 	:	DRL.dll

Contains the method to generate a two dimensional integer array that represents a maze.


<table>
  <tr>
   <td><strong>Class</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>MazeGenerator.GenerateMaze()
   </td>
   <td>Generates a two-dimensional array of integers (0s and 1s) with a specified threshold, rows and columns. Threshold is the density of 1s in the generated array. The generated  array represents a maze with 0s as open cells and 1s and walls.
   </td>
  </tr>
</table>



### MazeGenerator.GenerateMaze : Method

Namespace	:	Algorithms

Assembly 	:	DRL.dll

Generates a two-dimensional array of integers (0s and 1s) with a specified threshold, rows and columns. Threshold is the density of 1s in the generated array. The generated  array represents a maze with 0s as open cells and 1s and walls.


<table>
  <tr>
   <td><strong>Usage</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >public int[,] GenerateMaze(int rows, int cols, float placementThreshold)
   </td>
  </tr>
  <tr>
   <td><strong>Parameters</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td>rows : <em>int</em>
   </td>
   <td>Number of rows in the array
   </td>
  </tr>
  <tr>
   <td>cols : <em>int</em>
   </td>
   <td>Number of columns in the array
   </td>
  </tr>
  <tr>
   <td>placementThreshold : <em>float</em>
   </td>
   <td>Density of the 1s in generated array \
Range: 0-1
   </td>
  </tr>
  <tr>
   <td><strong>Example</strong>
   </td>
   <td>
   </td>
  </tr>
  <tr>
   <td colspan="2" >int[,] maze = new MazeGenerator.GenerateMaze(4, 6, 0.8);
   </td>
  </tr>
  <tr>
   <td><strong>Return Value</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>[[1, 1, 1, 1, 1, 1],
<p>
 [1, 0, 1, 0, 0, 1],
<p>
 [1, 0, 0, 0, 0, 1],
<p>
 [1, 1, 1, 1, 1, 1]]
   </td>
   <td>1s represent the walls of the maze. \
0s represent the open cells of the maze.
   </td>
  </tr>
</table>

