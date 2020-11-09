using System;
using System.Collections.Generic;
using TetrisOptimization.Blocks;

namespace TetrisOptimization
{
    public static class Program
    {
        static void Main(string[] args)
        {
            
            List<(int, Block)> blocks = new List<(int, Block)>() { (2, FiveBlocks.GetBlock(1)), (1, FiveBlocks.GetBlock(3)), (1, FiveBlocks.GetBlock(6)) };
            int blockSize = 5;
            HeuristicSquare heuristicSquare = new HeuristicSquare(blocks, blockSize, 600, 0.4, 1);
            heuristicSquare.Solve().Print(true, true);
            Console.WriteLine("Minimal square calculated by heuristic algorithm: " + heuristicSquare.minimalAchivedSize);

            PreciseSquareSolver squareSolver = new PreciseSquareSolver(blocks, blockSize);
            squareSolver.Solve().Print(true, true);
        }
    }
}
