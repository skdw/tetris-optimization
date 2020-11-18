using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisOptimization
{
    public class FindingGaps
    {
        /// <summary>
        /// Copy of baseBoard for each frame used to fill the found gaps
        /// </summary>
        public Board changedBoard;
        public Board board;
        public FindingGaps(Board board)
        {
            this.board = board;
            this.changedBoard = new Board(board);
        }
        
        /// <summary>
        /// It finds gaps in frame, which is on loaded baseBoard in class 
        /// </summary>
        /// <param name="frame">it is rectangle size frame</param>
        /// <returns>list of gaps in frame on loaded board</returns>
        public List<Gap> FindGaps((int y0, int y1, int x0, int x1) frame)
        {
            List<Gap> gaps = new List<Gap>();
            for (int x = frame.x0; x <= frame.x1; x++)
            {
                for (int y = frame.y0; y <= frame.y1; y++)
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
            return gaps;
        }

        /// <summary>
        /// When Finding gaps find empty field on Board this function finds every empty field connected to this field
        /// </summary>
        /// <param name="frame">positio of frame on baseBoard</param>
        /// <param name="position">position of empty field</param>
        /// <param name="gap">previous gap to increase</param>
        /// <returns>Whole gap inside the frame</returns>
        private  Gap Req((int y0, int y1, int x0, int x1) frame, (int y, int x) position, Gap gap)
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
        /// <summary>
        /// Prepare matrix for gap for exact board
        /// </summary>
        /// <param name="size">gap size</param>
        /// <param name="position">gap position on Board</param>
        /// <param name="fileds">fields from which gap consists</param>
        /// <returns>gap matrix</returns>
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
