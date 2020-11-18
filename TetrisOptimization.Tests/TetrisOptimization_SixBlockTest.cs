using NUnit.Framework;
using System.Collections.Generic;

namespace TetrisOptimization.UnitTests
{
    public class SixBlockTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSixBlock()
        {
            List<(int, Block)> blocks = new List<(int, Block)>() {  (1, new SixBlock(3)), (1, new SixBlock(17)), (1, new SixBlock(33)) };
            int blockSize = 6;
            PreciseSquareSolver precise= new PreciseSquareSolver(blocks, blockSize);
            precise.Solve();
            HeuristicSquareSolver heuristic=new HeuristicSquareSolver(blocks, blockSize, 6000, 0.3, 1, 0.6);
            heuristic.Solve();
            PreciseRectangleSolver preciseRec= new PreciseRectangleSolver(blocks, blockSize);
            preciseRec.Solve();
            HeuristicRectangleSolver heuristicRectangle= new HeuristicRectangleSolver(blocks, blockSize, 1,2);
            heuristicRectangle.Solve();
            Assert.Pass();
        }
    }
}
