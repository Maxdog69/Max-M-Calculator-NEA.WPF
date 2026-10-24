using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Packaging;
using System.Text;

namespace Calculator.WPF.Models.Editing
{
    public class Row
    {
        public List<Node> Nodes { get; } = new();

        private const string CursorMarker = @"\vert";

        public bool ContainsApproximate() => Nodes.Any(n => n.isApproximate);

        public string ToLaTex(Cursor cursor)
        {
            string latex = "";

            for (int i = 0; i<= Nodes.Count; i++)
            {
                if (cursor != null && cursor.Row == this && cursor.Index == i)
                {
                    latex += CursorMarker;
                }
                if (i < Nodes.Count)
                {
                    latex += Nodes[i].ToLaTex();
                }
            }

            return latex;

        }

        public void Flatten(List<RPNToken> output)
        {
            string digits = "";

            foreach (Node n in Nodes)
            {
                if (n is DigitNode d)
                {
                    digits += d.Digit;
                }
                else
                {
                    FlushDigits(ref digits, output);
                    n.Flatten(output);
                }
            }

            FlushDigits(ref digits, output);

        }

        private static void FlushDigits(ref string digits, List<RPNToken> output)
        {
            if (digits.Length == 0)
            {
                return;
            }

            string varText = digits;
            digits = "";

            if (double.TryParse(varText, out double value))
            {
                output.Add(new NumberToken(value));
            }
        }

    }
}
