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

namespace AVL_Tree_2ndAttempt
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AVL_VM vm = this.DataContext as AVL_VM;
            vm.TreeChangedEvent += RenderTree;
        }

        public void RenderTree(object sender, EventArgs e)
        {
            //clears the Visualizaiton Grid

            Visualization.Children.Clear();
            Visualization.RowDefinitions.Clear();
            Visualization.ColumnDefinitions.Clear();

            AVL_VM vm = (AVL_VM)this.DataContext;

            if (vm.root == null)
            {
                return;
            }

            //sets list to draw, gets the number of rows and calculates the number of columns
            vm.SetListToDraw();
            int numberOfRows = vm.rootHeight;
            int numberOfColumns = Convert.ToInt32(Math.Pow(2, numberOfRows) - 1);


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

            TextBlock node;

            //creates textblocks for every node with an entry and adds it to its spot in the grid
            for (int i = 0; i < vm.toDraw.Count; i++)
            {
                node = new TextBlock();

                if (vm.toDraw[i] != null)
                {
                    node.Text = vm.toDraw[i].Value.ToString();
                    node.FontSize = 15;

                    node.SetValue(Grid.RowProperty, vm.toDraw[i].ActualHeight);
                    node.SetValue(Grid.ColumnProperty, i);
                    node.HorizontalAlignment = HorizontalAlignment.Center;
                    Visualization.Children.Add(node);
                }
            }
        }
    }
}
