
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TetrisOptimization
{
    /// <summary>
    /// Precise solver for square board
    /// </summary>
    public class PreciseSquareSolver : BlocksSolver
    {
        private List<List<Block>> blocks_rot;

        public PreciseSquareSolver(List<(int, Block)> _blocks, int _blockSize) : base(_blocks, _blockSize)
        {
            cutBounds = true;
            forceSquare = true;
            var block_rotations = CommonMethods.GetRotations(blocks.Select(b => b.Item2).ToList());
            var zipp = blocks.Zip(block_rotations, (bl1, bl2) => (bl1.Item1, bl2));

            blocks_rot = new List<List<Block>>();
            foreach(var (count, block) in zipp)
                for(int i = 0; i < count; ++i)
                    blocks_rot.Add(block);
        }

        public override Board Solve()
        {
            Console.WriteLine("Solving the precise square problem");
            return InternalSolve(blocks_rot, blockSize);
        }

#nullable enable
        private Board? ProcessBlocksChoice(IEnumerable<IEnumerable<int>> combinations, IEnumerable<Block> blocks_choice, int a)
        {
            // Permutate over board positions
            foreach (var combination in combinations)
            {
                var comb_block = combination.Zip(blocks_choice, Tuple.Create);
                var board = TryToCreateBoard(comb_block, a);
                if(board != null)
                    return board;
            }
            return null;
        }

        private Board InternalSolve(List<List<Block>> blocks, int block_size)
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

            // Square root of the sum of blocks areas
            int a_min = (int)Math.Ceiling(Math.Sqrt(blocks.Count * block_size));

            // Sum of the long boxes edges
            int a_max = Enumerable.Sum(blocks.Select(bls =>
            {
                var b0s = bls[0].Size;
                if (b0s.Item1 > b0s.Item2)
                    return b0s.Item1;
                return b0s.Item2;
            }));

            // Iterate over square sizes
            for (int a = a_min; a <= a_max; ++a)
            {
                var board_indexes = Enumerable.Range(0, a * a - 1);
                var combinations = CommonMethods.GetCombinations(board_indexes, blocks.Count);

                // Board? res = blocks_chooses
                //     .AsParallel()
                //     .Select(blocks_choice => ProcessBlocksChoice(combinations, blocks_choice, a))
                //     .Where(b => b != null)
                //     .FirstOrDefault();

                // if(res != null)
                //     return res;

                // Iterate over blocks variations
                foreach(var blocks_choice in blocks_chooses)
                {
                    // Permutate over board positions
                    foreach (var combination in combinations)
                    {
                        var comb_block = combination.Zip(blocks_choice, Tuple.Create);
                        var board = TryToCreateBoard(comb_block, a);
                        if(board != null)
                            return board;
                    }
                }
            }
            throw new System.OperationCanceledException("A board should be returned before");
        }

        /// <summary>
        /// Tries to create square board of size a*a with blocks on specified positions
        /// </summary>
        /// <param name="perm_block"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private Board? TryToCreateBoard(IEnumerable<Tuple<int, Block>> perm_block, int a)
        {
            Board board = new Board(a, a);
            foreach ((int index, Block block) in perm_block)
            {
                var coords = CommonMethods.DecodeCoords(index, a, a);
                bool failure = board.TryToAdd(coords.Item1, coords.Item2, block);
                if (failure)
                    return null;
            }
            return board;
        }
        #nullable disable
    }
}
