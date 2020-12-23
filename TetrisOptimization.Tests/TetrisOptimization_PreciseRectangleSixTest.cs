using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TetrisOptimization.UnitTests
{
    public class TestPreciseRectangleSix
    {
        [Test]
        public void TestPreciseRectSix01()
        {
            var blocks = new List<(int, Block)>() { (1, new SixBlock(17)), (1, new SixBlock(11)) };
            var solver = new PreciseRectangleSolver(blocks, 6);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("10d3a3748e1ff15b0bc1d203543e7b03a0728a72f6b628cc844514f39efb5690", hash);
        }

        [Test]
        public void TestPreciseRectSix02()
        {
            var blocks = new List<(int, Block)>() { (1, new SixBlock(8)), (1, new SixBlock(25)) };
            var solver = new PreciseRectangleSolver(blocks, 6);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(1, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("6716ec39d745533e86621ce6ef985e3e27989b2f2fa4e0c62399396569a899b8", hash);
        }

        [Test]
        public void TestPreciseRectSix03()
        {
            var blocks = new List<(int, Block)>() { (1, new SixBlock(35)), (1, new SixBlock(28)) };
            var solver = new PreciseRectangleSolver(blocks, 6);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("1f6c9f0ce01271c27c42f1535526d0b8d95a7f1b8158e786019027d7ad6ac1e6", hash);
        }

        [Test]
        public void TestPreciseRectSix04()
        {
            var blocks = new List<(int, Block)>() { (1, new SixBlock(10)), (1, new SixBlock(22)) };
            var solver = new PreciseRectangleSolver(blocks, 6);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(0, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("bced5afd5da41bf1df2731860a2f85620f0574a1679543fafb2b8785509ede5a", hash);
        }
    }
}
