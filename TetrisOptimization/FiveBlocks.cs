using System;
using System.Collections.Generic;

namespace TetrisOptimization
{
    public class Block
    {
        public bool[,] matrix { get; }
        public (int x, int y) size { get; }

        public Block(bool[,] matrix, (int, int) size)
        {
            this.matrix = matrix;
            this.size = size;
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
            return new Block(rot, (size.y, size.x));
        }

        public ConsoleColor?[,] GetColorMatrix(ConsoleColor color)
        {
            ConsoleColor?[,] color_block = new ConsoleColor?[size.y, size.x];
            for (int i = 0; i < size.y; ++i)
                for (int j = 0; j < size.x; ++j)
                    if (matrix[i, j])
                        color_block[i, j] = color;
                    else
                        color_block[i, j] = null;
            return color_block;
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

        static readonly List<Block> blocks = new List<Block> { I, L1, L2, V, S1, S2, R1, R2, F1, F2, X, W, U, T };
        public static Block GetBlock(int i) => blocks[i];
        static readonly Random random = new Random();

        static int GetBlockId() => random.Next(blocks.Count);

        public static Block GetRandomBlock() => blocks[GetBlockId()];

        public static ConsoleColor GetRandomColor() => (ConsoleColor)(random.Next(14) + 1);

        public static ConsoleColor?[,] GetRandomColorBlock() =>
            GetRandomBlock().GetColorMatrix(GetRandomColor());
    }
}
