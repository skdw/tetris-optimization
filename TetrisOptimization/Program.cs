using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace TetrisOptimization
{
    public static class Program
    {
        /// <summary>
        /// Call algorithms given lines of input data
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="printBlocks"></param>
        static void CallAlgorithms(Queue<string> lines, bool printBlocks)
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
                if(printBlocks)
                {
                    solver.PrintBlocks();
                    printBlocks = false;
                }
                solver.SolveMeasurePrint();
            }
        }

        /// <summary>
        /// Parse input file
        /// </summary>
        /// <param name="path">Input file path</param>
        /// <param name="printBlocks"></param>
        static void ParseInput(string path, bool printBlocks)
        {
            try
            {
                Queue<string> lines = new Queue<string>(File.ReadAllLines(path));
                CallAlgorithms(lines, printBlocks);
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
            List<(int, Block)> blocks = new List<(int, Block)>() { (2, new FiveBlock(1)), (5, new FiveBlock(3)), (10, new FiveBlock(6)) };
            List<(int, Block)> blocks2 = new List<(int, Block)>() { (2, new FiveBlock(1)), (1, new FiveBlock(3)) };
            int blockSize = 5;

            var preciseSquareSolver = BlocksSolverFactory.GetSolver("ok", blocks2, blockSize);
            preciseSquareSolver.PrintBlocks();
            preciseSquareSolver.SolveMeasurePrint();

            var preciseRectangleSolver = BlocksSolverFactory.GetSolver("op", blocks2, blockSize);
            preciseRectangleSolver.SolveMeasurePrint();

            var heuristicSquareSolver = (HeuristicSquareSolver)BlocksSolverFactory.GetSolver("hk", blocks, blockSize);
            heuristicSquareSolver.PrintBlocks();
            heuristicSquareSolver.SolveMeasurePrint();
            Console.WriteLine("Minimal square calculated by heuristic algorithm: " + heuristicSquareSolver.minimalAchivedSize);

            var heuristicRectangleSolver = BlocksSolverFactory.GetSolver("hp", blocks, blockSize);
            heuristicRectangleSolver.SolveMeasurePrint();
        }

        public static void Main(string[] args)
        {
            switch(args.Length)
            {
                case 0:
                    SetConfiguration();
                    ExampleCallback();
                    break;
                case 1:
                    SetConfiguration();
                    ParseInput(args[0], true);
                    break;
                case 2:
                    SetConfiguration(args[1]);
                    ParseInput(args[0], true);
                    break;
                default:
                    throw new ArgumentException("Usage: TetrisOptimization [data_path] [config_path]");
            }
        }

        private static string GetConfigPath(string inputPath)
        {
            const string pattern = @"TetrisOptimization(?:\.Tests)?\/bin\/(Debug|Release)\/.+$";
            const string patternWindows = @"TetrisOptimization(?:\.Tests)?\\bin\\(Debug|Release)\\.+$";
            var curDir1 = Regex.Replace(Directory.GetCurrentDirectory(), pattern, string.Empty);
            var curDir =  Regex.Replace(curDir1, patternWindows, string.Empty);
            var firstChar = inputPath.Substring(0, 1);
            var basePath = firstChar == "/" ? "" : curDir;
            var configPath = Path.GetFullPath(Path.Combine(basePath, inputPath));
            if (!File.Exists(configPath))
                throw new FileNotFoundException($"Cannot load the solver configuration {configPath}", configPath);
            Console.WriteLine($"Config path: {configPath}");
            return configPath;
        }

        private static void SetConfiguration(string inputPath = "solverconfig.json")
        {
            BlocksSolverFactory.Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(GetConfigPath(inputPath),
                    optional: false,
                    reloadOnChange: true)
                .Build();
        }
    }
}
