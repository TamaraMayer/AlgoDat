using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SudokuSolver.Test
{
    [TestClass]
    public class SolverUnitTest
    {
        Solver solver;

        [TestInitialize]
        public void Setup()
        {
            this.solver = new Solver();
        }

        [TestMethod]
        public void Should_Solve_9x9_Sudoku()
        {
            //Sets sudoku string

            solver.SudokuString = @"0, 0, 8, 0, 6, 0, 4, 0, 9
                               4, 3, 0, 0, 9, 0, 0, 0, 5
                               0, 0, 2, 4, 0, 0, 0, 0, 3
                               0, 0, 0, 9, 0, 0, 0, 7, 6
                               8, 0, 0, 0, 0, 7, 0, 0, 4
                               5, 4, 0, 0, 3, 6, 0, 9, 0
                               0, 0, 0, 2, 0, 5, 0, 3, 0
                               9, 1, 5, 0, 0, 0, 2, 0, 0
                               0, 0, 0, 0, 0, 9, 6, 0, 0";

            //is the solved sudoku as array
            int[] solvedSudoku =
            {
                7,5,8,3,6,1,4,2,9,4,3,6,7,9,2,1,8,5,1,9,2,4,5,8,7,6,3,3,2,1,9,8,4,5,7,6,8,6,9,5,2,7,3,1,4,5,4,7,1,3,6,8,9,2,6,8,4,2,1,5,9,3,7,9,1,5,6,7,3,2,4,8,2,7,3,8,4,9,6,5,1
            };

            solver.Run();

            //compares the two sudokus
            Assert.IsTrue(CompareSudokus(solvedSudoku, solver.SudokuField));
        }

        [TestMethod]
        //impossible to solve cause uneven amount of numbers
        public void Should_Throw_ArgumentException_becauseToManyNumbersInOneLine()
        {
            //sudoku has a digit to much in one line
            solver.SudokuString = @"7, 3, 0, 0, 0, 0, 0, 9, 8
                                    4, 0, 2, 0, 0, 0, 0, 0, 6
                                    0, 0, 0, 0, 1, 7, 0, 0, 0
                                    1, 5, 4, 7, 0, 0, 2, 0, 0, 8
                                    0, 0, 9, 0, 0, 0, 0, 0, 7
                                    0, 0, 0, 0, 0, 6, 5, 0, 0
                                    0, 6, 0, 1, 0, 0, 4, 0, 0
                                    0, 0, 1, 5, 0, 0, 0, 8, 3
                                    0, 0, 5, 0, 9, 0, 0, 0, 0";

            Assert.ThrowsException<ArgumentException>(solver.SetSudoku, "There is at least one character that is not a number nor a comma in the original Sudoku. That is invalid!");
        }

        [TestMethod]
        public void Should_Throw_ArgumentExceptionBecauseMoreColumnsThanRows()
        {
            //sudoku has 7 rows and 8 columns

            solver.SudokuString = @"7, 3, 0, 0, 0, 0, 0, 9, 8
                                    4, 0, 2, 0, 0, 0, 0, 0, 6
                                    0, 0, 0, 0, 1, 7, 0, 0, 0
                                    1, 5, 4, 7, 0, 0, 2, 0, 0
                                    0, 0, 9, 0, 0, 0, 0, 0, 7
                                    0, 0, 1, 5, 0, 0, 0, 8, 3
                                    0, 0, 5, 0, 9, 0, 0, 0, 0";

            Assert.ThrowsException<ArgumentException>(solver.SetSudoku, "The given Sudoko does not meet the dimension criteria. The Sudoku is invalid!");
        }

        [TestMethod]
        public void Should_ReturnFalse_becauseGivenSudokuDoesNotMeetRules()
        {
            //two 8 in one row
            solver.SudokuString = @"0, 0, 8, 8, 6, 0, 4, 0, 9
                                    4, 3, 0, 0, 9, 0, 0, 0, 5
                                    0, 0, 2, 4, 0, 0, 0, 0, 3
                                    0, 0, 0, 9, 0, 0, 0, 7, 6
                                    8, 0, 0, 0, 0, 7, 0, 0, 4
                                    5, 4, 0, 0, 3, 6, 0, 9, 0
                                    0, 0, 0, 2, 0, 5, 0, 3, 0
                                    9, 1, 5, 0, 0, 0, 2, 0, 0
                                    0, 0, 0, 0, 0, 9, 6, 0, 0";

            solver.SetSudoku();

            Assert.IsFalse(solver.IsSolvable());
        }

        [TestMethod]
        public void Should_Throw_ArgumentException_becauseGivenSudokuHasNotOnlyNumbers()
        {
            //there is an 'a' within the sudoku string

            solver.SudokuString = @"0, 0, 8, a, 6, 0, 4, 0, 9
                                    4, 3, 0, 0, 9, 0, 0, 0, 5
                                    0, 0, 2, 4, 0, 0, 0, 0, 3
                                    0, 0, 0, 9, 0, 0, 0, 7, 6
                                    8, 0, 0, 0, 0, 7, 0, 0, 4
                                    5, 4, 0, 0, 3, 6, 0, 9, 0
                                    0, 0, 0, 2, 0, 5, 0, 3, 0
                                    9, 1, 5, 0, 0, 0, 2, 0, 0
                                    0, 0, 0, 0, 0, 9, 6, 0, 0";

            Assert.ThrowsException<ArgumentException>(solver.SetSudoku, "There is at least one character that is not a number nor a comma in the original Sudoku. That is invalid!");
        }

        [TestMethod]
        public void Should_ReturnFalse_becauseGivenSudokuCouldNotBeSolved()
        {
            //impossible to solve in small
            solver.SudokuString = @"0,0,1,0
                                    0,0,2,0
                                    1,2,0,0
                                    0,0,0,0";

            solver.SetSudoku();

            //we start solve with 0 because thats the index where the first 0 is.
            Assert.IsFalse(solver.Solve(0));
        }

        [TestMethod]
        public void Should_Solve_12x12_Sudoku()
        {
            solver.SudokuString = @"0,3,8,0,0,10,7,0,0,6,4,0
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

            int[] solvedSudoku =
            {
                12,3,8,1,5,10,7,11,2,6,4,9,4,7,2,11,9,3,12,6,10,1,8,5,10,5,6,9,1,4,8,2,12,7,11,3,3,4,12,7,11,8,2,10,5,9,6,1,6,1,11,5,3,7,9,12,8,4,2,10,2,10,9,8,4,6,1,5,7,3,12,11,8,12,7,6,2,1,10,3,11,5,9,4,9,2,10,4,6,5,11,8,1,12,3,7,1,11,5,3,7,12,4,9,6,8,10,2,7,9,3,12,10,2,6,1,4,11,5,8,5,8,4,10,12,11,3,7,9,2,1,6,11,6,1,2,8,9,5,4,3,10,7,12
            };

            solver.Run();

            Assert.IsTrue(CompareSudokus(solvedSudoku, solver.SudokuField));
        }

        [TestMethod]
        public void Should_BeSolvable_16x16_Sudoku()
        {
            solver.SudokuString = @"2,0,8,4,0,0,10,9,6,0,0,0,0,12,7,5
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

            //int[] solvedSudoku =
            //{
            //    2,13,8,4,14,11,10,9,6,15,1,3,16,12,7,5,6,1,15,3,5,12,7,16,14,10,11,9,2,8,4,13,14,11,10,9,13,8,4,2,5,7,12,16,6,15,3,1,5,12,7,16,1,15,3,6,13,4,8,2,14,10,9,11,8,4,2,13,10,9,14,11,15,6,3,1,12,16,5,7,
            //    15,3,6,1,7,16,5,12,10,14,9,11,8,2,13,4,10,9,14,11,4,2,13,8,7,5,16,12,15,6,1,3,7,16,5,12,3,6,1,15,4,13,2,8,10,14,11,9,4,2,13,8,9,14,11,10,3,1,6,15,7,5,12,16,3,6,1,15,16,5,12,7,9,11,14,10,4,13,8,2,
            //    9,14,11,10,2,13,8,4,16,12,5,7,3,1,15,6,16,5,12,7,6,1,15,3,2,8,13,4,9,11,10,14,13,8,4,2,11,10,9,14,1,3,15,6,5,7,16,12,1,15,3,6,12,7,16,5,11,9,10,14,13,4,2,8,11,10,9,14,8,4,2,13,12,16,7,5,1,3,6,15,
            //    12,7,16,5,15,3,6,1,8,2,4,13,11,9,14,10
            //};

            solver.SetSudoku();

            Assert.IsTrue(solver.IsSolvable());
        }

        [TestMethod]
        public void Should_Solve_6x6_Sudoku()
        {
            solver.SudokuString = @"0,0,0,0,0,0
                                    0,3,6,4,2,0
                                    6,5,0,0,1,4
                                    0,4,0,0,6,0
                                    0,1,5,2,3,0
                                    0,0,0,0,0,0";

            int[] solvedSudoku =
            {
                1,2,4,6,5,3,5,3,6,4,2,1,6,5,2,3,1,4,3,4,1,5,6,2,4,1,5,2,3,6,2,6,3,1,4,5
            };

            solver.Run();

            Assert.IsTrue(CompareSudokus(solvedSudoku, solver.SudokuField));
        }

        [TestMethod]
        public void Should_BeSolvable_20x20_Sudoku()
        {
            solver.SudokuString = @"0,0,7,10,0,20,1,5,0,0,0,0,0,19,18,2,0,0,4,14
                                    0,11,6,0,0,0,2,0,0,14,12,0,0,13,0,0,3,1,16,0
                                    0,17,2,4,20,0,0,0,19,3,0,0,7,0,0,0,5,12,0,0
                                    8,13,16,0,1,0,0,0,0,0,17,0,2,0,0,7,9,0,0,0
                                    0,0,11,3,0,7,0,0,8,0,1,15,0,10,0,18,4,0,0,0
                                    0,9,0,0,6,13,4,17,5,0,8,7,0,0,0,0,0,15,3,0
                                    0,0,13,0,0,14,0,0,0,0,0,3,18,0,0,9,8,0,11,0
                                    0,0,12,8,0,2,0,0,15,0,0,0,0,4,0,0,0,0,13,0
                                    19,0,5,0,11,8,17,0,10,0,2,0,15,18,0,3,0,0,0,0
                                    20,8,15,0,7,0,11,2,14,0,0,0,0,0,3,0,0,0,0,0
                                    0,0,0,0,0,3,0,0,0,0,0,17,1,16,0,13,0,8,2,9
                                    0,0,0,0,18,0,5,4,0,12,0,10,0,6,20,16,0,11,0,17
                                    0,20,0,0,0,0,8,0,0,0,0,16,0,0,11,0,6,10,0,0
                                    0,3,0,1,4,0,0,12,6,0,0,0,0,0,19,0,0,2,0,0
                                    0,7,18,0,0,0,0,0,13,20,0,5,9,2,15,17,0,0,1,0
                                    0,0,0,9,12,0,10,0,18,1,0,6,0,0,17,0,7,3,0,0
                                    0,0,0,11,5,0,0,19,0,13,0,0,0,0,0,15,0,18,6,10
                                    0,0,14,2,0,0,0,16,0,0,20,18,0,0,0,11,1,9,12,0
                                    0,10,1,12,0,0,9,0,0,5,15,0,0,11,0,0,0,17,19,0
                                    6,16,0,0,15,1,18,0,0,0,0,0,10,5,13,0,14,7,0,0";

            solver.SetSudoku();

            Assert.IsTrue(solver.IsSolvable());
        }

        [TestMethod]
        public void Should_BeSolvable_15x15_Sudoku()
        {
            solver.SudokuString = @"5,0,3,0,2,1,4,0,13,7,9,0,10,0,6
                                    0,0,0,8,6,0,0,0,0,0,14,3,0,0,0
                                    11,1,4,0,0,0,8,0,2,0,0,0,5,15,13
                                    0,4,0,5,0,2,0,0,0,1,0,11,0,9,0
                                    0,0,0,0,14,12,0,0,0,6,13,0,0,0,0
                                    12,15,8,0,11,0,0,10,0,0,2,0,3,1,5
                                    9,0,0,0,0,4,0,0,0,12,0,0,0,0,2
                                    0,0,0,0,0,0,15,1,11,0,0,0,0,0,0
                                    1,0,0,0,0,8,0,0,0,10,0,0,0,0,3
                                    4,5,10,0,3,0,0,2,0,0,1,0,9,14,15
                                    0,0,0,0,9,10,0,0,0,5,11,0,0,0,0
                                    0,6,0,7,0,9,0,0,0,13,0,2,0,3,0
                                    15,14,5,0,0,0,10,0,1,0,0,0,2,13,11
                                    0,0,0,12,8,0,0,0,0,0,3,5,0,0,0
                                    2,0,13,0,1,7,14,0,8,4,15,0,6,0,9";

            solver.SetSudoku();

            Assert.IsTrue(solver.IsSolvable());
        }

        [TestMethod]
        public void Should_BeSolvable_25x25_Sudoku()
        {
            solver.SudokuString = @"13,20,0,21,0,16,0,0,0,7,0,1,0,8,23,0,10,12,0,0,24,17,0,18,15
                                    0,19,0,0,6,0,24,17,0,18,4,0,20,0,21,1,22,11,8,23,5,0,12,0,3
                                    1,0,0,23,8,0,0,4,0,21,12,0,0,0,3,0,17,9,0,0,19,0,0,7,0
                                    5,0,12,3,0,0,0,11,0,23,9,24,0,0,15,0,25,6,0,16,20,0,2,21,13
                                    0,0,0,15,0,0,0,12,14,3,6,0,25,0,0,20,0,0,21,13,0,11,8,23,1
                                    0,6,7,0,16,17,0,0,0,0,0,4,2,13,20,0,8,0,1,22,0,14,3,5,10
                                    0,2,0,0,0,25,6,0,0,19,23,0,0,0,0,12,0,3,0,0,0,0,0,0,0
                                    11,0,23,22,0,0,2,21,13,0,0,0,0,5,10,9,18,0,0,17,6,0,0,19,25
                                    0,14,3,0,0,11,8,23,1,22,15,0,18,24,17,0,7,16,0,0,0,0,13,0,4
                                    9,0,15,17,24,0,14,0,0,10,16,0,7,19,25,0,21,0,0,0,8,23,1,22,0
                                    19,25,0,16,0,24,17,9,18,0,2,20,4,0,0,22,0,8,0,0,10,0,14,0,5
                                    20,4,2,0,21,0,25,6,0,16,8,0,11,0,1,0,0,0,0,5,0,9,0,15,24
                                    22,11,0,0,23,0,0,2,21,0,0,0,0,3,5,0,9,18,15,0,0,6,0,0,0
                                    10,0,0,5,3,0,0,0,0,1,0,17,9,0,24,25,0,0,16,19,4,0,21,13,20
                                    0,0,18,24,15,0,12,0,0,0,0,25,0,16,0,4,2,0,0,0,11,0,0,1,22
                                    0,16,19,0,0,0,15,0,17,0,0,0,13,4,0,23,0,22,11,8,3,0,0,12,0
                                    21,0,0,0,0,0,0,0,0,6,22,0,1,0,0,3,5,10,12,0,0,24,17,9,18
                                    0,1,22,8,0,0,0,0,4,0,0,3,5,0,14,15,0,0,9,0,0,0,25,6,0
                                    0,0,0,14,0,0,1,0,0,8,17,15,0,9,18,16,19,25,6,7,13,20,0,2,0
                                    15,24,17,18,9,0,5,0,0,0,25,0,19,0,7,0,0,0,0,21,0,22,11,0,23
                                    6,0,0,0,19,9,18,0,0,0,0,2,21,20,0,0,0,0,0,0,14,3,0,10,12
                                    2,0,13,0,20,0,7,16,0,25,1,0,0,0,11,14,0,0,0,0,18,15,0,17,0
                                    8,0,1,11,0,2,21,13,0,4,5,14,3,10,0,0,15,0,17,9,7,16,0,25,0
                                    0,3,0,0,0,8,0,0,0,11,24,0,0,17,9,7,16,19,0,0,21,0,20,0,0
                                    18,0,0,9,0,14,0,5,0,0,0,7,16,25,0,21,0,20,4,2,23,1,22,0,0";

            solver.SetSudoku();

            Assert.IsTrue(solver.IsSolvable());
        }

        public bool CompareSudokus(int[] expected, int[] actual)
        {
            //checks if arrays are the same length

            if (expected.Length != actual.Length)
            {
                return false;
            }

            //checks if values are the same
            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != actual[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
