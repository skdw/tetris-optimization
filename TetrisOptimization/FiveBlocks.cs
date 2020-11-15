using System.Collections.Generic;

namespace TetrisOptimization
{
    public static class FiveBlocks
    {
        static readonly bool[,] I_m = {
            { true, true, true, true, true}
        };
        //static FiveBlock I = new FiveBlock(I_m, (5, 1));

        // 1000
        // 1111
        static readonly bool[,] L1_m = {
            { true, false, false, false},
            { true, true, true, true}
        };

        // 0001
        // 1111
        static readonly bool[,] L2_m = {
            { false, false, false, true},
            { true, true, true, true}
        };

        // 100
        // 100
        // 111
        static readonly bool[,] V_m = {
            { true, false, false},
            { true, false, false},
            { true, true, true}
        };

        // 011
        // 010
        // 110
        static readonly bool[,] S1_m = {
            { false, true, true},
            { false, true, false},
            { true, true, false}
        };

        // 110
        // 010
        // 011
        static readonly bool[,] S2_m = {
            { true, true, false},
            { false, true, false},
            { false, true, true}
        };

        // 011
        // 110
        // 010
        static readonly bool[,] R1_m = {
            { false, true, true},
            { true, true, false},
            { false, true, false}
        };

        // 110
        // 011
        // 010
        static readonly bool[,] R2_m = {
            { true, true, false},
            { false, true, true},
            { false, true, false}
        };

        // 1100
        // 0111
        static readonly bool[,] N1_m = {
            { true, true, false, false},
            { false, true, true, true}
        };

        // 0011
        // 1110
        static readonly bool[,] N2_m = {
            { false, false, true, true },
            { true, true, true, false}
        };

        // 0100
        // 1111
        static readonly bool[,] F1_m = {
            { false, true, false, false},
            { true, true, true, true}
        };

        // 0010
        // 1111
        static readonly bool[,] F2_m = {
            { false, false,true, false },
            { true, true, true, true}
        };

        // 110
        // 111
        static readonly bool[,] P1_m = {
            { true, true, false },
            { true, true, true }
        };

        // 111
        // 110
        static readonly bool[,] P2_m = {
            { true, true, true },
            { true, true, false }
        };

        // 010
        // 111
        // 010
        static readonly bool[,] X_m = {
            { false, true, false },
            { true, true, true },
            { false, true, false }
        };

        // 001
        // 011
        // 110
        static readonly bool[,] W_m = {
            { false, false, true },
            { false, true, true },
            { true, true, false }
        };

        // 101
        // 111
        static readonly bool[,] U_m = {
            { true, false, true },
            { true, true, true }
        };

        // 010
        // 010
        // 111
        static readonly bool[,] T_m = {
            { false, true,  false},
            { false, true,  false},
            { true, true, true }
        };

        public static readonly Dictionary<int, bool[,]> Blocks = new Dictionary<int, bool[,]>()
        {
            {1, I_m},
            {2, R1_m},
            {3, R2_m},
            {4, L1_m},
            {5, L2_m},
            {6, P2_m},
            {7, P1_m},
            {8, N1_m},
            {9, N2_m},
            {10, T_m},
            {11, U_m},
            {12, V_m},
            {13, W_m},
            {14, X_m},
            {15, F2_m},
            {16, F1_m},
            {17, S1_m},
            {18, S2_m}
        };
    }
}
