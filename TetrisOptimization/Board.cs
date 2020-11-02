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

        /// <summary>
        /// Tries to add block to the board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color_matrix"></param>
        /// <returns></returns>
        public bool TryToAdd(int x, int y, Block block)
        {
            var color_matrix = block.GetColorMatrix(FiveBlocks.GetRandomColor());
            for (int i = 0; i < color_matrix.GetLength(0); ++i)
                for (int j = 0; j < color_matrix.GetLength(1); ++j)
                    if ((x + i >= B.GetLength(0)) || (y + j >= B.GetLength(1)))
                    {
                        //Console.Error.WriteLine("Out of the board");
                        return true;
                    }
                    else if (B[x + i, y + j].HasValue && color_matrix[i, j].HasValue)
                    {
                        //Console.Error.WriteLine("Trying to override the block");
                        return true;
                    }
                    else
                        B[x + i, y + j] = color_matrix[i, j];
            return false;
        }
    }
}
