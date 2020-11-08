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
                        .Range(1, 17)
                        .Select(i => new FiveBlock(i) as Block)
                        .ToList();

            var rotations = CommonMethods.GetRotations(blocks);
            foreach((Block block, var rots) in blocks.Zip(rotations))
            {
                Block rot3 = block.Rotate().Rotate().Rotate();
                Assert.AreEqual(rot3, rots[3]);
            }
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
