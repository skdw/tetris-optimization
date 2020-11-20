using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisOptimization
{
    class SixBlocks
    {
        //11111
        static readonly bool[,] K1 = {
            { true, true, true, true, true,true}
        };
        //static FiveBlock I = new FiveBlock(I_m, (5, 1));

        // 10000
        // 11111
        static readonly bool[,] K2 = {
            { true, false, false, false, false},
            { true, true, true, true, true}
        };

        // 01000
        // 11111
        static readonly bool[,] K3 = {
            { false, true, false, false, false},
            { true, true, true, true, true}
        };
        // 00100
        // 11111
        static readonly bool[,] K4 = {
            { false, false, true, false, false},
            { true, true, true, true, true}
        };
        // 11000
        // 01111
        static readonly bool[,] K5 = {
            { true, true, false, false,false},
            { false, true, true, true, true}
        };
        // 1100
        // 1111
        static readonly bool[,] K6 = {
            { true, true, false, false },
            { true, true, true, true }
        };
        // 1010
        // 1111
        static readonly bool[,] K7 = {
            { true,false, true, false },
            { true,true, true, true }
        };
        // 1001
        // 1111
        static readonly bool[,] K8 = {
            { true, false, false, true },
            { true, true, true, true }
        };
        // 0110
        // 1111
        static readonly bool[,] K9 = {
            { false, true, true, false },
            { true,true, true, true }
        };
        // 1000
        // 1000
        // 1111
        static readonly bool[,] K10 = {
            { true, false, false, false },
            { true, false, false, false },
            { true, true, true, true }
        };
        // 0100
        // 0100
        // 1111
        static readonly bool[,] K11 = {
            { false, true, false, false },
            { false, true, false, false },
            { true, true, true, true }
        };
        // 010
        // 010
        // 111
        static readonly bool[,] K12 = {
            { false, true,  false},
            { false, true,  false},
            { false, true,  false},
            { true, true, true }
        };
        // 011
        // 110
        // 010
        // 010
        static readonly bool[,] K13 = {
            { false, true, true},
            { true, true, false},
            { false, true, false},
            { false, true, false}
        };
        // 011
        // 010
        // 110
        // 010
        static readonly bool[,] K14 = {
            { false, true, true},
            { false, true, false},
            { true, true, false},
            { false, true, false}
        };
        // 011
        // 010
        // 010
        // 110
        static readonly bool[,] K15 = {
            { false, true, true},
            { false, true, false},
            { false, true, false},
            { true, true, false}
        };
        // 010
        // 011
        // 110
        // 010
        static readonly bool[,] K16 = {
            { false, true, false},
            { false, true, true},
            { true, true, false},
            { false, true, false}            
        };
        // 010
        // 111
        // 010
        // 010
        static readonly bool[,] K17 = {
            { false, true, false },
            { true, true, true },
            { false, true, false },
            { false, true, false }
        };
        // 010
        // 111
        // 100
        // 100
        static readonly bool[,] K18 = {
            { false, true, false },
            { true, true, true },
            { true, false, false },
            { true, false, false }
        };
        // 1101
        // 0111
        static readonly bool[,] K19 = {
            { true, true, false, true },
            { false, true, true, true}
        };
        // 11100
        // 00111
        static readonly bool[,] K20 = {
            { true, true, true, false, false },
            { false, false, true, true, true}
        };
        // 1110
        // 0111
        static readonly bool[,] K21 = {
            { true, true, true, false },
            { false, true, true, true}
        };
        // 111
        // 111
        static readonly bool[,] K22 = {
            { true, true, true},
            { true, true, true}
        };
        // 001
        // 111
        // 010
        // 010
        static readonly bool[,] K23 = {
            { false, false, true},
            { true, true, true},
            { false, true, false},
            { false, true, false}
        };        
        // 111
        // 011
        // 010
        static readonly bool[,] K24 = {
            { true, true, true},
            { false, true, true},
            { false, true, false}
        };
        // 001
        // 011
        // 110
        // 010
        static readonly bool[,] K25 = {
            { false, false, true },
            { false, true, true },
            { true, true, false },
            { false, true, false }
        };
        // 001
        // 001
        // 111
        // 100
        static readonly bool[,] K26 = {
            { false, false, true },
            { false, false, true },
            { true, true, true },
            { true, false, false }
        };
        // 001
        // 001
        // 011
        // 110
        static readonly bool[,] K27 = {
            { false, false, true },
            { false, false, true },
            { false, true, true },
            { true, true, false }
        };
        // 100
        // 101
        // 111
        static readonly bool[,] K28 = {
            { true, false, false },
            { true, false, true },
            { true, true, true }
        };
        // 101
        // 111
        // 100
        static readonly bool[,] K29 = {
            { true, false, true },
            { true, true, true },
            { true, false, false }
        };
        // 101
        // 111
        // 010
        static readonly bool[,] K30 = {
            { true, false, true },
            { true, true, true },
            { false, true, false }
        };
        // 011
        // 010
        // 110
        // 100
        static readonly bool[,] K31 = {
            { false, true, true },
            { false, true, false },
            { true, true, false },
            { true, false, false }
        };
        // 100
        // 110
        // 111
        static readonly bool[,] K32 = {
            { true, false, false },
            { true, true, false },
            { true, true, true }
        };
        // 010
        // 111
        // 110
        static readonly bool[,] K33 = {
            { false, true, false },
            { true, true, true },
            { true, true, false }
        };
        // 001
        // 111
        // 110
        static readonly bool[,] K34 = {
            { false, false, true },
            { true, true, true },
            { true, true, false }
        };
        // 001
        // 011
        // 110
        // 100
        static readonly bool[,] K35 = {
            { false, false, true },
            { false, true, true },
            { true, true, false },
            { true, false, false }
        };


        public static readonly Dictionary<int, bool[,]> Blocks = new Dictionary<int, bool[,]>()
        {
            {1, K1 },
            {2, K2},
            {3, K3},
            {4, K4},
            {5, K5},
            {6, K6},
            {7, K7 },
            {8, K8},
            {9, K9},
            {10, K10},
            {11, K11},
            {12, K12},
            {13, K13},
            {14, K14},
            {15, K15},
            {16, K16},
            {17, K17},
            {18, K18},
            {19, K19},
            {20, K20},
            {21, K21},
            {22, K22},
            {23, K23},
            {24, K24},
            {25, K25},
            {26, K26},
            {27, K27},
            {28, K28},
            {29, K29},
            {30, K30},
            {31, K31},
            {32, K32},
            {33, K33},
            {34, K34},
            {35, K35}
        };

    }
}
