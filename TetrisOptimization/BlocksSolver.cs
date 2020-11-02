using System.Collections.Generic;

namespace TetrisOptimization
{
    public abstract class BlocksSolver
    {
        public Board Solve(List<Block> blocks, int block_size)
        {
            var block_rotations = CommonMethods.GetRotations(blocks);
            return InternalSolve(block_rotations, block_size);
        }
        protected abstract Board InternalSolve(List<List<Block>> blocks, int block_size);
    }
}
