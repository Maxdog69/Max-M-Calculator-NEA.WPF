using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.WPF.Models
{
    public class InfixPreprocessor
    {
        public static List<RPNToken> Prepare(List<RPNToken> tokens)
        {
            return MarkUnaryMinus(InsertImplicitMultipliers(tokens));
        }


        private static bool EndsValue(RPNToken t)
        {
            return t is NumberToken || t is VariableToken || t is RightParenToken;
        }

        private static bool StartsValue(RPNToken t)
        {
            return t is NumberToken || t is VariableToken || t is LeftParenToken || t is FuncToken;
        }

        private static List<RPNToken> InsertImplicitMultipliers(List<RPNToken> tokens)
        {
            List<RPNToken> result = new List<RPNToken>();

            for (int i = 0;  i<tokens.Count; i++)
            {
                if (i > 0 && EndsValue(tokens[i-1]) && StartsValue(tokens[i]))
                {
                    result.Add(new OperatorToken('*', 1));
                }

                result.Add(tokens[i]);
            }

            return result;

        }

        private static List<RPNToken> MarkUnaryMinus(List<RPNToken> tokens)
        {
            List<RPNToken> result = new List<RPNToken>();

            foreach (RPNToken token in tokens)
            {
                bool isMinus;
                if (token is OperatorToken op && op.Symbol == '-')
                {
                    isMinus = true;
                }
                else
                {
                    isMinus = false;
                }

                if (isMinus)
                {
                    RPNToken prev = result.Count > 0 ? result[result.Count-1] : null;

                    bool isNegation = prev == null || prev is OperatorToken || prev is LeftParenToken || prev is FuncToken; // a minus sign is a negation if the previous token is NOT an operand

                    if (isNegation)
                    {
                        result.Add(new NegateToken());
                        continue;
                    }

                }

                result.Add(token);

            }
            return result;
            
        }

    }
}
