using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TetrisOptimization.Blocks
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

        static readonly List<bool[,]> blocks = new List<bool[,]> { I, J, Z, T, S, O, L };

        static readonly Random random = new Random();

        static int GetBlockId() => random.Next(blocks.Count);

        public static bool[,] GetBlock() => blocks[GetBlockId()];
    }
}
