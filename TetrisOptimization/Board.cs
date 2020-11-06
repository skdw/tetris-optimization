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
        public ConsoleColor? this[int i,int j]
        {
            get { return B[i, j]; }
            set { B[i, j] = value; }
        }


        /// <summary>
        /// Print the board
        /// </summary>
        /// <param name="forceSquare">Force the board to be a square</param>
        public void Print(bool forceSquare = false)
        {
            var bounds = GetBounds(forceSquare);
            var size = GetSize(bounds);
            if(forceSquare)
                Console.WriteLine($"\nPrinting square of side: {size.h}");
            else
                Console.WriteLine($"\nPrinting rectangle of size: h={size.h} w={size.w}");

            for (int i = bounds.minY; i < bounds.maxY; ++i)
            {
                Console.Write("|");
                for (int j = bounds.minX; j < bounds.maxX; ++j)
                {
                    try
                    {
                        if (B[i, j].HasValue)
                            Console.BackgroundColor = B[i,j].Value;
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write("  ");
                    Console.ResetColor();
                }
                Console.Write("|\n");
            }
        }

        int GetMinYFilled()
        {
            for(int i = 0; i < B.GetLength(0); ++i)
                for(int j = 0; j < B.GetLength(1); ++j)
                    if(B[i, j].HasValue)
                        return i;
            return -1;
        }

        int GetMaxYFilled()
        {
            for(int i = B.GetLength(0) - 1; i >= 0; --i)
                for(int j = 0; j < B.GetLength(1); ++j)
                    if(B[i, j].HasValue)
                        return i;
            return -1;
        }

        int GetMinXFilled()
        {
            for(int j = 0; j < B.GetLength(1); ++j)
                for(int i = 0; i < B.GetLength(0); ++i)
                    if(B[i, j].HasValue)
                        return j;
            return -1;
        }

        int GetMaxXFilled()
        {
            for(int j = B.GetLength(1) - 1; j >= 0; --j)
                for(int i = 0; i < B.GetLength(0); ++i)
                    if(B[i, j].HasValue)
                        return j;
            return -1;
        }

        (int minY, int maxY, int minX, int maxX) GetBounds(bool forceSquare)
        {
            int minY = GetMinYFilled();
            int maxY = GetMaxYFilled();
            int minX = GetMinXFilled();
            int maxX = GetMaxXFilled();
            if(forceSquare)
            {
                var size = GetSize((minY, maxY, minX, maxX));
                if(size.w > size.h)
                {
                    int diff = size.w - size.h;
                    maxY += diff;
                }
                else if(size.h > size.w)
                {
                    int diff = size.h - size.w;
                    maxX += diff;
                }
            }
            return (minY, maxY, minX, maxX);
        }

        static (int h, int w) GetSize((int minY, int maxY, int minX, int maxX) bounds)
        {
            int h = bounds.maxY - bounds.minY + 1;
            int w = bounds.maxX - bounds.minX + 1;
            return (h, w);
        }

        /// <summary>
        /// Tries to add block to the board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color_matrix"></param>
        /// <returns>True if an error occurs</returns>
        public bool TryToAdd(int x, int y, Block block)
        {
            var color_matrix = block.GetColorMatrix(FiveBlocks.GetNextColor());
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
