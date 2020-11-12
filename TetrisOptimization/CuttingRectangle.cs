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
        /// <summary>
        /// This is copy of baseBoard for each frame, to fill found gaps
        /// </summary>
        public static Board changedBoard;
        /// <summary>
        /// This function check each possible recangle place and chceck cutting length
        ///
        /// </summary>
        /// <param name="board">board with blocks</param>
        /// <param name="rectangle">size of rectangle</param>
        /// <param name="x">where are blocks on board in x ax</param>
        /// <param name="y">where are blocks on board in y ax</param>
        /// <returns></returns>
        public static int Cutting(Board board, (int x, int y) rectangle, (int x0, int x1) x, (int y0, int y1) y)
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
                    (int x0, int x1, int y1, int y2) frame = (xAx + x.x0, xAx + rectangle.x, yAx + y.y0, yAx + rectangle.y);
                    int achivedCut = CountCuttingLine(frame, x, y);
                    if (achivedCut < minimalcutting)
                        minimalcutting = achivedCut;
                }
            }
            return minimalcutting;
        }
        private static int CountCuttingLine((int x0, int x1, int y0, int y1) frame, (int x0, int x1) x, (int y0, int y1) y)
        {
            List<Gap> gaps = FindingGaps(frame);
            //int l = LengthCut(board, frame, x, y, gaps);
            //trying to fill gaps maby ++ way
            return 0;
        }
        /// <summary>
        /// It finds gaps in frame, which is on loaded baseBoard in class 
        /// </summary>
        /// <param name="frame">it is rectangle size frame</param>
        /// <returns>list of gaps in frame on loaded board</returns>
        public static List<Gap> FindingGaps((int x0, int x1, int y0, int y1) frame)
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
                        List<(int x, int y)> fieldlist = new List<(int x, int y)>();
                        Gap gap = new Gap((1, 1), (x, y), fieldlist);
                        Gap gapTmp = Req(frame, (x, y), gap);
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
        /// <summary>
        /// When Finding gaps find empty field on Board this function finds every empty field connected to this field
        /// </summary>
        /// <param name="frame">positio of frame on baseBoard</param>
        /// <param name="position">position of empty field</param>
        /// <param name="gap">previous gap to increase</param>
        /// <returns>Whole gap inside the frame</returns>
        private static Gap Req((int x0, int x1, int y0, int y1) frame, (int x, int y) position, Gap gap)
        {
            int color = 12;
            changedBoard[position.y, position.x] = color;
            gap.fields.Add((position.x, position.y));
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
                    (int x, int y) newSize = (gapTmp.size.x + 1, gapTmp.size.y);
                    (int x, int y) newPosition = (gapTmp.position.x - 1, gapTmp.position.y);
                    Gap newGap = new Gap(newSize, newPosition, gapTmp.fields);
                    gapTmp = Req(frame, (position.x - 1, position.y), newGap);
                }
                else
                {

                    gapTmp = Req(frame, (position.x - 1, position.y), gapTmp);
                }

            }
            if (position.x + 1 < frame.x1 && !changedBoard[position.y, position.x + 1].HasValue)
            {
                if (position.x + 1 >= gapTmp.position.x + gapTmp.size.x)
                {
                    (int x, int y) newSize = (gapTmp.size.x + 1, gapTmp.size.y);
                    (int x, int y) newPosition = gapTmp.position;
                    Gap newGap = new Gap(newSize, newPosition, gapTmp.fields);
                    gapTmp = Req(frame, (position.x + 1, position.y), newGap);
                }
                else
                {
                    gapTmp = Req(frame, (position.x + 1, position.y), gapTmp);
                }


            }
            if (position.y - 1 >= frame.y0 && !changedBoard[position.y - 1, position.x].HasValue)
            {
                if (position.y - 1 < gapTmp.position.y)
                {
                    (int x, int y) newSize = (gapTmp.size.x, gapTmp.size.y + 1);
                    (int x, int y) newPosition = (gapTmp.position.x, gapTmp.position.y - 1);
                    Gap newGap = new Gap(newSize, newPosition, gapTmp.fields);
                    gapTmp = Req(frame, (position.x, position.y - 1), newGap);
                }
                else
                {
                    gapTmp = Req(frame, (position.x, position.y - 1), gapTmp);
                }

            }
            if (position.y + 1 < frame.y1 && !changedBoard[position.y + 1, position.x].HasValue)
            {
                if (position.y + 1 >= gapTmp.position.y + gapTmp.size.y)
                {
                    (int x, int y) newSize = (gapTmp.size.x, gapTmp.size.y + 1);
                    (int x, int y) newPosition = gapTmp.position;
                    Gap newGap = new Gap(newSize, newPosition, gapTmp.fields);
                    gapTmp = Req(frame, (position.x, position.y + 1), newGap);
                }
                else
                {
                    gapTmp = Req(frame, (position.x, position.y + 1), gapTmp);
                }
            }
            return gapTmp;
        }
        public static int[,] prepareMatrix((int x, int y) size, (int x, int y) position, List<(int x, int y)> fileds)
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
