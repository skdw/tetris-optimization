using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using TetrisOptimization.Blocks;



namespace TetrisOptimization
{
    class TetrisList
    {
        List<(int, int)> tetris_list = new List<(int, int)>();
        int min_achvied_size;
    }
    class HeuristicSquare
    {
        int numBlocks;
        int numLists;
        int maxBlockSize=0;
        double procentage;
        int minSquareSize;
        public int minimalAchivedSize=int.MaxValue;
        Board bestBoard;
        List<Block> best_permutation;
        List<(int, int)> bestEvolutionList;
        List<(int num, Block b)> blocks;
        int maxSquareSize=0;

        PermutationHeuristic permutation;
        List<List<(int, int)>> EvolutionList = new List<List<(int, int)>>();
        public HeuristicSquare(List<(int,Block)> blocks, int blockSize, int numLists, double procentage, int numPermutation)
        {
            this.blocks = blocks;
            this.numLists = numLists;
            this.procentage = procentage;
            this.permutation=new PermutationHeuristic(numPermutation, blocks);
            this.numBlocks = this.permutation.numBlock;
            //Console.WriteLine(blocks);
            //Console.WriteLine(numLists);
            //Console.WriteLine(numBlocks);
            Sizes();
            this.minSquareSize = CommonMethods.MinSqareSize(numBlocks, blockSize);
            //Console.WriteLine(maxSquareSize);
            generate_list();            
        }       
        private void Sizes()
        {
            foreach (var t in blocks)
            {
                int m = Math.Max(t.Item2.size.x, t.Item2.size.y);
                if (m > maxBlockSize)
                    maxBlockSize = m;
                maxSquareSize += m*t.Item1;
            }
        }

        public void generate_list()
        {            
            Random r = new Random();
            for(int i=0; i< numLists;i++)
            {
                List<(int x, int rot)> tmp_list = new List<(int, int)>();
                for(int j=0;j< numBlocks; j++)
                {
                    tmp_list.Add((r.Next(0, maxSquareSize-maxBlockSize), r.Next(0, 3)));
                }                
                 EvolutionList.Add(tmp_list);
            }
            foreach (var l in EvolutionList)
            {
                Console.WriteLine();
                foreach (var t in l)
                {
                    Console.Write("(" + t.Item1 + " + " + t.Item2 + ")");
                }
            }

        }
        public int tetris(List<(int,int)> arrange , List<Block> blocks)
        {
            Board board = new Board(maxSquareSize, maxSquareSize);
            int y2 = 0;
            int x1 = arrange[0].Item1;
            int x2 = x1;
            for(int i=0;i<arrange.Count;i++)
            {
                int x = arrange[i].Item1;
                Block b = CommonMethods.GetSpecyficRotation(blocks[i], arrange[i].Item2);
                bool flag = false;
                int y = 0;
                do
                {
                    flag = board.TryToAdd(x, y, b);
                    y++;
                } while (flag);
                if (x1 > x)
                    x1 = x;
                if (x2 < b.size.x + x)
                    x2 = b.size.x + x;
                if (y + b.size.y > y2)
                    y2 = y + b.size.y;
                
            }
            int size = Math.Max(x2 - x1, y2 -1);
            board.Print();
            if (size < minimalAchivedSize)
            {
                minimalAchivedSize = size;
                bestBoard = board;
            }
            return size;
        }
        public Board algorithm()
        {
            foreach (var perm in permutation.permutrationBlock)
            {
               
                foreach (var list in EvolutionList)
                {
                    int size = tetris(list, perm);
                    if (size == minSquareSize)
                        return bestBoard;
                }
            }
            return bestBoard;
        }


    }
}
