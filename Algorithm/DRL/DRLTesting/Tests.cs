using System;
using Algorithms;
using NUnit.Framework;
using UnityEngine;

namespace DRLTesting
{
    [TestFixture]
    public class Tests
    {
        private Exploration _exploration;

        [SetUp]
        public void SetUp()
        {
            _exploration = new Exploration(30, 30);
        }

        [Test]
        public void Test1()
        {
            for(int i=0;i<3;i++){
                for (int j = 0; j < 3; j++)
                {
                   
                }
                
            }
        }
    }
}