using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TetrisOptimization.UnitTests
{
    public class TestPreciseRectangle
    {
        [Test]
        public void TestPreciseRect1()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(1)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            solver.SolveMeasurePrint(true);
            TestContext.Out.WriteLine("Message to write to log");
            Assert.Pass();
        }

        [Test]
        public void TestPreciseRect2()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(2)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            solver.SolveMeasurePrint(true);
            Assert.Pass();
        }
    }
}
