using System;
using Algorithms;
using NUnit.Framework;
using UnityEngine;

namespace DRL
{
    [TestFixture(30, 30, 0.5f)]
    [TestFixture(30, 30, 0.5f)]
    public class DrlTests
    {
        private int _rows, _cols;
        private float _threshold;
        private int[,] result;

        public DrlTests(int x, int y, float threshold, int cols, int rows)
        {
            this._threshold = threshold;
            this._cols = cols;
            this._rows = rows;
        }

        [Test]
        public void TestArguments()
        {
            
        }

        [Test]
        public void CheckArrayConvert()
        {
            ExploredMap em = new ExploredMap(new Vector2Int(3, 4), new Vector2Int(0, 1));
            int[,] result = em.GetMazeArray();
            Assert.That(result.GetLength(0), Is.EqualTo(3));
            Assert.That(result.GetLength(1), Is.EqualTo(4));
            Assert.That(result[0, 1], Is.EqualTo(2));
            Assert.That(result[0, 0], Is.EqualTo(-1));
        }
        
    }
}