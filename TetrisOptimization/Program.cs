using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TetrisOptimization
{
    public static class Program
    {
        /// <summary>
        /// Call algorithms given lines of input data
        /// </summary>
        /// <param name="lines"></param>
        static void CallAlgorithms(Queue<string> lines)
        {
            while(lines.Count > 0)
            {
                int blockSize = int.Parse(lines.Dequeue());
                string solverType = lines.Dequeue();

                string blocksText = lines.Dequeue();
                var blocksNumbers = blocksText
                    .Split(" ")
                    .Select(Int32.Parse)
                    .ToList();

                List<(int, Block)> blocks;
                if(blocksNumbers.Count == 1)
                {
                    int number = blocksNumbers.First();
                    blocks = Enumerable
                        .Range(1, number)
                        .Select(i => (1, BlockFactory.GetBlock(blockSize)))
                        .ToList();
                }
                else
                {
                    var blocksCands = Enumerable
                        .Range(1, blocksNumbers.Count)
                        .Select(i => BlockFactory.GetBlock(blockSize, i));
                    blocks = blocksNumbers
                        .Zip(blocksCands)
                        .Where(x => x.First > 0)
                        .ToList();
                }
                BlocksSolver solver = BlocksSolverFactory.GetSolver(solverType, blocks, blockSize);
                solver.SolveMeasurePrint();
            }
        }

        /// <summary>
        /// Parse input file
        /// </summary>
        /// <param name="path">Input file path</param>
        static void ParseInput(string path)
        {
            try
            {
                Queue<string> lines = new Queue<string>(File.ReadAllLines(path));
                CallAlgorithms(lines);
            }
            catch
            {
                Console.WriteLine($"Cannot read the input file: {path}");
                throw;
            }
        }

        static void ExampleCallback()
        {
            Console.WriteLine("Processing the example callback");
            Board board = new Board(6, 5);
            board.TryToAdd(1, 0, new FiveBlock(2));
            board.TryToAdd(0, 2, new FiveBlock(4));
            board.TryToAdd(3, 0, new FiveBlock(2));
            board.Print(false, false);
            CuttingRectangle.Cutting(board, (6, 5), (0, 6), (0, 5));

            List<(int, Block)> blocks = new List<(int, Block)>() { (2, new FiveBlock(1)), (1, new FiveBlock(3)), (1, new FiveBlock(6)) };
            int blockSize = 5;
            HeuristicSquareSolver heuristicSquareSolver = new HeuristicSquareSolver(blocks, blockSize, 600, 0.4, 1);
            heuristicSquareSolver.SolveMeasurePrint();
            Console.WriteLine("Minimal square calculated by heuristic algorithm: " + heuristicSquareSolver.minimalAchivedSize);

            PreciseSquareSolver preciseSquareSolver = new PreciseSquareSolver(blocks, blockSize);
            preciseSquareSolver.SolveMeasurePrint();

            PreciseRectangleSolver preciseRectangleSolver = new PreciseRectangleSolver(blocks, blockSize);
            preciseRectangleSolver.SolveMeasurePrint();
        }

        public static void Main(string[] args)
        {
            switch(args.Length)
            {
                case 0:
                    ExampleCallback();
                    break;
                case 1:
                    ParseInput(args[0]);
                    break;
                default:
                    Console.WriteLine("Usage: TetrisOptimization [path]");
                    break;
            }
        }
    }
}
