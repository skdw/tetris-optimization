using System;

namespace TetrisOptimization
{
    /// <summary>
    /// Class handling the block matrix
    /// </summary>
    public class TetrisBlock : Block
    {
        static readonly Random random = new Random();

        static int GetRandomBlockId() => random.Next(TetrisBlocks.Blocks.Count) + 1;

        /// <summary>
        /// Gets a TetrisBlock of given index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TetrisBlock(int i): base(TetrisBlocks.Blocks[i]) { }

        /// <summary>
        /// Gets a random TetrisBlock
        /// </summary>
        public TetrisBlock(): base(TetrisBlocks.Blocks[GetRandomBlockId()]) { }
    }
}
