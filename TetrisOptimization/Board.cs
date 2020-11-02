using System;

namespace TetrisOptimization
{
    public class Board
    {
        public Board(int x, int y)
        {
            B = new ConsoleColor?[x, y];
        }

        readonly ConsoleColor?[,] B;

        public ConsoleColor?[,] GetBoard()
        {
            return B;
        }

        public ConsoleColor? this[int i, int j]
        {
            get { return B[i, j]; }
            set { B[i, j] = value; }
        }

        public void Print()
        {
            for (int i = 0; i < B.GetLength(0); ++i)
            {
                Console.Write("|");
                for (int j = 0; j < B.GetLength(1); ++j)
                {
                    if (B[i, j].HasValue)
                        Console.BackgroundColor = B[i,j].Value;
                    Console.Write(" ");
                    Console.ResetColor();
                }
                Console.Write("|\n");
            }
        }

        public void Add(int x, int y, ConsoleColor?[,] block)
        {
            for (int i = 0; i < block.GetLength(0); ++i)
                for (int j = 0; j < block.GetLength(1); ++j)
                    if ((x + i >= B.GetLength(0)) || (y + j >= B.GetLength(1)))
                        Console.Error.WriteLine("Out of the board");
                    else if (B[x + i, y + j].HasValue && block[i, j].HasValue)
                        Console.Error.WriteLine("Trying to override the block");
                    else
                        B[x + i, y + j] = block[i, j];
        }
    }
}
