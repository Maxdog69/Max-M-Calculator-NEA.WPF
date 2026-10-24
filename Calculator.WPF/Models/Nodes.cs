using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.WPF.Models
{
    public abstract class Node
    {
        public abstract string ToLaTex();
        public abstract void Flatten(List<RPNToken> output);

        public virtual bool isApproximate => false;

    }

    public class DigitNode : Node
    {

        public char Digit { get; }

        public DigitNode(char digit)
        {
            Digit = digit;
        }

        public override void Flatten(List<RPNToken> output)
        {
            // do nothing
        }

        public override string ToLaTex()
        {
            return Digit.ToString();
        }
    }

    public class ConstantNode : Node
    {
        private string _latex;
        private double _value;

        public override bool isApproximate => true;

        public ConstantNode(string val)
        {
            if (val == "PI")
            {
                _latex = @"\pi ";
                _value = Math.PI; // need to get these constants without using MATH at some point 
            }
            else if (val == "E")
            {
                _latex = "e";
                _value = Math.E;
            }
        }

        public override string ToLaTex()
        {
            return _latex;
        }

        public override void Flatten(List<RPNToken> output)
        {
            output.Add(new NumberToken(_value));
        }

    }

    public class OperatorNode : Node
    {
        public char Symbol { get; } // +, -, *, /

        public OperatorNode(char symbol)
        {
            Symbol = symbol;
        }

        public override string ToLaTex()
        {
            if (Symbol == '*')
            {
                return @"\times";
            }
            else if (Symbol == '/')
            {
                return @"\div";
            }
            else
            {
                return Symbol.ToString();
            }
        }

        public override void Flatten(List<RPNToken> output)
        {
            if (Symbol == '+' || Symbol == '-')
            {
                output.Add(new OperatorToken(Symbol, 0));
            }
            else if (Symbol == '*' || Symbol == '/')
            {
                output.Add(new OperatorToken(Symbol, 1));
            }

        }

    }

    public class ParenNode : Node
    {
        public char Symbol { get; }

        public ParenNode(char symbol)
        {
            Symbol = symbol;
        }

        public override string ToLaTex()
        {
            return Symbol.ToString();
        }

        public override void Flatten(List<RPNToken> output)
        {
            if (Symbol == '(')
            {
                output.Add(new LeftParenToken());
            }
            else
            {
                output.Add(new RightParenToken());
            }
        }

    }


}
