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

        public Gap((int y, int x) size, (int y, int x) position, List<(int y, int x)> fields)
        {
            this.size = size;
            this.position = position;
            this.fields = fields;
        }
    }
}
