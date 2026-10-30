using Calculator.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Calculator.WPF.ViewModels
{
    public class MainScreenViewModel : INotifyPropertyChanged
    {
        private const int MaxExpressionHistory = 2;
        public ObservableCollection<Expression> ExpressionHistory { get; } = new(); // would like to make this a queue ?
        private Expression _currentExpression = new(); 
        public Expression CurrentExpression
        {
            get => _currentExpression;
            set
            {
                _currentExpression = value;
                OnPropertyChanged();
            }
        }

        public ICommand KeyCommand { get; }


        public MainScreenViewModel()
        {
            KeyCommand = new RelayCommand(p => OnKey((string)p));
        }


        public bool AlphaPressed = false;

        private void OnKey(string key)
        {
            //if (key == "=") { PressEquals(); } NOT DONE HISTORY YET WILL SWAP BACK IN
            if (key == "=") { CurrentExpression.Compute(); }
            else if (key == "DEL") { CurrentExpression.Backspace(); }
            else if (key == "LEFT") { CurrentExpression.MoveLeft(); }
            else if (key == "RIGHT") { CurrentExpression.MoveRight(); }
            else if (key == "AC") { CurrentExpression = new Expression(); }
            else if (key == "(") { CurrentExpression.Insert(new ParenNode('(')); }
            else if (key == ")") { CurrentExpression.Insert(new ParenNode(')')); }
            else if (key == "PI") { CurrentExpression.Insert(ConstantNode.PI()); }
            else if (key == "E") { CurrentExpression.Insert(ConstantNode.E()); }
            else if (key == "SD") { CurrentExpression.ToggleResultFormat(); }
            else if (key == "SIN") { CurrentExpression.Insert(FunctionNode.Sin()); }
            else if (key == "COS") { CurrentExpression.Insert(FunctionNode.Cos()); }
            else if (key == "TAN") { CurrentExpression.Insert(FunctionNode.Tan()); }
            else if (key == "SQRT") { CurrentExpression.Insert(FunctionNode.Sqrt()); }
            else if (key == "EXP") { CurrentExpression.InsertPower(); }
            else if (key == "SQR") { CurrentExpression.InsertSquare(); }
            else if (key == "FRAC") { CurrentExpression.InsertFraction(); }
            else if (key == "GOEND") { CurrentExpression.MoveEnd(); }
            else if (key == "GOHOME") { CurrentExpression.MoveHome(); }

            else if (new[] { "+", "-", "*", "/" }.Contains(key))
            {
                CurrentExpression.Insert(new OperatorNode(key[0]));
            }

            else // key must be a digit/decimal point atp
            {
                CurrentExpression.Insert(new DigitNode(key[0]));
            }
        }


        private void PressEquals()
        {
            if (!CurrentExpression.Compute()) return; // don't add a failed expression to expression history

            CurrentExpression.IsEditing = false;
            ExpressionHistory.Add(CurrentExpression);

            while (ExpressionHistory.Count > MaxExpressionHistory)
            {
                ExpressionHistory.RemoveAt(0);
            }

            CurrentExpression = new Expression();

        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
