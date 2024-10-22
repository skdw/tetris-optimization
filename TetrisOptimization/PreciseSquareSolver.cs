
#nullable enable
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading.Tasks;

namespace TetrisOptimization
{
    /// <summary>
    /// Precise solver for square board
    /// </summary>
    public class PreciseSquareSolver : PreciseSolver
    {
        public PreciseSquareSolver(List<(int, Block)> _blocks, int _blockSize, bool _concurrent = true, int _parallelStep = 1) : base(_blocks, _blockSize, _concurrent, _parallelStep)
        {
            forceSquare = true;
        }

        public override Board Solve()
        {
            Console.WriteLine("Solving the precise square problem");
            return InternalSolve();
        }

        private Board InternalSolve()
        {
            // Get blocks rotations
            var blocks_rot = BlocksRot();

            // Square root of the sum of blocks areas
            int a_min = (int)Math.Ceiling(Math.Sqrt(blocks_rot.Count * blockSize));

            // Sum of the long boxes edges
            int a_max = Enumerable.Sum(blocks_rot.Select(bls =>
            {
                var b0s = bls[0].Size;
                if (b0s.Item1 > b0s.Item2)
                    return b0s.Item1;
                return b0s.Item2;
            }));

            // Iterate over square sizes
            for (int a = a_min; a <= a_max; ++a)
            {
                Console.WriteLine($"Trying to fill blocks into the square of side {a}");
                int leftToFill = a * a;

                // Get the combinations
                (var combs, var combsCounts) = Combs(leftToFill);

                // Total number of combinations
                long combinationsNum = CombsNum(combsCounts);

                // Iterate over board positions' combinations
                Board? CheckCombination(long i)
                {
                    if (i % 1e3 == 0)
                        Console.WriteLine($"Analyzing combination #{i}...");

                    // get the combination
                    var chosen_comb = Comb(combs, combsCounts, i);

                    int?[] choose = new int?[blocks.Sum(b => b.Item1)];
                    int k = 0;
                    foreach (var combBlockType in chosen_comb) // for each block type
                    {
                        var blank_ids = Enumerable
                            .Range(0, a * a)
                            .Where(i => !choose.Contains(i))
                            .ToArray();

                        // at which blank id do we place the block?
                        foreach (var blockID in combBlockType) // for each block
                            choose[k++] = blank_ids[blockID];
                    }

                    // No nulls are left here
                    int[] combination = Array.ConvertAll(choose, value => value ?? default(int));

                    // Get an iterator
                    var blocks_chooses = BlocksChooses(blocks_rot);

                    // Iterate over blocks variations
                    foreach (var blocks_choice in blocks_chooses)
                    {
                        var comb_block = combination.Zip(blocks_choice, Tuple.Create);
                        var board = TryToCreateBoard(comb_block, a);
                        if (board != null)
                            return board;
                    }
                    return null;
                }

                if(concurrent)
                {
                    var resultCollection = new ConcurrentBag<Board?>();
                    for (int k = 0; k < combinationsNum; k += parallelStep)
                    {
                        Parallel.For(k, k + parallelStep, i => 
                            resultCollection.Add(CheckCombination(i)));
                        foreach (var board in resultCollection)
                            if (board != null)
                                return board;
                        resultCollection.Clear();
                    }
                }
                else
                {
                    for(int i = 0; i < combinationsNum; ++i)
                    {
                        var board = CheckCombination(i);
                        if(board != null)
                            return board;
                    }
                }
            }

            throw new OperationCanceledException("A board should be returned before");
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
                int flag = board.TryToAdd(coords.Item1, coords.Item2, block);
                if (flag < 0)
                    return null;
            }
            return board;
        }
#nullable disable
    }
}
