using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace TetrisOptimization
{
    public static class BlocksSolverFactory
    {
        public static IConfiguration Configuration { private get; set; }

        private static int ParallelStep => int.Parse(Configuration["Precise:ParallelStep"]);

        private static int HKnumLists => int.Parse(Configuration["HeuristicSquare:NumLists"]);

        private static double HKpercentage => double.Parse(Configuration["HeuristicSquare:Percentage"], CultureInfo.InvariantCulture);

        private static int HKnumPermutation => int.Parse(Configuration["HeuristicSquare:NumPermutation"]);

        private static int HPnumPermutation => int.Parse(Configuration["HeuristicRectangle:NumPermutation"]);

        private static int HPmultiplier => int.Parse(Configuration["HeuristicRectangle:Multiplier"]);

        private static double HPercentageBoardSize => 0.6;

        public static BlocksSolver GetSolver(string solverType, List<(int, Block)> blocks, int blockSize)
        {
            switch (solverType)
            {
                case "ok":
                    return new PreciseSquareSolver(blocks, blockSize, ParallelStep);
                case "hk":
                    return new HeuristicSquareSolver(blocks, blockSize, HKnumLists, HKpercentage, HKnumPermutation, HPercentageBoardSize);
                case "op":
                    return new PreciseRectangleSolver(blocks, blockSize, ParallelStep);
                case "hp":
                    return new HeuristicRectangleSolver(blocks, blockSize, HPnumPermutation, HPmultiplier<10?10:HPmultiplier);
                default:
                    throw new ArgumentException("Unknown solver type");
            }
        }
    }
}
