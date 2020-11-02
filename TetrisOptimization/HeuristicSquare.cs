using System;
using System.Collections.Generic;
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
        double procentage;
        int min_square_size;
        int minimal_achived_size;
        Board bestBoard;
        List<Block> best_permutation;
        List<(int, int)> bestEvolutionList;
        List<(int num, Block b)> blocks;
        int maxSquareSize=0;
        int x1;
        int x2;
        int y2 = 0;

        PermutationHeuristic permutation;
        List<List<(int, int)>> EvolutionList = new List<List<(int, int)>>();
        public HeuristicSquare(List<(int,Block)> blocks, int numLists, double procentage, int numPermutation)
        {
            this.blocks = blocks;
            this.numLists = numLists;
            this.procentage = procentage;
            this.permutation=new PermutationHeuristic(numPermutation, blocks);
            this.numBlocks = this.permutation.numBlock;
            //Console.WriteLine(blocks);
            //Console.WriteLine(numLists);
            //Console.WriteLine(numBlocks);
            foreach(var t in blocks)
            {
                int m = Math.Max(t.Item2.size.x, t.Item2.size.y);
                this.maxSquareSize += m;
            }
            //Console.WriteLine(maxSquareSize);
            generate_list();            
        }       
        

        public void generate_list()
        {            
            Random r = new Random();
            for(int i=0; i< numLists;i++)
            {
                List<(int, int)> tmp_list = new List<(int, int)>();
                for(int j=0;j< numBlocks; j++)
                {
                    tmp_list.Add((r.Next(0, maxSquareSize), r.Next(0, 3)));

                }                
                 EvolutionList.Add(tmp_list);
            }
            //foreach (var l in EvolutionList)
            //{
            //    Console.WriteLine();
            //    foreach (var t in l)
            //    {
            //        Console.Write("(" + t.Item1 + " + " + t.Item2 + ")");
            //    }
            //}
                
        }
        public int tetris(List<(int,int)> arrange , List<Block> blocks)
        {
            return 0;
        }
        public void algorithm()
        {
            foreach (var perm in permutation.permutrationBlock)
            {
                foreach (var list in EvolutionList)
                {
                    int size = tetris(list, perm);
                    if (size == min_square_size)
                        return;

                }
            }
        }


    }
}
