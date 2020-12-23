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
        public void TestPreciseRect01()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(1)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(0, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("c8c362c52a33bdd8c9fa09b44caf5284e23fd17e4976103c51b9f2fadf957033", hash);
        }

        [Test]
        public void TestPreciseRect02()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(2)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("3a795ee993dcfcd47ae2fd7282fae2348dadc51429112235f8375626f9439a0b", hash);
        }

        [Test]
        public void TestPreciseRect03()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(3)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("3a795ee993dcfcd47ae2fd7282fae2348dadc51429112235f8375626f9439a0b", hash);
        }

        [Test]
        public void TestPreciseRect04()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(4)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(1, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("0b34ad0511d0a9bb2f8ee6befab4dd933690463abce8935a489305144d6ae693", hash);
        }

        [Test]
        public void TestPreciseRect05()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(5)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(1, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("0b34ad0511d0a9bb2f8ee6befab4dd933690463abce8935a489305144d6ae693", hash);
        }

        [Test]
        public void TestPreciseRect06()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(6)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("39e62b6bb7269b2fb4a788191d003a795438da21470ea12561520a926bcd6c65", hash);
        }

        [Test]
        public void TestPreciseRect07()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(7)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("39e62b6bb7269b2fb4a788191d003a795438da21470ea12561520a926bcd6c65", hash);
        }

        [Test]
        public void TestPreciseRect08()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(8)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(1, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("39e62b6bb7269b2fb4a788191d003a795438da21470ea12561520a926bcd6c65", hash);
        }

        [Test]
        public void TestPreciseRect09()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(9)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(1, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("39e62b6bb7269b2fb4a788191d003a795438da21470ea12561520a926bcd6c65", hash);
        }

        [Test]
        public void TestPreciseRect10()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(10)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(1, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("39e62b6bb7269b2fb4a788191d003a795438da21470ea12561520a926bcd6c65", hash);
        }

        [Test]
        public void TestPreciseRect11()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(11)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("39e62b6bb7269b2fb4a788191d003a795438da21470ea12561520a926bcd6c65", hash);
        }

        [Test]
        public void TestPreciseRect12()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(12)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(1, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("39e62b6bb7269b2fb4a788191d003a795438da21470ea12561520a926bcd6c65", hash);
        }

        [Test]
        public void TestPreciseRect13()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(13)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("3a795ee993dcfcd47ae2fd7282fae2348dadc51429112235f8375626f9439a0b", hash);
        }

        [Test]
        public void TestPreciseRect14()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(14)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("389c0e5e125399e4556df695d86d514692456a6b9c2035dec16fd4bdb0df1bf6", hash);
        }

        [Test]
        public void TestPreciseRect15()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(15)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(1, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("0b34ad0511d0a9bb2f8ee6befab4dd933690463abce8935a489305144d6ae693", hash);
        }

        [Test]
        public void TestPreciseRect16()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(16)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(1, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("0b34ad0511d0a9bb2f8ee6befab4dd933690463abce8935a489305144d6ae693", hash);
        }

        [Test]
        public void TestPreciseRect17()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(17)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("3a795ee993dcfcd47ae2fd7282fae2348dadc51429112235f8375626f9439a0b", hash);
        }

        [Test]
        public void TestPreciseRect18()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(18)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("3a795ee993dcfcd47ae2fd7282fae2348dadc51429112235f8375626f9439a0b", hash);
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
        public void TestCuttingBlock2()
        {
            bool[,] m = new bool[4, 2];
            m[0, 0] = true;
            m[1, 0] = true;
            m[2, 0] = true;
            m[3, 0] = true;
            m[1, 1] = true;
            Block b = new Block(m);
            var l = CuttingRectangle.GenerateCuts(b);
            foreach(var t in l)
            {
                int elemsSum = 0;
                Console.WriteLine($"{t.Item1}​​​​​​​ times:");
                foreach (Block block in t.Item2)
                {
                    var blBoard = new Board(block.Size.Y, block.Size.X);
                    blBoard.TryToAdd(0, 0, block);
                    blBoard.Print(true, false, true);
                    elemsSum += blBoard.CountElems();
                }
                Assert.AreEqual(5, elemsSum);
            }
        }

        [Test]
        public void TestCuttingBlock3()
        {
            bool[,] m = new bool[3, 3];
            m[0, 1] = true;
            m[1, 1] = true;
            m[2, 1] = true;
            m[1, 0] = true;
            m[1, 2] = true;
            Block b = new Block(m);
            var l = CuttingRectangle.GenerateCuts(b);
            foreach(var t in l)
            {
                int elemsSum = 0;
                Console.WriteLine($"{t.Item1}​​​​​​​ times:");
                foreach (Block block in t.Item2)
                {
                    var blBoard = new Board(block.Size.Y, block.Size.X);
                    blBoard.TryToAdd(0, 0, block);
                    blBoard.Print(true, false, true);
                    elemsSum += blBoard.CountElems();
                }
                Assert.AreEqual(5, elemsSum);
            }
        }

        [Test]
        public void TestPreciseRectGroup()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(1)), (1, new FiveBlock(2)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            Assert.AreEqual(2, board.CutsNumber);
            var hash = board.GetHash();
            Assert.AreEqual("215a313dd03e819a430003c9265895e97d0a46ee20982942f755f0c8ed2e6f8b", hash);
        }
    }
}
