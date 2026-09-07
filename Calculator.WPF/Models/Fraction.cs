using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using XamlMath.Utils;

namespace Calculator.WPF.Models
{
    public class Fraction// want to define operators, and simplification/rationalisation (if there are surds - up to 2 surds on denominator are supported)
    {
        private long _numerator;
        private long _denominator;

        public long Numerator
        {
            get { return _numerator; }
            set
            {
                _numerator = value;
                simplify();
            }
        }
        public long Denominator
        {
            get { return _denominator; }
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("denominator cannot be zero");
                }

                _denominator = value;
                simplify();
            }
        }

        public double Magnitude
        {
            get { return (double)Numerator / Denominator;  }
        }

        public Fraction(long new_numerator, long new_denominator)
        {
            _numerator = new_numerator;
            _denominator = new_denominator;

            simplify();
            
        }

        public void simplify()
        {
            long GCD = MathTools.GCD(_numerator, _denominator);
            _numerator = _numerator / GCD;
            _denominator = _denominator / GCD;

        }

        public override string ToString()
        {
            return $"({Numerator} / {Denominator})";
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            long commonDenominator = MathTools.LCM(a.Denominator, b.Denominator);


            long scaleFactorA = commonDenominator / a.Denominator;
            long scaleFactorB = commonDenominator / b.Denominator;


            Fraction result = new Fraction((a.Numerator * scaleFactorA) + (b.Numerator * scaleFactorB), commonDenominator);
            return result;

        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            Fraction NegativeB = new Fraction(-b.Numerator, b.Denominator);
            return a + NegativeB;
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            Fraction result = new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
            return result;

        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.Numerator == 0 || b.Denominator == 0)
            {
                throw new InvalidOperationException("Division by zero error");
            }
            else
            {
                Fraction inverseB = new Fraction(b.Denominator, b.Numerator);

                return a * inverseB;
            }
        }

        public static Fraction operator ^(Fraction a, Fraction b)
        {
            return MathTools.Exponent(a, b);
        }

    }
}
