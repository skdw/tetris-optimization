using System;
using System.Collections.Generic;

namespace TetrisOptimization
{
    public class HeuristicRectangle : BlocksSolver
    {
        //xmin, xmax, ymin, ymax, current - aktualna figura na boardzie, temp - ostatnio kladziony blok
        public int[] currentFigure,tempFigure;

        //wymiary, polozenie ramki
        int rectangleX, rectangleY;
        int rectangleWidth,rectangleHeight;

        //aktualny kat zegara
        double currentAngle;
        public HeuristicRectangle(List<(int, Block)> list, int blockSize) : base(list, blockSize)
        {
            currentFigure = new int[4];
            tempFigure = new int[4];
            rectangleX = 0;
            rectangleY = 0;
            rectangleWidth = 0;
            rectangleHeight = 0;
            currentAngle = 0;
        }

        public override Board Solve()
        {
            var res = Algorithm();
            Console.WriteLine($"Best cuts: {res.Item1}");
            return res.Item2;
        }

        private (int, Board) Algorithm()
        {
            //znajdujemy wymiary ramki prostokata
            var tp = GetMainRectangleDim();
            rectangleHeight = tp.Item2;
            rectangleWidth = tp.Item1;

            //bierzemy liste permutacji klockow
            //var list = GetPermutatedList();
            //narazie jedna permutacja wejsciowa, bo nie wiem o co chodzi w permutationHeuristic
            var ph = new PermutationHeuristic(5, blocks);

            List<List<Block>> list = ph.permutrationBlock;
            //List<List<Block>> list = new List<List<Block>>();
            ////list.Add(blocks);
            //var temp = new List<Block>();
            //foreach (var it in blocks)
            //{
            //    //var temp = new List<Block>();
            //    for(int i=0;i<it.Item1;i++)
            //    {
            //        temp.Add(new Block(it.Item2.matrix));
            //    }
            //    //list.Add(temp);
            //}
            //list.Add(temp);
            //tworzymy plaszczyzne planszy - np. 10 * wieksze wymiary niz prostokat, takie ze nie wyjdziemy za nie
            //zakladamy ze poczatkowo ramka prostokata ktora bedziemy ruszac znajduje sie na srodku planszy
            int multiplier = 10;
            int planeX = tp.Item1 * multiplier, planeY = tp.Item2 * multiplier;
            var board = new Board(planeY, planeX);
            var boardFinal = new Board(planeY, planeX);
            int bestCuts = int.MaxValue;
            

            //inicjalizujemy xmin i ymin na int.max
            currentFigure[0] = int.MaxValue; currentFigure[2] = int.MaxValue;
            tempFigure[0] = int.MaxValue; tempFigure[2] = int.MaxValue;

            var rand = new Random();
            //petla
            bool first = true;
            foreach (List<Block> rots in list)
            {
                //dla kazdej permutacji nowy board
                board = new Board(planeY, planeX);
                foreach (Block blck1 in rots)
                {
                    //losowe wybranie obrotu
                    var rot = rand.Next() % rots.Count;
                    var blck = CommonMethods.GetSpecyficRotation(blck1, rot);

                    if (first) // pierwszy klocek kladziemy na srodku glownego prostokata
                    {
                        //poloz klocek na srodku
                       // var isAdded = board.TryToAdd(planeY / 2 - blck.matrix.GetLength(0) / 2 + 1,planeX / 2 - blck.matrix.GetLength(1) / 2, blck);
                        bool err = board.TryToAdd(planeY / 2, planeX / 2, blck);
                        //jak blad dodawania - wychodzimy
                        if (err) return (-1,boardFinal);
                        //zaktualizuj wymiary figury
                        UpdateTempDim(planeX / 2 - blck.matrix.GetLength(0) / 2, planeY / 2 - blck.matrix.GetLength(1) / 2, blck.matrix.GetLength(0) + planeX / 2 - blck.matrix.GetLength(0) / 2, blck.matrix.GetLength(1) + planeY / 2 - blck.matrix.GetLength(1) / 2 + 1);
                        first = !first;
                    }
                    else
                    {
                        //wyliczamy punkt z ktorego zaczniemy spacer
                        (int, int) point = CalculateCoordOnCircle(planeY / 2, planeX / 2, GetCircleRadius(planeX / 2, planeY / 2),planeX,planeY,blck);
                        //spacer po prostej
                        var walk = WalkTheLine(point, blck, board, planeY / 2, planeX / 2);
                        if (!walk) board.TryToAdd(point.Item2,point.Item1, blck);
                        //UpdateTempDim(point.Item1, point.Item2, blck.matrix.GetLength(0) + point.Item1, blck.matrix.GetLength(1) + point.Item2);
                        //zwiekszamy kat
                        IncrementAngle();
                    }
                    //zaktualizuj wymiary figury
                    UpdateCurrentDim();
                    //board.Print(false, false);
                    //Console.ReadLine();
                }
                first = true;
                var boardTempVert = board;
                var boardTempHoriz = board;
                //cutting
                var bounds = boardTempVert.GetBoundsPublic(true, false);
                //Console.WriteLine("Board initial");
                //boardTempVert.Print(true, false);
                //Console.ReadLine();
                var test = CuttingRectangle.Cutting(boardTempVert, (rectangleWidth, rectangleHeight), (bounds.minY, bounds.maxY), (bounds.minX, bounds.maxX));
                //Console.WriteLine($"test vertical: {test.Item1}");
                //test.Item2.Print(true, false);
                //Console.ReadLine();
                //Console.WriteLine(test.Item1);
                if(test.Item1<bestCuts)
                {
                    bestCuts = test.Item1;
                    boardFinal = test.Item2;
                }
                var test2 = CuttingRectangle.Cutting(boardTempHoriz, (rectangleHeight, rectangleWidth), (bounds.minY, bounds.maxY), (bounds.minX, bounds.maxX));
                //Console.WriteLine($"test horizontal: {test2.Item1}");
                //test2.Item2.Print(true, false);
                //Console.WriteLine(test.Item1);
                if (test2.Item1 < bestCuts)
                {
                    bestCuts = test2.Item1;
                    boardFinal = test2.Item2;
                }
                //Console.ReadLine();
            }
            return (bestCuts,boardFinal);
        }
        public (int,int) GetMainRectangleDim()
        {
            List<Block> firstList = new List<Block>();
            foreach (var t in blocks)
            {
                for (int i = 0; i < t.Item1; i++)
                {
                    firstList.Add(t.Item2);
                }
            }
            int howManyOnes = 0;
            foreach(Block block in firstList)
                for (int i = 0; i < block.matrix.GetLength(0); ++i)
                    for (int j = 0; j < block.matrix.GetLength(1); ++j)
                        if (block.matrix[i, j])
                            howManyOnes++;
            var divs = GetDivisors(howManyOnes);
            if(divs.Count%2==1)
                return (divs[(int)Math.Round((double)divs.Count/2)-1],divs[(int)Math.Round((double)divs.Count/2)-1]);
            else
                 return (divs[divs.Count/2 - 1],divs[divs.Count/2]);
        }
        static List<int> GetDivisors(int n)
        {
            // Vector to store half
            // of the divisors
            var v = new List<int>();
            var v2 = new List<int>();
            for (int i = 1;
                i <= Math.Sqrt(n); i++) {
                if (n % i == 0) {

                    // check if divisors are equal
                    if (n / i == i)
                        v.Add(i);
                    else
                    {
                        v.Add(i);

                        // push the second divisor
                        // in the vector
                        v2.Add(n / i);
                    }
                }
            }
            v2.Reverse();
            v.AddRange(v2);
            // The vector will be
            // printed in reverse
            return v;
        }

