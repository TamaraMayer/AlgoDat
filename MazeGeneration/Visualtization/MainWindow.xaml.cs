using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Visualtization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ShowMaze_Click(object sender, RoutedEventArgs e)
        {
            Visualization.Children.Clear();
            Visualization.RowDefinitions.Clear();
            Visualization.ColumnDefinitions.Clear();

            MainWindowVM vm = (MainWindowVM)this.DataContext;

            //sets list to draw, gets the number of rows and calculates the number of columns
            int numberOfRows = vm.Height * 2 + 1;
            int numberOfColumns = vm.Width * 2 + 1;


            //adds the row and columnsdefinition to the grid
            RowDefinition rowDefintion;
            ColumnDefinition columnDefintion;

            for (int i = 0; i < numberOfRows; i++)
            {
                rowDefintion = new RowDefinition();
                rowDefintion.Height = new GridLength(1, GridUnitType.Star);

                Visualization.RowDefinitions.Add(rowDefintion);
            }

            for (int i = 0; i < numberOfColumns; i++)
            {
                columnDefintion = new ColumnDefinition();
                columnDefintion.Width = new GridLength(1, GridUnitType.Star);

                Visualization.ColumnDefinitions.Add(columnDefintion);
            }

            TextBlock block;

            for (int i = 0; i < vm.Maze.MazeCells.GetLength(0); i++)
            {
                for (int j = 0; j < vm.Maze.MazeCells.GetLength(1); j++)
                {
                    if (vm.Maze.MazeCells[i, j].North)
                    {
                        for (int h = 0; i < 3; i++)
                        {
                            block = new TextBlock();
                            block.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                            block.SetValue(Grid.RowProperty, i);
                            block.SetValue(Grid.ColumnProperty, j * 2 + h);
                            Visualization.Children.Add(block);
                        }
                    }
                }
            }
        }

        public void ShowPath_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
