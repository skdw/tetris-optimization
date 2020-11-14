using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TetrisOptimization
{
    public static class CuttingRectangle
    {
        public static Board baseBoard;
        public static Board changedBoard;

        public static int Cutting(Board board, (int y, int x) rectangle, (int y0, int y1) y, (int x0, int x1) x)
        {
            baseBoard = board;

            int xDif = x.x1 - x.x0;
            int yDif = y.y1 - y.y0;
            int minimalcutting = int.MaxValue;
            //przesuwanie ramki prostokątnej nad klockami
            for (int xAx = 0; xAx <= xDif - rectangle.x; xAx++)
            {
                for (int yAx = 0; yAx <= yDif - rectangle.y; yAx++)
                {
                    changedBoard = new Board(board);
                    (int y0, int y1, int x1, int x2) frame = (yAx + y.y0, yAx + rectangle.y, xAx + x.x0, xAx + rectangle.x);
                    int achivedCut = CountCuttingLine(frame, y, x);
                    if (achivedCut < minimalcutting)
                        minimalcutting = achivedCut;
                }
            }
            return minimalcutting;
        }
        private static int CountCuttingLine((int y0, int y1, int x0, int x1) frame, (int y0, int y1) y, (int x0, int x1) x)
        {
            List<Gap> gaps = FindingGaps(frame);
            //int l = LengthCut(board, frame, x, y, gaps);
            //trying to fill gaps maby ++ way
            return 0;
        }
        public static List<Gap> FindingGaps((int y0, int y1, int x0, int x1) frame)
        {

            List<Gap> gaps = new List<Gap>();
            for (int x = frame.x0; x < frame.x1; x++)
            {
                for (int y = frame.y0; y < frame.y1; y++)
                {
                    if (!changedBoard[y, x].HasValue)
                    {
                        int[,] matrix = new int[1, 1];
                        matrix[0, 0] = 1;
                        List<(int y, int x)> fieldlist = new List<(int y, int x)>();
                        Gap gap = new Gap((1, 1), (y, x), fieldlist);
                        Gap gapTmp = Req(frame, (y, x), gap);
                        gapTmp.matrix = prepareMatrix(gapTmp.size, gapTmp.position, gapTmp.fields);
                        gaps.Add(gapTmp);

                    }
                }
            }
            foreach (var g in gaps)
            {

                for (int i = 0; i < g.matrix.GetLength(0); ++i)
                {
                    Console.Write("|");
                    for (int j = 0; j < g.matrix.GetLength(1); ++j)
                    {
                        if (g.matrix[i, j] == 0)
                            Console.BackgroundColor = ConsoleColor.Red;
                        else
                            Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("  ");
                        Console.ResetColor();
                    }
                    Console.Write("|\n");
                }
                Console.Write("\n");
            }
            return gaps;
        }
        private static Gap Req((int y0, int y1, int x0, int x1) frame, (int y, int x) position, Gap gap)
        {
            int color = 12;
            changedBoard[position.y, position.x] = color;
            gap.fields.Add((position.y, position.x));
            if (position.x - 1 < frame.x0 || changedBoard[position.y, position.x - 1].HasValue)
            {
                if (position.x + 1 >= frame.x1 || changedBoard[position.y, position.x + 1].HasValue)
                {
                    if (position.y - 1 < frame.y0 || changedBoard[position.y - 1, position.x].HasValue)
                    {
                        if (position.y + 1 >= frame.y1 || changedBoard[position.y + 1, position.x].HasValue)
                        {
                            return gap;
                        }
                    }
                }
            }
            Gap gapTmp = new Gap(gap);
            if (position.x - 1 >= frame.x0 && !changedBoard[position.y, position.x - 1].HasValue)
            {
                if (position.x - 1 < gapTmp.position.x)
                {
                    (int y, int x) newSize = (gapTmp.size.y, gapTmp.size.x + 1);
                    (int y, int x) newPosition = (gapTmp.position.y, gapTmp.position.x - 1);
                    Gap newGap = new Gap(newSize, newPosition, gapTmp.fields);
                    gapTmp = Req(frame, (position.y, position.x - 1), newGap);
                }
                else
                {

                    gapTmp = Req(frame, (position.y, position.x - 1), gapTmp);
                }

            }
            if (position.x + 1 < frame.x1 && !changedBoard[position.y, position.x + 1].HasValue)
            {
                if (position.x + 1 >= gapTmp.position.x + gapTmp.size.x)
                {
                    (int y, int x) newSize = (gapTmp.size.y, gapTmp.size.x + 1);
                    (int y, int x) newPosition = gapTmp.position;
                    Gap newGap = new Gap(newSize, newPosition, gapTmp.fields);
                    gapTmp = Req(frame, (position.y, position.x + 1), newGap);
                }
                else
                {
                    gapTmp = Req(frame, (position.y, position.x + 1), gapTmp);
                }


            }
            if (position.y - 1 >= frame.y0 && !changedBoard[position.y - 1, position.x].HasValue)
            {
                if (position.y - 1 < gapTmp.position.y)
                {
                    (int y, int x) newSize = (gapTmp.size.y + 1, gapTmp.size.x);
                    (int y, int x) newPosition = (gapTmp.position.y - 1, gapTmp.position.x);
                    Gap newGap = new Gap(newSize, newPosition, gapTmp.fields);
                    gapTmp = Req(frame, (position.y - 1, position.x), newGap);
                }
                else
                {
                    gapTmp = Req(frame, (position.y - 1, position.x), gapTmp);
                }

            }
            if (position.y + 1 < frame.y1 && !changedBoard[position.y + 1, position.x].HasValue)
            {
                if (position.y + 1 >= gapTmp.position.y + gapTmp.size.y)
                {
                    (int y, int x) newSize = (gapTmp.size.y + 1, gapTmp.size.x);
                    (int y, int x) newPosition = gapTmp.position;
                    Gap newGap = new Gap(newSize, newPosition, gapTmp.fields);
                    gapTmp = Req(frame, (position.y + 1, position.x), newGap);
                }
                else
                {
                    gapTmp = Req(frame, (position.y + 1, position.x), gapTmp);
                }
            }
            return gapTmp;
        }
        public static int[,] prepareMatrix((int y, int x) size, (int y, int x) position, List<(int y, int x)> fileds)
        {
            int[,] matrix = new int[size.y, size.x];
            foreach (var p in fileds)
            {
                matrix[p.y - position.y, p.x - position.x] = 1;
            }

            return matrix;
        }

    }
}
