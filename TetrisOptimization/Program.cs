using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace TetrisOptimization
{
    public static class Program
    {
        public static bool KeyToPass = true;

        // print the numbers instead of colorful blocks in a console which does not support colors
        public static bool MonochromeConsole = false;

        /// <summary>
        /// Call algorithms given lines of input data
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="printBlocks"></param>
        static void CallAlgorithms(Queue<string> lines, bool printBlocks)
        {
            while (lines.Count > 0)
            {
                int blockSize = int.Parse(lines.Dequeue());
                string solverType = lines.Dequeue();

                string blocksText = lines.Dequeue();
                var blocksNumbers = blocksText
                    .Split(" ")
                    .Select(Int32.Parse)
                    .ToList();

                List<(int, Block)> blocks;
                if (blocksNumbers.Count == 1)
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
                if (printBlocks)
                {
                    solver.PrintBlocks(MonochromeConsole);
                    printBlocks = false;
                }
                solver.SolveMeasurePrint(MonochromeConsole);
                if(KeyToPass && lines.Count > 0)
                    if (AskForQuit() == 'q')
                        break;
            }
        }

        /// <summary>
        /// Parse input file
        /// </summary>
        /// <param name="path">Input file path</param>
        /// <param name="printBlocks"></param>
        static void ParseInputFile(string path, bool printBlocks)
        {
            try
            {
                var linesArray = File.ReadAllLines(path);
                Console.WriteLine("Reading input from file: {0}", path);
                foreach (var line in linesArray)
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

        /// <summary>
        /// Parse input from keyboard
        /// </summary>
        [ExcludeFromCodeCoverage]
        static void ParseFromKeyboard()
        {
            while (true)
            {
                var input = new Queue<string>();
                Console.WriteLine("Pass an entry from keyboard");
                Console.WriteLine("Type the blocks size [4-6]:");
                string blocks = Console.ReadLine();
                input.Enqueue(blocks);
                Console.WriteLine("Type the solver type [ok/hk/op/hp]:");
                string solverType = Console.ReadLine();
                input.Enqueue(solverType);
                Console.WriteLine("Type the blocks ID-s:");
                string blocksIds = Console.ReadLine();
                input.Enqueue(blocksIds);
                CallAlgorithms(input, true);
                if (AskForQuit() == 'q')
                    break;
            }
        }

        [ExcludeFromCodeCoverage]
        static char AskForQuit()
        {
            Console.WriteLine("Press 'q' to quit or press any other key to solve the next problem...");
            var key = Console.ReadKey();
            return key.KeyChar;
        }

        public static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    ParseFromKeyboard();
                    break;
                case 1:
                    ParseInputFile(args[0], true);
                    break;
                default:
                    throw new ArgumentException("Usage: TetrisOptimization [data_path]");
            }
        }
    }
}
