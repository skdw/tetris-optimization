using NUnit.Framework;
using System.Collections.Generic;

namespace TetrisOptimization.UnitTests
{
    public class FingingGapsTest
    {
        Board board;
        [SetUp]
        public void Setup()
        {
            board = new Board(6, 5);
            board.TryToAdd(0, 1, new FiveBlock(2));
            board.TryToAdd(2, 0, new FiveBlock(4));
            board.TryToAdd(0, 3, new FiveBlock(2));
        }
        [Test]
        public void TestFindingGaps()
        {            
            FindingGaps findingGaps= new FindingGaps(board);
            List<Gap> gaps = findingGaps.FindGaps((0, 4, 0, 4));

            List<(int, int)> fields = new List<(int, int)>() { (0, 0), (1, 0), (1, 1), (2, 1), (2, 0) };
            Gap g = new Gap((3, 2), (0, 0), fields);
            
            Assert.AreEqual(gaps[0].fields, fields);
        }
        [Test]
        public void TestPrepareMatrix()
        {
            List<(int, int)> fields = new List<(int, int)>() { (0, 0), (1, 0), (1, 1), (2, 1), (2, 0) };
            int[,] matrix = TetrisOptimization.FindingGaps.prepareMatrix((3,2), (0,0), fields);
            int[,] tmpmatrix = new int[3, 2];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                    if (i != 0 || j != 1)
                        tmpmatrix[i, j]++;
            Assert.AreEqual(matrix, tmpmatrix);
        }
    }
}
