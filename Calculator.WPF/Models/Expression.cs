using Calculator.WPF.Models.Editing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.CompilerServices;

namespace Calculator.WPF.Models
{
    public class Expression : INotifyPropertyChanged
    {
        public Row Root { get; } = new();
        public Cursor Cursor { get; }

        public Expression()
        {
            Cursor = new Cursor(Root);
        }

        private bool CanTakeExponent()
        {
            Node before = Cursor.NodeBefore;
            Node after = Cursor.NodeAfter;

            if (before == null) return false;
            if (before is OperatorNode) return false;
            if (before is ParenNode p && p.Symbol == '(') return false;
            if (before is PowerNode) return false;
            if (after is PowerNode) return false;

            return true;
        }


        // true while this is the expression being typed, false once it is in history
        private bool _isEditing = true;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(LaTeX));
            }
        }

        public string LaTeX => Root.ToLaTex(IsEditing ? Cursor : null);


        private bool _hasError;
        private bool _hasResult;
        private string _errorMessage;
        private bool _showDecimal;

        private bool _isApproximate;

        public string ResultLaTeX
        {
            get
            {
                if (_hasError) return @"\text{" + _errorMessage + "}";
                if (!_hasResult) return "";

                if (_showDecimal)
                {
                    return ResultFrac.Magnitude.ToString();
                }
                else if (ResultFrac.Denominator == 1)
                {
                    return ResultFrac.Numerator.ToString();
                }
                else
                {
                    return @"\frac{" + ResultFrac.Numerator.ToString() + "}{" + ResultFrac.Denominator.ToString() + "}";
                }

            }
        }

        public Fraction ResultFrac { get; private set; }

        public void Insert(Node node)
        {
            Cursor.Insert(node); Refresh();
        }

        public void Backspace()
        {
            Cursor.Backspace(); Refresh();
        }

        public void MoveLeft()
        {
            Cursor.MoveLeft();
            Refresh(); 
        }

        public void MoveRight()
        {
            Cursor.MoveRight(); 
            Refresh(); 
        }

        public void MoveHome()
        {
            Cursor.MoveHome();
            Refresh();
        }

        public void MoveEnd()
        {
            Cursor.MoveEnd();
            Refresh();
        }

        public void InsertPower()
        {
            if (!CanTakeExponent())
            {
                return; // do nothing if the current node is not able to be a base
            }
            Cursor.Insert(new PowerNode());
            Refresh();
        }

        public void InsertFraction()
        {
            var taken = new List<Node>();
            Row row = Cursor.Row;
            int start = Cursor.Index;

            while (start > 0 && row.Nodes[start - 1] is DigitNode)
                start--;

            if (start < Cursor.Index)
            {
                for (int i = start; i < Cursor.Index; i++)
                    taken.Add(row.Nodes[i]);
            }
            else if (start > 0 && (row.Nodes[start - 1] is ConstantNode))
            {
                taken.Add(row.Nodes[start - 1]);                 // a single constant
            }

            // take out of the row
            for (int i = 0; i < taken.Count; i++)
                Cursor.Backspace();

            Cursor.Insert(new FractionNode());               

            foreach (var node in taken)
                Cursor.Insert(node);                         

            if (taken.Count > 0)
                Cursor.MoveRight();                             

            Refresh();
        }

        public void InsertSquare()
        {
            if (!CanTakeExponent())
            {
                return; // do nothing if the current node is not able to be a base
            }
            Cursor.Insert(new PowerNode());
            Cursor.Insert(new DigitNode('2'));
            Cursor.MoveRight();
            Refresh();
        }


        // ---- evaluating ----
        public bool Compute()
        {
            if (Root.Nodes.Count == 0) return false;      // nothing to calculate

            try
            {
                var infix = new List<RPNToken>();        
                Root.Flatten(infix);
                infix = InfixPreprocessor.Prepare(infix);
                var postfix = RPNProcessing.Gen_RPN(infix);   
                ResultFrac = RPNProcessing.ComputeRPN(postfix);

                _hasResult = true;
                _hasError = false;
                _isApproximate = Root.ContainsApproximate();
                _showDecimal = _isApproximate;
                OnPropertyChanged(nameof(ResultLaTeX));
                return true;

            }
            catch (Exception error)
            {
                _hasResult = false;
                _hasError = true;
                _errorMessage = error.Message;
                OnPropertyChanged(nameof(ResultLaTeX));
                return false;
            }
        }

        public void ToggleResultFormat() // SD NO LONGER WORKS WITH HISTORY
        {
            if (!_hasResult || _isApproximate) return; // nothing to change into decimal
            _showDecimal = !_showDecimal;

            OnPropertyChanged(nameof(ResultLaTeX));
        }

        private void Refresh()
        {
            _hasResult = false;
            _hasError = false;
            OnPropertyChanged(nameof(LaTeX));
            OnPropertyChanged(nameof(ResultLaTeX));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
