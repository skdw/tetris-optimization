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
        //lista wejsciowa przed obrotami
        List<bool[,]> blocks;
        //xmin, xmax, ymin, ymax, current - aktualna figura na boardzie, temp - ostatnio kladziony blok
        int[] currentFigure,tempFigure;

        //wymiary, polozenie ramki
        int rectangleX, rectangleY;
        int rectangleWidth,rectangleHeight;

        //aktualny kat zegara
        double currentAngle;
        public Heuristic_rectangle(List<bool[,]> list)
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
        public bool Algorithm()
        {
            //znajdujemy wymiary ramki prostokata
            var tp = GetMainRectangleDim();
            rectangleHeight = tp.Item2;
            rectangleWidth = tp.Item1;
            //bierzemy liste permutacji klockow
            var list = GetPermutatedList();

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

            //petla
            bool first = true;
            foreach (var rots in list)
            {
                foreach (var blck in rots)
                {
                    if (first)
                    {
                        //poloz klocek na srodku
                        var isAdded = AddToBoard(board, blck, planeX / 2 - blck.GetLength(0) / 2, planeY / 2 - blck.GetLength(1) / 2);
                        //jak blad dodawania - wychodzimy
                        if (!isAdded) return false;
                        //zaktualizuj wymiary figury
                        UpdateCurrentDim();
                        first = !first;
                    }
                    else
                    {
                        //wyliczamy punkt z ktorego zaczniemy spacer
                        (int, int) point = CalculateCoordOnCircle(planeX / 2, planeY / 2, GetCircleRadius(planeX / 2, planeY / 2));
                        //spacer po prostej
                        WalkTheLine(point, blck, board, planeX / 2, planeY / 2);
                        //zwiekszamy kat
                        IncrementAngle();
                    }
                    //zaktualizuj wymiary figury
                    UpdateCurrentDim();
                    //sprawdzenie czy wyszlismy poza prostokat - jesli tak, to przesuwamy ramke ???
                }
            }
            return true;
        }
        public (int,int) GetMainRectangleDim()
        {
            int howManyOnes = 0;
            foreach(var block in blocks)
                for (int i = 0; i < block.GetLength(0); ++i)
                    for (int j = 0; j < block.GetLength(1); ++j)
                        if (block[i, j])
                            howManyOnes++;
            int[] divs = GetDivisors(howManyOnes);
            if(divs.Length%2==1)
                return (divs[(int)Math.Round((double)divs.Length/2)-1],divs[(int)Math.Round((double)divs.Length/2)-1]);
            else
                 return (divs[divs.Length/2 - 1],divs[divs.Length/2]);
        }
        static int[] GetDivisors(int n)
        {
            // Vector to store half
            // of the divisors
            int[] v = new int[n];
            int t = 0;
            for (int i = 1;
                i <= Math.Sqrt(n); i++) {
                if (n % i == 0) {
    
                    // check if divisors are equal
                    if (n / i == i)
                        Console.Write(i + " ");
                    else {
                        Console.Write(i + " ");
    
                        // push the second divisor
                        // in the vector
                        v[t++] = n / i;
                    }
                }
            }
    
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
            return 2*Math.Max(Math.Max(Math.Abs(centerX - currentFigure[0]), Math.Abs(centerX-currentFigure[1])),Math.Max(centerY-currentFigure[2],centerY=currentFigure[3]));
        }
        //funkcja liczaca punkt okregu, na ktorym stawiamy kolejna figure
        public (int,int) CalculateCoordOnCircle(int centerX,int centerY,int r)
        {
            return ((int)(centerX + r*Math.Cos(currentAngle)), (int)(centerY +r*Math.Sin(currentAngle))) ;
        }

        //funkcja ktora updatuje wymiary xmin, xmax, ... ostatnio polozonego bloku
        public void UpdateTempDim(int x, int y, int xmax, int ymax)
        {
            if (x < tempFigure[0]) tempFigure[0] = x;
            if (x > tempFigure[1]) tempFigure[1] = xmax;
            if (y < tempFigure[2]) tempFigure[2] = y;
            if (y > tempFigure[3]) tempFigure[3] = ymax;
        }
        //funkcja ktora updateuje wymiary aktualnej figury na podstawie ostatnio polozonego bloczku
        public void UpdateCurrentDim()
        {
            currentFigure[0] = tempFigure[0] < currentFigure[0] ? tempFigure[0] : currentFigure[0];
            currentFigure[1] = tempFigure[1] > currentFigure[1] ? tempFigure[1] : currentFigure[1];
            currentFigure[2] = tempFigure[2] < currentFigure[2] ? tempFigure[2] : currentFigure[2];
            currentFigure[3] = tempFigure[3] > currentFigure[3] ? tempFigure[3] : currentFigure[3];
        }
        //funkcja sprawdzajaca czy mozna dodac bloczek
        public bool ScanBoard(Board bd, ConsoleColor?[,] blck, int x, int y)
        {
            var newp = new List<(int, int)>();
            var board = bd.GetBoard();
            for (int i = 0; i < blck.GetLength(0); ++i)
            {
                for (int j = 0; j < blck.GetLength(1); ++j)
                {
                    if ((x + i >= board.GetLength(0)) || (y + j >= board.GetLength(1)))
                    {    //Console.Error.WriteLine("Out of the board");
                        return false;
                    }
                    else if (board[x + i, y + j].HasValue && blck[i, j].HasValue)
                    {    //Console.Error.WriteLine("Trying to override the block");
                        return false;
                    }
                }
            }
            return true;
        }
        //funkcja dodajaca bloczek 
        public bool AddToBoard(Board bd, ConsoleColor?[,] blck, int x, int y)
        {
            var newp = new List<(int, int)>();
            var board = bd.GetBoard();
            for (int i = 0; i < blck.GetLength(0); ++i)
            {
                for (int j = 0; j < blck.GetLength(1); ++j)
                {
                    if ((x + i >= board.GetLength(0)) || (y + j >= board.GetLength(1)))
                    {    //Console.Error.WriteLine("Out of the board");
                        return false;
                    }
                    else if (board[x + i, y + j].HasValue && blck[i, j].HasValue)
                    {    //Console.Error.WriteLine("Trying to override the block");
                        return false;
                    }
                    else
                    {
                        board[x + i, y + j] = blck[i, j];
                    }
                    //MoveRectangle(i,j,x,y);
                }
            }
            UpdateTempDim(x, y, blck.GetLength(0), blck.GetLength(1));
            return true;
        }
        
        //funkcja usuwaj¹ca bloczek
        public bool RemoveFromBoard(Board bd, ConsoleColor?[,] blck, int x, int y)
        {
            var newp = new List<(int, int)>();
            var board = bd.GetBoard();
            for (int i = 0; i < blck.GetLength(0); ++i)
            {
                for (int j = 0; j < blck.GetLength(1); ++j)
                {
                    if ((x + i >= board.GetLength(0)) || (y + j >= board.GetLength(1)))
                    {    //Console.Error.WriteLine("Out of the board");
                        return false;
                    }
                    else if (board[x + i, y + j].HasValue && blck[i, j].HasValue)
                    {    //Console.Error.WriteLine("Trying to override the block");
                        return false;
                    }
                    else
                    {
                        board[x + i, y + j] = null;
                    }
                }
            }
            return true;
        }
        //narazie ustawione na dodawanie co 45* 
        public void IncrementAngle()
        {
            currentAngle += 2 * Math.PI / 8;
            if (currentAngle == 2 * Math.PI) currentAngle = 0;
        }
        //zwiekszanie iteratorow dla wersji dodawania z 8 stron
        public (int, int) IncrementIterators8((int, int) start)
        {
            switch (currentAngle)
            {
                case 0:
                    return (start.Item1--, start.Item2);
                case Math.PI:
                    return (start.Item1++, start.Item2);
                case Math.PI / 2:
                    return (start.Item1, start.Item2++);
                case 3 * Math.PI / 2:
                    return (start.Item1, start.Item2--);
                case Math.PI / 4:
                    return (start.Item1--, start.Item2++);
                case 3 * Math.PI / 4:
                    return (start.Item1++, start.Item2++);
                case 5 * Math.PI / 4:
                    return (start.Item1++, start.Item2--);
                case 7 * Math.PI / 4:
                    return (start.Item1--, start.Item2--);
            }
            return start;
        }

        //funkcja przyblizajaca klocek krok po kroku do srodka
        public bool WalkTheLine((int,int) start, ConsoleColor?[,] block, Board board,int centerX, int centerY)
        {
            var scanned = ScanBoard(board, block, start.Item1, start.Item2);
            (int, int) startPrev = start;
            //(int, int) delta = GetDeltaXY(start, centerX, centerY);
            bool first = true;
            //petla poki mozemy przesunac
            while (scanned)
            {
                //tu remove przy kolejnych puszczeniach, w pierwszym nie robimy
                if(!first)
                {
                    RemoveFromBoard(board, block, startPrev.Item1, startPrev.Item2);
                }
                else
                {
                    first = !first;
                }
                //jesli zeskanowalismy ze da sie dodac ale sie nie dodalo to konczymy z bledem
                if (!AddToBoard(board, block, start.Item1, start.Item2)) return false;
                startPrev = start;
                //zwiekszamy iteratory
                start = IncrementIterators8(start);

                scanned = ScanBoard(board, block, start.Item1, start.Item2);
            }
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