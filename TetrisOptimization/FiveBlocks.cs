﻿using System;
using System.Collections.Generic;

namespace TetrisOptimization
{
    public class FiveBlocks
    {
        static readonly bool[,] I_m = {
            { true, true, true, true , true}
        };
        static Block I = new Block(I_m, (5, 1));

        // 1000
        // 1111
        static readonly bool[,] L1_m = {
            { true, false, false, false},
            { true, true, true, true}
        };
        static Block L1 = new Block(L1_m, (4, 2));

        // 0001
        // 1111
        static readonly bool[,] L2_m = {
            { false, false, false, true},
            { true, true, true, true}
        };
        static Block L2 = new Block(L2_m, (4, 2));

        // 100
        // 100
        // 111
        static readonly bool[,] V_m = {
            { true, false, false},
            { true, false, false},
            { true, true, true}
        };
        static Block V = new Block(V_m, (3, 3));

        // 011
        // 010
        // 110
        static readonly bool[,] S1_m = {
            { false, true, true},
            { false, true, false},
            { true, true, false}
        };
        static Block S1 = new Block(S1_m, (3, 3));

        // 110
        // 010
        // 011
        static readonly bool[,] S2_m = {
            { true, true, false},
            { false, true, false},
            { false, true, true}
        };
        static Block S2 = new Block(S2_m, (3, 3));

        // 011
        // 110
        // 010
        static readonly bool[,] R1_m = {
            { false, true, true},
            { true, true, false},
            { false, true, false}
        };
        static Block R1 = new Block(R1_m, (3, 3));

        // 110
        // 011
        // 010
        static readonly bool[,] R2_m = {
            { true, true, false},
            { false, true, true},
            { false, true, false}
        };
        static Block R2 = new Block(R2_m, (3, 3));

        // 1100
        // 0111
        static readonly bool[,] N1_m = {
            { true, true, false, false},
            { false, true, true, true}
        };
        static Block N1 = new Block(N1_m, (4, 2));

        // 0011
        // 1110
        static readonly bool[,] N2_m = {
            { false, false,true, true },
            { true, true, true, false}
        };
        static Block N2 = new Block(N2_m, (4, 2));

        // 0100
        // 1111
        static readonly bool[,] F1_m = {
            { false, true, false, false},
            { true, true, true, true}
        };
        static Block F1 = new Block(F1_m, (4, 2));

        // 0010
        // 1111
        static readonly bool[,] F2_m = {
            { false, false,true, false },
            { true, true, true, true}
        };
        static Block F2 = new Block(F2_m, (4, 2));

        // 110
        // 111
        static readonly bool[,] P1_m = {
            { true, true, false },
            { true, true, true }
        };
        static Block P1 = new Block(P1_m, (3, 2));

        // 111
        // 110
        static readonly bool[,] P2_m = {
            { true, true, true },
            { true, true, false }
        };
        static Block P2 = new Block(P2_m, (3, 2));

        // 010
        // 111
        // 010
        static readonly bool[,] X_m = {
            { false, true, false },
            { true, true, true },
            { false, true, false }
        };
        static Block X = new Block(X_m, (3, 3));

        // 001
        // 011
        // 110
        static readonly bool[,] W_m = {
            { false, false, true },
            { false, true, true },
            { true, true, false }
        };
        static Block W = new Block(W_m, (3, 3));

        // 101
        // 111
        static readonly bool[,] U_m = {
            { true, false, true },
            { true, true, true }
        };
        static Block U = new Block(U_m, (3, 2));

        // 010
        // 010
        // 111
        static readonly bool[,] T_m = {
            { false, true,  false},
            { false, true,  false},
            { true, true, true }
        };
        static Block T = new Block(T_m, (3, 3));

        static readonly Dictionary<int, Block> blocks = new Dictionary<int, Block>()
        {
            {1, I},
            {2, R1},
            {3, R2},
            {4, L1},
            {5, L2},
            {6, P2},
            {7, P1},
            {8, N1},
            {9, N2},
            {10, T},
            {11, U},
            {12, V},
            {13, W},
            {14, X},
            {15, F2},
            {16, F1},
            {17, S1},
            {18, S2}
        };

        public static Block GetBlock(int i) => blocks[i + 1];
        static readonly Random random = new Random();

        static int GetBlockId() => random.Next(blocks.Count);

        public static Block GetRandomBlock() => blocks[GetBlockId() + 1];

        static int lastColor;
        public static ConsoleColor GetNextColor() => (ConsoleColor)((lastColor += 2) % 15 + 1);
        public static ConsoleColor GetRandomColor() => (ConsoleColor)(random.Next(14) + 1);
    }
}
