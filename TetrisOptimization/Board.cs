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
            Size = (y, x);
        }
        public Board(Board b)
        {
            B = b.B.Clone() as int?[,];
            if (B != null)
                Size = (B.GetLength(0), B.GetLength(1));
        }

        public readonly int?[,] B;
        public int? this[int i, int j]
        {
            get => B[i, j];
            set => B[i, j] = value;
        }

        private (int Y, int X) Size { get; }

        int _colorId = 0;

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
        /// <returns>True if an error occurs</returns>
        public bool TryToAdd(int y, int x, Block block, int? force_override_id = null)
        {
            int maxBoardSize = Math.Max(Size.X, Size.Y);
            int minBoardSize = Math.Min(Size.X, Size.Y);
            if(force_override_id.HasValue && (block.Size.X > maxBoardSize || block.Size.Y > minBoardSize))
            {
                Board board1 = new Board(this);
                bool status = board1.TryToAddCutBlock(y, x, new Block(block), force_override_id.Value);
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
                        return true;
                    }
                    else if (B[y + cy, x + cx].HasValue && block.matrix[cy, cx])
                    {
                        // Trying to override the block
                        if (force_override_id is null) // prevent overriding (square)
                            return true;
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
            return false;
        }

        private bool TryToAddCutBlock(int y, int x, Block block, int force_override_id)
        {
            Block blockCpy = new Block(block);
            int colortmpID = ++_colorId;

            if(block.Size.Y > Size.Y)
            {
                for(int ccy = 0; ccy + Size.Y - y - 1 < block.Size.Y; ccy += Math.Max(Size.Y - y, 1))// starting row of block to be placed
                {
                    for (int cy = 0; cy < Size.Y - y; ++cy) // board rows
                        for (int cx = 0; cx < block.Size.X; ++cx)
                            if (block.matrix[cy + ccy, cx])
                            {
                                if (B[y + cy, x + cx].HasValue) // overriding
                                {
                                    // cuts - different ids for the rows !!!
                                    B[y + cy, x + cx] *= force_override_id;
                                    B[y + cy, x + cx] += colortmpID;
                                }
                                else // just adding
                                    B[y + cy, x + cx] = colortmpID;
                                block.matrix[cy + ccy, cx] = false;
                                if(x == 0 && y == 0)
                                    Console.WriteLine($"cy: {cy}   ccy: {ccy}   cx: {cx}");
                            }
                    colortmpID = ++_colorId;
                }
                // block should be empty
            }

            else if(block.Size.X > Size.X)
            {
                for(int ccx = 0; ccx + Size.X - x - 1 < block.Size.X; ccx += Math.Max(Size.X - x, 1)) // starting column of block to be placed
                {
                    for (int cy = 0; cy < block.Size.Y; ++cy) 
                        for (int cx = 0; cx < Size.X - x; ++cx) // board columns
                            if (block.matrix[cy, cx + ccx])
                            {
                                if (B[y + cy, x + cx].HasValue) // overriding
                                {
                                    B[y + cy, x + cx] *= force_override_id;
                                    B[y + cy, x + cx] += colortmpID;
                                }
                                else // just adding
                                    B[y + cy, x + cx] = colortmpID;
                                block.matrix[cy, cx + ccx] = false;
                            }
                    colortmpID = ++_colorId;
                }
                // block should be empty
            }
            return false;
        }

        public bool TryToRemove(int y, int x, Block block)
        {
            for (int cy = 0; cy < block.Size.Y; ++cy)//y
                for (int cx = 0; cx < block.Size.X; ++cx)//x
                    if ((y + cy >= Size.Y) || (x + cx >= Size.X))
                    {
                        //Console.Error.WriteLine("Out of the board");
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
                        //Console.Error.WriteLine("Out of the board");
                        return false;
                    }
                    else if (B[y + cy, x + cx].HasValue && block.matrix[cy, cx])
                    {
                        //Console.Error.WriteLine("Trying to override the block");
                        return false;
                    }
            return true;
        }

        public int Badness()
        {
            int sum = 0;
            for (int i = 0; i < Size.Y; ++i)
                for (int j = 0; j < Size.X; ++j)
                    if (B[i, j].HasValue)
                        sum += B[i, j].Value;
            return sum;
        }

        /// <summary>
        /// Moves overlapped blocks into blank locations
        /// </summary>
        /// <param name="forceOverrideId">override id - base of the numeral system</param>
        public int MoveOverlapped(int forceOverrideId)
        {
            var finding = new FindingGaps(this);
            List<Gap> gaps = finding.FindGaps((0, Size.Y - 1, 0, Size.X - 1));

            var holes = new List<(int, int)>();
            var overlaps = new List<(int, int, int)>();

            for (int i = 0; i < Size.Y; ++i)
                for (int j = 0; j < Size.X; ++j)
                {
                    if (B[i, j] == null)
                    {
                        holes.Add((i, j));
                    }
                    while (B[i, j].HasValue && B[i, j].Value > forceOverrideId)
                    {
                        overlaps.Add((i, j, B[i, j].Value % forceOverrideId));
                        B[i, j] /= forceOverrideId;
                    }
                }

            List<List<int>> adjacentOverlapsIDs = new List<List<int>>();

            int?[,] overlapsDistances = new int?[overlaps.Count, overlaps.Count];
            for (int i = 0; i < overlaps.Count; ++i)
                for (int j = 0; j <= i; ++j)
                {
                    overlapsDistances[i, j] = GetDistance(overlaps[i], overlaps[j]);

                    // dist = 1 -> many cells of the same blocks are overlapping
                    if (overlapsDistances[i, j] <= 1)
                        adjacentOverlapsIDs.Add(new List<int>() { i, j });
                }

            for (int i = 1; i < adjacentOverlapsIDs.Count; ++i)
                for (int j = 0; j < i; ++j)
                    if (adjacentOverlapsIDs[j].Intersect(adjacentOverlapsIDs[i]).Any())
                    {
                        adjacentOverlapsIDs[j].AddRange(adjacentOverlapsIDs[i]);
                        adjacentOverlapsIDs[i].Clear();
                    }

            List<List<int>> unionOverlaps = adjacentOverlapsIDs
                .Where(x => x.Any())
                .Select(x => x.Distinct())
                .Select(x => x.ToList())
                .ToList();

            unionOverlaps.ForEach(x => x.Sort());
            // min X, Y coords of overlapping cells
            var minmaxYX = unionOverlaps.Select(ids =>
                (ids.Min(x => overlaps[x].Item1), (ids.Min(x => overlaps[x].Item2)), ids.Max(x => overlaps[x].Item1), (ids.Max(x => overlaps[x].Item2)))
                ).ToList();

            List<bool[,]> overlapsMatrices = unionOverlaps
                .Zip(minmaxYX)
                .Select(x =>
                {
                    var union = x.First;
                    var minmax = x.Second;
                    var matrix = new bool[minmax.Item3 - minmax.Item1 + 1, minmax.Item4 - minmax.Item2 + 1];
                    union?.ForEach(p =>
                        matrix[overlaps[p].Item1 - minmax.Item1, overlaps[p].Item2 - minmax.Item2] = true);
                    return matrix;
                }
            ).ToList();

            var overlapsBlocks = overlapsMatrices.Select(m => new Block(m)).ToList();
            var (ovBlocks, ovGaps) = CuttingRectangle.ExactFit(gaps, overlapsBlocks, this);
            var result = CuttingRectangle.UnitCut((ovBlocks, ovGaps), this, 0);
            return Badness();
        }

        /// <summary>
        /// Gets the distance between two blocks on the board
        /// </summary>
        /// <param name="y"></param>
        /// <param name="b1"></param>
        /// <param name="y"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static int GetDistance((int y, int x, int id) b1, (int y, int x, int id) b2)
        {
            var differentBlocks = b1.id == b2.id ? 0 : 2;
            return Math.Abs(b1.y - b2.y) + Math.Abs(b1.x - b2.x) + differentBlocks;
        }
    }
}
