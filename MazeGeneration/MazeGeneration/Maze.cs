using System;
using System.Collections.Generic;
using System.Text;

namespace MazeGeneration
{

    public class Maze
    {
        private Random random;
        public int Height { get; set; }
        public int Width { get; set; }

        public Cell[,] MazeCells { get; set; }

        public Maze(int height, int width)
        {
            this.Height = height;
            this.Width = width;
            this.MazeCells = new Cell[height, width];
            this.random = new Random();
        }

        public void Generate()
        {
            int numberofEmptyFields;
            int randCol = random.Next(this.Height);
            int randRow = random.Next(this.Width);

            MazeCells[randRow, randCol] = new Cell();

            List<Cell_PathGeneration> path;

            numberofEmptyFields = GetNumberOfEmptyFields();

            while (numberofEmptyFields > 0)
            {
                path = CreatePath();

                AddPathToMaze(path);

                numberofEmptyFields = GetNumberOfEmptyFields();
            }
        }

        private int GetNumberOfEmptyFields()
        {
            int counter=0;

            for (int i = 0; i < MazeCells.GetLength(0); i++)
            {
                for (int j = 0; j < MazeCells.GetLength(1); j++)
                {
                    if (MazeCells[i, j] == null)
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        private void AddPathToMaze(List<Cell_PathGeneration> path)
        {
            Cell_PathGeneration lastCell = path[path.Count - 1];
            path.RemoveAt(path.Count - 1);

            foreach (Cell_PathGeneration cell in path)
            {
                MazeCells[cell.Row, cell.Column] = new Cell();

                if (cell.CameFrom == Direction.North || cell.GoTo == Direction.North)
                {
                    MazeCells[cell.Row, cell.Column].North = false;
                }

                if (cell.CameFrom == Direction.East || cell.GoTo == Direction.East)
                {
                    MazeCells[cell.Row, cell.Column].East = false;
                }

                if (cell.CameFrom == Direction.South || cell.GoTo == Direction.South)
                {
                    MazeCells[cell.Row, cell.Column].South = false;
                }

                if (cell.CameFrom == Direction.West || cell.GoTo == Direction.West)
                {
                    MazeCells[cell.Row, cell.Column].West = false;
                }
            }

            if (lastCell.CameFrom == Direction.North)
            {
                MazeCells[lastCell.Row, lastCell.Column].North = false;
            }

            if (lastCell.CameFrom == Direction.East)
            {
                MazeCells[lastCell.Row, lastCell.Column].East = false;
            }

            if (lastCell.CameFrom == Direction.South)
            {
                MazeCells[lastCell.Row, lastCell.Column].South = false;
            }

            if (lastCell.CameFrom == Direction.West)
            {
                MazeCells[lastCell.Row, lastCell.Column].West = false;
            }
        }

        private List<Cell_PathGeneration> CreatePath()
        {
            List<Cell_PathGeneration> path = new List<Cell_PathGeneration>();
            int column;
            int row;
            int goTo;
            int cameFrom;
            Cell_PathGeneration newCell;
            Cell_PathGeneration isInPath;

            do
            {
                column = random.Next(this.Height);
                row = random.Next(this.Width);
            }
            while (MazeCells[row, column] != null);

            do
            {
                goTo = random.Next(1, 5);
            } while (!IsInsideMaze(column,row,goTo, true));


            path.Add(new Cell_PathGeneration(column, row, (Direction)goTo, Direction.None));

            do
            {
                row = SetNewRow(goTo, path[path.Count-1].Row);
                column = SetNewColumn(goTo, path[path.Count - 1].Column);

                if (!IsInsideMaze(column, row, goTo, false))
                {
                    goTo = random.Next(1, 5);
                    continue;
                }

                cameFrom = SetCameFrom(goTo);
                goTo = random.Next(1,5);

                if (MazeCells[row, column] != null)
                {
                    path.Add(new Cell_PathGeneration(column, row, Direction.None, (Direction)cameFrom));
                    return path;
                }
                else
                {
                    newCell = new Cell_PathGeneration(column, row, (Direction)goTo, (Direction)cameFrom);

                    isInPath = path.Find(p => p.Column == newCell.Column && p.Row == newCell.Row);

                    if (isInPath != null)
                    {
                        // delete up to the found one, and change goto

                       int index= path.FindIndex(p => p.Column == newCell.Column && p.Row == newCell.Row);

                        path.RemoveRange(index + 1, path.Count - index - 1);

                        path[index].GoTo = (Direction)goTo;
                    }
                    else
                    {
                        path.Add(newCell);
                    }
                }
            }
            while (true);
        }

        private bool IsInsideMaze(int column, int row, int goTo, bool calculateNewFields)
        {
            if (calculateNewFields)
            {
                row = SetNewRow(goTo, row);
                column = SetNewColumn(goTo, column);
            }

            if (row < 0 || row >= this.Width)
            {
                return false;
            }

            if (column < 0 || column >= this.Height)
            {
                return false;                
            }

            return true;
        }

        private int SetCameFrom(int goTo)
        {
            switch (goTo)
            {
                case 1:
                    return 3;
                case 2:
                    return 4;
                case 3:
                    return 1;
                case 4:
                    return 2;
                default:
                    return 0;
            }
        }

        private int SetNewRow(int goTo, int row)
        {
            switch (goTo)
            {
                case 1:
                    return row - 1;
                case 3:
                    return row + 1;
                default:
                    return row;
            }
        }

        private int SetNewColumn(int goTo, int column)
        {
            switch (goTo)
            {
                case 2:
                    return column + 1;
                case 4:
                    return column - 1;
                default:
                    return column;
            }
        }
    }
}
