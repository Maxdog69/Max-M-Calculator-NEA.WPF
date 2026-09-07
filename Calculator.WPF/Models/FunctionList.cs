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
            return arg * new Fraction(3,1); // this is obviously not sin(x) but its just to test the framework is correct
        }
        public static Fraction cos(Fraction arg)
        {
            throw new NotImplementedException();
        }
        public static Fraction tan(Fraction arg)
        {
            throw new NotImplementedException();
        }
    }
}
