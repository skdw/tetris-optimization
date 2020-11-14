using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisOptimization
{
    public class Board
    {
        public Board(int y, int x)
        {
            B = new int?[y, x];
        }
        public Board(Board b)
        {
            this.B = b.B.Clone() as int?[,];
        }

        public readonly int?[,] B;
        public int? this[int i, int j]
        {
            get { return B[i, j]; }
            set { B[i, j] = value; }
        }

        int colorID = 0;

        /// <summary>
        /// Get ConsoleColor for printing blocks
        /// </summary>
        /// <param name="colorID">integer ID of color</param>
        /// <returns>console color</returns>
        static ConsoleColor GetColor(int colorID) => (ConsoleColor)((colorID * 2) % 15 + 1);

        /// <summary>
        /// Print the size of the board
        /// </summary>
        void PrintSize(bool forceSquare, (int, int, int, int) bounds)
        {
            var size = GetSize(bounds);
            if (forceSquare)
                Console.WriteLine($"Printing square of side: {size.h}");
            else
                Console.WriteLine($"Printing rectangle of size: h={size.h} w={size.w}");
        }

        /// <summary>
        /// Print the board
        /// </summary>
        /// <param name="cutBounds">Cuts the board's bounds</param>
        /// <param name="forceSquare">Force the board to be a square</param>
        public void Print(bool cutBounds = true, bool forceSquare = false)
        {
            var bounds = GetBounds(cutBounds, forceSquare);
            PrintSize(forceSquare, bounds);

            for (int i = bounds.minY; i < bounds.maxY; ++i)
            {
                Console.Write("|");
                for (int j = bounds.minX; j < bounds.maxX; ++j)
                {
                    try
                    {
                        if (B[i, j].HasValue)
                            Console.BackgroundColor = GetColor(B[i, j].Value);
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
            for (int i = 0; i < B.GetLength(0); ++i)
                for (int j = 0; j < B.GetLength(1); ++j)
                    if (B[i, j].HasValue)
                        return i;
            return -1;
        }

        int GetMaxYFilled()
        {
            for (int i = B.GetLength(0) - 1; i >= 0; --i)
                for (int j = 0; j < B.GetLength(1); ++j)
                    if (B[i, j].HasValue)
                        return i;
            return -1;
        }

        int GetMinXFilled()
        {
            for (int j = 0; j < B.GetLength(1); ++j)
                for (int i = 0; i < B.GetLength(0); ++i)
                    if (B[i, j].HasValue)
                        return j;
            return -1;
        }

        int GetMaxXFilled()
        {
            for (int j = B.GetLength(1) - 1; j >= 0; --j)
                for (int i = 0; i < B.GetLength(0); ++i)
                    if (B[i, j].HasValue)
                        return j;
            return -1;
        }

        (int minY, int maxY, int minX, int maxX) GetBounds(bool cutBounds, bool forceSquare)
        {
            if (cutBounds == false)
                return (0, B.GetLength(0), 0, B.GetLength(1));

            int minY = GetMinYFilled();
            int maxY = GetMaxYFilled();
            int minX = GetMinXFilled();
            int maxX = GetMaxXFilled();
            if (forceSquare)
            {
                var size = GetSize((minY, maxY, minX, maxX));
                if (size.w > size.h)
                {
                    int diff = size.w - size.h;
                    maxY += diff;
                }
                else if (size.h > size.w)
                {
                    int diff = size.h - size.w;
                    maxX += diff;
                }
            }
            return (minY, maxY + 1, minX, maxX + 1);
        }

        static (int h, int w) GetSize((int minY, int maxY, int minX, int maxX) bounds)
        {
            int h = bounds.maxY - bounds.minY;
            int w = bounds.maxX - bounds.minX;
            return (h, w);
        }

        /// <summary>
        /// Try to add block to the board
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <param name="force_override_id">Null to prevent overriding. Number of blocks to code the block id.</param>
        /// <returns>True if an error occurs</returns>
        public bool TryToAdd(int y, int x, Block block, int? force_override_id = null)
        {
            // Check for errors
            for (int cy = 0; cy < block.size.y; ++cy)
                for (int cx = 0; cx < block.size.x; ++cx)
                    if ((y + cy >= B.GetLength(0)) || (x + cx >= B.GetLength(1)))
                    {
                        // Out of the board
                        return true;
                    }
                    else if (B[y + cy, x + cx].HasValue && block.matrix[cy, cx])
                    {
                        // Trying to override the block
                        if(force_override_id is null) // prevent overriding (square)
                            return true;
                    }

            // Add block to the board
            int colortmpID = ++colorID;
            for (int cy = 0; cy < block.size.y; ++cy)
                for (int cx = 0; cx < block.size.x; ++cx)
                    if (block.matrix[cy, cx])
                    {
                        if(B[y + cy, x + cx].HasValue) // overriding
                        {
                            B[y + cy, x + cx] *= force_override_id;
                            B[y + cy, x + cx] += colortmpID;
                        }
                        else // just adding
                            B[y + cy, x + cx] = colortmpID;
                    }
            return false;
        }

        public int SumIDs()
        {
            int sum = 0;
            for(int i = 0; i < B.GetLength(0); ++i)
                for(int j = 0; j < B.GetLength(1); ++j)
                    if(B[i, j].HasValue)
                        sum += B[i, j].Value;
            return sum;
        }

        /// <summary>
        /// Moves overlapped blocks into blank locations
        /// </summary>
        /// <param name="force_override_id">override id - base of the numeral system</param>
        public void MoveOverlapped(int force_override_id)
        {
            List<(int, int)> holes = new List<(int, int)>();
            List<(int, int, int)> overlaps = new List<(int, int, int)>();

            for(int i = 0; i < B.GetLength(0); ++i)
                for(int j = 0; j < B.GetLength(1); ++j)
                {
                    if(B[i, j] == null)
                    {
                        holes.Add((i, j));
                    }
                    while(B[i, j].HasValue && B[i, j].Value > force_override_id)
                    {
                        overlaps.Add((i, j, B[i, j].Value % force_override_id));
                        B[i, j] /= force_override_id;
                    }
                }

            var zzip = overlaps.Zip(holes);
            // invent a good way to place overlapped blocks (minimize the cuts!)
            foreach((var overlap, var hole) in zzip)
            {
                B[hole.Item1, hole.Item2] = overlap.Item3;
            }
        }
    }
}
