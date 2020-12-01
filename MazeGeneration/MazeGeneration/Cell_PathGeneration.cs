using System;
using System.Collections.Generic;
using System.Text;

namespace MazeGeneration
{
    public enum Direction
    {
        North = 1,
        East = 2,
        South = 3,
        West = 4,
        None = 5
    }

    public class Cell_PathGeneration
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public Direction GoTo { get; set; }
        public Direction CameFrom { get; set; }

        public Cell_PathGeneration(int column, int row, Direction goTo, Direction cameFrom)
        {
            this.Column = column;
            this.Row = row;
            this.GoTo = goTo;
            this.CameFrom = cameFrom;
        }
    }
}
