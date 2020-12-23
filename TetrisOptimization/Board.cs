﻿using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace TetrisOptimization
{
    public class Board
    {
        public Board(int y, int x)
        {
            B = new int?[y, x];
            Size = (y, x);
        }
        public Board(Board b)
        {
            B = b.B.Clone() as int?[,];
            if (B != null)
                Size = (B.GetLength(0), B.GetLength(1));
            _colorId = b._colorId;
        }

        public Board(Board b, ref int colorId)
        {
            B = b.B.Clone() as int?[,];
            if (B != null)
                Size = (B.GetLength(0), B.GetLength(1));
            _colorId = colorId;
        }

        public readonly int?[,] B;
        public int? this[int i, int j]
        {
            get => B[i, j];
            set => B[i, j] = value;
        }

        public (int Y, int X) Size { get; }

        public int CutsNumber { get; set; }

        public int _colorId = 0;

        // Hash algorithm
        private static SHA256 sha256Hash = SHA256.Create();

        public string GetHash()
        {
            var S = Size;            
            var buffer = new byte[S.X * S.Y * 4];
            var span = new Span<byte>(buffer);
            for(int i = 0; i < S.Y; ++i)
                for(int j = 0; j < S.X; ++j)
                    BinaryPrimitives.WriteInt32LittleEndian(span.Slice((i * S.X + j) * 4, 4), B[i, j] ?? 0);

            // Convert the input string to a byte array and compute the hash.
            byte[] data = sha256Hash.ComputeHash(buffer);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Get ConsoleColor for printing blocks
        /// </summary>
        /// <param name="colorId">integer ID of color</param>
        /// <returns>console color</returns>
        private static ConsoleColor GetColor(int colorId) => (ConsoleColor)(colorId * 2 % 15 + 1);

        /// <summary>
        /// Print the size of the board
        /// </summary>
        static void PrintSize(bool forceSquare, (int, int, int, int) bounds)
        {
            var (h, w) = GetSize(bounds);
            Console.WriteLine(forceSquare ? $"Square of side: {h}" : $"Rectangle of size: h={h} w={w}");
        }

        /// <summary>
        /// Print the board
        /// </summary>
        /// <param name="cutBounds">Cuts the board's bounds</param>
        /// <param name="forceSquare">Force the board to be a square</param>
        public void Print(bool cutBounds = true, bool forceSquare = false, bool monochrome = false)
        {
            var hash = GetHash();
            Console.WriteLine($"board SHA256 hash: {hash}");

            if(CountElems() == 0)
            {
                Console.WriteLine("All of the board's elements are nulls!");
                return;
            }
            var bounds = GetBounds(cutBounds, forceSquare);
            PrintSize(forceSquare, bounds);

            if(bounds.minX < 0 || bounds.minY < 0)
            {
                Console.WriteLine("Wrong bounds! {0} {1} {2} {3}", bounds.minY, bounds.maxY, bounds.minX, bounds.maxX);
                return;
            }

            for (int i = bounds.minY; i < bounds.maxY; ++i)
            {
                Console.Write($"{i,5}");
                Console.Write("|");
                char consoleChar = ' ';
                for (var j = bounds.minX; j < bounds.maxX; ++j)
                {
                    try
                    {
                        if (B[i, j].HasValue)
                        {
                            Console.BackgroundColor = GetColor(B[i, j].Value);
                            if(monochrome)
                                consoleChar = B[i, j].Value.ToString().First();
                            else
                                consoleChar = ' ';
                        }
                        else
                            consoleChar = ' ';
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        consoleChar = ' ';
                    }
                    Console.Write(new String(consoleChar, 2));
                    Console.ResetColor();
                }
                Console.Write("|\n");
            }
        }

        /// <summary>
        /// Counts the board's elements
        /// </summary>
        /// <returns>int</returns>
        public int CountElems()
        {
            int elems = 0;
            for(int i = 0; i < Size.Y; ++i)
                for(int j = 0; j < Size.X; ++j)
                    if(B[i, j].HasValue)
                        elems++;
            return elems;
        }

        int GetMinYFilled()
        {
            for (int i = 0; i < Size.Y; ++i)
                for (int j = 0; j < Size.X; ++j)
                    if (B[i, j].HasValue)
                        return i;
            return -1;
        }

        int GetMaxYFilled()
        {
            for (int i = Size.Y - 1; i >= 0; --i)
                for (int j = 0; j < Size.X; ++j)
                    if (B[i, j].HasValue)
                        return i;
            return -1;
        }

        int GetMinXFilled()
        {
            for (int j = 0; j < Size.X; ++j)
                for (int i = 0; i < Size.Y; ++i)
                    if (B[i, j].HasValue)
                        return j;
            return -1;
        }

        int GetMaxXFilled()
        {
            for (int j = Size.X - 1; j >= 0; --j)
                for (int i = 0; i < Size.Y; ++i)
                    if (B[i, j].HasValue)
                        return j;
            return -1;
        }

        (int minY, int maxY, int minX, int maxX) GetBounds(bool cutBounds, bool forceSquare)
        {
            if (cutBounds == false)
                return (0, Size.Y, 0, Size.X);

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

        public (int minY, int maxY, int minX, int maxX) GetBoundsPublic(bool cutBounds, bool forceSquare)
        {
            return this.GetBounds(cutBounds, forceSquare);
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
        /// <returns>Number of cuts / -1 if the block is not placed</returns>
        public int TryToAdd(int y, int x, Block block, int? force_override_id = null,bool cut = true)
        {
            int shortBlockSize = Math.Min(block.Size.X, block.Size.Y);
            int longBlockSize = Math.Max(block.Size.X, block.Size.Y);
            int shortBoardSize = Math.Min(Size.X, Size.Y);
            int longBoardSize = Math.Max(Size.X, Size.Y);
            if(force_override_id.HasValue && (block.Size.X > shortBoardSize || block.Size.Y > shortBoardSize) && cut)
            {
                Board board1 = new Board(this, ref _colorId);
                int status = board1.TryToAddCutBlock(y, x, new Block(block), force_override_id.Value);
                for(int cy = 0; cy < Size.Y; cy++)
                    for(int cx = 0; cx < Size.X; cx++)
                        B[cy, cx] = board1.B[cy, cx];
                return status;
            }

            // Check for errors
            for (int cy = 0; cy < block.Size.Y; ++cy)
                for (int cx = 0; cx < block.Size.X; ++cx)
                    if ((y + cy >= Size.Y) || (x + cx >= Size.X))
                    {
                        // Out of the board
                        return -1;
                    }
                    else if (B[y + cy, x + cx].HasValue && block.matrix[cy, cx])
                    {
                        // Trying to override the block
                        if (force_override_id is null) // prevent overriding (square)
                            return -1;
                    }

            // Add block to the board
            int colortmpID = ++_colorId;
            for (int cy = 0; cy < block.Size.Y; ++cy)
                for (int cx = 0; cx < block.Size.X; ++cx)
                    if (block.matrix[cy, cx])
                    {
                        if (B[y + cy, x + cx].HasValue) // overriding
                        {
                            B[y + cy, x + cx] *= force_override_id;
                            B[y + cy, x + cx] += colortmpID;
                        }
                        else // just adding
                            B[y + cy, x + cx] = colortmpID;
                    }

            return 0;
        }

        private int TryToAddCutBlock(int y, int x, Block block, int force_override_id)
        {
            //int startElems = CountElems();
            Block blockCpy = new Block(block);
            int colortmpID = ++_colorId;
            int cutsCount = 0;

            if(block.Size.Y > Size.Y)
            {
                for(int ccy = 0; ccy + Size.Y - y - Math.Max(Size.Y - y, 1) < block.Size.Y; ccy += Math.Max(Size.Y - y, 1))// starting row of block to be placed
                {
                    for (int cy = 0; cy < Size.Y - y; ++cy) // board rows
                        for (int cx = 0; cx < block.Size.X; ++cx)
                            if (cy+ccy< block.matrix.GetLength(0) && cx< block.matrix.GetLength(1) && block.matrix[cy + ccy, cx] && y+cy <B.GetLength(0) && x+cx<B.GetLength(1))
                            {
                                if (B[y + cy, x + cx].HasValue) // overriding
                                {
                                    // cuts - different ids for the rows !!!
                                    B[y + cy, x + cx] *= force_override_id;
                                    B[y + cy, x + cx] += colortmpID;
                                }
                                else // just adding
                                    B[y + cy, x + cx] = colortmpID;
                            }
                    colortmpID = ++_colorId;
                    if(ccy > 0)
                        cutsCount++;
                }
                // block should be empty
            }

            else if(block.Size.X > Size.X)
            {
                for(int ccx = 0; ccx + Size.X - x - Math.Max(Size.X - x, 1) < block.Size.X; ccx += Math.Max(Size.X - x, 1)) // starting column of block to be placed
                {
                    for (int cy = 0; cy < block.Size.Y; ++cy) 
                        for (int cx = 0; cx < Size.X - x; ++cx) // board columns
                            if (cy  < block.matrix.GetLength(0) && cx + ccx < block.matrix.GetLength(1) && block.matrix[cy, cx + ccx] && y + cy < B.GetLength(0) && x + cx < B.GetLength(1))
                            {
                                if (B[y + cy, x + cx].HasValue) // overriding
                                {
                                    B[y + cy, x + cx] *= force_override_id;
                                    B[y + cy, x + cx] += colortmpID;
                                }
                                else // just adding
                                    B[y + cy, x + cx] = colortmpID;
                            }
                    colortmpID = ++_colorId;
                    if(ccx > 0)
                        cutsCount++;
                }
            }

            return cutsCount;
        }

        public bool TryToRemove(int y, int x, Block block)
        {
            for (int cy = 0; cy < block.Size.Y; ++cy)//y
                for (int cx = 0; cx < block.Size.X; ++cx)//x
                    if ((y + cy >= Size.Y) || (x + cx >= Size.X))
                    {
                        return false;
                    }
            for (int cy = 0; cy < block.Size.Y; ++cy)//y
                for (int cx = 0; cx < block.Size.X; ++cx)
                    if (block.matrix[cy, cx])
                        B[y + cy, x + cx] = null;
            return true;
        }

        public bool ScanBoard(int y, int x, Block block)
        {
            for (int cy = 0; cy < block.Size.Y; ++cy)//y
                for (int cx = 0; cx < block.Size.X; ++cx)//x
                    if ((y + cy >= Size.Y) || (x + cx >= Size.X))
                    {
                        return false;
                    }
                    else if (B[y + cy, x + cx].HasValue && block.matrix[cy, cx])
                    {
                        return false;
                    }
            return true;
        }
    }
}
