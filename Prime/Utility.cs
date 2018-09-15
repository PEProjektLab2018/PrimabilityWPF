using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime
{
    public class Utility
    {
        /**
         * Using Euklides algorithm - get gcd
         */
        public static uint getGreatestCommonDivisor(uint number1, uint number2)
        {
            uint gcd;

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
    }
}
