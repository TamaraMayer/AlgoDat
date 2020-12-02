using MazeGeneration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Input;

namespace Visualtization
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private Maze maze;
        private int height;
        private int width;
        private Bitmap bitmap;

        public Bitmap Bitmap
        {
            get
            {
                return this.bitmap;
            }
            set
            {
                this.bitmap = value;
            }
        }

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

        public ICommand DrawMazeCommand
        {
            get
            {
                return new Command(obj =>
                {
                    //sets list to draw, gets the number of rows and calculates the number of columns
                    int numberOfRows = Height * 2 + 1;
                    int numberOfColumns = Width * 2 + 1;

                    Bitmap tempBitmap = new Bitmap(numberOfColumns, numberOfRows);

                    Cell cell;

                    for (int i = 0; i < Maze.MazeCells.GetLength(0); i++)
                    {
                        for (int j = 0; j < Maze.MazeCells.GetLength(1); j++)
                        {
                            cell = Maze.MazeCells[i, j];

                            if (cell.North)
                            {
                                for (int h = 0; h < 3; h++)
                                {
                                    bitmap.SetPixel(j * 2 + h, i * 2, Color.Black);
                                }
                            }

                            if (Maze.MazeCells[i, j].East)
                            {
                                //todo
                                for (int h = 0; h < 3; h++)
                                {
                                    bitmap.SetPixel(j * 2 + 2, i * 2 + h, Color.Black);
                                }
                            }
                            if (Maze.MazeCells[i, j].South)
                            {
                                //todo
                                for (int h = 0; h < 3; h++)
                                {
                                    bitmap.SetPixel(j * 2 + h, i * 2 + 2, Color.Black);
                                }
                            }
                            if (Maze.MazeCells[i, j].West)
                            {
                                //todo
                                for (int h = 0; h < 3; h++)
                                {
                                    bitmap.SetPixel(j * 2, i * 2 + h, Color.Black);

                                }
                            }
                        }
                    }

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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
