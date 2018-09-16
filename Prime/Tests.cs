using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime
{
    /**
     * Class for Prime tests
     */
    public static class Tests
    {
        public static bool Naive(ulong number)
        {
            if (number < 4 && number > 1) {
                return true;
            }
            if (number % 2 == 0)
            {
                return false;
            }

            ulong i = 3;
            ulong SqrtOfNumber = Convert.ToUInt64(Math.Sqrt(number));

            while (i <= SqrtOfNumber)
            {
                if (number % i == 0)
                {
                    return false;
                }
                i += 2;
            }
            return true;
        }

        public static bool Erastothenes(ulong number)
        {
            if (number < 3 || number % 2 == 0)
            {
                //0,1,2 not prime
                return false;
            }

            Dictionary<ulong, bool>Sieve = MakeSieveErastothenes(number);
            return Sieve.ContainsKey(number) && Sieve[number];
        }

        /**
         * Helper method for Erastothenes algorithm
         */
        private static Dictionary<ulong, bool> MakeSieveErastothenes(ulong maximum)
        {
            Dictionary<ulong, bool> IsPrime = new Dictionary<ulong, bool>();

            for (ulong i = 3; i <= maximum; i+=2)
            {
                IsPrime[i] = true;
            }


            // Cross out multiples.
            for (ulong i = 3; i <= maximum; i++)
            {
                // See if i is probably a prime.
                if (IsPrime.ContainsKey(i) && IsPrime[i])
                {
                    // Knock out multiples of i.
                    for (ulong j = i * 2; j <= maximum; j += i)
                    {
                        IsPrime[j] = false;
                    }
                }
            }

            return IsPrime;
        }

        /**
         * Wilson test
         * n is prime if n > 1 and n|(n-1)!+1
         */
        public static bool Wilson(ulong number)
        {
            return 
                number > 1 && 
                number % (Factorial(number - 1) + 1) == 0;

        }

        /**
         * Factorial calculation - n!
         */
        private static ulong Factorial(ulong number)
        {
            return number == 1 ? 1 : number * Factorial(number - 1);
        }

        /**
         * Using Euklides algorithm - get gcd
         */
        private static ulong getGreatestCommonDivisor(ulong number1, ulong number2)
        {
            ulong gcd;

            if (number1 > number2)
            {
                if (number1 % number2 == 0)
                {
                    return number2;
                }
                else
                {
                    number2 = number1 % number2;

                }
            }
            else
            {
                if (number2 % number1 == 0)
                {
                    return number1;
                }
                else
                {
                    number1 = number2 % number1;

                }
            }

            gcd = getGreatestCommonDivisor(number1, number2);

            return gcd;
        }

        /**
         * Fermat test
         */
        public static bool Fermat(ulong number, ulong Chance)
        {
            // Shortcut for even numbers
            if (number % 2 == 0)
            {
                return false;
            }
            for (ulong i = 0; i < Chance; i++)
            {
                ulong a = generateRandomNumber(number-1, 1);
                // Fermat theorem
                if ((a ^ (number - 1) % number) != 1)
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * Generate 64bit unsigned integer value
         */
        public static ulong generateRandomNumber(ulong max, ulong min = 3)
        {
            if (max == min)
            {
                return max;
            }

            Random RandomGenerator = new Random();

            byte[] buf = new byte[8];
            RandomGenerator.NextBytes(buf);
            ulong longRand = BitConverter.ToUInt64(buf, 0);

            return longRand % (max - min) + min;
        }

        private static ulong Jacobi(ulong a, ulong n)
        {
            ulong temp;
            long j = 1;
            while (a != 0)
            {
                while (a % 2 == 0)
                {
                    a = a / 2;
                    if (n % 8 == 3 || n % 8 == 5) { j = -j; }
                }
                temp = a;
                a = n;
                n = temp;
                if (a % 4 == 3 && n % 4 == 3) { j = -j; }
                a = a % n;
            }
            return n == 1 ? (ulong) j : 0;
        }

        //Solovay–Strassen
        public static bool SolovayStrassen(ulong number, ulong chance)
        {
            ulong a;
            ulong gcd;
            ulong i;
            for (i = 0; i < chance; i++)
            {
                a = generateRandomNumber(number);
                gcd = getGreatestCommonDivisor(Convert.ToUInt64(number), Convert.ToUInt64(a));
                if (gcd > 1)
                {
                    break;
                }
                if (Jacobi(a, number) == 0 || Math.Pow(a, (number - 1) / 2) % number != Jacobi(a, number)) { break; }
            }
            return i == chance;

        }

        public static bool MillerRabin(ulong number)
        {
            // Shortcut for even numbers
            if ((number < 2) || (number % 2 == 0))
            {
                return number == 2;
            }

            ulong s = number - 1;
            while (s % 2 == 0)  s >>= 1;

            for (int i = 0; i > 100; i++)
            {
                ulong a = generateRandomNumber(number - 1) + 1;
                ulong temp = s;
                ulong mod = 1;
                for (ulong j = 0; j < temp; ++j)
                {
                    mod = (mod * a) % number;
                }
                while (temp != number - 1 && mod != 1 && mod != number - 1)
                {
                    mod = (mod * mod) % number;
                    temp *= 2;
                }

                if (mod != number - 1 && temp % 2 == 0)
                {
                    return false;
                }
                
            }
            return true;
        }
    }
}
