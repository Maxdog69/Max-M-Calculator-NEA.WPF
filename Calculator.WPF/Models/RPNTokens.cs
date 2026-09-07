using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace Calculator.WPF.Models
{
    public abstract class RPNToken
    {

    }

    public class NumberToken : RPNToken
    {
        public Fraction Value;

        public NumberToken(double value)
        {
            Value = MathTools.convertDoubleToFrac(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }

    public class FuncToken : RPNToken
    {
        public string Identifier;
        public Func<Fraction, Fraction> ApplyFunc;

        public FuncToken(string identifier)
        {
            Identifier = identifier;
            ApplyFunc = FunctionLookup.FetchFunction(identifier);
        }

        public override string ToString()
        {
            return $"FuncToken({Identifier})";
        }

    }

    public class OperatorToken : RPNToken
    {
        public char Symbol;
        public int Precedence;
        public bool LeftAssociative = true; // only exponentiation is rightAssociative

        public OperatorToken(char symbol, int precedence)
        {
            Symbol = symbol;
            Precedence = precedence;
        }

        public OperatorToken(char symbol, int precedence, bool leftAssociative)
        {
            Symbol = symbol;
            Precedence = precedence;
            LeftAssociative = leftAssociative;

        }

        public override string ToString()
        {
            return $"OperatorToken({Symbol})";
        }
    }

    public class VariableToken : RPNToken
    {
        public string Symbol;
        public Fraction Value;

        public VariableToken(string symbol)
        {
            Symbol = symbol;
        }

        public override string ToString()
        {
            return $"VarToken({Symbol})";
        }
    }

    public class LeftParenToken : RPNToken
    {
        public override string ToString()
        {
            return $"LeftParenToken";
        }

    }

    public class RightParenToken : RPNToken
    {
        public override string ToString()
        {
            return $"RightParenToken";
        }
    }



}
