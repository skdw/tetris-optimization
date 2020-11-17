﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;

namespace TetrisOptimization.Tests
{
    class SquareHeuristicTest
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void TestGenerateList()
        {
            HeuristicSquareSolver heuristicSquareSolver = new HeuristicSquareSolver(new List<(int, Block)>() { (1, new FiveBlock(5)), (3, new FiveBlock(3)) }, 5, 3, 0.2, 5, 0.6);
            heuristicSquareSolver.Solve();

            Assert.Pass();
        }
    }
}
