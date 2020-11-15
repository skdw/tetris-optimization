using System;
using System.Collections.Generic;

namespace TetrisOptimization
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(6, 5);
            board.TryToAdd(1, 0, new FiveBlock(2));
            board.TryToAdd(0, 2, new FiveBlock(4));
            board.TryToAdd(3, 0, new FiveBlock(2));
            board.Print(false, false);
            var ct = CuttingRectangle.Cutting(board, (5, 4), (0, 6), (0, 5));
            ct.Item2.Print(false, false);
            Console.WriteLine(ct.Item1);

            List<(int, Block)> blocks = new List<(int, Block)>() { (2, new FiveBlock(1)), (1, new FiveBlock(3)), (1, new FiveBlock(6)) };
            int blockSize = 5;
            HeuristicSquare heuristicSquare = new HeuristicSquare(blocks, blockSize, 600, 0.4, 1);
            
        }
    }
}
