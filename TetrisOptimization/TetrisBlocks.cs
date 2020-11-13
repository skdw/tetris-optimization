using System;
using System.Collections.Generic;

namespace TetrisOptimization
{
    public static class TetrisBlocks
    {
        // Tetris blocks
        // https://www.quora.com/What-are-the-different-blocks-in-Tetris-called-Is-there-a-specific-name-for-each-block

        // 1111
        static readonly bool[,] I = {
            { true, true, true, true }
        };

        // 100
        // 111
        static readonly bool[,] J = {
            { true, false, false },
            { true, true, true }
        };

        // 110
        // 011
        static readonly bool[,] Z = {
            { true, true, false },
            { false, true, true }
        };

        // 010
        // 111
        static readonly bool[,] T = {
            { false, true, false },
            { true, true, true }
        };

        // 011
        // 110
        static readonly bool[,] S = {
            { false, true, true },
            { true, true, false }
        };

        // 11
        // 11
        static readonly bool[,] O = {
            { true, true },
            { true, true }
        };

        // 001
        // 111
        static readonly bool[,] L = {
            { false, false, true },
            { true, true, true }
        };

        public static readonly Dictionary<int, bool[,]> Blocks = new Dictionary<int, bool[,]> 
        { 
            {1, I},
            {2, J},
            {3, Z},
            {4, T},
            {5, S},
            {6, O},
            {7, L} 
        };
    }
}
