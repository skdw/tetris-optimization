using NUnit.Framework;

namespace TetrisOptimization.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBoard1()
        {
            Board board = new Board(15, 20);
            board.TryToAdd(5, 3, new FiveBlock(1));
            board.TryToAdd(10, 2, new FiveBlock(2));
            board.TryToAdd(8, 7, new FiveBlock(3));
            board.TryToAdd(5, 13, new FiveBlock(4));
            board.TryToAdd(10, 16, new FiveBlock(5));
            board.TryToAdd(0, 0, new Block(new bool[,] {{true, false}, {false, true}}));
            board.Print(false, false);
            Assert.Pass();
        }

        [Test]
        public void TestBoard2()
        {
            Board board = new Board(20, 15);
            board.TryToAdd(5, 3, new FiveBlock(1));
            board.TryToAdd(10, 2, new FiveBlock(2));
            board.TryToAdd(8, 7, new FiveBlock(3));
            board.TryToAdd(0, 0, new Block(new bool[,] {{true, false}, {false, true}}));
            board.Print(true, true);
            Assert.Pass();
        }

        [Test]
        public void TestBoardRemove()
        {
            Board board = new Board(15, 20);
            var block = new FiveBlock(1);
            board.TryToAdd(5, 3, block);
            board.TryToAdd(10, 2, new FiveBlock(2));
            board.TryToAdd(8, 7, new FiveBlock(3));
            board.TryToAdd(0, 0, new Block(new bool[,] {{true, false}, {false, true}}));
            Assert.AreEqual(board.TryToRemove(100000, 0, block), false);
        }

        [Test]
        public void TestBoardScan()
        {
            Board board = new Board(15, 20);
            var block = new FiveBlock(1);
            board.TryToAdd(5, 3, block);
            board.TryToAdd(10, 2, new FiveBlock(2));
            board.TryToAdd(8, 7, new FiveBlock(3));
            board.TryToAdd(0, 0, new Block(new bool[,] {{true, false}, {false, true}}));
            Assert.AreEqual(board.ScanBoard(0, 9999, block), false);
        }

        [Test]
        public void TestBoardErrorCheck1()
        {
            Board board = new Board(15, 15);
            board.TryToAdd(5, 3, new FiveBlock(1));
            board.TryToAdd(10, 2, new FiveBlock(2));
            board.TryToAdd(8, 7, new FiveBlock(3));
            int check = CuttingRectangle.ErrorCheck(board);
            Assert.AreEqual(check, -1);
        }

        [Test]
        public void TestBoardErrorCheck2()
        {
            Board board = new Board(2, 2);
            board.TryToAdd(0, 0, new TetrisBlock(6));
            int check = CuttingRectangle.ErrorCheck(board);
            Assert.AreEqual(check, 0);
        }
    }
}
