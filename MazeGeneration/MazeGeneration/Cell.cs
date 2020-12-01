using System;
using System.Collections.Generic;
using System.Text;

namespace MazeGeneration
{
    public class Cell
    {
        public bool North { get; set; }
        public bool West { get; set; }
        public bool South { get; set; }
        public bool East { get; set; }

        public Cell()
        {
            this.North = true;
            this.West = true;
            this.South = true;
            this.East = true;
        }

        public Cell(bool north, bool west, bool south, bool east)
        {
            this.North = north;
            this.West = west;
            this.South = south;
            this.East = east;
        }
    }
}
