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
            board.TryToAdd(5, 3, FiveBlocks.GetRandomBlock());
            board.TryToAdd(10, 2, FiveBlocks.GetRandomBlock());
            board.TryToAdd(8, 7, FiveBlocks.GetRandomBlock());
            board.TryToAdd(5, 13, FiveBlocks.GetRandomBlock());
            board.TryToAdd(10, 16, FiveBlocks.GetRandomBlock());
            board.TryToAdd(0, 0, new Block(new bool[,] {{true, false}, {false, true}}, (2, 2)));
            Assert.Pass();
        }
    }
}
