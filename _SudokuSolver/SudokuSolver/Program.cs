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

            //middle Sudoku
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
            //string toSolve = @"0, 2, 4, 6, 0, 3 
            //                   1, 0, 6, 0, 5, 2 
            //                   2, 6, 0, 5, 3, 4
            //                   3, 4, 5, 0, 6, 1
            //                   6, 1, 0, 3, 0, 5
            //                   4, 0, 3, 1, 2, 0";

            //6x6 unsolvable because eine Zahl doppelt in Zeile
            //string toSolve = @"0, 2, 4, 6, 0, 3 
            //                   1, 0, 6, 0, 5, 2 
            //                   2, 6, 0, 5, 3, 4
            //                   3, 4, 5, 0, 6, 1
            //                   6, 1, 3, 3, 0, 5
            //                   4, 0, 3, 1, 2, 0";

            //16x16 Sudoku
            string toSolve = @"2,0,8,4,0,0,10,9,6,0,0,0,0,12,7,5
                               6,1,0,0,5,0,7,16,14,10,11,0,0,0,0,0
                               0,0,0,9,13,0,4,0,5,7,0,16,0,0,0,1
                               0,0,0,0,1,0,0,6,0,0,0,0,0,0,9,0
                               0,0,0,0,0,0,0,0,15,0,3,1,0,0,0,0
                               0,0,6,0,7,0,0,0,0,14,9,11,8,2,13,4
                               10,0,14,0,4,0,0,8,7,5,0,12,15,0,1,3
                               0,16,5,12,0,0,0,0,4,0,0,8,0,14,0,0
                               0,0,0,8,0,14,11,10,0,0,0,15,0,5,12,0
                               3,0,0,15,0,0,0,7,9,0,0,0,0,0,0,2
                               0,14,11,10,2,0,8,0,0,0,0,0,3,0,0,0
                               16,0,0,7,6,0,0,0,2,8,13,4,9,0,0,0
                               13,0,0,2,11,0,9,0,1,0,15,0,5,0,0,0
                               0,0,3,6,12,7,0,0,0,0,0,14,0,4,2,0
                               0,0,0,14,0,0,0,13,12,16,7,0,1,0,0,15
                               0,0,0,5,0,3,0,0,0,2,0,13,11,0,14,0";


            Solver solver = new Solver(toSolve);
            solver.Run();

            Console.ReadLine();
        }
    }
}
