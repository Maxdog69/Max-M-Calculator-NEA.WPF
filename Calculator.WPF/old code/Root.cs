using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.WPF.Models
{
    public class Root
    {
        public Fraction Coefficient;
        public long Radicand;

        // i want to allow the user to enter a root with a decimal number, so i will have two constructors: one with int and one with frac

        public Root(Fraction NewCoefficient, long NewRadicand)
        {
            Coefficient = NewCoefficient;
            Radicand = NewRadicand; 
        }

        public Root(Fraction NewCoefficient, Fraction NewRadicand)
        {
            Radicand = NewRadicand.Numerator * NewRadicand.Denominator;
            Coefficient = new Fraction(NewCoefficient.Numerator, NewCoefficient.Denominator * NewRadicand.Denominator);

        }

        public override string ToString()
        {
            return $"{Coefficient.ToString()}*sqrt({Radicand})";
        }

    }
}
