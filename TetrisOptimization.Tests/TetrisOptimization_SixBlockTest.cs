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
            string path = "../../../../Data/Example10.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }
    }
}
