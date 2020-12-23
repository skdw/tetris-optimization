using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TetrisOptimization.UnitTests
{
    public class TestPreciseConcurrent
    {
        [Test]
        public void TestPreciseNonConcurrentSquare()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(1)) };
            var solver = new PreciseSquareSolver(blocks, 5, false);
            var board = solver.SolveMeasurePrint(true);
            var hash = board.GetHash();
            Assert.AreEqual("f751c095b14dbcb4dd6c4ff7fa6774d095ed0b6cda2fe6806fcb4577b7f1c1d0", hash);
        }

        [Test]
        public void TestPreciseConcurrentSquare()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(1)) };
            var solver = new PreciseSquareSolver(blocks, 5, true);
            var board = solver.SolveMeasurePrint(true);
            var hash = board.GetHash();
            Assert.AreEqual("f751c095b14dbcb4dd6c4ff7fa6774d095ed0b6cda2fe6806fcb4577b7f1c1d0", hash);
        }

        [Test]
        public void TestPreciseNonConcurrentRect()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(1)) };
            var solver = new PreciseRectangleSolver(blocks, 5, false);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(0, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("c8c362c52a33bdd8c9fa09b44caf5284e23fd17e4976103c51b9f2fadf957033", hash);
        }

        [Test]
        public void TestPreciseConcurrentRect()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(1)) };
            var solver = new PreciseRectangleSolver(blocks, 5, true);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(0, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("c8c362c52a33bdd8c9fa09b44caf5284e23fd17e4976103c51b9f2fadf957033", hash);
        }
    }
}
