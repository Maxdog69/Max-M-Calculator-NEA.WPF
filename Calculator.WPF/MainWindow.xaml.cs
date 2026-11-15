using System.Net.Quic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XamlMath;
using Calculator.WPF.ViewModels;

namespace Calculator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainScreenViewModel();
            DataContext = vm;

        }


        private void FunctionListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GraphButton_Click(object sender, RoutedEventArgs e)
        {
            GraphingWindow graphingScreen = new GraphingWindow();
            graphingScreen.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show(
            //    "Are you sure you want to quit?",
            //    "Quit Graphing Calculator",
            //    MessageBoxButton.YesNo,
            //    MessageBoxImage.Question);

            //if (result == MessageBoxResult.Yes)
            //{
            //    Application.Current.Shutdown();
            Application.Current.Shutdown();
            //}
        }
    }
}