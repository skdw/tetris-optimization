namespace TetrisOptimization
{
    /// <summary>
    /// Class handling the block matrix
    /// </summary>
    public class Block
    {
        public bool[,] matrix { get; }
        public (int y, int x) size { get; }

        public Block(bool[,] matrix, (int, int) size)
        {
            this.matrix = matrix;
            this.size = size;
        }

        public Block(bool[,] matrix)
        {
            this.matrix = matrix;
            this.size = (matrix.GetLength(0), matrix.GetLength(1));
        }

        /// <summary>
        /// Rotates block by 90 degrees
        /// </summary>
        /// <returns>Rotated block (deep copy)</returns>
        public Block Rotate()
        {
            bool[,] rot = new bool[size.x, size.y];
            for (int i = 0; i < size.y; ++i)
                for (int j = 0; j < size.x; ++j)
                    rot[j, i] = matrix[size.y - i - 1, j];
            return new Block(rot, (size.x, size.y));
        }

        public override bool Equals(object obj)
        {
            Block block = (Block)obj;
            if (size.x != block.size.x || size.y != block.size.y)
                return false;
            for (int i = 0; i < size.y; ++i)
                for (int j = 0; j < size.x; ++j)
                    if (matrix[i, j] != block.matrix[i, j])
                        return false;
            return true;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
