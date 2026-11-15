using Calculator.WPF.ViewModels;
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

namespace Calculator.WPF
{
    /// <summary>
    /// Interaction logic for GraphingWindow.xaml
    /// </summary>
    public partial class GraphingWindow : Window
    {
        public GraphingWindow()
        {
            InitializeComponent();

            DataContext = new GraphingVM();
        }
    }
}
