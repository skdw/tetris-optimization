using System;

namespace TetrisOptimization
{
    /// <summary>
    /// Class handling the block matrix
    /// </summary>
    public class FiveBlock : Block
    {
        private static readonly Random Random = new Random();

        static int GetRandomBlockId() => Random.Next(FiveBlocks.Blocks.Count) + 1;

        /// <summary>
        /// Gets a FiveBlock of given index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public FiveBlock(int i): base(FiveBlocks.Blocks[i]) { }

        /// <summary>
        /// Gets a random FiveBlock
        /// </summary>
        public FiveBlock(): base(FiveBlocks.Blocks[GetRandomBlockId()]) { }
    }
}
