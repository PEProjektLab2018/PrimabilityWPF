using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClsPolinom
{
    public class Monom
    {
        private long _coefficient;
        private string _variable;
        private ulong _exponent;


        public Monom() { }

        public Monom(long Coefficient, string Variable, ulong Exponent) {
            this._coefficient = Coefficient;
            this._exponent = Exponent;
            this._variable = Variable;
        }

        //one variable polinom
        public Monom(long Coefficient, ulong Exponent)
        {
            this._coefficient = Coefficient;
            this._exponent = Exponent;
            this._variable = "x";
        }

        //Constant polinom 
        public Monom(long Coefficient) {
            this._coefficient = Coefficient;
            this.Exponent = 0;
            this.Variable = "x";
        }

        public long Coefficient { get => _coefficient; set => _coefficient = value; }
        public string Variable { get => _variable; set => _variable = value; }
        public ulong Exponent { get => _exponent; set => _exponent = value; }
    }
}
