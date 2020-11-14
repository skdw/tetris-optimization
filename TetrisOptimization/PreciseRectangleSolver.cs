
using System.Collections.Generic;
using System.Linq;
using System;

namespace TetrisOptimization
{
    /// <summary>
    /// Precise solver for rectangle board
    /// </summary>
    public class PreciseRectangleSolver : BlocksSolver
    {
        private List<List<Block>> blocks_rot;

        public PreciseRectangleSolver(List<(int, Block)> _blocks, int _blockSize) : base(_blocks, _blockSize)
        {
            cutBounds = false;
            forceSquare = false;
            var block_rotations = CommonMethods.GetRotations(blocks.Select(b => b.Item2).ToList());
            var zipp = blocks.Zip(block_rotations, (bl1, bl2) => (bl1.Item1, bl2));

            blocks_rot = new List<List<Block>>();
            foreach(var (count, block) in zipp)
                for(int i = 0; i < count; ++i)
                    blocks_rot.Add(block);
        }

        public override Board Solve()
        {
            Console.WriteLine("Solving the precise rectangle problem");
            return InternalSolve(blocks_rot, blockSize).Item1;
        }

        /// <summary>
        /// Solves the task for precise rectangle.
        /// </summary>
        /// <param name="blocks"></param>
        /// <param name="block_size"></param>
        /// <returns></returns>
        private (Board, int) InternalSolve(List<List<Block>> blocks, int block_size)
        {
            // Count the number of rotations of each block
            var counts = CommonMethods.CountBlocks(blocks);

            // Number of blocks sequences
            int counts_product = counts.Aggregate(1, (acc, bl) => acc * bl);

            // Rotate blocks chooses
            var blocks_chooses = Enumerable
                .Range(0, counts_product)
                .Select(i => blocks.Zip(CommonMethods.DecodeVariation(counts, i), (block, ind) => block[ind]))
                .ToList();

            // Get rectangle size
            (int a, int b) = GetRectangleSize();

            var board_indexes = Enumerable.Range(0, a * b - 1);
            var combinations = CommonMethods.GetCombinations(board_indexes, blocks.Count);

            int bestLength = int.MaxValue;
            Board bestBoard = new Board(a, b);

            // Iterate over blocks variations
            foreach (var blocks_choice in blocks_chooses)
            {
                // Permutate over board positions
                foreach (var combination in combinations)
                {
                    var comb_block = combination.Zip(blocks_choice, Tuple.Create);
                    (Board board, int cutLength) = CreateCutBoard(comb_block, a, b);
                    if(cutLength == 0)
                        return (board, cutLength);
                    if(cutLength < bestLength)
                    {
                        bestBoard = board;
                        bestLength = cutLength;
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
            for(int i = sqrt; i > 0; --i)
            {
                int a = i;
                int b = area / a;
                if(a * b == area)
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
            //List<Tuple<int, Block>> overlappingBlocks = new List<Tuple<int, Block>>(); 
            foreach (var ind_bl in perm_block)
            {
                (int index, Block block) = ind_bl;
                var coords = CommonMethods.DecodeCoords(index, a, b);
                bool failure = board.TryToAdd(coords.Item1, coords.Item2, block, force_override_id);
                if (failure)
                    return (board, Int32.MaxValue);
                    //overlappingBlocks.Add(ind_bl);
            }

            //int cuts = overlappingBlocks.Count; // count it
            // solve the cuts
            // var gaps = GetGaps(board);
            //board.Print();
            int cuts = board.SumIDs();
            board.MoveOverlapped(force_override_id);
            return (board, cuts);
        }
    }
}
