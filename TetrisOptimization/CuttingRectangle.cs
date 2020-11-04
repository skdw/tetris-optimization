using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisOptimization
{
    public class Gap
    {
        bool[,] matrix;
        (int x, int y) size;
        (int x, int y) position;

        public Gap(bool[,] matrix, (int x, int y) size, (int x, int y) position)
        {
            this.matrix = matrix;
            this.size = size;
            this.position = position;
        }
    }
    public static class CuttingRectangle
    {
        public static int Cutting(Board board,(int x,int y) rectangle, (int x0, int x1) x, (int y0,int y1) y)
        {
            int xDif = x.x1 - x.x0;
            int yDif = y.y0 - y.y1;
            int minimalcutting = int.MaxValue;
            //przesuwanie ramki prostokątnej nad klockami
            for(int xAx=0;xAx< xDif-rectangle.x;xAx++)
            {
                for(int yAx=0;yAx<yDif-rectangle.y;yAx++)
                {
                    (int x0, int x1, int y1, int y2) frame = (xAx + x.x0, xAx + rectangle.x, yAx + y.y0, yAx + rectangle.y);
                    int achivedCut = CountCuttingLine(board,frame,x,y);
                    if (achivedCut < minimalcutting)
                        minimalcutting = achivedCut;
                }
            }
            return minimalcutting;
        }
        private static int CountCuttingLine(Board board,(int x0,int x1,int y0,int y1) frame, (int x0, int x1) x, (int y0, int y1) y)
        {
            List<Gap> gaps = FindingGaps(board, frame);
            //trying to fill gaps maby heurictic way
            return 0;
        }
        private static List<Gap> FindingGaps(Board board, (int x0, int x1, int y0, int y1) frame)
        {
            Board boardTmp = new Board(board);
            List<Gap> gaps = new List<Gap>();
            for( int x=frame.x0;x<frame.x1;x++)
            {
                for(int y=frame.y0;y<frame.y1;y++)
                {
                    if(!board.B[x,y].HasValue)
                    {
                        //rekurencyjne wyszukiwanie 4rek pustych
                    }
                }
            }
            return gaps;
        }

    }
}
