﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public static (int,Board) Cutting(Board board, (int y, int x) rectangle,  (int y0, int y1) y, (int x0, int x1) x)
        {
            baseBoard = new Board(board);
            changedBoard = new Board(board);
            var cutBoard = new Board(board);
            int xDif = x.x1 - x.x0;
            int yDif = y.y1 - y.y0;
            int minimalcutting = int.MaxValue;
            var boarderX = (xDif - rectangle.x) < 0 ? 0 : xDif - rectangle.x;
            var boarderY = (yDif - rectangle.y) < 0 ? 0 : yDif - rectangle.y;
            //przesuwanie ramki prostokątnej nad klockami
            for (int xAx = 0; xAx <=boarderX; xAx++)
            {
                for (int yAx = 0; yAx <= boarderY; yAx++)
                {
                    changedBoard = new Board(board);
                    var newBoard = new Board(baseBoard);
                    (int y1, int y2,int x0, int x1) frame = (yAx + y.y0, yAx + y.y0 + rectangle.y-1,xAx + x.x0, xAx + x.x0 + rectangle.x-1);
                    var achivedCut = CountCuttingLine(newBoard, frame,y,x);
                    //Console.WriteLine($"Board po zakonczeniu ciec i achieveCut={achivedCut}");
                    //check newBoard if its filled
                    //int error = ErrorCheck(board)
                    //Console.WriteLine($"achieved cut: {achivedCut.Item1}");
                    //newBoard.Print(true, false);
                    //Console.ReadLine();
                    if (achivedCut.Item1 < minimalcutting && achivedCut.Item1 > 0 /*&& error!=-1*/)
                    {
                        cutBoard = achivedCut.Item2;
                        //Console.WriteLine("cutBoard assigned here");
                        //cutBoard.Print(false, false);
                        minimalcutting = achivedCut.Item1;
                    }
                    //Console.WriteLine("cutBoard shouldn't change");
                    //cutBoard.Print(false, false);
                }
            }
            //Console.WriteLine("board");
            //board.Print(false, false);
            //cutBoard.Print(false, false);
            return (minimalcutting, cutBoard);
        }
        public static int ErrorCheck(Board board)
        {
            var bounds = board.GetBoundsPublic(true, false);
            for(int y=0;y<board.B.GetLength(0);y++)
            {
                for(int x=0;x<board.B.GetLength(1);x++)
                {
                    if (!board.B[y, x].HasValue) return -1;
                }
            }
            return 0;
        }
        private static (int,Board) CountCuttingLine(Board board,(int y0,int y1, int x0, int x1) frame, (int y0, int y1) y,(int x0, int x1) x)
        {
            List<Gap> gaps = FindingGaps(frame);
            //Console.WriteLine($"gaps: {gaps.Count}");
            //changedBoard.Print();
            //Console.ReadLine();
            foreach (var gap in gaps)
            {
                gap.matrix = prepareMatrix(gap.size, gap.position, gap.fields);
            }
            //dodac klocki ktore sa na zewnatrz ramki
            //int l = LengthCut(board, frame, x, y, gaps);
            //trying to fill gaps maby ++ way
            return LengthCut(board, frame, y, x, gaps); ;
        }
        //maja
        /// <summary>
        /// Funkcja wycina z boarda wszystkie napotkane bloki i zwraca je w liscie
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static List<Block> GetCutBlock(Board rectangle)
        {
            var bounds = rectangle.GetBoundsPublic(true,false);
            //musimy sprawdzic ile kolorow jest w rectangle
            var colors = new Dictionary<int, bool[,]>();
            for(int y=0;y<rectangle.B.GetLength(0);y++)
            {
                for(int x=0;x<rectangle.B.GetLength(1);x++)
                {
                    if(rectangle.B[y,x].HasValue)
                    {
                        if(!colors.ContainsKey(rectangle.B[y,x].Value))
                        {
                            colors.Add(rectangle.B[y,x].Value, new bool[bounds.maxY - bounds.minY + 1, bounds.maxX - bounds.minX + 1]);
                        }
                    }
                }    
            }
            bool[,] mtrx = new bool[bounds.maxY - bounds.minY + 1, bounds.maxX - bounds.minX + 1];
            for (int i = bounds.minY; i < bounds.maxY + 1; i++)
            {
                for (int j = bounds.minX; j < bounds.maxX + 1; j++)
                {
                    if (rectangle.B.GetLength(0) > i && rectangle.B.GetLength(1) > j &&
                        mtrx.GetLength(0) > i - bounds.minY && mtrx.GetLength(1) > j - bounds.minX &&
                        rectangle.B[i, j].HasValue)
                        //mtrx[i - bounds.minY, j - bounds.minX] = true;
                        colors[rectangle.B[i, j].Value][i - bounds.minY, j - bounds.minX] = true;
                }
            }
            var lst = new List<Block>();
            foreach(var pair in colors)
            {
                lst.Add(new Block(pair.Value, (pair.Value.GetLength(0), pair.Value.GetLength(1))));
            }
            return lst;
        }
        //maja 
        //funkcja sprawdza prostokat 9x5 dla prawej czesci ramki dookola znalezionego wystajacego kwadracika na ramki o wspol x y
        public static List<Block> CheckAreaRight(Board board, int y, int x)
        {
            int magic5 = 5, magic9 = 9,magic0=0;
            var rectangle = new Board(magic9, magic5);
            var color = /*System.ConsoleColor.Red*/12;
            rectangle.B[magic9 - magic5, magic0] = color; //kolor czerwony
            int startX = x;
            int startY = y-(magic9-magic5);
            for (int i = startY; i < startY + magic9; i++)
            {
                for (int j = startX; j < startX + magic5; j++)
                {
                    if (board.B.GetLength(0) > i && board.B.GetLength(1) > j && i >= 0 && j >= 0 && board.B[i, j].HasValue)
                    {
                        board.B[i, j] = null;
                        rectangle.B[i - startY, j - startX] = color;
                    }
                }
            }
            return GetCutBlock(rectangle);
        }
        //maja 
        //funkcja sprawdza prostokat 9x5 dla lewej czesci ramki dookola znalezionego wystajacego kwadracika na ramki o wspol x y
        public static List<Block> CheckAreaLeft(Board board, int y, int x)
        {
            int magic5 = 5, magic9 = 9;
            var rectangle = new Board(magic9, magic5);
            var color = 12;
            rectangle.B[magic9 - magic5, magic9 - magic5] = color;
            int startX = x - (magic9 - magic5);
            int startY = y - (magic9 - magic5);
            for (int i = startY; i < startY + magic9; i++)
            {
                for (int j = startX; j < startX + magic5; j++)
                {
                    if (board.B.GetLength(0) > i && i>=0 && j>=0 && board.B.GetLength(1) > j && board.B[i, j].HasValue)
                    {
                        board.B[i, j] = null;
                        rectangle.B[i - startY, j - startX] = color;
                    }
                }
            }
            return GetCutBlock(rectangle);
        }
        //maja
        //funkcja sprawdza prostokat 9x5 dla dolnej czesci ramki dookola znalezionego wystajacego kwadracika na ramki o wspol x y
        public static List<Block> CheckAreaDown(Board board, int y, int x, int magic5 = 5, int magic9 = 9)
        {
            int magic0=0;
            var rectangle = new Board(magic5, magic9);
            var color = 12;
            rectangle.B[magic0, magic9 - magic5] = color;
            int startX = x - (magic9-magic5);
            int startY = y;
            for (int i = startY; i < startY + magic5; i++)
            {
                for (int j = startX; j < startX + magic9; j++)
                {
                    if (board.B.GetLength(0) > i && board.B.GetLength(1) > j && i >= 0 && j >= 0 && board.B[i, j].HasValue)
                    {
                        board.B[i, j] = null;
                        rectangle.B[i - startY, j - startX] = color;
                    }
                }
            }
            return GetCutBlock(rectangle);
        }
        //maja
        //funkcja sprawdza prostokat 9x5 dla gornej czesci ramki dookola znalezionego wystajacego kwadracika na ramki o wspol x y
        public static List<Block> CheckAreaUp(Board board, int y, int x)
        {
            int magic5 = 5,magic9 = 9;
            var rectangle = new Board(magic5, magic9);
            var color = 12;
            rectangle.B[magic9 - magic5, magic9 - magic5] = color;
            int startX = x - (magic9- magic5);
            int startY = y - (magic9 - magic5);
            for(int i=startY;i<startY+ magic5; i++)
            {
                for(int j=startX;j<startX+ magic9; j++)
                {
                    if (board.B.GetLength(0) > i && board.B.GetLength(1) > j && i >= 0 && j >= 0 && board.B[i, j].HasValue)
                    {
                        board.B[i, j] = null;
                        rectangle.B[i - startY, j - startX] = color;
                    }
                }
            }
            return GetCutBlock(rectangle);
        }
        //maja
        //funkcja rozcina bloki wystajace za ramke, nastepnie, dla tak powstalej listy blokow, 
        //szuka idealnej gap (ExactFit), a pozostale bloki tnie na pojedyncze kwadraty i wypelnia reszte dziur
        //dodac szukanie klockow spoza ramki
        public static (int,Board) LengthCut(Board board, (int y0, int y1, int x0, int x1) frame, (int y0, int y1) y,(int x0,int x1) x, List<Gap> gaps)
        {
            int cuts = 0;
            ConsoleColor? lastColor = ConsoleColor.Red;
            //czym jest x i y?
            List<Block> cutOffBlocks = new List<Block>();
            //przejscie po ramce 
            for(int i=frame.y0;i<=frame.y1;i++)
            {
                //lewa czesc ramki
                
                //sprawdzamy czy mamy taki sam kolor nad i pod ramka 
                if(frame.x0 - 1>=0 &&
                    board.B.GetLength(0)>i && board.B.GetLength(1)>frame.x0 &&
                    board.B[i, frame.x0].HasValue &&
                    board.B[i, frame.x0 - 1].HasValue &&
                    board.B[i,frame.x0].Value == board.B[i, frame.x0-1].Value)
                {
                    //zapisac ostatni kolor i sprawdzac
                    cutOffBlocks.AddRange(CheckAreaLeft(board, i, frame.x0-1));
                    cuts++;
                }

                //prawa czesc ramki
                //sprawdzamy czy mamy taki sam kolor nad i pod ramka 
                if (frame.x1 + 1 >= 0 && board.B.GetLength(0)>i && board.B.GetLength(1)>frame.x1+1 && board.B[i, frame.x1].HasValue &&
                    board.B[i, frame.x1 + 1].HasValue &&
                    board.B[i, frame.x1].Value == board.B[i, frame.x1 + 1].Value)
                {
                    cutOffBlocks.AddRange(CheckAreaRight(board, i, frame.x1+1));
                    cuts++;
                }
            }
            for(int k=frame.x0;k<=frame.x1;k++)
            {
                //gorna czesc ramki
                //sprawdzamy czy mamy taki sam kolor nad i pod ramka 
                if (frame.y0 - 1 >= 0 && board.B.GetLength(0)>frame.y0 && board.B.GetLength(1)>k &&
                    board.B[frame.y0, k].HasValue &&
                    board.B[frame.y0 - 1, k].HasValue &&
                    board.B[frame.y0, k].Value == board.B[frame.y0-1,k].Value)
                {
                    cutOffBlocks.AddRange(CheckAreaUp(board,frame.y0-1, k));
                    cuts++;
                }
                //dolna czesc ramki
                //sprawdzamy czy mamy taki sam kolor nad i pod ramka 
                if (frame.y1 + 1 >= 0 && board.B.GetLength(0)>frame.y1+1 && board.B.GetLength(1)>k && board.B[frame.y1, k].HasValue &&
                    board.B[frame.y1 + 1, k].HasValue &&
                    board.B[frame.y1, k].Value == board.B[frame.y1 + 1, k].Value)
                {
                    cutOffBlocks.AddRange(CheckAreaDown(board, frame.y1+1, k));
                    cuts++;
                }
            }
            //Console.WriteLine("board po obcieciu ramki");
            //board.Print(false,false);
            //Console.ReadLine();
            ////usuwanie z poza ramki
            for(int a = 0;a<board.B.GetLength(0);a++)
            {
                for(int b=0;b<board.B.GetLength(1);b++)
                {
                    //jesli poza ramka i cos jest
                    if(CheckIfOutOfFrame(a, b, frame) && board.B[a,b].HasValue)
                    {
                        cuts=VerifyCutsUnit(cuts, board, a, b);
                        board.B[a, b] = null;
                        cutOffBlocks.Add(new Block(new bool[1, 1] { { true } }));
                    }
                }
            }
            //for(int m=y.y0;m<=y.y1;m+=5)
            //{
            //    for(int n=x.x0;n<=x.x1;n+=5)
            //    {
            //        if(m<board.B.GetLength(0) && n<board.B.GetLength(1) && board.B[m,n].HasValue)
            //        {
            //            cutOffBlocks.AddRange(CheckAreaRight(board, m, n));
            //        }
            //    }
            //}

            ////usuwanie poza ramka
            //int magic5 = 5;
            ////cesc lewa
            //for(int a=0;a<board.B.GetLength(0) - magic5;a++)
            //{
            //    for(int b=0;b<frame.y0;b++)
            //    {
            //        if(board.B[a,b].HasValue)
            //        {
            //            cutOffBlocks.AddRange(CheckAreaDown(board, a, b,magic5,frame.y0-b));
            //        }
            //    }
            //}
            ////czesc prawa
            //for (int a = 0; a < board.B.GetLength(0) - magic5; a++)
            //{
            //    for (int b = frame.y1+1; b < board.B.GetLength(1); b++)
            //    {
            //        if (board.B[a, b].HasValue)
            //        {
            //            cutOffBlocks.AddRange(CheckAreaDown(board, a, b,magic5, board.B.GetLength(1)-b));
            //        }
            //    }
            //}



            //dla listy odcietych blokow, patrzymy czy ktoras dziura pasuje idealnie do bloku
            var remaining = ExactFit(gaps, cutOffBlocks, board);
            //dla pozostalych wypelniamy pojedynczymi kwadratami
            return (UnitCut(remaining, board, cuts),board);
        }
        public static int VerifyCutsUnit(int cuts, Board board, int y, int x)
        {
            bool up = y - 1 >= 0 && board.B[y - 1, x].HasValue;
            bool down = y + 1 < board.B.GetLength(0) && board.B[y + 1, x].HasValue;
            bool left = x - 1 >= 0 && board.B[y, x - 1].HasValue;
            bool right = x + 1 < board.B.GetLength(1) && board.B[y, x + 1].HasValue;
            var list = new List<bool> { up, down, left, right };
            foreach(var b in list)
            {
                if (b) cuts++;
            }
            return cuts;
        }
        public static bool CheckIfOutOfFrame(int a, int b, (int y0,int y1, int x0, int x1) frame)
        {
            bool lewa = b < frame.x0;
            bool prawa = b > frame.x1;
            bool dol = a > frame.y1;
            bool gora = a < frame.y0;
            if (lewa || prawa || gora || dol) return true;
            return false;
        }
        //maja
        public static int HowManyUnitCuts(Block block)
        {
            int cuts = 0;
            HashSet<int> cutY = new HashSet<int>();
            HashSet<int> cutX = new HashSet<int>();
            //pionowe ciecia
            for (int i = 0; i < block.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < block.matrix.GetLength(1)-1; j++)
                {
                    if (block.matrix[i, j] && block.matrix[i , j + 1] && !cutX.Contains(j))
                    {
                        cutX.Add(j);
                        cuts++;
                        //break;
                    }
                }
            }
            //poziome ciecia
            for(int x = 0; x < block.matrix.GetLength(1); x++)
            {
                for (int y = 0; y < block.matrix.GetLength(0)-1; y++)
                {
                    if(block.matrix[y,x] && block.matrix[y + 1, x] && !cutY.Contains(y))
                    {
                        cutY.Add(y);
                        cuts++;
                        //break;
                    }
                }
            }
            return cuts;
        }
        //maja
        /// <summary>
        /// Funkcja tnie pozostale bloki na jednostkowe bloczki i wypelnia po kolei dziury - size blocku na odwrot!
        /// </summary>
        /// <param name="remaining"></param>
        /// <param name="board"></param>
        /// <param name="cuts"></param>
        /// <returns></returns>
        public static int UnitCut((List<Block> blocks, List<Gap> gaps) remaining,Board board, int cuts)
        {
            int howManyUnitBlocks = 0;
            foreach(Block block in remaining.blocks)
            {
                //tu size blockow na odwrot
                for(int i=0; i<block.matrix.GetLength(0);i++)
                {
                    for(int j=0;j<block.matrix.GetLength(1);j++)
                    {
                        if (block.matrix[i, j]) howManyUnitBlocks++;
                    }
                }
                cuts += HowManyUnitCuts(block);
            }

            var unitMatrix = new bool[1,1];
            unitMatrix[0, 0] = true;
            var unitBlock = new Block(unitMatrix, (1, 1));
            var unitBlockList = new List<Block>();
            for(int i=0;i<howManyUnitBlocks;i++)
            {
                unitBlockList.Add(new Block(unitMatrix, (1, 1)));
            }
            foreach(Gap gap in remaining.gaps)
            {
                if (gap.matrix == null)
                {
                    gap.matrix = prepareMatrix(gap.size, gap.position, gap.fields);
                }
                for (int y=gap.position.y;y<gap.matrix.GetLength(0)+ gap.position.y; y++)
                {
                    for(int x= gap.position.x; x< gap.position.x+ gap.matrix.GetLength(1); x++)
                    {
                        if(gap.matrix[y- gap.position.y, x- gap.position.x]==1 && unitBlockList.Count>0) //gap 1 jak jest dziura, 0 jak jest klocek
                        {
                            if (board.TryToAdd(y,x, unitBlockList[0]))
                            {
                                unitBlockList.RemoveAt(0);
                                howManyUnitBlocks--;
                            }
                            else return -1;
                        }
                    }
                }
                //jesli skonczyly sie dziury ale cos sie nie dodalo
                if(gap==remaining.gaps.Last() && howManyUnitBlocks>0)
                {
                    return -1;
                }
            }
            return cuts;
        }
        //maja
        /// <summary>
        /// Sprawdza czy blok idealnie pasuj do dziury - uwaga: size w block jest na odwrot!
        /// </summary>
        /// <param name="rot"></param>
        /// <param name="gap"></param>
        /// <returns></returns>
        public static bool DoesBlockFit(Block rot, Gap gap)
        {
            gap.matrix = prepareMatrix(gap.size, gap.position, gap.fields);
            if (rot.matrix.GetLength(0)!= gap.matrix.GetLength(0) || rot.matrix.GetLength(1) != gap.matrix.GetLength(1)) return false;
            for(int i=0;i<gap.matrix.GetLength(0);i++)
            {
                for(int j=0;j<gap.matrix.GetLength(1);j++)
                {
                    //if ((gap.matrix[i, j].HasValue && rot.matrix[i, j])
                    //    || (!gap.matrix[i, j].HasValue) && !rot.matrix[i, j]) return false;
                    if ((gap.matrix[i, j]==0 && rot.matrix[i, j])//gap 0 jest klocek
                        || (gap.matrix[i, j]==1) && !rot.matrix[i, j]) return false;

                }
            }
            return true;
        }
        //maja
        public static (List<Block>,List<Gap>) ExactFit(List<Gap> gaps,List<Block> blocks, Board board)
        {
            var newBlocks = new List<Block>(blocks);
            var newGaps = new Dictionary<Gap,int>();
            foreach(var gap in gaps)
            {
                newGaps.Add(gap, 0);
            }    
            bool breakFrom = false;
            //dla kazdego bloku
            foreach(Block block in blocks)
            {
                //dla kazdego obrotu
                for(int i=0;i<4;i++)
                {
                    Block rot = CommonMethods.GetSpecyficRotation(block,i);
                    foreach(Gap gap in gaps)
                    {
                        if(newGaps[gap]==0 && DoesBlockFit(rot, gap))
                        {
                            if(board.TryToAdd(gap.position.y, gap.position.x, rot))
                            {
                                newBlocks.Remove(block);
                                newGaps[gap]++;
                                breakFrom = true;
                                break;
                            }
                        }
                    }
                    if(breakFrom)
                    {
                        breakFrom = false;
                        break;
                    }
                }
            }
            var gps = new List<Gap>();
            foreach(var dc in newGaps)
            {
                if (dc.Value == 0) gps.Add(dc.Key);
            }
            return (newBlocks,gps);
        }
        /// <summary>
        /// It finds gaps in frame, which is on loaded baseBoard in class 
        /// </summary>
        /// <param name="frame">it is rectangle size frame</param>
        /// <returns>list of gaps in frame on loaded board</returns>
        public static List<Gap> FindingGaps((int y0, int y1, int x0, int x1) frame)
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
        private static Gap Req((int y0, int y1,int x0, int x1) frame, (int y, int x) position, Gap gap)
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
