using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Calculator.WPF.Models
{
    /*
     * Contains a series of miscellenous and potentially useful functions including:
     * 
     * Converting a double to a fraction
     * Prime factorisations of integers
     * Max(a,b) returns the max of two numbers
     * GCD(a,b) returns the gcd of two integers
     * LCM(a,b) returns lcm of two integers
     * ABS(a) returns modulus
     * 
     * 
     * 
     */


    public static class MathTools
    {

        public static Fraction convertDoubleToFrac(double d)
        {
            string digitStr = d.ToString();

            if (digitStr.Length >= 10)
            {
            digitStr = digitStr.Substring(0, 10); // truncate the number so that the denominator is not too large for an integer to store
            }

            Fraction fr;

            if (digitStr.Contains("."))
            {
                int counter = 0;
                bool passedDP = false;
                foreach (char c in digitStr)
                {

                    if (passedDP)
                    {
                        counter += 1;
                    }
                    if (c == '.')
                    {
                        passedDP = true;
                    }
                }

                digitStr = digitStr.Replace(".", "");


                long numerator = Convert.ToInt64(digitStr);
                long denominator = Convert.ToInt64(MathTools.integerExponentiation(10, counter));

                fr = new Fraction(numerator, denominator);

            }

            else
            {

                fr = new Fraction((long)d, 1);
            }

            fr.simplify();

            return fr;
        }

        public static List<int[]> PrimeFactorisation(int n)
        {

            if (n == 1)
            {
                List<int[]> result = new List<int[]>();
                result.Add([1, 1]);
                //Console.WriteLine($"prime: {result[0][0]}, exponent: {result[0][1]}");
                return result;
            }


            List<int[]> factors = new List<int[]>();

            int[] currentFactorExponentPair = new int[2];
            currentFactorExponentPair = [0, 0];

            while (n % 2 == 0)
            {
                currentFactorExponentPair[0] = 2;
                currentFactorExponentPair[1] += 1;
                n /= 2;
            }

            if (currentFactorExponentPair[0] != 0)
            {
                factors.Add(currentFactorExponentPair);

            }


            int i = 3;
            while (i * i <= n)
            {
                currentFactorExponentPair = [0, 0];

                while (n % i == 0)
                {
                    currentFactorExponentPair[0] = i;
                    currentFactorExponentPair[1] += 1;

                    n /= i;
                }

                if (currentFactorExponentPair[0] != 0)
                {
                    factors.Add(currentFactorExponentPair);

                }

                i += 2;
            }

            if (n > 1)
            {
                factors.Add([n, 1]);
            }

            //foreach (int[] pair in factors)
            //{
            //    Console.WriteLine($"prime: {pair[0]}, exponent: {pair[1]}");
            //}

            return factors;

        }

        public static double MAX(double a, double b)
        {
            if (a > b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        public static long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = a;
                a = b;

                b = temp % b;
            }
            return a;

        }

        public static long LCM(long a, long b)
        {
            return (a * b) / GCD(a, b);
        }

        public static double ABS(double a)
        {
            if (a < 0)
            {
                return -a;
            }
            else
            {
                return a;
            }
        }

        public static double integerExponentiation(double baseNum, long exponent)
        {

            double result = 1;

            if (exponent == 0)
            {
                return 1;
            }

            for (long i = 0; i < exponent; i++)
            {
                result = (result * baseNum);
            }
            return result;
        }

        public static double nthRoot(double basenum, long n, long precision = 100_000)
        {
            double x1 = 2;

            for (long i = 0; i < precision; i++)
            {
                x1 = x1 - (MathTools.integerExponentiation(x1, n) - basenum) / (n * x1);
            }

            return x1;
        }


        public static Fraction Exponent(Fraction basenum, Fraction exponent) 
        {
            // need qth root, then just raise to power of p(integer)

            if (exponent.Denominator == 1)
            {
                long numerator = (long)integerExponentiation(basenum.Numerator, exponent.Numerator);
                long denominator = (long)integerExponentiation(basenum.Denominator, exponent.Numerator);
                return new Fraction(numerator, denominator);

            }
            
            
            
            double root = nthRoot(basenum.Magnitude, exponent.Denominator);
            double result = integerExponentiation(root, exponent.Numerator);

            return convertDoubleToFrac(result);

        }


        public static double sqrt(double n, int PRECISION_MARKER = 100_000)
        {

            double x0 = 2;

            for (int i = 0; i < PRECISION_MARKER; i++) // babilonian method to calculate square roots
            {
                x0 = 0.5 * (x0 + n / x0);
            }

            return x0;
        }


    }


}
