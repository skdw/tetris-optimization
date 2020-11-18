
using System.Collections.Generic;
using System.Linq;
using System;
using System.Numerics;

namespace TetrisOptimization
{
    /// <summary>
    /// Precise solver for rectangle board
    /// </summary>
    public class PreciseRectangleSolver : PreciseSolver
    {
        public PreciseRectangleSolver(List<(int, Block)> _blocks, int _blockSize) : base(_blocks, _blockSize) { }

        public override Board Solve()
        {
            Console.WriteLine("Solving the precise rectangle problem");
            return InternalSolve().Item1;
        }

        /// <summary>
        /// Solves the task for precise rectangle.
        /// </summary>
        /// <param name="blocks"></param>
        /// <param name="block_size"></param>
        /// <returns></returns>
        private (Board, int) InternalSolve()
        {
            // Get blocks rotations
            var blocks_rot = BlocksRot();

            // Get rectangle size
            (int a, int b) = GetRectangleSize();

            var board_indexes = Enumerable.Range(0, a * b);
            var combinations = CommonMethods.GetCombinations(board_indexes, blocks.Count);

            int bestLength = int.MaxValue;
            Board bestBoard = new Board(a, b);

            Console.WriteLine($"Trying to fill blocks into the rectangle of size {a} x {b}");
            int leftToFill = a * b;

            // Get the combinations
            (var combs, var combsCounts) = Combs(leftToFill);

            // Total number of combinations
            long combinationsNum = CombsNum(combsCounts);

            // Iterate over board positions' combinations
            for (long i = 0; i < combinationsNum; ++i)
            {
                if (i % 1e3 == 0)
                    Console.WriteLine($"Analyzing combination #{i}...");

                // get the combination
                var chosen_comb = Comb(combs, combsCounts, i);

                int?[] choose = new int?[blocks.Sum(b => b.Item1)];
                int k = 0;
                foreach (var combBlockType in chosen_comb) // for each block type
                {
                    List<int> blank_ids = Enumerable
                        .Range(0, a * b)
                        .Where(i => !choose.Contains(i))
                        .ToList();

                    // at which blank id do we place the block?
                    foreach (var blockID in combBlockType) // for each block
                        choose[k++] = blank_ids[blockID];

                    // No nulls are left here
                    int[] combination = Array.ConvertAll(choose, value => value ?? default(int));

                    // Get an iterator
                    var blocks_chooses = BlocksChooses(blocks_rot);

                    // Iterate over blocks variations
                    foreach (var blocks_choice in blocks_chooses)
                    {
                        var comb_block = combination.Zip(blocks_choice, Tuple.Create);
                        (Board board, int cutLength) = CreateCutBoard(comb_block, a, b);
                        if (cutLength == 0)
                            return (board, cutLength);
                        if (cutLength < bestLength)
                        {
                            bestBoard = board;
                            bestLength = cutLength;
                        }
                    }
                }
            }

            Console.WriteLine($"Badness: {bestLength}");
            return (bestBoard, bestLength);
        }

        /// <summary>
        /// Counts the proper rectangle size
        /// </summary>
        /// <returns></returns>
        private (int, int) GetRectangleSize()
        {
            int blocks_count = blocks.Sum(b => b.Item1);
            int area = blocks_count * blockSize;
            int sqrt = (int)Math.Sqrt(area);
            for (int i = sqrt; i > 0; --i)
            {
                int a = i;
                int b = area / a;
                if (a * b == area)
                    return (a, b);
            }
            return (1, area);
        }

        /// <summary>
        /// Tries to create rectangle board of size a*b with blocks on specified positions
        /// </summary>
        /// <param name="perm_block"></param>
        /// <param name="a">board rows</param>
        /// <param name="b">board columns</param>
        /// <returns>(Board, int) - optimal board, minimal cuts number</returns>
        private (Board, int) CreateCutBoard(IEnumerable<Tuple<int, Block>> perm_block, int a, int b)
        {
            int force_override_id = perm_block.Count() + 1;
            Board board = new Board(a, b);

            // At first, place the blocks which do not conflict each other
            List<Tuple<int, Block>> overlappingBlocks = new List<Tuple<int, Block>>(); 
            foreach (var ind_bl in perm_block)
            {
                (int index, Block block) = ind_bl;
                var coords = CommonMethods.DecodeCoords(index, a, b);
                bool failure = board.TryToAdd(coords.Item1, coords.Item2, block);
                if (failure)
                    overlappingBlocks.Add(ind_bl);
            }

            // Then force to place the blocks which did not fit earlier
            // (place them on top of previous blocks)
            foreach (var ind_bl in overlappingBlocks)
            {
                (int index, Block block) = ind_bl;
                var coords = CommonMethods.DecodeCoords(index, a, b);
                bool failure = board.TryToAdd(coords.Item1, coords.Item2, block, force_override_id);
                if (failure)
                    return (board, Int32.MaxValue);
            }

            int cuts = board.MoveOverlapped(force_override_id);
            return (board, cuts);
        }
    }
}
