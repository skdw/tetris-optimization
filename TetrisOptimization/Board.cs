using System;

namespace TetrisOptimization
{
    public class Board
    {
        public Board(int x, int y)
        {
            B = new ConsoleColor?[x, y];
        }
        public Board(Board b)
        {
            this.B = b.B;

        }

        public readonly ConsoleColor?[,] B;

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
            for (int cy = 0; cy < color_matrix.GetLength(0); ++cy)//y
                for (int cx = 0; cx < color_matrix.GetLength(1); ++cx)//x
                    if ((y + cy >= B.GetLength(0)) || (x + cx >= B.GetLength(1)))
                    {
                        //Console.Error.WriteLine("Out of the board");
                        return true;
                    }
                    else if (B[y + cy, x + cx].HasValue && color_matrix[cy, cx].HasValue)
                    {
                        //Console.Error.WriteLine("Trying to override the block");
                        return true;
                    }
            for (int cy = 0; cy < color_matrix.GetLength(0); ++cy)//y
                for (int cx = 0; cx < color_matrix.GetLength(1); ++cx)
                    if(color_matrix[cy,cx].HasValue)
                        B[y + cy, x + cx] = color_matrix[cy, cx];
            return false;
        }
    }
}
