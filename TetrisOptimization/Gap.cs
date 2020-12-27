using System.Collections.Generic;

namespace TetrisOptimization
{
    public class Gap
    {
        public int[,] matrix { get; set; }
        public (int y, int x) size { get; set; }
        public (int y, int x) position { get; set; }
        public List<(int y, int x)> fields { get; set; }
        public Gap(Gap gap)
        {
            matrix = gap.matrix;
            size = gap.size;
            position = gap.position;
            fields = gap.fields;
        }

        /// <summary>
        /// Create a new gap
        /// </summary>
        /// <param name="size">size of matrix</param>
        /// <param name="position">position on board</param>
        /// <param name="fields">empty fields on board contained by the gap</param>
        public Gap((int y, int x) size, (int y, int x) position, List<(int y, int x)> fields)
        {
            this.size = size;
            this.position = position;
            this.fields = fields;
        }

        public void PrepareMatrix()
        {
            matrix = FindingGaps.PrepareMatrix(size, position, fields);
        }
    }
}
