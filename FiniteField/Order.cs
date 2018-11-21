using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteFieldLibrary
{
    public class Order
    {
        public int Mantissa { get; set; }
        public int Exponent { get; set; }

        public Order()
        {

        }

        public Order(int mantissa, int exponent)
        {
            this.Mantissa = mantissa;
            this.Exponent = exponent;
        }
    }
}
