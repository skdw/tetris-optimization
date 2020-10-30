using System;
using System.Collections.Generic;
using System.Text;
using TetrisOptimization.Blocks;

namespace TetrisOptimization
{
    class TetrisList
    {
        List<(int, int)> tetris_list = new List<(int, int)>();
        int min_achvied_size;
    }
    class Heuristic_square
    {
        int num_blocks;
        int num_lists;
        double procentage;
        int min_square_size;
        int minimal_achived_size;


        List<TetrisList> tetris_list = new List<TetrisList>();


        public Heuristic_square(int num_blocks, int num_lists, double procentage)
        {
            this.num_blocks = num_blocks;
            this.num_lists = num_lists;
            this.procentage = procentage;
        }


        public void generate_list( int max_square_size)
        {
            tetris_list.Clear();
            Random r = new Random();
            for(int i=0; i< num_lists;i++)
            {
                List<(int, int)> tmp_list = new List<(int, int)>();
                for(int j=0;j< num_blocks; j++)
                {
                    tmp_list.Add((r.Next(0, max_square_size), r.Next(0, 3)));
                }
                 tetris_list.Add(tmp_list);
            }
            
        }
        public int tetris(List<(int,int)> arrange , List<TetrisBlocks> tetrisBlocks)
        {
            
            return 0;
        }
        public void algorithm()
        {
            
            while(num_lists!=1|| minimal_achived_size!=min_square_size)
            {
                foreach( var l in tetris_list)
                {

                }
            }
        }


    }
}
