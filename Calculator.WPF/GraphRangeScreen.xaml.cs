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
    /// Interaction logic for GraphRangeScreen.xaml
    /// </summary>
    public partial class GraphRangeScreen : Window
    {
        public double XMin { get; private set; }
        public double XMax { get; private set; }
        public double YMin { get; private set; }
        public double YMax { get; private set; }

        public GraphRangeScreen(double xMin, double xMax, double yMin, double yMax)
        {
            InitializeComponent();
            XMinBox.Text = xMin.ToString();
            XMaxBox.Text = xMax.ToString();
            YMinBox.Text = yMin.ToString();
            YMaxBox.Text = yMax.ToString();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(XMinBox.Text, out double xMin) ||
                !double.TryParse(XMaxBox.Text, out double xMax) ||
                !double.TryParse(YMinBox.Text, out double yMin) ||
                !double.TryParse(YMaxBox.Text, out double yMax))
            {
                MessageBox.Show("Please enter valid numbers.");
                return;
            }
            if (xMin >= xMax || yMin >= yMax)
            {
                MessageBox.Show("Min must be less than max.");
                return;
            }

            XMin = xMin; XMax = xMax; YMin = yMin; YMax = yMax;
            DialogResult = true;   // closes the dialog
        }
    }
}
