using System;

namespace MazeGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze(50, 20);
            maze.Generate();
        }
    }
}
