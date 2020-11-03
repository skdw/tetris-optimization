using System;
using System.Collections.Generic;
using TetrisOptimization.Blocks;

namespace TetrisOptimization
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //wypisanie
            //Board board = new Board(15, 20);
            //board.TryToAdd(0, 0, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(5, 3, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(10, 2, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(8, 7, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(5, 13, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(10, 16, FiveBlocks.GetRandomBlock());
            //board.Print();

            //wariant prostokatny heurystyczny
            List<Block> blocks = new List<Block>
            {
                FiveBlocks.GetBlock(1),
                FiveBlocks.GetBlock(3),
                FiveBlocks.GetBlock(6),
                FiveBlocks.GetBlock(4),
                FiveBlocks.GetBlock(3)
            };

            List < List < Block >> blocksRotated = CommonMethods.GetRotations(blocks);
            var recH = new Heuristic_rectangle(blocksRotated);
            var alg = recH.Algorithm();
            Console.WriteLine($"Arrangement for the first permutation of {blocks.Count} different blocks:");
            if (alg != null) alg.PrintRestricted(recH.currentFigure[2]-1, recH.currentFigure[3]+1, recH.currentFigure[0]-1, recH.currentFigure[1]+1);

            ////wariant kwadratowy heurystyczny
            //List<(int, Block)> blocks2 = new List<(int, Block)>() { (2, FiveBlocks.GetBlock(1)), (3, FiveBlocks.GetBlock(3)), (1, FiveBlocks.GetBlock(6)) };
            //HeuristicSquare heuristicSquare = new HeuristicSquare(blocks2, 5, 600, 0.4, 1);
            //heuristicSquare.algorithm().Print();
            //Console.WriteLine("minimal square calculaed by heuristic algorithm is : " + heuristicSquare.minimalAchivedSize);

        }
    }
}
