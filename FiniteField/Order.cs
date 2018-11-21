using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteFieldLibrary
{
    public class Order
    {
        public uint Mantissa { get; set; }
        public uint Exponent { get; set; }

        public Order()
        {

        }

        public Order(uint mantissa, uint exponent)
        {
            this.Mantissa = mantissa;
            this.Exponent = exponent;
        }
    }
}
