
using System.Collections.Generic;
using System.Linq;
using System;
using System.Numerics;

namespace TetrisOptimization
{
    /// <summary>
    /// Precise solver for square board
    /// </summary>
    public class PreciseSquareSolver : BlocksSolver
    {
        public PreciseSquareSolver(List<(int, Block)> _blocks, int _blockSize) : base(_blocks, _blockSize)
        {
            cutBounds = true;
            forceSquare = true;
            var block_rotations = CommonMethods.GetRotations(blocks.Select(b => b.Item2).ToList());
            var zipp = blocks.Zip(block_rotations, (bl1, bl2) => (bl1.Item1, bl2));
        }

        public override Board Solve()
        {
            Console.WriteLine("Solving the precise square problem");
            return InternalSolve();
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

        public IEnumerable<IEnumerable<Block>> BlocksChooses(IEnumerable<List<Block>> blocks_rot)
        {
            // Count the number of rotations of each block
            var counts = CommonMethods.CountBlocks(blocks_rot);

            // Number of blocks sequences
            long counts_product = counts.Aggregate((long)1, (acc, bl) => acc * bl);

            for (long i = 0; i < counts_product; i++)
            {
                // Rotate blocks chooses
                yield return blocks_rot.Zip(CommonMethods.Decode(counts, i), (block, ind) => block[ind]);
            }
        }

        private Board InternalSolve()
        {
            int blocks_no = blocks.Sum(b => b.Item1);
            var block_rotations = CommonMethods.GetRotations(blocks.Select(b => b.Item2).ToList());
            var zipp = blocks.Zip(block_rotations, (bl1, bl2) => (bl1.Item1, bl2));
            var blocks_rot = new List<List<Block>>();
            foreach(var (count, block) in zipp)
                for(int i = 0; i < count; ++i)
                    blocks_rot.Add(block);

            // Square root of the sum of blocks areas
            int a_min = (int)Math.Ceiling(Math.Sqrt(blocks_rot.Count * blockSize));

            // Sum of the long boxes edges
            int a_max = Enumerable.Sum(blocks_rot.Select(bls =>
            {
                var b0s = bls[0].size;
                if (b0s.Item1 > b0s.Item2)
                    return b0s.Item1;
                return b0s.Item2;
            }));

            // Iterate over square sizes
            for (int a = a_min; a <= a_max; ++a)
            {
                Console.WriteLine($"Trying to fill blocks into the square of side {a}");
                int leftToFill = a * a;

                // list of lists of combinations for each blocks_count for each id on board
                var combs = new List<List<IEnumerable<int>>>();
                List<long> combs_counts = new List<long>();
                foreach((int num, Block b) in blocks)
                {
                    Console.WriteLine($"{leftToFill} choose {num}...");
                    var board_indexes = Enumerable.Range(0, leftToFill);
                    var combs1 = CommonMethods.GetKCombs(board_indexes, num).ToList();
                    combs.Add(combs1);
                    int cnt = combs1.Count;
                    combs_counts.Add(cnt);
                    Console.WriteLine($"{leftToFill} choose {num} = {cnt} combinations without repetitions");
                    leftToFill -= num;
                }

                BigInteger combinations_big_num = combs_counts.Aggregate(BigInteger.One, (acc, bl) => acc * bl );
                Console.WriteLine($"Together: {combinations_big_num} combinations without repetitions");

                long combinations_num = long.MaxValue;
                if(combinations_big_num > long.MaxValue)
                    Console.WriteLine($"Processing only the first {long.MaxValue} combinations...");
                else
                    combinations_num = (long)combinations_big_num;

                // Iterate over board positions' combinations
                for(long i = 0; i < combinations_num; ++i)
                {
                    if(i % 1e3 == 0)
                        Console.WriteLine($"Analyzing combination #{i}...");

                    List<int> variation = CommonMethods.Decode(combs_counts, i);
                    IEnumerable<int>[] chosen_comb = new IEnumerable<int>[combs_counts.Count];
                    for(int j = 0; j < combs_counts.Count; ++j)
                        chosen_comb[j] = combs[j][variation[j]];

                    int?[] choose = new int?[blocks_no];
                    int k = 0;
                    foreach(var combBlockType in chosen_comb) // for each block type
                    {
                        var blank_ids = Enumerable
                            .Range(0, a * a)
                            .Where(i => !choose.Contains(i))
                            .ToList();

                        // at which blank id do we place the block?
                        foreach(var blockID in combBlockType) // for each block
                            choose[k++] = blank_ids[blockID];
                    }

                    // No nulls are left here
                    int[] combination = Array.ConvertAll(choose, value => value ?? default(int));

                    // Get an iterator
                    var blocks_chooses = BlocksChooses(blocks_rot);

                    // Iterate over blocks variations
                    foreach(var blocks_choice in blocks_chooses)
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
