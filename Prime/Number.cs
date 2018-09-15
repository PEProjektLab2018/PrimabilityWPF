using System;

namespace Prime
{
    public class Number
    {
        private ulong _number;

        public ulong LocalNumber { get => _number; set => _number = value; }

        public Number()
        {
        }

        public Number(uint number)
        {
            _number = number;
        }

        public Factors FactorizeNumber()
        {
            // Make a copy of the stored number, and use it further
            ulong number = _number;
            Factors factors = new Factors();
            while (number % 2 == 0)
            {
                factors.Add(2);
                number /= 2;
            }

            for (uint i = 3; i <= Math.Sqrt(number); i += 2)
            {
                while (number % i == 0)
                {
                    factors.Add(i);
                    number /= i;
                }

            }

            // If the remaining number is greater than 2, then it is a prime number.
            if (number > 2)
            {
                factors.Add(number);
            }
            return factors;
        }
    }
}
