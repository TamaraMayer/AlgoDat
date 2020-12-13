using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Visualisierung
{
    /// <summary>
    /// Interaktionslogik für InputPopUp.xaml
    /// </summary>
    public partial class InputPopUp : Window
    {
        public InputPopUp()
        {
            InitializeComponent();
        }

        private void enter_btn_click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
