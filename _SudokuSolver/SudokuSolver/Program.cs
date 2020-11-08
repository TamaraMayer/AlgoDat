﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {

            //string toSolve = @"5, 3, 0, 0, 7, 0, 0, 0, 0
            //                   6, 0, 0, 1, 9, 5, 0, 0, 0
            //                   0, 9, 8, 0, 0, 0, 0, 6, 0
            //                   8, 0, 0, 0, 6, 0, 0, 0, 3
            //                   4, 0, 0, 8, 0, 3, 0, 0, 1
            //                   7, 0, 0, 0, 2, 0, 0, 0, 6
            //                   0, 6, 0, 0, 0, 0, 2, 8, 0
            //                   0, 0, 0, 4, 1, 9, 0, 0, 5
            //                   0, 0, 0, 0, 8, 0, 0, 7, 9";

            //easy Sudoku
            //string toSolve = @"8, 3, 1, 0, 7, 5, 4, 0, 0
            //                   0, 0, 6, 0, 0, 0, 3, 0, 1
            //                   0, 4, 0, 2, 1, 3, 0, 8, 0
            //                   0, 8, 0, 3, 5, 6, 0, 2, 0
            //                   6, 0, 9, 0, 0, 4, 0, 3, 0
            //                   4, 0, 0, 0, 0, 0, 0, 6, 7
            //                   0, 0, 8, 5, 6, 0, 0, 4, 9
            //                   0, 0, 0, 1, 0, 0, 6, 0, 0
            //                   7, 6, 2, 8, 0, 0, 5, 0, 0";

            ////middle Sudoku
            //string toSolve = @"0, 0, 8, 0, 6, 0, 4, 0, 9
            //                   4, 3, 0, 0, 9, 0, 0, 0, 5
            //                   0, 0, 2, 4, 0, 0, 0, 0, 3
            //                   0, 0, 0, 9, 0, 0, 0, 7, 6
            //                   8, 0, 0, 0, 0, 7, 0, 0, 4
            //                   5, 4, 0, 0, 3, 6, 0, 9, 0
            //                   0, 0, 0, 2, 0, 5, 0, 3, 0
            //                   9, 1, 5, 0, 0, 0, 2, 0, 0
            //                   0, 0, 0, 0, 0, 9, 6, 0, 0";

            //schwer Sudoku
            //string toSolve = @"7, 3, 0, 0, 0, 0, 0, 9, 8
            //                   4, 0, 2, 0, 0, 0, 0, 0, 6
            //                   0, 0, 0, 0, 1, 7, 0, 0, 0
            //                   1, 5, 4, 7, 0, 0, 2, 0, 0
            //                   0, 0, 9, 0, 0, 0, 0, 0, 7
            //                   0, 0, 0, 0, 0, 6, 5, 0, 0
            //                   0, 6, 0, 1, 0, 0, 4, 0, 0
            //                   0, 0, 1, 5, 0, 0, 0, 8, 3
            //                   0, 0, 5, 0, 9, 0, 0, 0, 0";

            //////worlds hardest sudoku
            //string toSolve = @"8,0,0,0,0,0,0,0,0
            //                   0,0,3,6,0,0,0,0,0
            //                   0,7,0,0,9,0,2,0,0
            //                   0,5,0,0,0,7,0,0,0
            //                   0,0,0,0,4,5,7,0,0
            //                   0,0,0,1,0,0,0,3,0
            //                   0,0,1,0,0,0,0,6,8
            //                   0,0,8,5,0,0,0,1,0
            //                   0,9,0,0,0,0,4,0,0";

            ////impossible to solve
            //string toSolve = @"0,0,0,0,0,0,1,0,0
            //                   0,0,0,0,0,0,2,0,0
            //                   0,0,0,0,0,0,0,0,0
            //                   0,0,0,0,0,0,0,1,0
            //                   0,0,0,0,0,0,0,2,0
            //                   0,0,0,0,0,0,0,0,0
            //                   1,2,0,0,0,0,0,0,0
            //                   0,0,0,0,1,2,0,0,0
            //                   0,0,0,0,0,0,0,0,0";

            //impossible to solve in small
            //string toSolve = @"0,0,1,0
            //                   0,0,2,0
            //                   1,2,0,0
            //                   0,0,0,0";

            //impossible to solve cause uneven amount of numbers
            //string toSolve = @"7, 3, 0, 0, 0, 0, 0, 9, 8
            //                   4, 0, 2, 0, 0, 0, 0, 0, 6
            //                   0, 0, 0, 0, 1, 7, 0, 0, 0
            //                   1, 5, 4, 7, 0, 0, 2, 0, 0, 8
            //                   0, 0, 9, 0, 0, 0, 0, 0, 7
            //                   0, 0, 0, 0, 0, 6, 5, 0, 0
            //                   0, 6, 0, 1, 0, 0, 4, 0, 0
            //                   0, 0, 1, 5, 0, 0, 0, 8, 3
            //                   0, 0, 5, 0, 9, 0, 0, 0, 0";

            ////6x6 solvable
         //   string toSolve = "0, 2, 4, 6, 0, 3 \r\n 1, 0, 6, 0, 5, 2 \r\n 2, 6, 0, 5, 3, 4\r\n 3, 4, 5, 0, 6, 1 \r\n 6, 1, 0, 3, 0, 5 \r\n 4, 0, 3, 1, 2, 0";

            //6x6 unsolvable because eine Zahl doppelt in Zeile
            //string toSolve = @"0, 2, 4, 6, 0, 3 
            //                   1, 0, 6, 0, 5, 2 
            //                   2, 6, 0, 5, 3, 4
            //                   3, 4, 5, 0, 6, 1
            //                   6, 1, 3, 3, 0, 5
            //                   4, 0, 3, 1, 2, 0";

            //   16x16 Sudoku
            //string toSolve = @"2,0,8,4,0,0,10,9,6,0,0,0,0,12,7,5
            //                   6,1,0,0,5,0,7,16,14,10,11,0,0,0,0,0
            //                   0,0,0,9,13,0,4,0,5,7,0,16,0,0,0,1
            //                   0,0,0,0,1,0,0,6,0,0,0,0,0,0,9,0
            //                   0,0,0,0,0,0,0,0,15,0,3,1,0,0,0,0
            //                   0,0,6,0,7,0,0,0,0,14,9,11,8,2,13,4
            //                   10,0,14,0,4,0,0,8,7,5,0,12,15,0,1,3
            //                   0,16,5,12,0,0,0,0,4,0,0,8,0,14,0,0
            //                   0,0,0,8,0,14,11,10,0,0,0,15,0,5,12,0
            //                   3,0,0,15,0,0,0,7,9,0,0,0,0,0,0,2
            //                   0,14,11,10,2,0,8,0,0,0,0,0,3,0,0,0
            //                   16,0,0,7,6,0,0,0,2,8,13,4,9,0,0,0
            //                   13,0,0,2,11,0,9,0,1,0,15,0,5,0,0,0
            //                   0,0,3,6,12,7,0,0,0,0,0,14,0,4,2,0
            //                   0,0,0,14,0,0,0,13,12,16,7,0,1,0,0,15
            //                   0,0,0,5,0,3,0,0,0,2,0,13,11,0,14,0";

            //            //25x25
            //            string toSolve = @"13,20,0,21,0,16,0,0,0,7,0,1,0,8,23,0,10,12,0,0,24,17,0,18,15
            //0,19,0,0,6,0,24,17,0,18,4,0,20,0,21,1,22,11,8,23,5,0,12,0,3
            //1,0,0,23,8,0,0,4,0,21,12,0,0,0,3,0,17,9,0,0,19,0,0,7,0
            //5,0,12,3,0,0,0,11,0,23,9,24,0,0,15,0,25,6,0,16,20,0,2,21,13
            //0,0,0,15,0,0,0,12,14,3,6,0,25,0,0,20,0,0,21,13,0,11,8,23,1
            //0,6,7,0,16,17,0,0,0,0,0,4,2,13,20,0,8,0,1,22,0,14,3,5,10
            //0,2,0,0,0,25,6,0,0,19,23,0,0,0,0,12,0,3,0,0,0,0,0,0,0
            //11,0,23,22,0,0,2,21,13,0,0,0,0,5,10,9,18,0,0,17,6,0,0,19,25
            //0,14,3,0,0,11,8,23,1,22,15,0,18,24,17,0,7,16,0,0,0,0,13,0,4
            //9,0,15,17,24,0,14,0,0,10,16,0,7,19,25,0,21,0,0,0,8,23,1,22,0
            //19,25,0,16,0,24,17,9,18,0,2,20,4,0,0,22,0,8,0,0,10,0,14,0,5
            //20,4,2,0,21,0,25,6,0,16,8,0,11,0,1,0,0,0,0,5,0,9,0,15,24
            //22,11,0,0,23,0,0,2,21,0,0,0,0,3,5,0,9,18,15,0,0,6,0,0,0
            //10,0,0,5,3,0,0,0,0,1,0,17,9,0,24,25,0,0,16,19,4,0,21,13,20
            //0,0,18,24,15,0,12,0,0,0,0,25,0,16,0,4,2,0,0,0,11,0,0,1,22
            //0,16,19,0,0,0,15,0,17,0,0,0,13,4,0,23,0,22,11,8,3,0,0,12,0
            //21,0,0,0,0,0,0,0,0,6,22,0,1,0,0,3,5,10,12,0,0,24,17,9,18
            //0,1,22,8,0,0,0,0,4,0,0,3,5,0,14,15,0,0,9,0,0,0,25,6,0
            //0,0,0,14,0,0,1,0,0,8,17,15,0,9,18,16,19,25,6,7,13,20,0,2,0
            //15,24,17,18,9,0,5,0,0,0,25,0,19,0,7,0,0,0,0,21,0,22,11,0,23
            //6,0,0,0,19,9,18,0,0,0,0,2,21,20,0,0,0,0,0,0,14,3,0,10,12
            //2,0,13,0,20,0,7,16,0,25,1,0,0,0,11,14,0,0,0,0,18,15,0,17,0
            //8,0,1,11,0,2,21,13,0,4,5,14,3,10,0,0,15,0,17,9,7,16,0,25,0
            //0,3,0,0,0,8,0,0,0,11,24,0,0,17,9,7,16,19,0,0,21,0,20,0,0
            //18,0,0,9,0,14,0,5,0,0,0,7,16,25,0,21,0,20,4,2,23,1,22,0,0";

            string toSolve = @"0,3,8,0,0,10,7,0,0,6,4,0
4,7,0,11,0,0,0,0,10,0,8,5
0,0,6,9,0,0,0,0,12,7,0,0
0,0,0,0,0,0,0,0,0,0,0,0
0,1,11,5,3,0,0,12,8,4,2,0
0,10,0,0,4,0,0,5,0,0,12,0
0,12,0,0,2,0,0,3,0,0,9,0
0,2,10,4,6,0,0,8,1,12,3,0
0,0,0,0,0,0,0,0,0,0,0,0
0,0,3,12,0,0,0,0,4,11,0,0
5,8,0,10,0,0,0,0,9,0,1,6
0,6,1,0,0,9,5,0,0,10,7,0";

            Solver solver = new Solver();
            solver.SudokuString = toSolve;
            solver.Run();


            Console.ReadLine();
        }
    }
}
