using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


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
            solver.SudokuString = @"0, 0, 8, 0, 6, 0, 4, 0, 9
                               4, 3, 0, 0, 9, 0, 0, 0, 5
                               0, 0, 2, 4, 0, 0, 0, 0, 3
                               0, 0, 0, 9, 0, 0, 0, 7, 6
                               8, 0, 0, 0, 0, 7, 0, 0, 4
                               5, 4, 0, 0, 3, 6, 0, 9, 0
                               0, 0, 0, 2, 0, 5, 0, 3, 0
                               9, 1, 5, 0, 0, 0, 2, 0, 0
                               0, 0, 0, 0, 0, 9, 6, 0, 0";

            int[] solvedSudoku =
            {
                7,5,8,3,6,1,4,2,9,4,3,6,7,9,2,1,8,5,1,9,2,4,5,8,7,6,3,3,2,1,9,8,4,5,7,6,8,6,9,5,2,7,3,1,4,5,4,7,1,3,6,8,9,2,6,8,4,2,1,5,9,3,7,9,1,5,6,7,3,2,4,8,2,7,3,8,4,9,6,5,1
            };

            solver.Run();

            NUnit.Framework.Assert.AreEqual(solvedSudoku, solver.SudokuField);
        }

        [TestMethod]
        //impossible to solve cause uneven amount of numbers
        public void Should_Throw_ArgumentException_becauseToManyNumbersInOneLine()
        {
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
        public void Should_Throw_ArgumentException_becauseGivenSudokuDoesNotMeetRules()
        {
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

            Assert.ThrowsException<ArgumentException>(solver.IsSolvable, "The given Sudoku cannot be solved");
        }

        [TestMethod]
        public void Should_Throw_ArgumentException_becauseGivenSudokuCouldNotBeSolved()
        {
            //impossible to solve in small
            solver.SudokuString = @"0,0,1,0
                               0,0,2,0
                               1,2,0,0
                               0,0,0,0";

            Assert.ThrowsException<ArgumentException>(solver.Run, "Sudoku could not be solved.");
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

            NUnit.Framework.Assert.AreEqual(solvedSudoku, solver.SudokuField);
        }
    }
}
