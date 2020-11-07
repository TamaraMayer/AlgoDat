using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SudokuSolver
{
    public class Solver
    {
        private int[] sudokuField;
        private string sudokuString;
        private int dimension;
        private int firstZero;
        private int blockHeight;
        private int blockWidth;

        public Solver(string toSolve)
        {
            this.sudokuString = toSolve;
        }

        public void Run()
        {
            try
            {
                SetSudoku(sudokuString);

                Console.WriteLine("Original Sudoku: ");
                Print();

                IsSolvable();

                firstZero = Array.IndexOf(sudokuField, 0);

                DateTime startTime;
                DateTime endTime;

                Timer timer = new Timer(2700000);
                timer.Elapsed += OnTimedEvent;
                timer.Start();

                startTime = DateTime.Now;

                if (!Solve(firstZero))
                {
                    throw new ArgumentOutOfRangeException("Sudoku could not be solved.");
                }

                endTime = DateTime.Now;

                TimeSpan neededTime = endTime - startTime;

                Console.WriteLine("Solved Sudoku: ");
                Console.WriteLine("Time needed to solve: {0} minutes {1}seconds {2} milliseconds", neededTime.Minutes, neededTime.Seconds, neededTime.Milliseconds);
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

        private bool Solve(int i)
        {
            if (i >= dimension * dimension)
            {
                return true;
            }

            if (sudokuField[i] != 0)//pre filled 
            {
                if (Solve(i + 1))
                {
                    return true;
                }
            }
            else
            {
                for (int j = 1; j <= dimension; j++)
                {

                    sudokuField[i] = j;

                    //   this.Print();

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
            if (this.dimension > 9)
            {
                PrintBig();
                return;
            }


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

                Console.Write((sudokuField[i] + " ").PadLeft(3));
            }
            Console.Write("|");
            Console.WriteLine();
            Console.WriteLine();
        }

        private bool IsValid(int i)
        {
            int[] numbers = new int[this.dimension];

            //row
            int rowNumber = 0;

            for (int j = dimension; j <= sudokuField.Length; j = j + dimension)
            {
                if (i < j)
                {
                    break;
                }

                rowNumber++;
            }

            for (int j = 0; j < numbers.Length; j++)
            {
                if (j + rowNumber * dimension == i)
                {
                    numbers[j] = 0;
                    continue;
                }

                numbers[j] = this.sudokuField[j + rowNumber * dimension];
            }
            if (numbers.Contains(this.sudokuField[i]))
            {
                return false;
            }

            //column
            int columnNumber = i % this.dimension;
            for (int j = 0; j < numbers.Length; j++)
            {
                if (columnNumber + j * this.dimension == i)
                {
                    numbers[j] = 0;
                    continue;
                }

                numbers[j] = this.sudokuField[columnNumber + j * this.dimension];
            }

            if (numbers.Contains(this.sudokuField[i]))
            {
                return false;
            }

            //box
            int firstRow = 0;
            int firstColumn = 0;

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

            if (numbers.Contains(this.sudokuField[i]))
            {
                return false;
            }

            return true;
        }

        private void SetSudoku(string sudokuString)
        {
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

            using (StringReader reader = new StringReader(sudokuString))
            {
                string line = reader.ReadLine();

                string[] seperatedLine = line.Split(',');
                amountOfColumns = seperatedLine.Length;
            }

            if (!IsDimension(amountOfRows, amountOfColumns))
            {
                throw new ArgumentException("The given Sudoko does not meet the dimension criteria. The Sudoku is invalid!");
            }

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

        private void IsSolvable()
        {
            for (int i = 0; i < sudokuField.Length; i++)
            {
                if (sudokuField[i] == 0)
                {
                    continue;
                }

                if (!IsValid(i))
                {
                    throw new ArgumentException("The given Sudoku cannot be solved");
                }
            }
        }
    }
}
