using System;
using System.Collections.Generic;

namespace TetrisOptimization
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //Board board = new Board(6, 5);
            //board.TryToAdd(1, 0, new FiveBlock(2));
            //board.TryToAdd(0, 2, new FiveBlock(4));
            //board.TryToAdd(3, 0, new FiveBlock(2));
            //board.Print(false, false);
            //var ct = CuttingRectangle.Cutting(board, (5, 4), (0, 6), (0, 5));
            //ct.Item2.Print(false, false);
            //Console.WriteLine(ct.Item1);

            //List<(int, Block)> blocks = new List<(int, Block)>() { (2, new FiveBlock(1)), (1, new FiveBlock(3)), (1, new FiveBlock(6)) };
            //int blockSize = 5;
            //HeuristicSquare heuristicSquare = new HeuristicSquare(blocks, blockSize, 600, 0.4, 1);
            //heuristicSquare.Solve().Print(true, true);
            //Console.WriteLine("Minimal square calculated by heuristic algorithm: " + heuristicSquare.minimalAchivedSize);
            //Board board = new Board(15, 20);
            //board.TryToAdd(0, 0, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(5, 3, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(10, 2, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(8, 7, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(5, 13, FiveBlocks.GetRandomBlock());
            //board.TryToAdd(10, 16, FiveBlocks.GetRandomBlock());
            //board.Print();
            //Board board = new Board(6, 5);
            //board.TryToAdd(1, 0, FiveBlocks.GetBlock(2));
            //board.TryToAdd(0, 2, FiveBlocks.GetBlock(4));
            //board.TryToAdd(3, 0, FiveBlocks.GetBlock(2));
            //board.Print();
            //CuttingRectangle.Cutting(board, (6, 5), (0, 6), (0, 5));

            List<(int, Block)> blocks2 = new List<(int, Block)>() { (10, new FiveBlock(1)), (20, new FiveBlock(3)), (15, new FiveBlock(6)) , (15, new FiveBlock(7)), (9, new FiveBlock(8)) };
            
            var hr = new HeuristicRectangle(blocks2, 5);
            Board bestBoard = hr.Solve();
            bestBoard.Print(true, false);
            //HeuristicSquare heuristicSquare = new HeuristicSquare(blocks, blockSize, 600, 0.4, 1);
            //heuristicSquare.Solve().Print(true);
            //Console.WriteLine("Minimal square calculated by heuristic algorithm: " + heuristicSquare.minimalAchivedSize);

            //PreciseSquareSolver squareSolver = new PreciseSquareSolver(blocks, blockSize);
            //squareSolver.Solve().Print(true);

            //ciecia
            //var test = FiveBlocks.GetBlock(5);
            //Console.WriteLine(CuttingRectangle.HowManyUnitCuts(test));
            //Console.WriteLine("Exxact Fit test");
            //var block1 = FiveBlocks.GetBlock(5);
            //var mtrx1 = new ConsoleColor?[2, 3]
            //{
            //    {null,null,null },
            //    { null,null,ConsoleColor.Red}
            //};
            //var gap1 = new Gap(mtrx1, (2, 3), (0,0));
            //var block2 = FiveBlocks.GetBlock(1);
            //var mtrx2 = new ConsoleColor?[3, 3]
            //{
            //    { ConsoleColor.Red, null, null},
            //{ null, null, ConsoleColor.Red},
            //{ ConsoleColor.Red, null, ConsoleColor.Red}
            //};
            //var gap2 = new Gap(mtrx2, (3, 3), (2, 3));
            //var block3 = FiveBlocks.GetBlock(6);//od 5
            //var gap3 = new Gap(mtrx2, (3, 3), (5, 0));
            //var boardTest3 = new Board(10, 10);
            //var ef = CuttingRectangle.ExactFit(new List<Gap> { gap1, gap2, gap3 }, new List<Block> { block1, block2, block3 }, boardTest3);
            //Console.WriteLine(ef.Item1.Count);
            //Console.WriteLine(ef.Item2.Count);
            //boardTest3.PrintBoard();
            //var uc = CuttingRectangle.UnitCut(ef, boardTest3, 0);
            //Console.WriteLine(uc);
            //boardTest3.PrintBoard();
            //Console.WriteLine("Check area up test");
            //var boardTest4 = new Board(10, 10);
            //boardTest4.TryToAdd(7, 5, new Block(new bool[2, 3] { { true, true, true }, { true, true, false } }, (3, 2)));
            ////boardTest4.PrintBoard();
            //Block test4 = CuttingRectangle.CheckAreaRight(boardTest4, 6, 8);
            //Console.WriteLine();
            //Console.WriteLine("GetCutBlock test");
            //var boardTest5 = new Board(5, 5);
            //boardTest5.TryToAdd(0, 0, FiveBlocks.GetBlock(6));
            //boardTest5.TryToAdd(2, 0, FiveBlocks.GetBlock(9));
            ////boardTest5.PrintBoard();
            //var cb = CuttingRectangle.GetCutBlock(boardTest5);
            //var board6 = new Board(10, 10);
            //board6.TryToAdd(0, 0, cb[0]);
            //board6.TryToAdd(5, 0, cb[1]);
            //board6.PrintBoard();
            //Console.WriteLine(cb.Count);

            //Console.WriteLine(CuttingRectangle.DoesBlockFit(block1, gap1));
            //Console.WriteLine("Length cut test");
            //var board7 = new Board(6, 6);
            //board7.TryToAdd(3, 0, new FiveBlock(6).Rotate().Rotate().Rotate());
            //board7.Print(false,false);
            //Console.WriteLine();
            //var gaps = new List<Gap>();
            //var g = new Gap((1, 2), (4, 4),new List<(int, int)> { (4,4),(4,5) });
            //gaps.Add(g);
            //var lc = CuttingRectangle.LengthCut(board7, (1, 4, 1, 4), (0, 5), (0, 5), gaps);
            //lc.Item2.Print(false, false);
            //Console.WriteLine(lc.Item1);
        }
    }
}