        //funckja liczaca permutacje klockow listy 
        public List<List<ConsoleColor?[,]>> GetPermutatedList()
        {
            return new List<List<ConsoleColor?[,]>>();
        }

        // p r o s z e  z w e r y f i k u j c i e   t o 
        //funkcja liczaca promien aktualnego okregu wg aktualnych xmax,xmin,... czy *2 wystarczy?
        public int GetCircleRadius(int centerX,int centerY)
        {
            return 10*Math.Max(Math.Max(Math.Abs(centerX - currentFigure[0]), Math.Abs(centerX-currentFigure[1])),Math.Max(Math.Abs(centerY-currentFigure[2]),Math.Abs(centerY-currentFigure[3])));
        }
        //funkcja liczaca punkt okregu, na ktorym stawiamy kolejna figure
        public (int,int) CalculateCoordOnCircle(int centerX,int centerY,int r,int boardWidth, int boardHeight,Block block)
        {
            (int, int) point = ((int)(centerX + r * Math.Cos(currentAngle)), (int)(centerY + r * Math.Sin(currentAngle)));
            while(point.Item1<0 || point.Item2<0 || point.Item1 + block.matrix.GetLength(0)>boardHeight || point.Item2 + block.matrix.GetLength(1)>boardWidth)
            {
                r--;
                point = ((int)(centerX + r * Math.Cos(currentAngle)), (int)(centerY + r * Math.Sin(currentAngle)));
            }
            return point;
        }

