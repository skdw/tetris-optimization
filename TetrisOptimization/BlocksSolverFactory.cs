using System;
using System.Collections.Generic;

namespace TetrisOptimization
{
    public static class BlocksSolverFactory
    {
        private static int HKnumLists = 600;

        private static double HKpercentage = 0.4;

        private static int HKnumPermutation = 1;

        private static int HPnumPermutation = 5;
        private static int HPmultiplier = 10;

        public static BlocksSolver GetSolver(string solverType, List<(int, Block)> blocks, int blockSize)
        {
            switch (solverType)
            {
                case "ok":
                    return new PreciseSquareSolver(blocks, blockSize);
                case "hk":
                    return new HeuristicSquareSolver(blocks, blockSize, HKnumLists, HKpercentage, HKnumPermutation);
                case "op":
                    return new PreciseRectangleSolver(blocks, blockSize);
                case "hp":
                    return new HeuristicRectangleSolver(blocks, blockSize, HPnumPermutation, HPmultiplier<10?10:HPmultiplier);
                default:
                    throw new ArgumentException("Unknown solver type");
            }
        }
    }
}
