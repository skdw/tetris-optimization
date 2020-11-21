using System;

namespace TetrisOptimization
{
    /// <summary>
    /// Class handling the block matrix
    /// </summary>
    public class SixBlock : Block
    {
        static readonly Random random = new Random();

        static int GetRandomBlockId() => random.Next(SixBlocks.Blocks.Count) + 1;

        /// <summary>
        /// Gets a TetrisBlock of given index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public SixBlock(int i): base(SixBlocks.Blocks[i]) { }

        /// <summary>
        /// Gets a random TetrisBlock
        /// </summary>
        public SixBlock(): base(SixBlocks.Blocks[GetRandomBlockId()]) { }
    }
}
