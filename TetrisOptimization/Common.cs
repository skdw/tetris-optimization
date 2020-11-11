using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisOptimization
{
    /// <summary>
    /// Common methods for handling multiple algorithms (2.1)
    /// </summary>
    public static class CommonMethods
    {
        /// <summary>
        /// Gets a list of available rotations for each block in a list
        /// </summary>
        /// <param name="blocks">Input blocks list</param>
        /// <returns>List of lists of blocks</returns>
        public static List<List<Block>> GetRotations(List<Block> blocks) =>
            blocks.Select(block => GetBlockRotations(block)).ToList();
        private static List<Block> GetBlockRotations(Block block)
        {
            var rot0 = block;
            var rot1 = block.Rotate();
            var rot2 = rot1.Rotate();
            var rot3 = rot2.Rotate();
            List<Block> rotations = new List<Block>() { rot0, rot1, rot2, rot3 };
            return rotations.Distinct().ToList();
        }

        public static Block GetSpecyficRotation(Block block, int rot)
        {
            if (rot == 0)
                return block;
            else if (rot == 1)
                return block.Rotate();
            else if (rot == 2)
                return block.Rotate().Rotate();
            else
                return block.Rotate().Rotate().Rotate();
        }
        public static int MinSqareSize(int num_block, int size)
        {
            return (int)Math.Ceiling(Math.Sqrt(num_block * size));
        }

        /// <summary>
        /// Gets all combinations from a list of a given length
        /// </summary>
        /// <param name="list">Input list</param>
        /// <param name="length">Given combination length</param>
        /// <typeparam name="T">List element type</typeparam>
        /// <returns>List of permutations</returns>
        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1)
                return list.Select(t => new T[] { t });
            return GetCombinations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static List<int> CountBlocks(List<List<Block>> blocks) =>
            blocks.Select(bl => bl.Count).ToList();

        public static List<int> DecodeVariation(List<int> counts, int code)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < counts.Count; ++i)
            {
                result.Add(code % counts[i]);
                code /= counts[i];
            }
            return result;
        }

        public static (int, int) DecodeCoords(int code, int a) =>
            (code / a, code % a);
    }
}
