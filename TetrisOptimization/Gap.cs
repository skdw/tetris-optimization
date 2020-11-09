﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisOptimization
{
    public class Gap
    {
        public int[,] matrix { get; set; }
        public (int x, int y) size { get; set; }
        public (int x, int y) position { get; set; }
        public List<(int x, int y)> fields { get; set; }
        public Gap(Gap gap)
        {
            matrix = gap.matrix;
            size = gap.size;
            position = gap.position;
            fields = gap.fields;
        }

        public Gap((int x, int y) size, (int x, int y) position, List<(int x, int y)> fields)
        {
            this.size = size;
            this.position = position;
            this.fields = fields;
        }
    }
}
