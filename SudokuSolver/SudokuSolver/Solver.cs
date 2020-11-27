using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SudokuSolver.Test")]

namespace SudokuSolver
{
    class Solver
    {
        private int[] sudokuField;
        private string sudokuString;
        public int dimension;
        public int firstZero;
        private int blockHeight;
        private int blockWidth;

        public int[] SudokuField
        {
            get
            {
                return this.sudokuField;
            }
        }

        public string SudokuString { set { this.sudokuString = value; } }

        public void Run()
        {
            try
            {
                //sets sudoku string to array
                SetSudoku();

                //print original sudoku
                Console.WriteLine("Original Sudoku: ");
                Print();

                //checks if it is solvable
                if (!IsSolvable())
                {
                    Console.WriteLine("The given Sudoku cannot be solved, it already breaks the rules!");
                    return;
                }

                //looks for first zero
                firstZero = Array.IndexOf(sudokuField, 0);

                DateTime startTime;
                DateTime endTime;

                //Sets a timer with 45 minutes
                //even if 45 minutes are over it will still keep solving
                Timer timer = new Timer(2700000);
                timer.Elapsed += OnTimedEvent;
                timer.Start();

                //saves the start time
                startTime = DateTime.Now;

                //tries to solve the sudoku starting at the first zero
                if (!Solve(firstZero))
                {
                    Console.WriteLine("Sudoku could not be solved.");
                    return;
                }

                //saves end time
                endTime = DateTime.Now;

                //calculates time needed
                TimeSpan neededTime = endTime - startTime;

                Console.WriteLine("Solved Sudoku: ");
                Console.WriteLine("Time needed to solve: {0} minutes {1} seconds {2} milliseconds", neededTime.Minutes, neededTime.Seconds, neededTime.Milliseconds);
                this.Print();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Already 45 minutes needed to solve. Probably insolvable. Press Enter to terminate, press nothing to keep the solver running.");
            Console.ReadLine();
            Environment.Exit(0);
        }

        public bool Solve(int i)
        {
            //checks if this index is still in sudoku
            if (i >= dimension * dimension)
            {
                return true;
            }

            //pre filled fields
            if (sudokuField[i] != 0)
            {
                //solve the next field
                if (Solve(i + 1))
                {
                    return true;
                }
            }
            else
            {
                //go through the possible numbers starting at 1
                //check if it is valid, if yes continue with next field, if not try next number
                //if for loop is over, set back to zero
                //if the firstzero retruns false sudoku is unsolvable
                for (int j = 1; j <= dimension; j++)
                {
                    sudokuField[i] = j;

                    if (IsValid(i))
                    {
                        if (Solve(i + 1))
                        {
                            return true;
                        }
                    }
                }
                sudokuField[i] = 0;
                return false;
            }

            return false;
        }

        private void Print()
        {
            //if the dimension is bigger than 9 printbig

            if (this.dimension > 9)
            {
                PrintBig();
                return;
            }

            //otherwise this print
            Console.WriteLine();

            for (int i = 0; i < sudokuField.Length; i++)
            {
                if (i == 0)
                {
                    Console.Write("| ");

                }
                else
                {
                    if (i % this.dimension == 0)
                    {

                        Console.WriteLine("|");
                    }

                    if (i % (this.dimension * blockHeight) == 0)
                    {
                        Console.WriteLine(new string('-', this.dimension + (this.dimension - 1) * 2));
                    }

                    if (i % this.blockWidth == 0)
                    {
                        Console.Write("| ");
                    }
                }

                Console.Write(sudokuField[i] + " ");
            }
            Console.Write("|");
            Console.WriteLine();
            Console.WriteLine();
        }

        private void PrintBig()
        {
            //prints the sudoku with dimensions bigger than 9

            Console.WriteLine();

            for (int i = 0; i < sudokuField.Length; i++)
            {
                if (i == 0)
                {
                    Console.Write("| ");

                }
                else
                {
                    if (i % this.dimension == 0)
                    {

                        Console.WriteLine("|");
                    }

                    if (i % (this.dimension * blockHeight) == 0)
                    {
                        Console.WriteLine(new string('-', this.dimension * 3 + this.blockWidth * 2 + 1));
                    }

                    if (i % this.blockWidth == 0)
                    {
                        Console.Write("| ");
                    }
                }

                Console.Write((sudokuField[i] + " ").PadLeft(3));
            }
            Console.Write("|");
            Console.WriteLine();
            Console.WriteLine();
        }

        private bool IsValid(int i)
        {
            //checks for each row, column, and block if the number is valid there

            int[] numbers = new int[this.dimension];

            #region row
            int rowNumber = 0;

            //checks and sets in which row number the number is
            for (int j = dimension; j <= sudokuField.Length; j = j + dimension)
            {
                if (i < j)
                {
                    break;
                }

                rowNumber++;
            }

            //gets the other numbers in that row, the index itself is set with 0
            for (int j = 0; j < this.dimension; j++)
            {
                if (j + rowNumber * dimension == i)
                {
                    numbers[j] = 0;
                    continue;
                }

                numbers[j] = this.sudokuField[j + rowNumber * dimension];
            }

            //checks if the number is already in the array(row)
            if (numbers.Contains(this.sudokuField[i]))
            {
                return false;
            }
            #endregion row

            #region column
            //sets the columnNumber based on the index and the dimension of the sudoku
            int columnNumber = i % this.dimension;

            //as for row gets the other numbers in that column
            for (int j = 0; j < this.dimension; j++)
            {
                if (columnNumber + j * this.dimension == i)
                {
                    numbers[j] = 0;
                    continue;
                }

                numbers[j] = this.sudokuField[columnNumber + j * this.dimension];
            }

            //check
            if (numbers.Contains(this.sudokuField[i]))
            {
                return false;
            }
            #endregion column

            #region block
            int firstRow = 0;
            int firstColumn = 0;

            //first figures out which is the first row of the block, and which is the first column of the block
            for (int j = blockHeight; j <= dimension; j = j + blockHeight)
            {
                if (rowNumber < j)
                {
                    break;
                }

                firstRow = firstRow + blockHeight;
            }

            for (int j = blockWidth; j <= dimension; j = j + blockWidth)
            {
                if (columnNumber < j)
                {
                    break;
                }

                firstColumn = firstColumn + blockWidth;
            }

            int row;
            int index;
            int counter = 0;

            //then gets the numbers in that block
            for (int r = 0; r < blockHeight; r++)
            {
                row = r * dimension + firstRow * dimension;

                for (int c = 0; c < blockWidth; c++)
                {
                    index = row + c + firstColumn;

                    if (index == i)
                    {
                        numbers[counter] = 0;
                        counter++;
                        continue;
                    }

                    numbers[counter] = this.sudokuField[index];
                    counter++;
                }
            }

            //check
            if (numbers.Contains(this.sudokuField[i]))
            {
                return false;
            }
            #endregion block

            return true;
        }

        internal void SetSudoku()
        {
            //converts the string that was given at initialiation of th class into an array
            //first sets the dimension
            //then goes through each line of the string and saves it each int into the array

            SetDimension();

            List<int> sudokuField = new List<int>();

            using (StringReader reader = new StringReader(sudokuString))
            {
                string line;
                string[] seperatedLine;
                int number;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {

                        seperatedLine = line.Split(',');

                        foreach (string c in seperatedLine)
                        {
                            if (!Int32.TryParse(c, out number))
                            {
                                throw new ArgumentException("There is at least one character that is not a number nor a comma in the original Sudoku. That is invalid!");
                            }

                            sudokuField.Add(number);
                        }
                    }

                } while (line != null);
            }

            this.sudokuField = sudokuField.ToArray();
        }

