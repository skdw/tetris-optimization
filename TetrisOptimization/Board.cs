using System;

namespace TetrisOptimization
{
    public class Board
    {
        public Board(int x, int y)
        {
            B = new bool[x, y];
        }

        readonly bool[,] B;

        public bool this[int i, int j]
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
                    if (B[i, j])
                        Console.Write("#");
                    else
                        Console.Write(" ");
                Console.Write("|\n");
            }
        }

        public void Add(int x, int y, bool[,] block)
        {
            for (int i = 0; i < block.GetLength(0); ++i)
                for (int j = 0; j < block.GetLength(1); ++j)
                    if ((x + i >= B.GetLength(0)) || (y + j >= B.GetLength(1)))
                        Console.Error.WriteLine("Out of the board");
                    else if (B[x + i, y + j] == true && block[i, j] == true)
                        Console.Error.WriteLine("Trying to override the block");
                    else
                        B[x + i, y + j] = block[i, j];
        }
    }
}
