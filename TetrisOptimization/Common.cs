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

        /// <summary>
        /// Gets all permutations from a list
        /// </summary>
        /// <param name="list">Input list</param>
        /// <param name="length">Input list length</param>
        /// <typeparam name="T">List element type</typeparam>
        /// <returns>List of permutations</returns>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if(length == 1)
                return list.Select(t => new T[] {t});
            return GetPermutations(list, length-1)
                    .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] {t2}));
        }

        public static List<int> CountBlocks(List<List<Block>> blocks) =>
            blocks.Select(bl => bl.Count).ToList();

        public static List<int> DecodeVariation(List<int> counts, int code)
        {
            List<int> result = new List<int>();
            for(int i = 0; i < counts.Count; ++i)
            {
                result.Add(code % counts[i]);
                code /= counts[i];
            }
            return result;
        }
    }
}
