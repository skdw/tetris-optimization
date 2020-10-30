using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisOptimization
{
    public class Block
    {
        public bool[,] matrix{get;}
        public (int, int) size { get; }

        public Block(bool[,] matrix, (int, int) size)
        {
            this.matrix = matrix;
            this.size = size;
        }
    }
    public class FiveBlocks
    {
        
        static bool[,] I_m = {
            { true, true, true, true , true}
        };
        Block I = new Block(I_m, (1, 5));

        // 1000
        // 1111
        static readonly bool[,] L1_m = {
            { true, false, false, false},
            { true, true, true, true}
        };
        Block L1 = new Block(L1_m, (2, 4));

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
        Block V = new Block(V_m, (3, 3));

        // 011
        // 010
        // 110
        static readonly bool[,] S1_m = {
            { false, true, true},
            { false, true, false},
            { true, true, false}
        };
        Block S1 = new Block(S1_m, (3, 3));
        // 110
        // 010
        // 011
        static readonly bool[,] S2_m = {
            { true, true, false},
            { false, true, false},
            { false, true, true}
        };
        Block S2 = new Block(S2_m, (3, 3));
        // 011
        // 110
        // 010
        static readonly bool[,] R1_m = {
            { false, true, true},
            { true, true, false},
            { false, true, false}
        };
        Block R1 = new Block(R1_m, (3, 3));
        // 110
        // 011
        // 010
        static readonly bool[,] R2_m = {
            { true, true, false},
            { false, true, true},
            { false, true, false}
        };
        Block R2 = new Block(R2_m, (3, 3));
        // 1100
        // 0111
        static readonly bool[,] N1_m = {
            { true, true, false, false},
            { false, true, true, true}
        };
        Block N1 = new Block(N1_m, (2, 4));
        // 0011
        // 1110
        static readonly bool[,] N2_m = {
            { false, false,true, true },
            { true, true, true, false}
        };
        Block N2 = new Block(N2_m, (2, 4));
        // 0100
        // 1111
        static readonly bool[,] F1_m = {
            { false, true, false, false},
            { true, true, true, true}
        };
        Block F1 = new Block(F1_m, (2, 4));
        // 0010
        // 1111
        static readonly bool[,] F2_m = {
            { false, false,true, false },
            { true, true, true, true}
        };
        Block F2 = new Block(F2_m, (2, 4));
        // 110
        // 111
        static readonly bool[,] P1_m = {
            { true, true, false },
            { true, true, true }
        };
        Block P1 = new Block(P1_m, (2, 3));
        // 111
        // 110
        static readonly bool[,] P2_m = {
            { true, true, true },
            { true, true, false }
        };
        Block P2 = new Block(P2_m, (2, 3));
        // 010
        // 111
        // 010
        static readonly bool[,] X_m= {
            { false, true, false },
            { true, true, true },
            { false, true, false }
        };
        Block X = new Block(X_m, (3, 3));
        // 001
        // 011
        // 110
        static readonly bool[,] W_m = {
            { false, false, true },
            { false, true, true },
            { true, true, false }
        };
        Block W = new Block(W_m, (3, 3));
        // 101
        // 111
        static readonly bool[,] U_m= {
            { true, false, true },
            { true, true, true }
        };
        Block U = new Block(U_m, (2, 3));
        // 010
        // 010
        // 111
        static readonly bool[,] T_m = {
            { false, true,  false},
            { false, true,  false},
            { true, true, true }
        };
        Block T = new Block(T_m, (3, 3));
    }
}
