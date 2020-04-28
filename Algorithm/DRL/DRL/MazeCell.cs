using UnityEngine;
using RandomSystem = System.Random;

namespace Algorithms
{
    public class MazeCell
    {
        private Vector2Int _position;
        private bool _visited;
        private bool _isWall = false;
        
        public MazeCell(int x, int y)
        {
            this._position = new Vector2Int(x, y);
            this._visited = false;
        }

        //change the cell to a wall
        public void MakeWall()
        {
            this._isWall = true;
        }
        
        public bool IsWallCell()
        {
            return this._isWall;
        }

        //marks the _visited label of the cell as True
        public void Visit()
        {
            this._visited = true;
        }
        
        public bool IsVisited()
        {
            return this._visited;
        }
        
        public Vector2Int GetPosition()
        {
            return new Vector2Int(_position.x, _position.y);
        }
    }
}