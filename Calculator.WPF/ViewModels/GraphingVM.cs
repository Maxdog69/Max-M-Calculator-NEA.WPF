using Calculator.WPF.Models.Editing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Calculator.WPF.Models;

namespace Calculator.WPF.ViewModels
{
    public class GraphingVM : INotifyPropertyChanged
    {

        public ICommand KeyCommand { get; }

        private Expression _expression0 = new();
        public Expression Expression0
        {
            get => _expression0;
            set
            {
                _expression0 = value;
                OnPropertyChanged();
            }
        }
        private Expression Expression1 = new();
        private Expression Expression2 = new();
        private Expression Expression3 = new();
        private Expression Expression4 = new();
        private Expression Expression5 = new();
        private Expression Expression6 = new();
        private Expression Expression7 = new();
        private Expression Expression8 = new();
        private Expression Expression9 = new();




        public GraphingVM()
        {
            KeyCommand = new RelayCommand(p => OnKey((string)p));


        }

        private void OnKey(string key)
        {

            if (key == "DEL") { Expression0.Backspace(); }
            else if (key == "LEFT") { Expression0.MoveLeft(); }
            else if (key == "RIGHT") { Expression0.MoveRight(); }
            else if (key == "AC") { Expression0 = new Expression(); }
            else if (key == "(") { Expression0.Insert(new ParenNode('(')); }
            else if (key == ")") { Expression0.Insert(new ParenNode(')')); }
            else if (key == "PI") { Expression0.Insert(ConstantNode.PI()); }
            else if (key == "E") { Expression0.Insert(ConstantNode.E()); }
            else if (key == "SIN") { Expression0.Insert(FunctionNode.Sin()); }
            else if (key == "COS") { Expression0.Insert(FunctionNode.Cos()); }
            else if (key == "TAN") { Expression0.Insert(FunctionNode.Tan()); }
            else if (key == "SQRT") { Expression0.Insert(FunctionNode.Sqrt()); }
            else if (key == "EXP") { Expression0.InsertPower(); }
            else if (key == "SQR") { Expression0.InsertSquare(); }
            else if (key == "FRAC") { Expression0.InsertFraction(); }
            else if (key == "GOEND") { Expression0.MoveEnd(); }
            else if (key == "GOHOME") { Expression0.MoveHome(); }
            else if (key== "VAR") { Expression0.Insert(VariableNode.X()); }

            else if (new[] { "+", "-", "*", "/" }.Contains(key))
            {
                Expression0.Insert(new OperatorNode(key[0]));
            }

            else // key must be a digit/decimal point atp
            {
                Expression0.Insert(new DigitNode(key[0]));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }

}

