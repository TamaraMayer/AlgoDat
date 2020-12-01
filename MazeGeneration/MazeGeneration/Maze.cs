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

        public Cell[,] MazeCells {get;set;}

        public Maze(int height, int width)
        {
            this.Height = height;
            this.Width = width;
            this.MazeCells = new Cell[height,width];
            this.random = new Random();
        }

        public void Generate()
        {
            int randCol = random.Next(this.Height + 1);
            int randRow = random.Next(this.Width + 1);

            MazeCells[randCol, randRow] = new Cell();

            List<Cell_PathGeneration> path;

            while(Array.IndexOf(MazeCells,null) > -1)
            {
               path = CreatePath();

                AddPathToMaze(path);
            }
        }

        private void AddPathToMaze(List<Cell_PathGeneration> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                MazeCells[path[i].Column, path[i].Row] = new Cell();

                if (path[i].CameFrom == Direction.North || path[i].GoTo == Direction.North)
                {
                    MazeCells[path[i].Column, path[i].Row].North = false;
                }

                if (path[i].CameFrom == Direction.East || path[i].GoTo == Direction.East)
                {
                    MazeCells[path[i].Column, path[i].Row].East = false;
                }

                if (path[i].CameFrom == Direction.South || path[i].GoTo == Direction.South)
                {
                    MazeCells[path[i].Column, path[i].Row].South = false;
                }

                if (path[i].CameFrom == Direction.West || path[i].GoTo == Direction.West)
                {
                    MazeCells[path[i].Column, path[i].Row].West = false;
                }
            }
        }

        private List<Cell_PathGeneration> CreatePath()
        {
            throw new NotImplementedException();
        }
    }
}
