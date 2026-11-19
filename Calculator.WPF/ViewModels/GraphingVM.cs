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


        private List<Expression> _expressions = new List<Expression>();
        public List<Expression> Expressions
        {
            get => _expressions;
        }

        private int _currentExpressionEditing = 0;

        public int CurrentExpressionEditing
        {
            get => _currentExpressionEditing;
            set
            {
                _currentExpressionEditing = value;
                UpdateEditingState();
                OnPropertyChanged();
            }
        }

        private void UpdateEditingState()
        {
            for (int i = 0; i < _expressions.Count; i++)
            {
                _expressions[i].IsEditing = (i == _currentExpressionEditing);
            }
        }


        public GraphingVM()
        {
            KeyCommand = new RelayCommand(p => OnKey((string)p));

            for (int i = 0; i < 5; i++)
            {
                _expressions.Add(new Expression());
                if (i != 0)
                {
                    _expressions[i].IsEditing = false;
                }
            }
        }

        private void OnKey(string key)
        {

            if (key == "DEL") { Expressions[CurrentExpressionEditing].Backspace(); }
            else if (key == "LEFT") { Expressions[CurrentExpressionEditing].MoveLeft(); }
            else if (key == "RIGHT") { Expressions[CurrentExpressionEditing].MoveRight(); }
            else if (key == "AC") { Expressions[CurrentExpressionEditing].Clear(); }
            else if (key == "(") { Expressions[CurrentExpressionEditing].Insert(new ParenNode('(')); }
            else if (key == ")") { Expressions[CurrentExpressionEditing].Insert(new ParenNode(')')); }
            else if (key == "PI") { Expressions[CurrentExpressionEditing].Insert(ConstantNode.PI()); }
            else if (key == "E") { Expressions[CurrentExpressionEditing].Insert(ConstantNode.E()); }
            else if (key == "SIN") { Expressions[CurrentExpressionEditing].Insert(FunctionNode.Sin()); }
            else if (key == "COS") { Expressions[CurrentExpressionEditing].Insert(FunctionNode.Cos()); }
            else if (key == "TAN") { Expressions[CurrentExpressionEditing].Insert(FunctionNode.Tan()); }
            else if (key == "SQRT") { Expressions[CurrentExpressionEditing].Insert(FunctionNode.Sqrt()); }
            else if (key == "EXP") { Expressions[CurrentExpressionEditing].InsertPower(); }
            else if (key == "SQR") { Expressions[CurrentExpressionEditing].InsertSquare(); }
            else if (key == "FRAC") { Expressions[CurrentExpressionEditing].InsertFraction(); }
            else if (key == "GOEND") { Expressions[CurrentExpressionEditing].MoveEnd(); }
            else if (key == "GOHOME") { Expressions[CurrentExpressionEditing].MoveHome(); }
            else if (key== "VAR") { Expressions[CurrentExpressionEditing].Insert(VariableNode.X()); }

            else if (key.StartsWith("EDIT")) { CurrentExpressionEditing = int.Parse(key[4].ToString()) - 1; }

            else if (new[] { "+", "-", "*", "/" }.Contains(key))
            {
                Expressions[CurrentExpressionEditing].Insert(new OperatorNode(key[0]));
            }

            else // key must be a digit/decimal point atp
            {
                Expressions[CurrentExpressionEditing].Insert(new DigitNode(key[0]));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }

}

