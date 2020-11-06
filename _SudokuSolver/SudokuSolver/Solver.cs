using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Solver
    {
        private int[] sudokuField;
        private string sudokuString;
        private int dimension;
        private int firstZero;
        private int blockSize;

        public Solver(string toSolve)
        {
            this.sudokuString = toSolve;
        }

        public void Run()
        {
            //try
            //{
            SetSudoku(sudokuString);

            Console.WriteLine("Original Sudoku: ");
            Print();

            // IsSolvable();

            firstZero = Array.IndexOf(sudokuField, 0);

            DateTime startTime = DateTime.Now;

            if (Solve(firstZero))
            {
                     throw new ArgumentOutOfRangeException("Sudoku could not be solved.");
            }

            DateTime endTime = DateTime.Now;

            TimeSpan neededTime = endTime - startTime;

            Console.WriteLine("Solved Sudoku: ");
            Console.WriteLine("Time needed to solve: {0}", neededTime.TotalMilliseconds);
            this.Print();

            //}
            //catch (Exception)
            //{
            //    //TODO
            //}
            //finally
            //{
            //    this.Print();
            //}



        }

        private bool Solve(int i)
        {
            if (i >= dimension * dimension)
            {
                return true;
            }

            if (sudokuField[i] != 0)//pre filled 
            {
                if(Solve(i + 1))
                {
                    return true;
                }
                else
                {
                //    sudokuField[i+1] = 0;
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
                        if(Solve(i + 1))
                        {
                            return true;
                        }
                        else
                        {
                     //       sudokuField[i+1] = 0;
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
           // Console.Clear();

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

                    if (i % (this.dimension *3) == 0)
                    {
                        Console.WriteLine(new string('-', this.dimension + (this.dimension - 1)*2));
                    }

                    if (i % this.blockSize == 0)
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

            for (int j = blockSize; j <= dimension; j = j + blockSize)
            {
                if (rowNumber < j)
                {
                    break;
                }

                firstRow = firstRow + blockSize;
            }

            for (int j = blockSize; j <= dimension; j = j + blockSize)
            {
                if (columnNumber < j)
                {
                    break;
                }

                firstColumn = firstColumn + blockSize;
            }

            int row;
            int index;
            int counter = 0;

            for (int r = 0; r < blockSize; r++)
            {
                row = r * dimension + firstRow * dimension;

                for (int c = 0; c < blockSize; c++)
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
                            //if (c.Trim().Length > 1)
                            //{
                            //    throw new ArgumentException("At least one number consists of more than one digit. The sudoku ");
                            //}

                            if (!Int32.TryParse(c, out number))
                            {
                                //TODO
                                throw new ArgumentException();
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
                //TODO
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
            this.blockSize = Convert.ToInt32(Math.Sqrt(dimension));

        }

        private bool IsDimension(int rows, int columns)
        {
            if (rows != columns)
            {
                return false;
            }


            if (Math.Pow(Math.Sqrt(rows), 2) != rows)
            {
                return false;
            }

            return true;

            //string dim = $"{rows}x{columns}";

            //switch (dim)
            //{
            //    case "9x9":
            //        return true;

            //    case "6x6":
            //        return true;
            //    default:
            //        return false;
            //}
        }

        private void IsSolvable()
        {
            //TODO
            new ArgumentException("The given Sudoku cannot be solved");

        }
    }
}
