using System;
using TetrisOptimization.Blocks;

namespace TetrisOptimization
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(15, 20);
            board.Add(0, 0, FiveBlocks.GetColorBlock());
            board.Add(5, 3, FiveBlocks.GetColorBlock());
            board.Add(10, 2, FiveBlocks.GetColorBlock());
            board.Add(8, 7, FiveBlocks.GetColorBlock());
            board.Add(5, 13, FiveBlocks.GetColorBlock());
            board.Add(10, 16, FiveBlocks.GetColorBlock());
            board.Print();
        }
    }
}
