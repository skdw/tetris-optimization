using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TetrisOptimization
{
    public class Gap
    {
        public ConsoleColor?[,] matrix { get; set; }
        public (int x, int y) size { get; set; }
        public (int x, int y) position { get; set; }
        public Gap(Gap gap)
        {
            matrix = gap.matrix;
            size = gap.size;
            position = gap.position;
        }

        public Gap(ConsoleColor?[,] matrix, (int x, int y) size, (int x, int y) position)
        {
            this.matrix = matrix.Clone() as ConsoleColor?[,];
            this.size = size;
            this.position = position;
        }
    }
    public static class CuttingRectangle
    {
        public static Board baseBoard;
        public static Board changedBoard;

        public static int Cutting(Board board,(int x,int y) rectangle, (int x0, int x1) x, (int y0,int y1) y)
        {
            baseBoard = board;
            changedBoard = new Board(board);
            int xDif = x.x1 - x.x0;
            int yDif = y.y1 - y.y0;
            int minimalcutting = int.MaxValue;
            //przesuwanie ramki prostokątnej nad klockami
            for(int xAx=0;xAx<=xDif-rectangle.x;xAx++)
            {
                for(int yAx=0;yAx<=yDif-rectangle.y;yAx++)
                {
                    (int x0, int x1, int y1, int y2) frame = (xAx + x.x0, xAx + rectangle.x, yAx + y.y0, yAx + rectangle.y);
                    int achivedCut = CountCuttingLine(frame,x,y);
                    if (achivedCut < minimalcutting)
                        minimalcutting = achivedCut;
                }
            }
            return minimalcutting;
        }
        private static int CountCuttingLine((int x0,int x1,int y0,int y1) frame, (int x0, int x1) x, (int y0, int y1) y)
        {
            List<Gap> gaps = FindingGaps(frame);
            //int l = LengthCut(board, frame, x, y, gaps);
            //trying to fill gaps maby ++ way
            return 0;
        }
        private static List<Gap> FindingGaps( (int x0, int x1, int y0, int y1) frame)
        {
            
            List<Gap> gaps = new List<Gap>();
            for( int x=frame.x0;x<frame.x1;x++)
            {
                for(int y=frame.y0;y<frame.y1;y++)
                {
                    if(!changedBoard[y,x].HasValue)
                    {
                        ConsoleColor?[,] matrix = new ConsoleColor?[1, 1];
                        matrix[0, 0] = null;
                        nMatrix = matrix;
                        Gap gap = new Gap(nMatrix, (1, 1), (x, y));
                        
                        gaps.Add(Req(frame, (x, y), gap));     
                        
                    }
                }
            }
           foreach(var g in gaps)
            {

                for (int i = 0; i < g.matrix.GetLength(0); ++i)
                {
                    Console.Write("|");
                    for (int j = 0; j < g.matrix.GetLength(1); ++j)
                    {
                        if (g.matrix[i, j].HasValue)
                            Console.BackgroundColor = g.matrix[i, j].Value;
                        Console.Write("  ");
                        Console.ResetColor();
                    }
                    Console.Write("|\n");
                }
                Console.Write("\n");
            }
            return gaps;
        }
        static Gap ngap;
        static ConsoleColor?[,] nMatrix;
        private static Gap Req( (int x0, int x1, int y0, int y1) frame, (int x, int y) position, Gap gap)
        {
            changedBoard[position.y, position.x] = ConsoleColor.Red;
            if(position.x-1<frame.x0|| changedBoard[position.y,position.x - 1].HasValue)
            {
                if (position.x + 1 >= frame.x1 || changedBoard[position.y, position.x + 1].HasValue)
                {
                    if (position.y - 1 < frame.y0 || changedBoard[position.y-1, position.x].HasValue)
                    { 
                        if (position.y + 1 >= frame.y1 || changedBoard[position.y+1, position.x].HasValue)
                        {
                            return gap;
                        }
                    }
                }
            }
            Gap gapTmp = new Gap(gap);
            if (position.x - 1 >= frame.x0 &&  !changedBoard[position.y, position.x-1].HasValue)
            {
                if (position.x - 1 < gapTmp.position.x)
                {
                    ConsoleColor?[,] newMatrix = new ConsoleColor?[gapTmp.size.y, gapTmp.size.x+1];

                    (int x, int y) newSize = (gapTmp.size.x + 1, gapTmp.size.y);
                    (int x, int y) newPosition = (gapTmp.position.x - 1, gapTmp.position.y);
                    for (int x = newPosition.x; x < newPosition.x + newSize.x; x++)
                    {
                        for (int y = newPosition.y; y < newPosition.y + newSize.y; y++)
                        {
                            if (baseBoard[y, x] != null)
                                newMatrix[ y - newPosition.y, x - newPosition.x] = baseBoard[y, x].Value;
                        }
                    }
                    nMatrix = newMatrix;
                    Gap newGap = new Gap(nMatrix, newSize, newPosition);
                    gapTmp = Req(frame, (position.x-1, position.y), newGap);
                }
                else
                {
                    gapTmp = Req(frame, (position.x - 1, position.y), gapTmp);
                }

            }
            if (position.x + 1 < frame.x1 && !changedBoard[position.y, position.x+1].HasValue)
            {
                
                if (position.x + 1 >= gapTmp.position.x + gapTmp.size.x)
                {
                    ConsoleColor?[,] newMatrix = new ConsoleColor?[gapTmp.size.y, gapTmp.size.x+1];
                    (int x, int y)  newSize = (gapTmp.size.x + 1, gapTmp.size.y);
                    (int x, int y) newPosition = gapTmp.position;
                    for (int x = newPosition.x; x < newPosition.x + newSize.x; x++)
                    {
                        for (int y = newPosition.y; y < newPosition.y + newSize.y; y++)
                        {
                            if (baseBoard[y, x] != null)
                                newMatrix[ y - newPosition.y, x - newPosition.x] = baseBoard[y, x].Value;
                        }
                    }
                    nMatrix = newMatrix;
                    Gap newGap = new Gap(nMatrix, newSize, newPosition);
                    gapTmp = Req( frame, (position.x + 1, position.y), newGap);

                }
                else
                {
                    gapTmp = Req(frame, (position.x + 1, position.y), gapTmp);
                }            
                
                
            }
            if (position.y - 1 >= frame.y0 && !changedBoard[position.y-1, position.x].HasValue)
            {
                if (position.y- 1 < gapTmp.position.y)
                {
                    ConsoleColor?[,] newMatrix = new ConsoleColor?[gapTmp.size.y+1 , gapTmp.size.x];

                    (int x, int y) newSize = (gapTmp.size.x, gapTmp.size.y+1);
                    (int x, int y) newPosition = (gapTmp.position.x, gapTmp.position.y-1);
                    for (int x = newPosition.x; x < newPosition.x + newSize.x; x++)
                    {
                        for (int y = newPosition.y; y < newPosition.y + newSize.y; y++)
                        {
                            if(baseBoard[y,x]!=null)
                                newMatrix[y - newPosition.y, x - newPosition.x] = baseBoard[y, x].Value;
                        }
                    }
                    nMatrix = newMatrix;
                    Gap newGap = new Gap(nMatrix, newSize, newPosition);
                    gapTmp = Req( frame, (position.x, position.y - 1), newGap);
                }
                else
                {
                    gapTmp = Req( frame, (position.x, position.y - 1), gapTmp);
                }

            }
            if (position.y + 1 < frame.y1 && !changedBoard[position.y+1, position.x].HasValue)
            {
                if (position.y + 1 >= gapTmp.position.y + gapTmp.size.y)
                {
                    ConsoleColor?[,] newMatrix = new ConsoleColor?[gapTmp.size.y+1, gapTmp.size.x];

                    (int x, int y) newSize = (gapTmp.size.x, gapTmp.size.y + 1);
                    (int x, int y) newPosition = gapTmp.position;
                    for (int x = newPosition.x; x < newPosition.x + newSize.x; x++)
                    {
                        for (int y = newPosition.y; y < newPosition.y + newSize.y; y++)
                        {
                            if(baseBoard[y, x]!=null)
                                newMatrix[ y - newPosition.y, x - newPosition.x] = baseBoard[y, x].Value;
                        }
                    }
                    nMatrix = newMatrix;
                    Gap newGap = new Gap(nMatrix, newSize, newPosition);
                    gapTmp = Req(frame, (position.x, position.y + 1), newGap);
                }
                else
                {
                    gapTmp = Req( frame, (position.x, position.y + 1), gapTmp);
                }
            }
            return gapTmp;
        }

    }
}
