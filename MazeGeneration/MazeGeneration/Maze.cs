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
            int numberofEmptyFields =0;
            int randCol = random.Next(this.Height);
            int randRow = random.Next(this.Width);

            this.MazeCells[randRow, randCol] = new Cell();

            List<Cell_PathGeneration> path;

            numberofEmptyFields = this.Height * this.Width - 1;
           // numberofEmptyFields = this.GetNumberOfEmptyFields();

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

            for (int i = 0; i <this.MazeCells.GetLength(0); i++)
            {
                for (int j = 0; j < this.MazeCells.GetLength(1); j++)
                {
                    if (this.MazeCells[i, j] == null)
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
                this.MazeCells[cell.Row, cell.Column] = new Cell();

                if (cell.CameFrom == Direction.North || cell.GoTo == Direction.North)
                {
                    this.MazeCells[cell.Row, cell.Column].North = false;
                }

                if (cell.CameFrom == Direction.East || cell.GoTo == Direction.East)
                {
                    this.MazeCells[cell.Row, cell.Column].East = false;
                }

                if (cell.CameFrom == Direction.South || cell.GoTo == Direction.South)
                {
                    this.MazeCells[cell.Row, cell.Column].South = false;
                }

                if (cell.CameFrom == Direction.West || cell.GoTo == Direction.West)
                {
                    this.MazeCells[cell.Row, cell.Column].West = false;
                }
            }

            if (lastCell.CameFrom == Direction.North)
            {
                this.MazeCells[lastCell.Row, lastCell.Column].North = false;
            }

            if (lastCell.CameFrom == Direction.East)
            {
                this.MazeCells[lastCell.Row, lastCell.Column].East = false;
            }

            if (lastCell.CameFrom == Direction.South)
            {
                this.MazeCells[lastCell.Row, lastCell.Column].South = false;
            }

            if (lastCell.CameFrom == Direction.West)
            {
                this.MazeCells[lastCell.Row, lastCell.Column].West = false;
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

            //get the start point for the maze to not be in the maze
            do
            {
                column = random.Next(this.Width);
                row = random.Next(this.Height);
            }
            while (this.MazeCells[row, column] != null);

            //check that the next step will be within the maze
            do
            {
                goTo = random.Next(1, 5);
            } while (!IsInsideMaze(column,row,goTo));

            //add the start point to the path
            path.Add(new Cell_PathGeneration(column, row, (Direction)goTo, Direction.None));

            do
            {
                //set row and column for the next cell
                row = SetNewRow(goTo, path[path.Count-1].Row);
                column = SetNewColumn(goTo, path[path.Count - 1].Column);

                //set came from to the opposite of the direction the previous cell went to
                cameFrom = SetCameFrom(goTo);


                if (this.MazeCells[row, column] != null)
                {
                    path.Add(new Cell_PathGeneration(column, row, Direction.None, (Direction)cameFrom));
                    return path;
                }
                else
                {
                    //check that the next step will be within the maze
                    do
                    {
                        goTo = random.Next(1, 5);
                    } while (!IsInsideMaze(column, row, goTo));


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

        private bool IsInsideMaze(int column, int row, int goTo)
        {
            
                row = SetNewRow(goTo, row);
                column = SetNewColumn(goTo, column);
            

            if (column < 0 || column >= this.Width)
            {
                return false;
            }

            if (row < 0 || row >= this.Height)
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
