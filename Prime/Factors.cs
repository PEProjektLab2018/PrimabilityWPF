using System;
using System.Collections.Generic;

namespace Prime
{
    public class Factors
    {
        private List<Power> list;

        public List<Power> List { get => list; }

        public Factors()
        {
            this.list = new List<Power>();
        }

        public Factors Add(uint mantissa)
        {
            // Find Power by mantissa - lambda function
            Predicate<Power> predicate = (el => el.Mantissa == mantissa);

            if (this.list.Exists(predicate))
            {
                Power power = this.list.Find(predicate);
                power.Exponent += 1;
            } else
            {
                this.list.Add(new Power(mantissa, 1));
            }

            return this;
        }

        public override String ToString()
        {
            String result = "";

            foreach(Power element in list)
            {
                result += element.Mantissa + "^" + element.Exponent + " + ";
            }

            return result.TrimEnd(new Char[] { ' ', '+' });
        }
    }
}