        //funkcja ktora updatuje wymiary xmin, xmax, ... ostatnio polozonego bloku
        public void UpdateTempDim(int x, int y, int xmax, int ymax)
        {
            //if (x < tempFigure[0]) tempFigure[0] = x;
            //if (xmax > tempFigure[1]) tempFigure[1] = xmax;
            //if (y < tempFigure[2]) tempFigure[2] = y;
            //if (ymax > tempFigure[3]) tempFigure[3] = ymax;
            tempFigure[0] = x;
            tempFigure[1] = xmax;
            tempFigure[2] = y;
            tempFigure[3] = ymax;
        }
        //funkcja ktora updateuje wymiary aktualnej figury na podstawie ostatnio polozonego bloczku
        public void UpdateCurrentDim()
        {
            currentFigure[0] = tempFigure[0] < currentFigure[0] ? tempFigure[0] : currentFigure[0];
            currentFigure[1] = tempFigure[1] > currentFigure[1] ? tempFigure[1] : currentFigure[1];
            currentFigure[2] = tempFigure[2] < currentFigure[2] ? tempFigure[2] : currentFigure[2];
            currentFigure[3] = tempFigure[3] > currentFigure[3] ? tempFigure[3] : currentFigure[3];
        }
        
        
        
        
        
        //narazie ustawione na dodawanie co 45* 
        public void IncrementAngle()
        {
            currentAngle -= 2 * Math.PI / 8;
            if (currentAngle ==  - 2 * Math.PI) currentAngle = 0;
        }
        //zwiekszanie iteratorow dla wersji dodawania z 8 stron
        public (int, int) IncrementIterators8((int, int) start)
        {
            switch (currentAngle)
            {
                case 0:
                    return (start.Item1-1, start.Item2);
                case -Math.PI:
                    return (start.Item1+1, start.Item2);
                case -Math.PI / 2:
                    return (start.Item1, start.Item2+1);
                case -3 * Math.PI / 2:
                    return (start.Item1, start.Item2-1);
                case -Math.PI / 4:
                    return (start.Item1-1, start.Item2+1);
                case -3 * Math.PI / 4:
                    return (start.Item1+1, start.Item2+1);
                case -5 * Math.PI / 4:
                    return (start.Item1+1, start.Item2-1);
                case -7 * Math.PI / 4:
                    return (start.Item1-1, start.Item2-1);
            }
            //switch (currentAngle)
            //{
            //    case 0:
            //        return (start.Item1, start.Item2-1);
            //    case Math.PI:
            //        return (start.Item1, start.Item2+1);
            //    case Math.PI / 2:
            //        return (start.Item1+1, start.Item2);
            //    case 3 * Math.PI / 2:
            //        return (start.Item1-1, start.Item2);
            //    case Math.PI / 4:
            //        return (start.Item1+1, start.Item2-1);
            //    case 3 * Math.PI / 4:
            //        return (start.Item1+1, start.Item2+1);
            //    case 5 * Math.PI / 4:
            //        return (start.Item1-1, start.Item2+1);
            //    case 7 * Math.PI / 4:
            //        return (start.Item1-1, start.Item2-1);
            //}
            return start;
        }
           
        public int Distance((int,int) start, (int,int) center)
        {
            return (int)(Math.Sqrt(Math.Pow(start.Item1 - center.Item1, 2) + Math.Pow(start.Item2 - center.Item2, 2)));
        }
        //funkcja przyblizajaca klocek krok po kroku do srodka
        public bool WalkTheLine((int,int) start, Block block, Board board,int centerX, int centerY)
        {
            var scanned = board.ScanBoard(start.Item1, start.Item2, block);
            (int, int) startPrev = start;
            //(int, int) delta = GetDeltaXY(start, centerX, centerY);
            int dist = Distance(start, (centerX, centerY));
            int distPrev = Distance(start, (centerX, centerY));
            int howFarFromCenter = 3;
            //petla poki mozemy przesunac
            while (scanned && howFarFromCenter>=0)
            {
                //jesli zeskanowalismy ze da sie dodac ale sie nie dodalo to konczymy z bledem
                bool err1 = board.TryToAdd(start.Item1, start.Item2, block);
                if (err1) return false;
                //updateujemy 
                UpdateTempDim(start.Item1, start.Item2, start.Item1+block.matrix.GetLength(0), start.Item2+block.matrix.GetLength(1));
                //
                startPrev = start;
                distPrev = dist;
                //zwiekszamy iteratory
                start = IncrementIterators8(start);
                dist = Distance(start, (centerX, centerY));
                if (dist > distPrev) howFarFromCenter--;
                //tu remove 
                board.TryToRemove(startPrev.Item1, startPrev.Item2, block);
                //tu skanujemy jedno blizej
                scanned = board.ScanBoard(start.Item1, start.Item2, block);

            }
            //board.TryToAdd(startPrev.Item1, startPrev.Item2, block);
            bool err = board.TryToAdd(startPrev.Item1, startPrev.Item2, block);
            return !err;
        }
       
        
	
        

    }
   

}