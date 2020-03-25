using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MazeExplorationMap
{
    Vector2Int robotPosition;
    List<MazeCell> cells;
    List<Vector2Int> moveHistory;
    MazeCell[,] mazeMap;

    public MazeExplorationMap(Vector2Int mazeDimension, Vector2Int robotPosition)
    {
        mazeMap = new MazeCell[mazeDimension.x, mazeDimension.y];
        this.robotPosition = new Vector2Int(robotPosition.x, robotPosition.y);
        this.cells = new List<MazeCell>();
        this.moveHistory = new List<Vector2Int>();
    }

    /**
     * <returns>A Vector2Int representation of the robot's current position</returns>
     */
    public Vector2Int GetCurrentPosition()
    {
        return new Vector2Int(robotPosition.x, robotPosition.y);
    }

    /**
     * Applies sensor readings to the current cell
     * Sensor values are indicated by a 3x3 bool array indicating open adjacent cells
     * Axis 0 corresponds to the X direction, and axis 1 corresponds to the Y direction
     * 
     * For example, the following array indicates a cell with three open adjacent cells,
     * in the +x and -x directions and the +y direction
     * {{F, T, F}, {T, F, F}, {F, T, F}}
     * 
     * For each newly discovered adjacent cell, and unvisited cell is added to the map
     * with its corresponding absolute position (position relative to the start cell)
     *
     * <param name="sensorReading">The sensor reading for the current robot position.</param>
     * <returns>true if the sensor reading was successfully applied</returns>
     */
    public bool ProcessSensor(bool[,] sensorReading)
    {
        if (sensorReading.GetLength(0) == 3 && sensorReading.GetLength(1) == 3)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (x == 1 && y == 1)
                    {
                        continue;   // skip the the center, which is this cell
                    }
                    if (sensorReading[x, y] == true && this.mazeMap[x, y] == null)
                    {
                        int xMaze = robotPosition.x + x - 1;
                        int yMaze = robotPosition.y + y - 1;
                        MazeCell neighbor = new MazeCell(xMaze, yMaze);   // create 
                        mazeMap[xMaze, yMaze] = neighbor;
                        cells.Add(neighbor);
                    }
                }
            }
            return true;
        } else
        {
            return false;
        }
    }

    private bool CheckAbsolutePosition(Vector2Int position)
    {
        return position.x >= 0
                && position.x < mazeMap.GetLength(0)
                && position.y >= 0
                && position.y < mazeMap.GetLength(1);
    }

    private bool CheckMoveBounds(Vector2Int relativeMove)
    {
        Vector2Int newPosition = robotPosition + relativeMove;
        return CheckAbsolutePosition(newPosition);
    }

    /**
     * Check if it is possible to move in the relative direction 
     */
    public bool CheckOpening(Vector2Int relativeMove)
    {
        if (CheckMoveBounds(relativeMove))
        {
            Vector2Int newPosition = robotPosition + relativeMove;
            return mazeMap[newPosition.x, newPosition.y] != null;
        }
        return false;
    }

    /**
     * Checks if the cell in the given relative position has been visited
     */
    public bool CheckVisited(Vector2Int relativeMove)
    {
        if (CheckMoveBounds(relativeMove))
        {
            Vector2Int newPosition = robotPosition + relativeMove;
            return mazeMap[newPosition.x, newPosition.y].IsVisited();
        }
        return false;
    }

    /**
     * Executes the given relative move, a cell exists at that position
     * The cell will not be marked as visited until ProcessSensor is called
     * on the new position.
     * Returns true if the move was successful, false if failed
     */
    public bool MoveRelative(Vector2Int relativeMove)
    {
        if (CheckMoveBounds(relativeMove))
        {
            Vector2Int newPosition = robotPosition + relativeMove;
            robotPosition = newPosition;
            moveHistory.Add(relativeMove);
            return true;
        }
        return false;
    }

    public MazeCell GetCell(Vector2Int absolutePosition)
    {
        if (CheckAbsolutePosition(absolutePosition))
        {
            return mazeMap[absolutePosition.x, absolutePosition.y];
        }
        return null;
    }

    public Vector2Int[] GetMoveHistoryArray()
    {
        Vector2Int[] moves = new Vector2Int[this.moveHistory.Count];
        this.moveHistory.CopyTo(moves);
        return moves;
    }

    public List<MazeCell> GetUnvisitedCells()
    {
        List<MazeCell> unvisited = new List<MazeCell>();
        foreach (MazeCell cell in this.cells)
        {
            if (!cell.IsVisited())
            {
                unvisited.Add(cell);
            }
        }
        return unvisited;
    }
}

public class MazeCell
{
    internal Vector2Int position;
    bool visited;

    public MazeCell(int x, int y)
    {
        this.position = new Vector2Int(x, y);
        this.visited = false;
    }

    public void Visit()
    {
        visited = true;
    }

    public bool IsVisited()
    {
        return this.visited;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(position.x, position.y);
    }
}
