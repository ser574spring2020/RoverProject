using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MazeExplorationMap
{
    MazeCell start;
    MazeCell current;
    List<MazeCell> cells;
    List<Vector2Int> moveHistory;

    public MazeExplorationMap()
    {
        this.cells = new List<MazeCell>();
        this.start = new MazeCell(0, 0);
        this.current = this.start;
        this.moveHistory = new List<Vector2Int>();
    }

    /**
     * <returns>A Vector2Int representation of the robot's current position</returns>
     */
    public Vector2Int GetCurrentPosition()
    {
        return this.current.position;
    }

    /**
     * Applies sensor readings to the current cell
     * Sensor values are indicated by a 3x3 bool array indicating open adjacent cells
     * Axis 0 corresponds to the X direction, and axis 1 corresponds to the Y direction
     * 
     * For example, the following array indicates a cell with three open adjacent cells,
     * in the +x and -x directions and the +y direction
     * {{F, T, F},
     *  {T, F, F},
     *  {F, T, F}}
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
            this.cells.AddRange(current.Visit(sensorReading));
            return true;
        } else
        {
            return false;
        }
    }

    /**
     * Check if it is possible to move in the relative direction 
     */
    public bool CheckOpening(Vector2Int relativeMove)
    {
        return (this.current.GetNeighbor(relativeMove) != null);
    }

    /**
     * Checks if the cell in the given relative position has been visited
     */
    public bool CheckVisited(Vector2Int relativeMove)
    {
        return this.current.GetNeighbor(relativeMove).IsVisited();
    }

    /**
     * Executes the given relative move, a cell exists at that position
     * The cell will not be marked as visited until ProcessSensor is called
     * on the new position.
     * Returns true if the move was successful, false if failed
     */
    public bool MoveRelative(Vector2Int relativeMove)
    {
        MazeCell nextCell = this.current.GetNeighbor(relativeMove);
        if (nextCell != null)
        {
            this.current = nextCell;
            return true;
        }
        return false;
    }

    public Vector2Int[] GetMoveHistoryArray()
    {
        Vector2Int[] moves = new Vector2Int[this.moveHistory.Count];
        this.moveHistory.CopyTo(moves);
        return moves;
    }
}

public class MazeCell
{
    internal Vector2Int position;
    bool visited;
    MazeCell[,] neighbors;

    public MazeCell(int x, int y)
    {
        this.position = new Vector2Int(x, y);
        this.visited = false;
        this.neighbors = new MazeCell[3, 3];
    }

    /**
     * Visit this cell, adding adjacent cells to the maze
     * Returns a list containing any new cells that were found
     */
    public List<MazeCell> Visit(bool[,] sensorReading)
    {
        List<MazeCell> newNeighbors = new List<MazeCell>();
        this.visited = true;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (x == 1 && y == 1)
                {
                    continue;   // skip the the center, which is this cell
                }
                if (sensorReading[x, y] == true && this.neighbors[x, y] == null)
                {
                    MazeCell neighbor = new MazeCell(position.x + x - 1, position.y + y - 1);   // create 
                    this.neighbors[x, y] = neighbor;
                    neighbor.neighbors[2 - x, 2 - y] = this;
                    newNeighbors.Add(neighbor);
                }
            }
        }
        return newNeighbors;
    }

    /**
     * 
     */
    public MazeCell GetNeighbor(Vector2Int relativePosition)
    {
        try
        {
            return this.neighbors[relativePosition.x + 1, relativePosition.y + 1];
        } catch (IndexOutOfRangeException e)
        {
            Console.WriteLine("Invalid neighbor relative position: \n" + e);
            return null;
        }
    }

    public bool IsVisited()
    {
        return this.visited;
    }
}
