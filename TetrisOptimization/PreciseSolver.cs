using System.Collections.Generic;
using System.Linq;
using System;
using System.Numerics;

namespace TetrisOptimization
{
    /// <summary>
    /// Precise solver
    /// </summary>
    public abstract class PreciseSolver : BlocksSolver
    {
        protected PreciseSolver(List<(int, Block)> _blocks, int _blockSize) : base(_blocks, _blockSize)
        {
            cutBounds = true;
            forceSquare = true;
            var block_rotations = CommonMethods.GetRotations(blocks.Select(b => b.Item2).ToList());
            var zipp = blocks.Zip(block_rotations, (bl1, bl2) => (bl1.Item1, bl2));
        }

        protected IEnumerable<IEnumerable<Block>> BlocksChooses(IEnumerable<List<Block>> blocks_rot)
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

        protected List<List<Block>> BlocksRot()
        {
            var block_rotations = CommonMethods.GetRotations(blocks.Select(b => b.Item2).ToList());
            var zipp = blocks.Zip(block_rotations, (bl1, bl2) => (bl1.Item1, bl2));
            var blocks_rot = new List<List<Block>>();
            foreach (var (count, block) in zipp)
                for (int i = 0; i < count; ++i)
                    blocks_rot.Add(block);
            return blocks_rot;
        }

        protected (List<List<IEnumerable<int>>>, List<long>) Combs(int leftToFill)
        {
            // List of lists of combinations for each blocks_count for each id on board
            var combs = new List<List<IEnumerable<int>>>();
            List<long> combsCounts = new List<long>();
            foreach ((int num, Block b) in blocks)
            {
                Console.WriteLine($"{leftToFill} choose {num}...");
                var board_indexes = Enumerable.Range(0, leftToFill);
                var combs1 = CommonMethods.GetKCombs(board_indexes, num).ToList();
                combs.Add(combs1);
                int cnt = combs1.Count;
                combsCounts.Add(cnt);
                Console.WriteLine($"{leftToFill} choose {num} = {cnt} combinations without repetitions");
                leftToFill -= num;
            }
            return (combs, combsCounts);
        }

        protected long CombsNum(List<long> combsCounts)
        {
            BigInteger combinationsBigNum = combsCounts.Aggregate(BigInteger.One, (acc, bl) => acc * bl);
            Console.WriteLine($"Together: {combinationsBigNum} combinations without repetitions");

            long combinationsNum = long.MaxValue;
            if (combinationsBigNum > long.MaxValue)
                Console.WriteLine($"Processing only the first {long.MaxValue} combinations...");
            else
                combinationsNum = (long)combinationsBigNum;
            return combinationsNum;
        }

        protected IEnumerable<int>[] Comb(List<List<IEnumerable<int>>> combs, List<long> combsCounts, long i)
        {
            List<int> variation = CommonMethods.Decode(combsCounts, i);
            IEnumerable<int>[] chosen_comb = new IEnumerable<int>[combsCounts.Count];
            for (int j = 0; j < combsCounts.Count; ++j)
                chosen_comb[j] = combs[j][variation[j]];
            return chosen_comb;
        }
    }
}
