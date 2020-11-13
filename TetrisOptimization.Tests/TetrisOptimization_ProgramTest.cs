using System;
using System.IO;
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
        /// <summary>
        /// Default call
        /// </summary>
        public void TestProgramMain0()
        {
            string[] args = { };
            Program.Main(args);
            Assert.Pass();
        }

        [Test]
        /// <summary>
        /// Five blocks - specified
        /// </summary>
        public void TestProgramMain112()
        {
            string path = "../../../../Data/Example2.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }

        [Test]
        /// <summary>
        /// Five blocks - random
        /// </summary>
        public void TestProgramMain113()
        {
            string path = "../../../../Data/Example3.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }

        [Test]
        /// <summary>
        /// Tetris (four) blocks
        /// </summary>
        public void TestProgramMain114()
        {
            string path = "../../../../Data/Example4.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }

        [Test]
        public void TestProgramMain12()
        {
            string path = "non_existing_path";
            string[] args = { path };
            var ex = Assert.Throws<FileNotFoundException>(() => Program.Main(args));
        }

        [Test]
        public void TestProgramMain2()
        {
            string[] args = { "", "" };
            var ex = Assert.Throws<ArgumentException>(() => Program.Main(args));
            Assert.That(ex.Message, Is.EqualTo("Usage: TetrisOptimization [path]"));
        }
    }
}
