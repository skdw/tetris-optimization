using System.Collections.Generic;
using System.Linq;

namespace TetrisOptimization
{
    public abstract class BlocksSolver
    {
        protected List<(int, Block)> blocks;
        protected int blockSize;

        public BlocksSolver(List<(int, Block)> _blocks, int _blockSize)
        {
            blocks = _blocks;
            blockSize = _blockSize;
        }
        public abstract Board Solve();
    }
}
