using MazeGeneration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Visualtization
{
    public class MainWindowVM
    {
        private Maze maze;
        private int height;
        private int width;

        public ICommand GenerateCommand
        {
            get
            {
                return new Command(obj =>
                {
                    this.Maze = new Maze(this.Height, this.Width);
                    this.Maze.Generate();
                });
            }
        }

        public Maze Maze
        {
            get
            {
                return this.maze;
            }
            set
            {
                this.maze = value;
            }
        }
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }
}
