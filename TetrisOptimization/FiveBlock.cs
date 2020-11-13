using System;

namespace TetrisOptimization
{
    /// <summary>
    /// Class handling the block matrix
    /// </summary>
    public class FiveBlock : Block
    {
        static readonly Random random = new Random();

        static int GetRandomBlockId() => random.Next(FiveBlocks.Blocks.Count) + 1;

        /// <summary>
        /// Gets a FiveBlock of given index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public FiveBlock(int i): base(FiveBlocks.Blocks[i + 1]) { }

        /// <summary>
        /// Gets a random FiveBlock
        /// </summary>
        public FiveBlock(): base(FiveBlocks.Blocks[GetRandomBlockId()]) { }
    }
}
