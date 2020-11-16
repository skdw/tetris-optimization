using System;
using System.Collections.Generic;

namespace TetrisOptimization
{
    public static class BlocksSolverFactory
    {
        private static int HKnumLists = 600;

        private static double HKpercentage = 0.4;

        private static int HKnumPermutation = 1;

        private static double HPercentageBoardSize = 0.6;

        public static BlocksSolver GetSolver(string solverType, List<(int, Block)> blocks, int blockSize)
        {
            switch (solverType)
            {
                case "ok":
                    return new PreciseSquareSolver(blocks, blockSize);
                case "hk":
                    return new HeuristicSquareSolver(blocks, blockSize, HKnumLists, HKpercentage, HKnumPermutation, HPercentageBoardSize);
                case "op":
                    return new PreciseRectangleSolver(blocks, blockSize);
                case "hp":
                    return new HeuristicRectangleSolver(blocks, blockSize);
                default:
                    throw new ArgumentException("Unknown solver type");
            }
        }
    }
}
