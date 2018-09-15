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
        public static bool Erastothenes(int number)
        {
            Boolean valid = false;
            if ((number > -3 && number < 3) || (number % 2 == 0))
            {
                //0,1,2 not prime
                return false;
            }

            valid = Erastothenes(number, number + 1);
            return valid;
        }

        /**
         * Helper method for Erastothenes algorithm
         */
        private static bool Erastothenes(int number, int maximum)
        {
            bool valid = false;

            bool[] numbers = new bool[maximum];

            numbers[0] = false;

            for (int i = 1; i < numbers.Length; i++)
            {
                numbers[i] = true;
            }

            int p = 2;

            while (Math.Pow(p, 2) < maximum)
            {
                if (numbers[p])
                {
                    int j = (int)Math.Pow(p, 2);

                    while (j < maximum)
                    {
                        numbers[j] = false;
                        j = j + p;
                    }
                }

                p++;
            }

            for (int i = 0; i < numbers.Length; i++)
            {
                if ((i + 1) == number)
                {
                    valid = numbers[i];
                }
            }

            return valid;
        }

        /**
         * Wilson test
         * n is prime if n > 1 and n|(n-1)!+1
         */
        public static bool Wilson(uint number)
        {
            return 
                number > 1 && 
                number % (Factorial(number - 1) + 1) == 0;

        }

        /**
         * Factorial calculation - n!
         */
        private static ulong Factorial(uint number)
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
        public static bool Fermat(ulong number, int chance)
        {
            ulong a;
            int i;

            // Shortcut for even numbers
            if (number % 2 == 0)
            {
                return false;
            }
            for (i = 0; i < chance; i++)
            {
                a = generateRandomNumber(number);
                while (getGreatestCommonDivisor(Convert.ToUInt64(number), Convert.ToUInt64(a)) != 1)
                {
                    a = generateRandomNumber(number);
                }
                // Fermat theorem
                if ((a ^ (number - 1) % number) == 1)
                {
                    break;
                }
            }

            return i == chance;
        }

        /**
         * Generate 64bit unsigned integer value
         */
        private static ulong generateRandomNumber(ulong max, ulong min = 3)
        {
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
        public static bool SolovayStrassen(ulong number, int chance)
        {
            ulong a;
            ulong gcd;
            int i;
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
