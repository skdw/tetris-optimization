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
        public void TestPreciseRect2()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(2)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            var hash = board.GetHash();
            Assert.AreEqual("bd258b00971be2a0c1dfd220267823fbc563ae67b963f9bcd3baae1e91fdfbb2", hash);
        }
        
        [Test]
        public void TestPreciseRect3()
        {
            var blocks = new List<(int, Block)>() { (1, new FiveBlock(1)), (1, new FiveBlock(2)) };
            var solver = new PreciseRectangleSolver(blocks, 5);
            var board = solver.SolveMeasurePrint(true);
            var hash = board.GetHash();
            Assert.AreEqual("908d2df398967a693321dd569ad746988e53ea99b7b37fc9c931fa58f876afac", hash);
        }
    }
}
