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
        public static bool KeyToPass = true;

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
                Console.WriteLine("\nCalling the solver: {0}", solverType);
                BlocksSolver solver = BlocksSolverFactory.GetSolver(solverType, blocks, blockSize);
                if(printBlocks)
                {
                    solver.PrintBlocks();
                    printBlocks = false;
                }
                solver.SolveMeasurePrint();
                if(KeyToPass && lines.Count > 0)
                {
                    Console.WriteLine("Press 'q' to quit or press any other key to solve the next problem...");
                    var key = Console.ReadKey();
                    if(key.KeyChar == 'q')
                        break;
                }
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
                var linesArray = File.ReadAllLines(path);
                Console.WriteLine("Reading input from file: {0}", path);
                foreach(var line in linesArray)
                    Console.WriteLine(line);
                Console.WriteLine();
                Queue<string> lines = new Queue<string>(linesArray);
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
            var linesArray = new string[] { "6", "hp", "2 1", "5", "hk", "0 20 1 1","6","op","0 2", "6","ok","0 6" };
            Console.WriteLine();
            Queue<string> lines = new Queue<string>(linesArray);
            CallAlgorithms(lines, true);
        }

        public static void Main(string[] args)
        {
            switch(args.Length)
            {
                case 0:
                    ExampleCallback();
                    break;
                case 1:
                    ParseInput(args[0], true);
                    break;
                default:
                    throw new ArgumentException("Usage: TetrisOptimization [data_path]");
            }
        }

       

    }
}
