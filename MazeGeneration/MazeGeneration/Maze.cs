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
            foreach(Cell_PathGeneration cell in path)
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
        }

        private List<Cell_PathGeneration> CreatePath()
        {
            int randCol;
            int randRow;

            do
            {
                randCol = random.Next(this.Height + 1);
                randRow = random.Next(this.Width + 1);
            }
            while (MazeCells[randCol, randRow] == null);
        }
    }
}
