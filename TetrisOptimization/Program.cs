using System;
using System.Collections.Generic;
using TetrisOptimization.Blocks;

namespace TetrisOptimization
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //Board board = new Board(15, 20);
            //board.TryToAdd(0, 0, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(5, 3, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(10, 2, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(8, 7, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(5, 13, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(10, 16, FiveBlocks.GetRandomBlock());
            //board.Print();

            List<(int, Block)> blocks = new List<(int, Block)>() { (2, FiveBlocks.GetBlock(1)) ,(3,FiveBlocks.GetBlock(3)),(1,FiveBlocks.GetBlock(6))};
            HeuristicSquare heuristicSquare=new HeuristicSquare(blocks, 5, 600, 0.4, 1);
            heuristicSquare.algorithm().Print();
            Console.WriteLine("minimal square calculaed by heuristic algorithm is : " + heuristicSquare.minimalAchivedSize);

            Board board = new Board(6, 5);
            board.TryToAdd(0, 0, FiveBlocks.GetBlock(2));
            board.TryToAdd(2, 3, FiveBlocks.GetBlock(4));
            board.TryToAdd(0, 2, FiveBlocks.GetBlock(6));
            board.Print();
            CuttingRectangle.Cutting(board, (6, 5), (0, 6), (0, 5));

        }
    }
}
