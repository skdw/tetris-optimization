using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TetrisOptimization
{
    public abstract class BlocksSolver
    {
        protected List<(int, Block)> blocks;
        protected int blockSize;
        protected bool cutBounds;
        protected bool forceSquare;

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

        public void SolveMeasurePrint()
        {
            Board board = SolveAndMeasure();
            board.Print(cutBounds, forceSquare);
        }

        public void PrintBlocks()
        {
            Console.WriteLine("Printing processed blocks");
            foreach((int no, Block block) in blocks)
            {
                Console.WriteLine($"{no} times:");
                Board blBoard = new Board(block.size.y, block.size.x);
                blBoard.TryToAdd(0, 0, block);
                blBoard.Print();
            }
        }
    }
}
