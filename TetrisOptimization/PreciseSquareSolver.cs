
using System.Collections.Generic;
using System.Linq;
using System;

namespace TetrisOptimization
{
    /// <summary>
    /// Precise solver for square board
    /// </summary>
    public class PreciseSquareSolver: BlocksSolver
    {
        public PreciseSquareSolver() {}
        protected override Board InternalSolve(List<List<Block>> blocks, int block_size)
        {
            // Count the number of rotations of each block
            var counts = CommonMethods.CountBlocks(blocks);

            // Number of blocks sequences
            int counts_product = counts.Aggregate(1, (acc, bl) => acc * bl);

            // Square root of the sum of blocks areas
            int a_min = (int)Math.Ceiling(Math.Sqrt(blocks.Count * block_size));

            // Sum of the long boxes edges
            int a_max = Enumerable.Sum(blocks.Select(bls => {
                var b0s = bls[0].size;
                if(b0s.Item1 > b0s.Item2)
                    return b0s.Item1;
                return b0s.Item2;
            }));

            // Iterate over square sizes
            for(int a = a_min; a < a_max; ++a)
            {
                Board board = new Board(a, a);
                var board_indexes = Enumerable.Range(0, a * a - 1);
                var permutations = CommonMethods.GetPermutations(board_indexes, a * a);

                // Permutate over board positions
                foreach(var permutation in permutations)
                {
                    // Iterate over blocks variations
                    for(int i = 0; i < counts_product; ++i)
                    {
                        var variation = CommonMethods.DecodeVariation(counts, i);
                        // ...
                    }
                }
            }
            throw new System.NotImplementedException();
        }
    }
}
