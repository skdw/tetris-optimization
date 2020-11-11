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
        public void TestBoard()
        {
            Board board = new Board(15, 20);
            board.TryToAdd(5, 3, new FiveBlock(1));
            board.TryToAdd(10, 2, new FiveBlock(2));
            board.TryToAdd(8, 7, new FiveBlock(3));
            board.TryToAdd(5, 13, new FiveBlock(4));
            board.TryToAdd(10, 16, new FiveBlock(5));
            board.TryToAdd(0, 0, new Block(new bool[,] {{true, false}, {false, true}}, (2, 2)));
            Assert.Pass();
        }
    }
}
