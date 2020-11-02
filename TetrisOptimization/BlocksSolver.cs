using System.Collections.Generic;

namespace TetrisOptimization
{
    public abstract class BlocksSolver
    {
        public Board Solve(List<Block> blocks)
        {
            var block_rotations = CommonMethods.GetRotations(blocks);
            return InternalSolve(block_rotations);
        }
        protected abstract Board InternalSolve(List<List<Block>> blocks);
    }
}
