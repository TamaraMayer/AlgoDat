using MazeGeneration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Visualtization
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private Maze maze;
        private int height;
        private int width;
        private BitmapSource bitmapSource;

        public BitmapSource BitmapSource
        {
            get
            {
                return this.bitmapSource;
            }
            set
            {
                this.bitmapSource = value;
                this.Notify();
            }
        }

        public ICommand GenerateCommand
        {
            get
            {
                return new Command(obj =>
                {
                    //initializes the maze class, generates the maze
                    if(this.Height <1 || this.Width < 1)
                    {
                        MessageBox.Show("Height or Width is invalid! Check that both values are grater than or equal to 1.","Error",MessageBoxButton.OK);
                        return;
                    }

                    this.Maze = new Maze(this.Height, this.Width);
                    this.Maze.Generate();
                });
            }
        }

        public ICommand ShowMazeCommand
        {
            get
            {
                return new Command(obj =>
                {
                    //sets list to draw, gets the number of rows and calculates the number of columns
                    int numberOfRows = Height * 2 + 1;
                    int numberOfColumns = Width * 2 + 1;

                    Bitmap tempBitmap = new Bitmap(numberOfColumns, numberOfRows);

                    Graphics g = Graphics.FromImage(tempBitmap);
                    g.Clear(Color.GreenYellow);

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
                                   tempBitmap.SetPixel(j * 2 + h, i * 2, Color.Black);
                                }
                            }

                            if (Maze.MazeCells[i, j].East)
                            {
                                for (int h = 0; h < 3; h++)
                                {
                                    tempBitmap.SetPixel(j * 2 + 2, i * 2 + h, Color.Black);
                                }
                            }
                            if (Maze.MazeCells[i, j].South)
                            {
                                for (int h = 0; h < 3; h++)
                                {
                                    tempBitmap.SetPixel(j * 2 + h, i * 2 + 2, Color.Black);
                                }
                            }
                            if (Maze.MazeCells[i, j].West)
                            {
                                for (int h = 0; h < 3; h++)
                                {
                                    tempBitmap.SetPixel(j * 2, i * 2 + h, Color.Black);
                                }
                            }
                        }
                    }


                    this.BitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        tempBitmap.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
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
                if (value < 1)
                {
                    this.height = value;
                    throw new ArgumentOutOfRangeException("The specified parameter must not be smaller than one!");
                }

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
                if (value < 1)
                {
                    this.width = value;
                    throw new ArgumentOutOfRangeException("The specified parameter must not be smaller than one!");
                }

                this.width = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify([CallerMemberName] string property = null)
        {
            //fires event that a property has changed

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
