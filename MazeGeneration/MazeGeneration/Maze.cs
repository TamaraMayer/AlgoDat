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
            int randCol = random.Next(this.Height + 1);
            int randRow = random.Next(this.Width + 1);

            MazeCells[randCol, randRow] = new Cell();

            List<Cell_PathGeneration> path;

            while (Array.IndexOf(MazeCells, null) > -1)
            {
                path = CreatePath();

                AddPathToMaze(path);
            }
        }

        private void AddPathToMaze(List<Cell_PathGeneration> path)
        {
            Cell_PathGeneration lastCell = path[path.Count - 1];
            path.RemoveAt(path.Count - 1);

            foreach (Cell_PathGeneration cell in path)
            {
                MazeCells[cell.Column, cell.Row] = new Cell();

                if (cell.CameFrom == Direction.North || cell.GoTo == Direction.North)
                {
                    MazeCells[cell.Column, cell.Row].North = false;
                }

                if (cell.CameFrom == Direction.East || cell.GoTo == Direction.East)
                {
                    MazeCells[cell.Column, cell.Row].East = false;
                }

                if (cell.CameFrom == Direction.South || cell.GoTo == Direction.South)
                {
                    MazeCells[cell.Column, cell.Row].South = false;
                }

                if (cell.CameFrom == Direction.West || cell.GoTo == Direction.West)
                {
                    MazeCells[cell.Column, cell.Row].West = false;
                }
            }

            if (lastCell.CameFrom == Direction.North)
            {
                MazeCells[lastCell.Column, lastCell.Row].North = false;
            }

            if (lastCell.CameFrom == Direction.East)
            {
                MazeCells[lastCell.Column, lastCell.Row].East = false;
            }

            if (lastCell.CameFrom == Direction.South)
            {
                MazeCells[lastCell.Column, lastCell.Row].South = false;
            }

            if (lastCell.CameFrom == Direction.West)
            {
                MazeCells[lastCell.Column, lastCell.Row].West = false;
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
                column = random.Next(this.Height + 1);
                row = random.Next(this.Width + 1);
            }
            while (MazeCells[column, row] != null);

            goTo = random.Next(5);

            path.Add(new Cell_PathGeneration(column, row, (Direction)goTo, Direction.None));

            do
            {
                row = SetNewRow(goTo, row);
                column = SetNewColumn(goTo, column);

                if (row < 0 || row > this.Width)
                {
                    continue;
                }

                if (column < 0 || column > this.Height)
                {
                    continue;
                }

                cameFrom = goTo;
                goTo = random.Next(5);

                if (MazeCells[column, row] != null)
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

        private int SetNewColumn(int goTo, int column)
        {
            switch (goTo)
            {
                case 1:
                    return column - 1;
                case 3:
                    return column + 1;
                default:
                    return column;
            }
        }

        private int SetNewRow(int goTo, int row)
        {
            switch (goTo)
            {
                case 2:
                    return row - 1;
                case 4:
                    return row + 1;
                default:
                    return row;
            }
        }
    }
}
