﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Text;
using TetrisOptimization.Blocks;



namespace TetrisOptimization
{
    class TetrisList
    {
        public List<(int, int)> tetris_list { get; }

        public TetrisList(List<(int, int)> tetris_list)
        {
            this.tetris_list = tetris_list;
        }

        public int achviedSize { get; set; }

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
        List<(int num, Block b)> blocks;
        int maxSquareSize=0;
        Random r = new Random();
        PermutationHeuristic permutation;
        List<TetrisList> EvolutionList = new List<TetrisList>();
        /// <summary>
        /// Heuristic square algorithm class
        /// </summary>
        /// <param name="blocks"> Blocks list</param>
        /// <param name="blockSize">Size of  block</param>
        /// <param name="numLists">Number of list</param>
        /// <param name="procentage">Procentage of block which remain</param>
        /// <param name="numPermutation">Number of permutations of block</param>
        public HeuristicSquare(List<(int,Block)> blocks, int blockSize, int numLists, double procentage, int numPermutation)
        {
            this.blocks = blocks;
            this.numLists = numLists;
            this.procentage = procentage;
            this.permutation=new PermutationHeuristic(numPermutation, blocks);
            this.numBlocks = this.permutation.numBlock;
            Sizes();
            this.minSquareSize = CommonMethods.MinSqareSize(numBlocks, blockSize);
            generate_list();            
        }       
        /// <summary>
        /// Count Board size which is maximal Sqaure size,
        /// Count maximal block size
        /// </summary>
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

        /// <summary>
        /// Generate Evolution List, which is List of Tetris Lists
        /// </summary>
        public void generate_list()
        {            
           
            for(int i=0; i< numLists;i++)
            {
                List<(int x, int rot)> tmp_list = new List<(int, int)>();
                for(int j=0;j< numBlocks; j++)
                {
                    tmp_list.Add((r.Next(0, maxSquareSize-maxBlockSize), r.Next(0, 3)));
                } 
                
                 EvolutionList.Add(new TetrisList(tmp_list));
            }

        }
        /// <summary>
        /// Put blocks one by one on Board correspondingly with arrange 
        /// 
        /// </summary>
        /// <param name="arrange">list of where and with what rotation put the each block</param>
        /// <param name="blocks">list of blocks</param>
        /// <returns></returns>
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
                bool flag = true;
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
            
            if (size < minimalAchivedSize)
            {
                minimalAchivedSize = size;
                bestBoard = board;
            }
            return size;
        }
        
        /// <summary>
        /// The main part of algorithm, takes each permutation, each evolution list 
        /// and cout achived square size by tetris
        /// Reduce and shuffle evolution lists.
        /// </summary>
        /// <returns>Return the best Board</returns>
        public Board algorithm()
        {
            while (EvolutionList.Count > 1)
            {
                foreach (var perm in permutation.permutrationBlock)
                {
                    foreach (var tlist in EvolutionList)
                    {
                        tlist.achviedSize = tetris(tlist.tetris_list, perm);
                        if (tlist.achviedSize == minSquareSize)
                            return bestBoard;
                    }
                }
                EvolutionList.Sort((e1, e2) => e2.achviedSize.CompareTo(e1.achviedSize));
                EvolutionList.RemoveRange(0, (int)Math.Floor(EvolutionList.Count * (1 - procentage)));
                if (EvolutionList.Count > 1)
                {
                    int randomNum = r.Next(1, numBlocks - 1);
                    List<TetrisList> tmp = new List<TetrisList>();
                    for (int i = 0; i < EvolutionList.Count - 1; i++)
                    {
                        
                            List<(int, int)> list1 = new List<(int, int)>();
                            List<(int, int)> list2 = new List<(int, int)>();
                            list1=EvolutionList[i].tetris_list.GetRange(0, randomNum);
                            list2= EvolutionList[i+1].tetris_list.GetRange(0, randomNum);
                            list1.AddRange(EvolutionList[i+1].tetris_list.GetRange(randomNum, numBlocks - randomNum));
                            list2.AddRange(EvolutionList[i].tetris_list.GetRange(randomNum, numBlocks - randomNum));
                            tmp.Add(new TetrisList(list1));
                        
                    }
                    EvolutionList = tmp;
                }                
            }
            return bestBoard;
        }
    }
}
