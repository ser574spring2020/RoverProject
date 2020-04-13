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
        
    }
}