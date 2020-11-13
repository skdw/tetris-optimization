using System;

namespace TetrisOptimization
{
    public static class BlockFactory
    {
        /// <summary>
        /// Gets the block
        /// </summary>
        /// <param name="blockSize"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Block GetBlock(int blockSize, int? index = null)
        {
            switch (blockSize)
            {
                case 4:
                    if(index.HasValue)
                        return new TetrisBlock(index.Value);
                    return new TetrisBlock();
                case 5:
                    if (index.HasValue)
                        return new FiveBlock(index.Value);
                    return new FiveBlock();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
