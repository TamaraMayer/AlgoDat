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

            MazeCells[randCol,randRow] = new cel
        }
    }
}
