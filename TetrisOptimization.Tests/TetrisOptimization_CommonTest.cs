using NUnit.Framework;
using System;
using System.Linq;

namespace TetrisOptimization.UnitTests
{
    public class TestCommon
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void TestRotations()
        {
            var blocks = Enumerable
                        .Range(0, 10)
                        .Select(_ => FiveBlocks.GetRandomBlock())
                        .ToList();

            var rotations = CommonMethods.GetRotations(blocks);
            var rot4 = blocks[0].Rotate().Rotate().Rotate().Rotate();
            Assert.AreEqual(blocks[0], rot4);
        }

        [Test]
        public void TestPermutations()
        {
            int len = 5;
            var input = Enumerable.Range(1, len);
            var result = CommonMethods.GetCombinations(input, len);
            Assert.AreEqual(result.First(), Enumerable.Range(1, len));
            Assert.AreEqual(result.Last(), Enumerable.Range(1, 5).Reverse());
        }
    }
}
