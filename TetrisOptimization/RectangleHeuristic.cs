using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TetrisOptimization.Blocks;

namespace TetrisOptimization
{
    
    public class Heuristic_rectangle
    {
        //lista wejsciowa z obrotami
        List<List<Block>> blocks;
        //xmin, xmax, ymin, ymax, current - aktualna figura na boardzie, temp - ostatnio kladziony blok
        public int[] currentFigure,tempFigure;

        //wymiary, polozenie ramki
        int rectangleX, rectangleY;
        int rectangleWidth,rectangleHeight;

        //aktualny kat zegara
        double currentAngle;
        public Heuristic_rectangle(List<List<Block>> list)
        {
            blocks = list;
            currentFigure =new int[4];
            tempFigure = new int[4];
            rectangleX = 0;
            rectangleY = 0;
            rectangleWidth = 0;
            rectangleHeight = 0;
            currentAngle = 0;
        }
        //algorytm - zwraca true przy powodzeniu, false przy bledzie
        public Board Algorithm()
        {
            //znajdujemy wymiary ramki prostokata
            var tp = GetMainRectangleDim();
            rectangleHeight = tp.Item2;
            rectangleWidth = tp.Item1;

            //bierzemy liste permutacji klockow
            //var list = GetPermutatedList();
            //narazie jedna permutacja wejsciowa, bo nie wiem o co chodzi w permutationHeuristic
            List<List<Block>> list = blocks;

            //tworzymy plaszczyzne planszy - np. 10 * wieksze wymiary niz prostokat, takie ze nie wyjdziemy za nie
            //zakladamy ze poczatkowo ramka prostokata ktora bedziemy ruszac znajduje sie na srodku planszy
            int multiplier = 10;
            int planeX = tp.Item1 * multiplier, planeY = tp.Item2 * multiplier;
            var board = new Board(planeX, planeY);

            //wspolrzedne ramki prostokta
            rectangleX = planeX / 2 - tp.Item1 / 2;
            rectangleY = planeY / 2 - tp.Item2 / 2;

            //inicjalizujemy xmin i ymin na int.max
            currentFigure[0] = int.MaxValue; currentFigure[2] = int.MaxValue;
            tempFigure[0] = int.MaxValue; tempFigure[2] = int.MaxValue;

            var rand = new Random();
            //petla
            bool first = true;
            foreach (List<Block> rots in list)
            {
                //losowe wybranie obrotu
                var rot = rand.Next() % rots.Count;
                var blck = rots[rot];
                
                    if (first)
                    {
                        //poloz klocek na srodku
                        var isAdded = board.TryToAdd( planeX / 2 - blck.matrix.GetLength(0) / 2, planeY / 2 - blck.matrix.GetLength(1) / 2 + 1, blck);
                        //jak blad dodawania - wychodzimy
                        if (!isAdded) return null;
                        //zaktualizuj wymiary figury
                        UpdateTempDim(planeX / 2 - blck.matrix.GetLength(0) / 2, planeY / 2 - blck.matrix.GetLength(1) / 2, blck.matrix.GetLength(0)+ planeX / 2 - blck.matrix.GetLength(0) / 2, blck.matrix.GetLength(1)+ planeY / 2 - blck.matrix.GetLength(1) / 2 + 1);
                        first = !first;
                    }
                    else
                    {
                        //wyliczamy punkt z ktorego zaczniemy spacer
                        (int, int) point = CalculateCoordOnCircle(planeX / 2, planeY / 2, GetCircleRadius(planeX / 2, planeY / 2));
                        //spacer po prostej
                        WalkTheLine(point, blck, board, planeX / 2, planeY / 2);
                        //UpdateTempDim(point.Item1, point.Item2, blck.matrix.GetLength(0) + point.Item1, blck.matrix.GetLength(1) + point.Item2);
                        //zwiekszamy kat
                        IncrementAngle();
                    }
                    //zaktualizuj wymiary figury
                    UpdateCurrentDim();
                //sprawdzenie czy wyszlismy poza prostokat - jesli tak, to przesuwamy ramke ???
                    board.PrintRestricted(currentFigure[2]-1, currentFigure[3]+1,currentFigure[0]-1, currentFigure[1]+1);
            }
            return board;
        }
        public (int,int) GetMainRectangleDim()
        {
            int howManyOnes = 0;
            foreach(var block in blocks)
                for (int i = 0; i < block[0].matrix.GetLength(0); ++i)
                    for (int j = 0; j < block[0].matrix.GetLength(1); ++j)
                        if (block[0].matrix[i, j])
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
            int t = 0;
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

        //funkcja liczaca promien aktualnego okregu wg aktualnych xmax,xmin,... czy *2 wystarczy?
        public int GetCircleRadius(int centerX,int centerY)
        {
            return 2*Math.Max(Math.Max(Math.Abs(centerX - currentFigure[0]), Math.Abs(centerX-currentFigure[1])),Math.Max(centerY-currentFigure[2],centerY-currentFigure[3]));
        }
        //funkcja liczaca punkt okregu, na ktorym stawiamy kolejna figure
        public (int,int) CalculateCoordOnCircle(int centerX,int centerY,int r)
        {
            return ((int)(centerX + r*Math.Cos(currentAngle)), (int)(centerY +r*Math.Sin(currentAngle))) ;
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

        //funkcja przyblizajaca klocek krok po kroku do srodka
        public bool WalkTheLine((int,int) start, Block block, Board board,int centerX, int centerY)
        {
            var scanned = board.ScanBoard(start.Item1, start.Item2, block);
            (int, int) startPrev = start;
            //(int, int) delta = GetDeltaXY(start, centerX, centerY);
            bool first = true;
            //petla poki mozemy przesunac
            while (scanned)
            {
                //jesli zeskanowalismy ze da sie dodac ale sie nie dodalo to konczymy z bledem
                if (!board.TryToAdd(start.Item1, start.Item2, block )) return false;
                //updateujemy 
                UpdateTempDim(start.Item1, start.Item2, start.Item1+block.matrix.GetLength(0), start.Item2+block.matrix.GetLength(1));
                //
                startPrev = start;
                //zwiekszamy iteratory
                start = IncrementIterators8(start);
                //tu remove 
                board.TryToRemove(startPrev.Item1, startPrev.Item2, block);
                //tu skanujemy jedno blizej
                scanned = board.ScanBoard(start.Item1, start.Item2, block);

            }
            board.TryToAdd(startPrev.Item1, startPrev.Item2, block);
            return true;
        }
       
        //ta funkcje trzeba poprawic
        public bool MoveRectangle(int i, int j, int x, int y)
        {
            if (x + i > rectangleWidth + rectangleX) rectangleX++;
            if (x + i < rectangleX) rectangleX--;
            if (y + j > rectangleHeight + rectangleY) rectangleY++;
            if (y + j < rectangleY) rectangleY--;
            return true;
        }

    }
   

}