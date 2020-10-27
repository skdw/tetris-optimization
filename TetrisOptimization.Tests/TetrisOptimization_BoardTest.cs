using NUnit.Framework;
using System;
using TetrisOptimization.Blocks;

namespace TetrisOptimization.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBoard()
        {
            Board board = new Board(15, 20);
            board.Add(5, 3, TetrisBlocks.GetColorBlock());
            board.Add(10, 2, TetrisBlocks.GetColorBlock());
            board.Add(8, 7, TetrisBlocks.GetColorBlock());
            board.Add(5, 13, TetrisBlocks.GetColorBlock());
            board.Add(10, 16, TetrisBlocks.GetColorBlock());
            board.Add(0, 0, new ConsoleColor?[,] {{ConsoleColor.Green}});
            Assert.AreEqual(board[0, 0], ConsoleColor.Green);
        }
    }
}
