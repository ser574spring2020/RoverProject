using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Algorithms;
using UnityEngine;

namespace DRL.Tests
{
    [TestFixture]
    public class ExploredMapTest
    {
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

