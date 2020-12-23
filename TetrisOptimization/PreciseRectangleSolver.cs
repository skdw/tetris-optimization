
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading.Tasks;

namespace TetrisOptimization
{
    /// <summary>
    /// Precise solver for rectangle board
    /// </summary>
    public class PreciseRectangleSolver : PreciseSolver
    {
        public PreciseRectangleSolver(List<(int, Block)> _blocks, int _blockSize, int _parallelStep = 1) : base(_blocks, _blockSize, _parallelStep)
        {
            forceSquare = false;
        }

        public override Board Solve()
        {
            Console.WriteLine("Solving the precise rectangle problem");
            var res = InternalSolve();
            res.Item1.CutsNumber = res.Item2;
            return res.Item1;
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
            (Board, int)? CheckCombination(long i)
            {
             
                if (i % 1e3 == 0)
                    Console.WriteLine($"Analyzing combination #{i}...");

                // get the combination
                var chosen_comb = Comb(combs, combsCounts, i);

                int?[] choose = new int?[blocks.Sum(b => b.Item1)];
                int k = 0;
                foreach (var combBlockType in chosen_comb) // for each block type
                {
                    // board elements which are not filled yet
                    var blank_ids = Enumerable
                        .Range(0, a * b)
                        .Where(i => !choose.Contains(i))
                        .ToArray();

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
                        if (cutLength == 0 && board.CountElems() == a * b)
                            return (board, cutLength);
                        if (cutLength < bestLength && board.CountElems() == a * b)
                        {
                            bestBoard = board;
                            bestLength = cutLength;
                        }
                    }
                }
                return null;
            }
            
            bool concurrent = false;
            if(concurrent)
            {
                var resultCollection = new ConcurrentBag<(Board, int)?>();
                for (int k = 0; k < combinationsNum; k += parallelStep)
                {
                    Parallel.For(k, k + parallelStep, i => 
                        resultCollection.Add(CheckCombination(i)));
                    foreach (var board in resultCollection)
                        if (board != null)
                        {
                            Console.WriteLine($"Number of cuts: {bestLength}");
                            return board.Value;
                        }
                    resultCollection.Clear();
                }
            }
            else
            {
                for(int i = 0; i < combinationsNum; ++i)
                {
                    var board = CheckCombination(i);
                    if (board != null)
                    {
                        Console.WriteLine($"Number of cuts: {board.Value.Item2}");
                        return board.Value;
                    }
                }
            }

            Console.WriteLine($"Number of cuts: {bestLength}");
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
            int cutsSum = 0;
            Board board = new Board(a, b);

            // At first, place the blocks which do not conflict each other
            List<Tuple<int, Block>> overlappingBlocks = new List<Tuple<int, Block>>(); 
            foreach (var ind_bl in perm_block)
            {
                (int index, Block block) = ind_bl;
                var coords = CommonMethods.DecodeCoords(index, a, b);
                int cutsFlag = board.TryToAdd(coords.Item1, coords.Item2, block);
                if (cutsFlag < 0)
                    overlappingBlocks.Add(ind_bl);
            }

            // tu mamy klocki, które nie weszły
            // mamy planszę z 1 poziomem
            overlappingBlocks.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            var cutBlocks = new List<Block>();
            // Then force to place the blocks which did not fit earlier
            // (place them on top of previous blocks)
            foreach (var ind_bl in overlappingBlocks)
            {
                (int index, Block block) = ind_bl;
                cutBlocks.Add(block);
            }
            var res = MoveOverlapped(board, cutBlocks);
            cutsSum += res.Item1;
            return (res.Item2, cutsSum);
        }

        /// <summary>
        /// Moves overlapped blocks into blank locations
        /// </summary>
        /// <returns>Number of cuts made to move the overlapped blocks into blank positions.</returns>
        public (int,Board) MoveOverlapped(Board board, List<Block> cutBlocks)
        {
            int cutsNumber = 0;

            // Find the gaps
            var finding = new FindingGaps(board);
            var Size = board.Size;
            List<Gap> gaps = finding.FindGaps((0, Size.Y - 1, 0, Size.X - 1));

            foreach (var block in cutBlocks)
            {
                var cuts = block.Cuts;
                foreach (var cut in cuts)
                {
                    var bls = cut.Item2;
                    var brd1 = new Board(board);
                    List<Gap> tmp_gaps = new List<Gap>(gaps);

                    (bls, tmp_gaps) = CuttingRectangle.ExactFit(tmp_gaps, bls, brd1);
                    if(bls.Count==0 && tmp_gaps.Count==0)
                    {
                        gaps = tmp_gaps;
                        board = brd1;
                        cutsNumber += cut.Item1;
                        break;
                    }
                    else
                    {
                        // not exact fit - czy udalo sie wrzucic cala reszte do dziur wiekszych?
                        var res = CuttingRectangle.NotExactFit(tmp_gaps, bls, brd1);
                        if (res.Item1)
                        {
                            gaps = tmp_gaps;
                            board = brd1;
                            cutsNumber += cut.Item1;
                            break;
                        }
                    }
                }
            }
            return (cutsNumber, board);
        }
    }
}
