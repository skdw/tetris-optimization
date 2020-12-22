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
            var board = solver.SolveMeasurePrint(true);
            var hash = board.GetHash();
            Assert.AreEqual("c8c362c52a33bdd8c9fa09b44caf5284e23fd17e4976103c51b9f2fadf957033", hash);
        }
        [Test]
        public void TestTrimBlock()
        {
            var matrix = new bool[10, 3];
            matrix[0, 0] = true;
            matrix[1, 1] = true;
            matrix[2, 2] = true;
            var test = CuttingRectangle.TrimBlock(matrix,true);
            Assert.IsNotNull(test);
        }
        [Test]
        public void TestCuttingBlock()
        {
            var testBlock = new FiveBlock(7);
            var test = CuttingRectangle.GenerateCuts(testBlock);
            Assert.IsNotNull(test);
        }
        [Test]
        public void TestPreciseRect2()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(2)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            var hash = board.GetHash();
            Assert.AreEqual("bae7577f4468e897cc8c308f10cb40e8aef92cbf75ad6f6dd774dbbc2f1ae655", hash);
        }
    }
}
