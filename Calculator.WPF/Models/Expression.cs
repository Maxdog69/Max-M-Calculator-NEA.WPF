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

        // derived from the Row every time, never stored
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

                return _showDecimal ? ResultFrac.Magnitude.ToString() : @"\frac{" + ResultFrac.Numerator.ToString() + "}{" + ResultFrac.Denominator.ToString() + "}";
            }
        }

        public Fraction ResultFrac { get; private set; }

        // ---- editing: each call changes the row, then tells the UI ----
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

        // ---- evaluating ----
        public bool Compute()
        {
            if (Root.Nodes.Count == 0) return false;      // nothing to calculate

            try
            {
                var infix = new List<RPNToken>();          // built fresh, then thrown away
                Root.Flatten(infix);
                infix = InfixPreprocessor.Prepare(infix);
                var postfix = RPNProcessing.Gen_RPN(infix);    // your existing code
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

        public void ToggleResultFormat()
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
