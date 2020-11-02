using System;
using TetrisOptimization.Blocks;

namespace TetrisOptimization
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(15, 20);
            board.TryToAdd(0, 0, FiveBlocks.GetRandomBlock());
            board.TryToAdd(5, 3, FiveBlocks.GetRandomBlock());
            board.TryToAdd(10, 2, FiveBlocks.GetRandomBlock());
            board.TryToAdd(8, 7, FiveBlocks.GetRandomBlock());
            board.TryToAdd(5, 13, FiveBlocks.GetRandomBlock());
            board.TryToAdd(10, 16, FiveBlocks.GetRandomBlock());
            board.Print();
        }
    }
}
