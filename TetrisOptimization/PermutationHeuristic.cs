using Medallion;
using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisOptimization
{
    class PermutationHeuristic
    {
        int numPermutation;
        int numBlock = 0;
        List<(int, Block)> blocks;
        List<List<Block>> permutrationBlock = new List<List<Block>>();

        public PermutationHeuristic(int numPermutation, List<(int, Block)> blocks)
        {            
            foreach(var t in blocks)
            {
                this.numBlock += t.Item1;
            }
            int fact = Factorial(this.numBlock);
            if (fact < numPermutation)
                this.numPermutation = fact;
            else
                this.numPermutation = numPermutation;
            this.blocks = blocks;
            preparePermutation();    
            
        }
        public void preparePermutation()
        {
            Random r=new Random();
            List<Block> firstList = new List<Block>();
            foreach(var t in blocks)
            {
                for(int i=0;i<t.Item1;i++)
                {
                    firstList.Add(t.Item2);
                }
            }
            permutrationBlock.Add(firstList);
            for(int i=0;i<numPermutation-1;i++)
            {
                List<Block> nextList = new List<Block>();
                nextList.AddRange(firstList);
                nextList.Shuffle(r);
                permutrationBlock.Add(nextList);
            }
        }
        public int Factorial(int x)
        {
            int result = 1;
            for (int i = 1; i <= x; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
