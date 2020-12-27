using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace TetrisOptimization
{
    public static class BlocksSolverFactory
    {
        private static bool ConcurrentSquare = true;

        private static bool ConcurrentRectangle = false;

        private static int ParallelStep = 1000;

        private static int HKnumLists = 600;

        private static double HKpercentage = 0.5;

        private static int HKnumPermutation = 2;

        private static int HPnumPermutation = 1;

        private static int HPmultiplier = 10;

        private static double HPercentageBoardSize = 0.6;

        public static BlocksSolver GetSolver(string solverType, List<(int, Block)> blocks, int blockSize)
        {
            switch (solverType)
            {
                case "ok":
                    return new PreciseSquareSolver(blocks, blockSize, ConcurrentSquare, ParallelStep);
                case "hk":
                    return new HeuristicSquareSolver(blocks, blockSize, HKnumLists, HKpercentage, HKnumPermutation, HPercentageBoardSize);
                case "op":
                    return new PreciseRectangleSolver(blocks, blockSize, ConcurrentRectangle, ParallelStep);
                case "hp":
                    return new HeuristicRectangleSolver(blocks, blockSize, HPnumPermutation, HPmultiplier<10?10:HPmultiplier);
                default:
                    throw new ArgumentException("Unknown solver type");
            }
        }
    }
}
