using Calculator.WPF.Models.Editing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.WPF.Models
{
    public abstract class Node
    {

        public Row ParentRow { get; set; }

        public virtual IReadOnlyList<Row> Slots => Array.Empty<Row>();

        public abstract string ToLaTex(Cursor cursor);
        public string ToLaTex() => ToLaTex(null); // maybe unecessary
        public abstract void Flatten(List<RPNToken> output);

        public virtual bool isApproximate => false;

    }


    public class FractionNode : Node
    {
        private readonly Row[] _slots;

        public Row Numerator { get; } = new();
        public Row Denominator { get; } = new();

        public FractionNode()
        {
            Numerator.Owner = this;
            Denominator.Owner = this;
            _slots = new[] { Numerator, Denominator };       // order matters: top first
        }

        public override IReadOnlyList<Row> Slots => _slots;

        public override bool isApproximate =>
            Numerator.ContainsApproximate() || Denominator.ContainsApproximate();

        public override string ToLaTex(Cursor cursor) =>
            $@"\frac{{{Numerator.ToLaTex(cursor)}}}{{{Denominator.ToLaTex(cursor)}}}";

        public override void Flatten(List<RPNToken> output)
        {
            if (Numerator.Nodes.Count == 0 || Denominator.Nodes.Count == 0)
                throw new FormatException("Fraction has an empty part");

            // ( (numerator) / (denominator) ), so the whole thing acts as one operand
            output.Add(new LeftParenToken());
            output.Add(new LeftParenToken());
            Numerator.Flatten(output);
            output.Add(new RightParenToken());
            output.Add(new OperatorToken('/', 2));
            output.Add(new LeftParenToken());
            Denominator.Flatten(output);
            output.Add(new RightParenToken());
            output.Add(new RightParenToken());
        }
    }


    public class PowerNode : Node
    {
        private readonly Row[] _slots;
        public Row Exponent { get; } = new();
        public PowerNode()
        {
            Exponent.Owner = this;
            _slots = new[] { Exponent };
        }

        public override IReadOnlyList<Row> Slots => _slots;

        public override bool isApproximate => Exponent.ContainsApproximate();

        public override string ToLaTex(Cursor cursor)
        {
            return $"^{{{Exponent.ToLaTex(cursor)}}}";
        }

        public override void Flatten(List<RPNToken> output)
        {
            if (Exponent.Nodes.Count == 0)
            {
                throw new InvalidOperationException("Empty exponent");
            }

            output.Add(new OperatorToken('^', 3, false)); // exponentitaion is right assosciative
            output.Add(new LeftParenToken());
            Exponent.Flatten(output);
            output.Add(new RightParenToken());
        }


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

        public override string ToLaTex(Cursor cursor)
        {
            return Digit.ToString();
        }
    }

    public class ConstantNode : Node
    {
        private string _latex;
        private double _value;


        public override bool isApproximate => true;

        public ConstantNode(string latex, double value)
        {
            _latex = latex;
            _value = value;
        }

        public static ConstantNode PI() => new(@"\pi ", Math.PI);
        public static ConstantNode E() => new("e", Math.E);

        public override string ToLaTex(Cursor cursor)
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


        public override string ToLaTex(Cursor cursor)
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

        public override string ToLaTex(Cursor cursor)
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

    public class FunctionNode : Node
    {
        private readonly string _identifier;                 // the name FunctionLookup knows
        private readonly Func<string, string> _wrap;         // argument LaTeX -> full LaTeX
        private readonly Row[] _slots;

        public Row Argument { get; } = new();

        private FunctionNode(string identifier, Func<string, string> wrap)
        {
            _identifier = identifier;
            _wrap = wrap;
            Argument.Owner = this;
            _slots = new[] { Argument };
        }

        // name followed by brackets that grow with the content
        private static FunctionNode Named(string id, string latexName) =>
            new(id, inner => $@"{latexName}\left({inner}\right)");

        public static FunctionNode Sin() => Named("sin", @"\sin ");
        public static FunctionNode Cos() => Named("cos", @"\cos ");
        public static FunctionNode Tan() => Named("tan", @"\tan ");
        public static FunctionNode Ln() => Named("ln", @"\ln ");
        public static FunctionNode Log() => Named("log", @"\log ");

        public static FunctionNode Sqrt() =>
            new("sqrt", inner => $@"\sqrt{{{inner}}}");

        public override IReadOnlyList<Row> Slots => _slots;

        public override bool isApproximate => true;

        public override string ToLaTex(Cursor cursor) => _wrap(Argument.ToLaTex(cursor));

        public override void Flatten(List<RPNToken> output)
        {
            if (Argument.Nodes.Count == 0)
                throw new FormatException("Function has no argument");

            output.Add(new FuncToken(_identifier));
            output.Add(new LeftParenToken());
            Argument.Flatten(output);
            output.Add(new RightParenToken());
        }
    }


}
