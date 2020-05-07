using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Astar
{
	public List<Vector2> result = new List<Vector2>();
	private string find;

	private class _Object
	{
		public int x
		{
			get;
			set;
		}
		public int y
		{
			get;
			set;
		}
		public double f
		{
			get;
			set;
		}
		public double g
		{
			get;
			set;
		}
		public int v
		{
			get;
			set;
		}
		public _Object p
		{
			get;
			set;
		}
		public _Object(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	private _Object[] diagonalSuccessors(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, _Object[] result, int i)
	{
		if (xN)
		{
			if (xE && grid[N][E] == 0)
			{
				result[i++] = new _Object(E, N);
			}
			if (xW && grid[N][W] == 0)
			{
				result[i++] = new _Object(W, N);
			}
		}
		if (xS)
		{
			if (xE && grid[S][E] == 0)
			{
				result[i++] = new _Object(E, S);
			}
			if (xW && grid[S][W] == 0)
			{
				result[i++] = new _Object(W, S);
			}
		}
		return result;
	}

	private _Object[] diagonalSuccessorsFree(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, _Object[] result, int i)
	{
		xN = N > -1;
		xS = S < rows;
		xE = E < cols;
		xW = W > -1;

		if (xE)
		{
			if (xN && grid[N][E] == 0)
			{
				result[i++] = new _Object(E, N);
			}
			if (xS && grid[S][E] == 0)
			{
				result[i++] = new _Object(E, S);
			}
		}
		if (xW)
		{
			if (xN && grid[N][W] == 0)
			{
				result[i++] = new _Object(W, N);
			}
			if (xS && grid[S][W] == 0)
			{
				result[i++] = new _Object(W, S);
			}
		}
		return result;
	}

	private _Object[] nothingToDo(bool xN, bool xS, bool xE, bool xW, int N, int S, int E, int W, int[][] grid, int rows, int cols, _Object[] result, int i)
	{
		return result;
	}

	private _Object[] successors(int x, int y, int[][] grid, int rows, int cols)
	{
		int N = y - 1;
		int S = y + 1;
		int E = x + 1;
		int W = x - 1;

		bool xN = N > -1 && grid[N][x] == 0;
		bool xS = S < rows && grid[S][x] == 0;
		bool xE = E < cols && grid[y][E] == 0;
		bool xW = W > -1 && grid[y][W] == 0;

		int i = 0;

		_Object[] result = new _Object[8];

		if (xN)
		{
			result[i++] = new _Object(x, N);
		}
		if (xE)
		{
			result[i++] = new _Object(E, y);
		}
		if (xS)
		{
			result[i++] = new _Object(x, S);
		}
		if (xW)
		{
			result[i++] = new _Object(W, y);
		}

		_Object[] obj =
			(this.find == "Diagonal" || this.find == "Euclidean") ? diagonalSuccessors(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i) :
			(this.find == "DiagonalFree" || this.find == "EuclideanFree") ? diagonalSuccessorsFree(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i) :
																			 		nothingToDo(xN, xS, xE, xW, N, S, E, W, grid, rows, cols, result, i);

		return obj;
	}

	private double diagonal(_Object start, _Object end)
	{
		return Math.Max(Math.Abs(start.x - end.x), Math.Abs(start.y - end.y));
	}

	private double euclidean(_Object start, _Object end)
	{
		var x = start.x - end.x;
		var y = start.y - end.y;

		return Math.Sqrt(x * x + y * y);
	}

	private double manhattan(_Object start, _Object end)
	{
		return Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y);
	}

	public Astar(int[][] grid, int[] s, int[] e, string f)
	{
		this.find = (f == null) ? "Diagonal" : f;

		int cols = grid[0].Length;
		int rows = grid.Length;
		int limit = cols * rows;
		int length = 1;

		List<_Object> open = new List<_Object>();
		open.Add(new _Object(s[0], s[1]));
		open[0].f = 0;
		open[0].g = 0;
		open[0].v = s[0] + s[1] * cols;

		_Object current;

		List<int> list = new List<int>();

		double distanceS;
		double distanceE;

		int i;
		int j;

		double max;
		int min;

		_Object[] next;
		_Object adj;

		_Object end = new _Object(e[0], e[1]);
		end.v = e[0] + e[1] * cols;

		bool inList;

		do
		{
			max = limit;
			min = 0;

			for (i = 0; i < length; i++)
			{
				if (open[i].f < max)
				{
					max = open[i].f;
					min = i;
				}
			}

			current = open[min];
			open.RemoveAt(min);

			if (current.v != end.v)
			{
				--length;
				next = successors(current.x, current.y, grid, rows, cols);

				for (i = 0, j = next.Length; i < j; ++i)
				{
					if (next[i] == null)
					{
						continue;
					}

					(adj = next[i]).p = current;
					adj.f = adj.g = 0;
					adj.v = adj.x + adj.y * cols;
					inList = false;

					foreach (int key in list)
					{
						if (adj.v == key)
						{
							inList = true;
						}
					}

					if (!inList)
					{
						if (this.find == "DiagonalFree" || this.find == "Diagonal")
						{
							distanceS = diagonal(adj, current);
							distanceE = diagonal(adj, end);
						}
						else if (this.find == "Euclidean" || this.find == "EuclideanFree")
						{
							distanceS = euclidean(adj, current);
							distanceE = euclidean(adj, end);
						}
						else
						{
							distanceS = manhattan(adj, current);
							distanceE = manhattan(adj, end);
						}

						adj.f = (adj.g = current.g + distanceS) + distanceE;
						open.Add(adj);
						list.Add(adj.v);
						length++;
					}
				}
			}
			else
			{
				i = length = 0;
				do
				{
					this.result.Add(new Vector2(current.x, current.y));
				}
				while ((current = current.p) != null);
				result.Reverse();
			}
		}
		while (length != 0);
	}

}
