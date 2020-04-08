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
        this.mazeMap[robotPosition.x, robotPosition.y] = new MazeCell(robotPosition, false);
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
     * Sensor values are indicated by a 3x3 int array indicating the wall distances
     * Axis 0 corresponds to the X direction, and axis 1 corresponds to the Y direction
     * 
     * For example, the following array indicates a cell with three open adjacent cells,
     * in the +x and -x directions and the +y direction
     * {{0, 1, 0}, {1, 0, 0}, {0, 1, 0}}
     * 
     * For each newly discovered adjacent cell, and unvisited cell is added to the map
     * with its corresponding absolute position (position relative to the start cell)
     *
     * <param name="sensorReading">The sensor reading for the current robot position.</param>
     * <returns>true if the sensor reading was successfully applied</returns>
     */
    public bool ProcessSensor(int[,] sensorReading)
    {
        if (sensorReading.GetLength(0) == 3 && sensorReading.GetLength(1) == 3)
        {
            Vector2Int[] sensorDirections =
            {
                Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down   // directions of valid sensor readings
            };
            foreach (Vector2Int direction in sensorDirections)
            {
                int distance = sensorReading[1 + direction.x, 1 + direction.y];
                Vector2Int position = new Vector2Int(this.robotPosition.x, this.robotPosition.y);
                for (int i = 0; i < distance; i++)
                {
                    position += direction;
                    if (CheckAbsolutePosition(position))
                    {
                        if (mazeMap[position.x, position.y] == null)    // prevents already visited cells from being reset
                        {
                            mazeMap[position.x, position.y] = new MazeCell(position, false);
                        }
                    } else
                    {
                        break;
                    }
                }
                position += direction;
                if (CheckAbsolutePosition(position))
                {
                    if (mazeMap[position.x, position.y] == null)
                    {
                        mazeMap[position.x, position.y] = new MazeCell(position, true);
                    }
                }
            }
            mazeMap[robotPosition.x, robotPosition.y].Visit();
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
        System.Console.WriteLine("newPosition: " + newPosition);
        return CheckAbsolutePosition(newPosition);
    }

    public override string ToString()
    {
        string text = "";
        for (int y = mazeMap.GetLength(1) - 1; y > 0; y--)
        {
            for (int x = 0; x < mazeMap.GetLength(0); x++)
            {
                text += (mazeMap[x, y] != null) ? "T" : "_";
            }
            text += "\n";
        }
        return text;
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
    bool isWall;

    public MazeCell(Vector2Int position, bool isWall)
    {
        this.position = new Vector2Int(position.x, position.y);
        this.visited = isWall;  // Walls should not be included in the list of unvisited cells
        this.isWall = isWall;
    }

    public void Visit()
    {
        visited = true;
    }

    public bool IsVisited()
    {
        return this.visited;
    }

    public bool IsWall()
    {
        return this.isWall;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(position.x, position.y);
    }
}
