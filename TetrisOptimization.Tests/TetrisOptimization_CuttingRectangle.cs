using NUnit.Framework;
using System.Collections.Generic;

namespace TetrisOptimization.UnitTests
{
    public class TestCuttiongRectangle
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void FindingGaps()
        {
            Board board = new Board(6, 5);
            board.TryToAdd(0, 1, new FiveBlock(2));
            board.TryToAdd(2, 0, new FiveBlock(4));
            board.TryToAdd(0, 3, new FiveBlock(2));


            //CuttingRectangle.baseBoard = board;

            //List<Gap> gaps = CuttingRectangle.FindingGaps((0, 4, 0, 4));
            //List<(int, int)> filds = new List<(int, int)>() { (0, 0), (1, 0), (1, 1), (2, 1), (2, 0) };
            //int[,] tmpmatrix = new int[3, 2];
            //for (int i = 0; i < 3; i++)
            //    for (int j = 0; j < 2; j++)
            //        if (i != 0 || j != 1)
            //            tmpmatrix[i, j]++;
            //Gap g = new Gap((3, 2), (0, 0), filds);
            //g.matrix = tmpmatrix;

            //Assert.AreEqual(gaps[0].fields, filds);
            //int[,] matrix = CuttingRectangle.prepareMatrix(gaps[0].size, gaps[0].position, gaps[0].fields);
            //Assert.AreEqual(matrix, tmpmatrix);
        }
    }
}
