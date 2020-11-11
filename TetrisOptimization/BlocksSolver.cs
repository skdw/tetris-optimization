using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TetrisOptimization
{
    public abstract class BlocksSolver
    {
        protected List<(int, Block)> blocks;
        protected int blockSize;

        public BlocksSolver(List<(int, Block)> _blocks, int _blockSize)
        {
            blocks = _blocks;
            blockSize = _blockSize;
        }
        public abstract Board Solve();

        public Board SolveAndMeasure()
        {
            Console.WriteLine("\n");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var board = Solve();
            stopwatch.Stop();
            Console.WriteLine($"Problem solved in {stopwatch.ElapsedMilliseconds} ms");
            return board;
        }
    }
}
