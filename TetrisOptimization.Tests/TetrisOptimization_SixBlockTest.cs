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
        public void TestSixBlock1()
        {
            string path = "../../../../Data/Example10.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }

        [Test]
        public void TestSixBlock2()
        {
            string path = "../../../../Data/Example11.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }
    }
}
