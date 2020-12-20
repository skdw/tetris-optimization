using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TetrisOptimization.UnitTests
{
    public class TestProgram
    {
        [SetUp]
        public void Setup()
        {
            Program.KeyToPass = false;
            Program.MonochromeConsole = true;
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
        /// <summary>
        /// Heuristic rectangle solver
        /// </summary>
        public void TestProgramMain115()
        {
            string path = "../../../../Data/Example5.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }

        [Test]
        /// <summary>
        /// Unknown solver type
        /// </summary>
        public void TestProgramMain116()
        {
            string path = "../../../../Data/Example6.txt";
            string[] args = { path };
            var ex = Assert.Throws<ArgumentException>(() => Program.Main(args));
            Assert.That(ex.Message, Is.EqualTo("Unknown solver type"));
        }

        [Test]
        /// <summary>
        /// Precise rectangle solver
        /// </summary>
        public void TestProgramMain119()
        {
            string path = "../../../../Data/Example9.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }

        [Test]
        public void TestProgramMain1112()
        {
            string path = "../../../../Data/Example12.txt";
            string[] args = { path };
            Assert.Throws<NotImplementedException>(() => Program.Main(args));
        }

        [Test]
        public void TestProgramMain1113()
        {
            string path = "../../../../Data/Example13.txt";
            string[] args = { path };
            var task = Task.Run(() => Program.Main(args));
            var completedInTime = task.Wait(2000);
            Assert.False(completedInTime);
        }

        [Test]
        /// <summary>
        /// 2x2 blocks
        /// </summary>
        public void TestProgramMain1114()
        {
            string path = "../../../../Data/Example14.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }

        [Test]
        public void TestProgram1PreciseRectangle()
        {
            string path = "../../../../Data/op511.txt";
            string[] args = { path };
            Program.Main(args);
            Assert.Pass();
        }

        [Test]
        public void TestProgramMain12()
        {
            string path = "non_existing_path";
            string[] args = { path };
            Assert.Throws<FileNotFoundException>(() => Program.Main(args));
        }

        [Test]
        public void TestProgramMain2()
        {
            string[] args = { "", "", "" };
            var ex = Assert.Throws<ArgumentException>(() => Program.Main(args));
            Assert.That(ex.Message, Is.EqualTo("Usage: TetrisOptimization [data_path] [config_path]"));
        }
    }
}
