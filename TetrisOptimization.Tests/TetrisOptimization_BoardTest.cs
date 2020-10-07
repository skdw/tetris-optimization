using NUnit.Framework;
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
            board.Add(5, 3, TetrisBlocks.GetBlock());
            board.Add(10, 2, TetrisBlocks.GetBlock());
            board.Add(8, 7, TetrisBlocks.GetBlock());
            board.Add(5, 13, TetrisBlocks.GetBlock());
            board.Add(10, 16, TetrisBlocks.GetBlock());
            board.Add(0, 0, new bool[,] {{true}});
            Assert.True(board[0, 0]);
        }
    }
}
