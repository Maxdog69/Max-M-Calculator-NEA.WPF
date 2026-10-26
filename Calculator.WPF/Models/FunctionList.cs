using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.WPF.Models
{
    public static class FunctionLookup
    {
        private static readonly Dictionary<string, Func<Fraction, Fraction>> _Functions = new Dictionary<string, Func<Fraction, Fraction>>

        {
            {"sin", FunctionList.sin},
            {"cos", FunctionList.cos},
            {"tan", FunctionList.tan},
            {"sqrt", FunctionList.sqrt },
        };

        public static Func<Fraction, Fraction> FetchFunction(string name)
        {
            try
            {
                return _Functions[name];
            }
            catch
            {
                throw new InvalidOperationException("invalid function");
            }
        }


    }

    public static class FunctionList
    {
        public static Fraction sin(Fraction arg)
        {
            double result = Math.Sin(arg.Magnitude);
            return MathTools.convertDoubleToFrac(result);
        }
        public static Fraction cos(Fraction arg)
        {
            double result = Math.Cos(arg.Magnitude);
            return MathTools.convertDoubleToFrac(result);
        }
        public static Fraction tan(Fraction arg)
        {
            return sin(arg) / cos(arg);
        }

        public static Fraction sqrt(Fraction arg)
        {

            if (arg.Magnitude < 0)
            {
                throw new InvalidOperationException("negative root");
            }

            double result = Math.Sqrt(arg.Magnitude);
            return MathTools.convertDoubleToFrac(result);
        }
    }
}
