using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClsPolinom
{
    public class Monom
    {
        public Monom() { }

        public Monom(long Coefficient, string Variable, ulong Exponent)
        {
            this.Coefficient = Coefficient;
            this.Exponent = Exponent;
            this.Variable = Variable;
        }

        //one variable polinom
        public Monom(long Coefficient, ulong Exponent)
        {
            this.Coefficient = Coefficient;
            this.Exponent = Exponent;
            this.Variable = "x";
        }

        //Constant polinom 
        public Monom(long Coefficient)
        {
            this.Coefficient = Coefficient;
            this.Exponent = 0;
            this.Variable = "x";
        }

        public long Coefficient { get; set; }
        public string Variable { get; set; }
        public ulong Exponent { get; set; }
    }
}
