using NUnit.Framework;

namespace TetrisOptimization.UnitTests
{
    public class TestProgram
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void TestProgramMain()
        {
            string path = "../../../../Data/Example2.txt";
            string[] args = {path};
            Program.Main(args);
            Assert.Pass();
        }
    }
}
