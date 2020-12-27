using System.Collections.Generic;

namespace TetrisOptimization
{
    /// <summary>
    /// Class handling the block matrix
    /// </summary>
    public class Block
    {
        public bool[,] matrix { get; }

        public Block(bool[,] matrix)
        {
            this.matrix = matrix;
            Size = (matrix.GetLength(0), matrix.GetLength(1));
        }

        public (int Y, int X) Size { get; }

        private List<(int, List<Block>)> cuts = null;

        public List<(int, List<Block>)> Cuts { get {
            if(cuts is null)
            {
                cuts = CuttingRectangle.GenerateCuts(this);
                cuts.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            }
            return cuts;
        } }

        /// <summary>
        /// Rotates block by 90 degrees
        /// </summary>
        /// <returns>Rotated block (deep copy)</returns>
        public Block Rotate()
        {
            bool[,] rot = new bool[Size.X, Size.Y];
            for (int i = 0; i < Size.Y; ++i)
                for (int j = 0; j < Size.X; ++j)
                    rot[j, i] = matrix[Size.Y - i - 1, j];
            return new Block(rot);
        }

        public override bool Equals(object obj)
        {
            Block block = (Block)obj;
            if (Size.X != block.Size.X || Size.Y != block.Size.Y)
                return false;
            for (int i = 0; i < Size.Y; ++i)
                for (int j = 0; j < Size.X; ++j)
                    if (matrix[i, j] != block.matrix[i, j])
                        return false;
            return true;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