        private void SetDimension()
        {
            int amountOfRows = 0;
            int amountOfColumns;

            //figures out how many lines (rows) there are
            using (StringReader reader = new StringReader(sudokuString))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        amountOfRows++;
                    }

                } while (line != null);
            }

            //figures out how many columns there are in the first line
            using (StringReader reader = new StringReader(sudokuString))
            {
                string line = reader.ReadLine();

                string[] seperatedLine = line.Split(',');
                amountOfColumns = seperatedLine.Length;
            }

            //checks if there is a dimension saved with those values
            if (!IsDimension(amountOfRows, amountOfColumns))
            {
                throw new ArgumentException("The given Sudoko does not meet the dimension criteria. The Sudoku is invalid!");
            }

            //checks if all other lines have the same amount of columns
            using (StringReader reader = new StringReader(sudokuString))
            {
                string line;
                string[] seperatedLine;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {

                        seperatedLine = line.Split(',');
                        if (amountOfRows != seperatedLine.Length)
                        {
                            throw new ArgumentException("The given Sudoku has a different amount of numbers in one row than the rows above! The Sudoko is invalid!");
                        }
                    }

                } while (line != null);
            }

            this.dimension = amountOfRows;
        }

        private bool IsDimension(int rows, int columns)
        {
            //sets parameters into a string and compares them to the saved dimensions

            string dim = $"{rows}x{columns}";

            switch (dim)
            {
                case "4x4":
                    this.blockHeight = 2;
                    this.blockWidth = 2;
                    return true;

                case "6x6":
                    this.blockHeight = 2;
                    this.blockWidth = 3;
                    return true;

                case "9x9":
                    this.blockHeight = 3;
                    this.blockWidth = 3;
                    return true;

                case "12x12":
                    this.blockHeight = 3;
                    this.blockWidth = 4;
                    return true;

                case "15x15":
                    this.blockHeight = 3;
                    this.blockWidth = 5;
                    return true;

                case "16x16":
                    this.blockHeight = 4;
                    this.blockWidth = 4;
                    return true;

                case "20x20":
                    this.blockHeight = 4;
                    this.blockWidth = 5;
                    return true;

                case "25x25":
                    this.blockHeight = 5;
                    this.blockWidth = 5;
                    return true;

                default:
                    return false;
            }
        }

        internal bool IsSolvable()
        {
            //goes through the original set array and checks for each value if it is valid in its position,
            //or if there is already a mistake in the given sudoku

            for (int i = 0; i < sudokuField.Length; i++)
            {
                if (sudokuField[i] == 0)
                {
                    continue;
                }

                if (!IsValid(i))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
