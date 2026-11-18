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
    /// Interaction logic for VarStoreScreen.xaml
    /// </summary>
    public partial class VarStoreScreen : Window
    {

        public double varValue { get; private set; }

        public VarStoreScreen()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(varInputField.Text, out double newX))
            {
                MessageBox.Show("Variable must be a real number");
                return;
            }
            varValue = newX;
            DialogResult = true;

        }
    }
}
