using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Algo_find
{
    public enum PathDirection { Up = 0, Down = 1, Left = 2, Right = 3, TopRight = 4, BottomRight = 5, BottomLeft = 6, TopLeft = 7 }
    public struct PathNode
    {
        /// <summary>
        /// moment to this node
        /// </summary>
        public float G;
        /// <summary>
        /// distance to the target
        /// </summary>
        public float H;

        /// <summary>
        /// total score for this node ie G + H
        /// </summary>
        public float F;
        public Point? Parent;
        public Point GridLocation;

        public PathNode(float movementToThisNode, float distancetToTarget, Point? parent, Point gridLocation)
        {
            G = movementToThisNode;
            H = distancetToTarget;
            F = G + H;
            Parent = parent;
            GridLocation = gridLocation;
        }
    }

    public interface PathFinding
    {
        List<PathNode> GetPath(bool[,] grid, Point start, Point end);
    }

    public class PathFindingAStar : PathFinding
    {
        private List<Point> _directions = new List<Point>() { new Point(0,-1), new Point(0, 1), new Point(-1, 0), new Point(1, 0),
    new Point(1,-1),new Point(1,1),new Point(-1,1),new Point(-1,-1)};

        private bool _haveSetMovmentCosts = false;
        /// <summary>
        /// weathe we can have diaganal movement or not
        /// 1 = no diaganal movment
        /// 2 = allow diaganal movement
        /// </summary>
        private byte _allowDiaganalMovment = 1;

        private float[] _movementCosts = new float[8];



        public PathFindingAStar(float normalMovement, float directionalMovement, bool allowDiaganalMovement)
        {


            //set up down left right movement costs
            _movementCosts[0] = normalMovement;
            _movementCosts[1] = normalMovement;
            _movementCosts[2] = normalMovement;
            _movementCosts[3] = normalMovement;

            //set all directional movement costs
            _movementCosts[4] = directionalMovement;
            _movementCosts[5] = directionalMovement;
            _movementCosts[6] = directionalMovement;
            _movementCosts[7] = directionalMovement;

            if (allowDiaganalMovement)
                _allowDiaganalMovment = 2; //set to allow diagal movment
            else
                _allowDiaganalMovment = 1; //disable diagal movment
            _haveSetMovmentCosts = true;
        }

        private bool ValidNode(Point node, bool[,] grid)
        {
            if (node.X < 0 || node.Y < 0)
                return false;

            if (node.X >= Game1.GridSize || node.Y >= Game1.GridSize)
                return false;

            if (grid[node.X, node.Y])
                return false;
            else
                return true;
        }

        private List<PathNode> GetNeighbouringNodes(PathNode node, bool[,] grid, Point end)
        {
            List<PathNode> neighboruingNodes = new List<PathNode>();

            for (int i = 0; i < 4 * _allowDiaganalMovment; i++) //loop through evey possible node
            {
                if (ValidNode(node.GridLocation + _directions[i], grid)) //check each node to see if its valid or not
                {
                    neighboruingNodes.Add(new PathNode(_movementCosts[i] + node.G,
                        TargetDistance(node.GridLocation + _directions[i], end), node.GridLocation, node.GridLocation + _directions[i])); //add the new node
                }
            }

            return neighboruingNodes;
        }

        private PathNode FindBestNode(ref List<PathNode> nodes, int start)
        {
            float bestf = float.MaxValue;
            int bestnode = -1;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].F < bestf)
                {
                    bestf = nodes[i].F;
                    bestnode = i;
                }

            }

            /* float shortestdistance = float.MaxValue;
             for (int i =0; i < nodes.Count; i++)
                 if (nodes[i].F == bestf)
                 {
                     if (nodes[i].H < shortestdistance)
                     {
                         shortestdistance = nodes[i].H;
                         bestnode = i;
                     }
                 }*/
            return nodes[bestnode];
        }

        private float TargetDistance(Point start, Point end)
        {
            return MathHelper.Distance(start.X, end.X) +
              MathHelper.Distance(start.Y, end.Y);
        }


        public List<PathNode> GetPath(bool[,] grid, Point start, Point end)
        {
            if (!_haveSetMovmentCosts)
                throw new Exception("Movement costs need to be set first");


            List<PathNode> Tmplist = new List<PathNode>();

            List<PathNode> _openList = new List<PathNode>();
            List<PathNode> _closedList = new List<PathNode>();

            _openList.Add(new PathNode(0, TargetDistance(start, end), null,
                new Point((int)start.X, (int)start.Y))); //create the first node


            PathNode currentNode;
            int nodeIndex = -1;
            while (_openList.Count > 0) //while there is something in the open list
            {
                currentNode = FindBestNode(ref _openList, 0); //get the nearts node to the destination

                _closedList.Add(currentNode); //add that node
                _openList.Remove(currentNode);


                if (currentNode.GridLocation == new Point((int)end.X, (int)end.Y)) //if we have reached the destination
                {
                    List<PathNode> path = new List<PathNode>();
                    PathNode pn = currentNode;
                    path.Add(pn);
                    int index;
                    while (pn.Parent != null)
                    {
                        index = _closedList.FindIndex(t => t.GridLocation == pn.Parent);
                        pn = _closedList[index];
                        path.Add(pn);
                    }
                    path.Reverse();
                    return path;
                }

                Tmplist = GetNeighbouringNodes(currentNode, grid, end);

                for (int i = 0; i < Tmplist.Count; i++)
                {
                    if (_closedList.FindIndex(t => t.GridLocation == Tmplist[i].GridLocation) < 0 && _closedList != null)
                    {
                        nodeIndex = _openList.FindIndex(t => t.GridLocation == Tmplist[i].GridLocation);//find the first node that has this grid location
                        if (nodeIndex < 0) //if not in the list
                        {
                            _openList.Insert(0, Tmplist[i]); //add to the open list
                        }
                        else
                            if (Tmplist[i].F < _openList[nodeIndex].F) //if the current node took less moves to get there than the old node
                        {
                            _openList[nodeIndex] = Tmplist[i]; //replace the old node
                        }
                    }
                }
            }
            return null;
        }
    }
}