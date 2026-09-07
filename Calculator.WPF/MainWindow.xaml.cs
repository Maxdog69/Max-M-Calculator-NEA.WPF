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
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            bool shiftPressed = (Keyboard.Modifiers == ModifierKeys.Shift);

            if (e.Key == Key.D0)
            {
                Num0Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D1)
            {
                Num1Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D2)
            {
                Num2Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D3)
            {
                Num3Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D4)
            {
                Num4Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D5)
            {
                Num5Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D6)
            {
                Num6Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D7)
            {
                Num7Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.D8 && !shiftPressed)
            {
                Num8Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }            
            else if (e.Key == Key.D9)
            {
                Num9Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.OemMinus)
            {
                MinusButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.OemPlus && shiftPressed) // want to avoid pressing equals when it should be plus
            {
                AddButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if ((e.Key == Key.D8 && shiftPressed) || e.Key == Key.X)
            {
                TimesButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else if (e.Key == Key.OemQuestion)
            {
                FracButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }




        }

        private void DecimalPointButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Num0Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "0";
        }

        private void Num1Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "1";

        }

        private void Num2Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "2";
        }

        private void Num3Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "3";
        }

        private void Num4Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "4";
        }

        private void Num5Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "5";
        }

        private void Num6Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "6";
        }

        private void Num7Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "7";
        }

        private void Num8Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "8";
        }

        private void Num9Button_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "9";
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "+";
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "-";
        }


        private void TimesButton_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "*";
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ACButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AlphaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FunctionListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StoreVarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecallVarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SquareButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExponentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RootButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LnButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FracButton_Click(object sender, RoutedEventArgs e)
        {
            BotExpression.Formula += "\\frac{a}{b}";
        }

        private void ConvertFracButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SinButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CosButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TanButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenBracButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseBracButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PiButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void eButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ANSButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}