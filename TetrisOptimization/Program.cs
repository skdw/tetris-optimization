using System;
using TetrisOptimization.Blocks;

namespace TetrisOptimization
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(15, 20);
            board.Add(0, 0, TetrisBlocks.GetColorBlock());
            board.Add(5, 3, TetrisBlocks.GetColorBlock());
            board.Add(10, 2, TetrisBlocks.GetColorBlock());
            board.Add(8, 7, TetrisBlocks.GetColorBlock());
            board.Add(5, 13, TetrisBlocks.GetColorBlock());
            board.Add(10, 16, TetrisBlocks.GetColorBlock());
            board.Print();
        }
    }
}
