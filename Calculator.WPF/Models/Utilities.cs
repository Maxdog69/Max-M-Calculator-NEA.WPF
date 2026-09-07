using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Net;
using System.Text;

namespace Calculator.WPF.Models
{
    public class Utilities
    {
        static public List<RPNToken> TokeniseInfix(string infix) 
        {

            List<RPNToken> tokens = new List<RPNToken>();
            int i = 0;

            while (i < infix.Length)
            {
                char current = infix[i];

                if (char.IsWhiteSpace(current))
                {
                    i += 1;
                }

                // tokenise numbers:
                if (char.IsDigit(current) || current == '.')
                {
                    string number = "";
                    while (i < infix.Length && (char.IsDigit(infix[i]) || infix[i] == '.'))
                    {
                        number += infix[i];
                        i += 1;
                    }

                    tokens.Add(new NumberToken(double.Parse(number)));
                }

                //functions/variables:

                else if (char.IsLetter(current))
                {
                    string id = "";

                    while (i < infix.Length && char.IsLetter(infix[i]))
                    {
                        id += infix[i];
                        i += 1;
                    }

                    if (infix[i] == '(')
                    {
                        tokens.Add(new FuncToken(id));
                    }
                    else
                    {
                        tokens.Add(new VariableToken(id));
                    }

                }

                // parantheses:
                else if (current == '(')
                {
                    tokens.Add(new LeftParenToken());
                    i += 1;
                }

                else if (current == ')')
                {
                    tokens.Add(new RightParenToken());
                    i += 1;
                }

                // operators;

                else if (current == '+' || current == '-')
                {
                    tokens.Add(new OperatorToken(current, 0));
                    i += 1;
                }

                else if (current == '*' || current == '/')
                {
                    tokens.Add(new OperatorToken(current, 1));
                    i += 1;
                }

                else if (current == '^')
                {
                    tokens.Add(new OperatorToken(current, 2, false)); // use overload constructor since exponentiation is right assosciative
                    i += 1;
                }


            }

            return tokens;

        }

        static public List<RPNToken> Gen_RPN(List<RPNToken> RPNTokens)
        {

            List<RPNToken> output = new List<RPNToken>();
            MyStack<RPNToken> operatorStack = new MyStack<RPNToken>();

            foreach (RPNToken token in RPNTokens)
            {

                if (token is NumberToken || token is VariableToken)
                {
                    output.Add(token);
                }
                else if (token is FuncToken)
                {
                    operatorStack.Push(token);
                }

                else if (token is OperatorToken CurrentOperator)
                {
                    while (!operatorStack.isEmpty())
                    {

                        if (operatorStack.Peek() is OperatorToken NextOperator)
                        {
                            bool greaterPrecedence = NextOperator.Precedence > CurrentOperator.Precedence;
                            bool equalPrecedenceLeftAssociative = (NextOperator.Precedence == CurrentOperator.Precedence) && NextOperator.LeftAssociative;

                            if (greaterPrecedence || equalPrecedenceLeftAssociative)
                            {
                                output.Add(operatorStack.Pop());

                                continue;
                            }
                        }

                        break;
                    }
                    operatorStack.Push(token);
                }

                else if (token is LeftParenToken)
                {
                    operatorStack.Push(token);
                }
                else if (token is RightParenToken)
                {
                    while (!(operatorStack.Peek() is LeftParenToken))
                    {
                        output.Add(operatorStack.Pop());

                    }

                    operatorStack.Pop(); // discard the left parenthesis

                    if (operatorStack.Peek() is FuncToken)
                    {
                        output.Add(operatorStack.Pop());

                    }
                }
            }

            while (!operatorStack.isEmpty())
            {
                output.Add(operatorStack.Pop());

            }

            return output;
           
        }


        static public Fraction ComputeRPN(List<RPNToken> RPN)
        {
            MyStack<Fraction> outputStack = new MyStack<Fraction>();

            foreach (RPNToken token in RPN)
            {
                if (token is NumberToken n)
                {
                    outputStack.Push(n.Value);
                }
                else if (token is VariableToken x)
                {
                    outputStack.Push(x.Value);
                }
                else if (token is OperatorToken op)
                {
                    Fraction RHS = outputStack.Pop();
                    Fraction LHS = outputStack.Pop();

                    if (op.Symbol == '+')
                    {
                        outputStack.Push(LHS + RHS);
                    }
                    else if (op.Symbol == '-')
                    {
                        outputStack.Push(LHS - RHS);
                    }
                    else if (op.Symbol == '*')
                    {
                        outputStack.Push(LHS * RHS);
                    }
                    else if (op.Symbol == '/')
                    {
                        outputStack.Push(LHS / RHS);
                    }
                    else if (op.Symbol == '^')
                    {
                        outputStack.Push(LHS ^ RHS);
                    }
                }

                else if (token is FuncToken f)
                {
                    Fraction arg = outputStack.Pop();
                    Fraction result = f.ApplyFunc(arg);
                    outputStack.Push(result);

                }
            }

            return outputStack.Pop();
        }




        // purely for testing purposes, creating a method to display my tokenised infix quickly
        static public void printList(List<RPNToken> tokens)
        {
            foreach (RPNToken token in tokens)
            {
                Console.WriteLine(token);
            }
        }

    }

}
