using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TetrisOptimization.Tests
{
    public class TetrisOptimization_CuttingRectangleHeuristicTest
    {
        [SetUp]
        public void Setup() { }
        [Test]
        public void CuttingTest()
        {
            List<(int, Block)> blocks2 = new List<(int, Block)>() { (10, new FiveBlock(1)), (10, new FiveBlock(3)), (5, new FiveBlock(6)), (15, new FiveBlock(7)) };
            var hr = new HeuristicRectangleSolver(blocks2,5,1,10);
            Board bestBoard = hr.Solve();
            bestBoard.Print(true, false);
            
        }

        [Test]
        public void HowManyUnitCutsTest()
        {
            var test1 = new FiveBlock(1);
            Assert.AreEqual(CuttingRectangle.HowManyUnitCuts(test1),4);
            var test2 = new FiveBlock(5);
            Assert.AreEqual(CuttingRectangle.HowManyUnitCuts(test2), 3);
        }
        [Test]
        public void ExactFitTest()
        {
            var block1 = new FiveBlock(5);
            var mtrx1 = new ConsoleColor?[2, 3]
            {
                 {null,null,null },
                 { null,null,ConsoleColor.Red}
            };
            var gap1 = new Gap((2, 3), (0, 0), new List < (int y, int x) >{ (0,0),(0,1),(0,2),(1,0),(1,1) });
            var block2 = new FiveBlock(1);
            var mtrx2 = new ConsoleColor?[3, 3]
            {
                 { ConsoleColor.Red, null, null},
             { null, null, ConsoleColor.Red},
             { ConsoleColor.Red, null, ConsoleColor.Red}
            };
            var gap2 = new Gap( (3, 3), (2, 3),new List<(int y, int x)> { (2,4),(2,5),(3,3),(3,4),(4,4) });
            var block3 = new FiveBlock(6);//od 5
            var gap3 = new Gap( (3, 3), (5, 0),new List<(int y, int x)> { (5,1),(5,2),(6,0),(6,1),(7,1) });
            var boardTest = new Board(10, 10);
            var ef = CuttingRectangle.ExactFit(new List<Gap> { gap1, gap2, gap3 }, new List<Block> { block1, block2, block3 }, boardTest);
            Assert.AreEqual(ef.Item1.Count, 1);
            Assert.AreEqual(ef.Item2.Count, 1);
        }
        [Test]
        public void GetCutBlockTest()
        {
            var boardTest5 = new Board(5, 5);
            boardTest5.TryToAdd(0, 0, new FiveBlock(6));
            boardTest5.TryToAdd(2, 0, new FiveBlock(9));
            //boardTest5.Print();
            var cb = CuttingRectangle.GetCutBlock(boardTest5);
            var board6 = new Board(10, 10);
            board6.TryToAdd(0, 0, cb[0]);
            board6.TryToAdd(0, 4, cb[1]);
            //board6.Print();
            Assert.AreEqual(cb[0].matrix.GetLength(0), 5);
            Assert.AreEqual(cb[0].matrix.GetLength(1), 3);
            Assert.AreEqual(cb[1].matrix.GetLength(0), 5);
            Assert.AreEqual(cb[1].matrix.GetLength(1), 3);
        }
        [Test]
        public void LengthCutTest()
        {
            var board7 = new Board(6, 6);
            board7.TryToAdd(2, 0, new FiveBlock(6));
            var gaps = new List<Gap>();
            var g = new Gap( (2, 2), (4, 4),new List<(int y, int x)> { (4,4),(4,5),(5,4),(5,5)});
            gaps.Add(g);
            board7.Print();
            var lc = CuttingRectangle.LengthCut(board7, (2, 5, 2, 4), (0, 5), (0, 5), gaps);
            Assert.AreEqual(lc.Item1, 1);
        }
        [Test]
        public void UnitCutTest()
        {
            Console.WriteLine("Exxact Fit test");
            var block1 = new FiveBlock(5);
            var mtrx1 = new ConsoleColor?[2, 3]
            {
                 {null,null,null },
                 { null,null,ConsoleColor.Red}
            };
            var gap1 = new Gap( (2, 3), (0, 0),new List<(int y, int x)> { (0,0),(0,1),(0,2),(1,0),(1,1)});
            var block2 = new FiveBlock(1);
            var mtrx2 = new ConsoleColor?[3, 3]
            {
                 { ConsoleColor.Red, null, null},
             { null, null, ConsoleColor.Red},
             { ConsoleColor.Red, null, ConsoleColor.Red}
            };
            var gap2 = new Gap((3, 3), (2, 3),new List<(int y, int x)> { (2,4),(2,5),(3,3),(3,4),(4,4)});
            var block3 = new FiveBlock(6);//od 5
            var gap3 = new Gap((3, 3), (5, 0),new List<(int y, int x)> { (5,1),(5,2),(6,0),(6,1),(7,1)});
            var boardTest3 = new Board(10, 10);
            var ef = CuttingRectangle.ExactFit(new List<Gap> { gap1, gap2, gap3 }, new List<Block> { block1, block2, block3 }, boardTest3);
            //boardTest3.PrintBoard();
            var uc = CuttingRectangle.UnitCut(ef, boardTest3, 0);
            Assert.AreEqual(uc, 3);
        }
        [Test]
        public void CheckAreaRightTest()
        {
            var boardTest4 = new Board(10, 10);
            boardTest4.TryToAdd(7, 5, new Block(new bool[2, 3] { { true, true, true }, { true, true, false } }));
            //boardTest4.PrintBoard();
            var test4 = CuttingRectangle.CheckAreaRight(boardTest4, 8, 5);
            Assert.AreEqual(test4[0].matrix.GetLength(0), 2);
            Assert.AreEqual(test4[0].matrix.GetLength(1), 3);
        }
        [Test]
        public void CheckAreaLeftTest()
        {
            var boardTest4 = new Board(10, 10);
            boardTest4.TryToAdd(7, 5, new Block(new bool[2, 3] { { true, true, true }, { true, true, false } }));
            //boardTest4.PrintBoard();
            var test4 = CuttingRectangle.CheckAreaLeft(boardTest4, 8, 5);
            Assert.AreEqual(test4[0].matrix.GetLength(0), 2);
            Assert.AreEqual(test4[0].matrix.GetLength(1), 1);
        }
        [Test]
        public void CheckAreaDownTest()
        {
            var boardTest4 = new Board(10, 10);
            boardTest4.TryToAdd(7, 5, new Block(new bool[2, 3] { { true, true, true }, { true, true, false } }));
            //boardTest4.PrintBoard();
            var test4 = CuttingRectangle.CheckAreaDown(boardTest4, 7, 6);
            Assert.AreEqual(test4[0].matrix.GetLength(0), 2);
            Assert.AreEqual(test4[0].matrix.GetLength(1), 3);
        }
        [Test]
        public void CheckAreaUpTest()
        {
            var boardTest4 = new Board(10, 10);
            boardTest4.TryToAdd(7, 5, new Block(new bool[2, 3] { { true, true, true }, { true, true, false } }));
            //boardTest4.PrintBoard();
            var test4 = CuttingRectangle.CheckAreaUp(boardTest4, 7, 5);
            Assert.AreEqual(test4[0].matrix.GetLength(0), 1);
            Assert.AreEqual(test4[0].matrix.GetLength(1), 3);
        }
        [Test]
        public void DoesBlockFitTest()
        {
            var block = new FiveBlock(5);
            var mtrx = new ConsoleColor?[2, 3]
            {
                 {null,null,null},
                 { null,null,ConsoleColor.Red }
            };
            var gap = new Gap( (2, 3), (2, 3),new List<(int y, int x)> { (2,3),(2,4),(2,5),(3,3),(3,4)});
            Assert.IsTrue(CuttingRectangle.DoesBlockFit(block, gap));
        }
    }
}
