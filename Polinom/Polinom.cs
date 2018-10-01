using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polinom
{
    class Polinom
    {

        public class Enumerator
        {
            public Monom Current { get; private set; }
            public bool MoveNext()
            {
                if (this.Current == null)
                {
                    this.Current = new Monom();
                    return true;
                }
                this.Current = null;
                return false;
            }
            public void Reset() { this.Current = null; }
        }

        public Enumerator GetEnumerator() { return new Enumerator(); }
    

    List<Monom> list = new List<Monom>();

        public List<Monom> List { get => list; }

        public Polinom() { this.list = new List<Monom>(); }

        public Polinom add(Monom monom) {
            Predicate<Monom> predicate = (mon => mon.Variable == monom.Variable && mon.Variable==monom.Variable);
            if (this.list.Exists(predicate))
            {
                Monom m = this.list.Find(predicate);
                m.Coefficient += monom.Coefficient;
            }
            else {
                this.list.Add(monom);

            }
            return this;
        }

        



        public static Polinom operator + (Polinom Polinom1, Polinom Polinom2) {
            Polinom pol = new Polinom();
            bool added, found;

            foreach (Monom m1 in Polinom1) {
                added = false;
                foreach (Monom m2 in Polinom2) {
                    if (m1.Variable == m2.Variable && m1.Exponent == m2.Exponent) {
                        pol.add(new Monom(m1.Coefficient + m2.Coefficient, m1.Variable, m1.Exponent));
                        added = true;
                    }

                }
                if (added == false) {
                    pol.add(m1);
                }
            }

            foreach (Monom m2 in Polinom2) {
                found = false;
                foreach (Monom m1 in Polinom1) {
                    if (m2.Variable == m1.Variable && m2.Exponent == m1.Exponent) {
                        found = true;
                    }
                }
                if (found == false) {
                    pol.add(m2);
                }

            }

            
            return pol;
        }

        public static Polinom operator - (Polinom Polinom1, Polinom Polinom2) {
            Polinom pol=new Polinom();

            bool substracted, found;

            foreach (Monom m1 in Polinom1)
            {
                substracted = false;
                foreach (Monom m2 in Polinom2)
                {
                    if (m1.Variable == m2.Variable && m1.Exponent == m2.Exponent)
                    {
                        pol.add(new Monom(m1.Coefficient - m2.Coefficient, m1.Variable, m1.Exponent));
                        substracted = true;
                    }

                }
                if (substracted == false)
                {
                    pol.add(m1);
                }
            }

            foreach (Monom m2 in Polinom2)
            {
                found = false;
                foreach (Monom m1 in Polinom1)
                {
                    if (m2.Variable == m1.Variable && m2.Exponent == m1.Exponent)
                    {
                        found = true;
                    }
                }
                if (found == false)
                {
                    pol.add(new Monom((m2.Coefficient)*(-1),m2.Variable,m2.Exponent));
                }

            }
            return pol;
        }

        public static Polinom operator * (Polinom Polinom1, Polinom Polinom2) {
            Polinom pol=new Polinom();
            foreach (Monom m1 in Polinom1)
            {
                foreach (Monom m2 in Polinom2)
                {
                    if (m1.Variable == m2.Variable) {
                        pol.add(new Monom(m1.Coefficient * m2.Coefficient, m1.Variable, m1.Exponent + m2.Exponent));
                    }
                }
                
            }
            return pol;
        }

        public static Polinom operator / (Polinom Polinom1, Polinom Polinom2)
        {
            throw new NotImplementedException();
        }

        public static Polinom operator % (Polinom Polinom1, Polinom Polinom2)
        {
            throw new NotImplementedException();
        }

        public static Polinom[] getIrreducible(int ElementNumber, int power) {

            throw new NotImplementedException();

        }

        public override string ToString()
        {
            //return base.ToString();
            string ret = "";

            foreach (Monom m in list) {

                if (m.Coefficient > 1) { ret += m.Coefficient; }
                if (m.Exponent != 0) {
                    if (m.Exponent == 1)
                    {
                        ret += m.Variable;
                    }
                    else {
                        ret += m.Variable+"^"+m.Exponent;
                    }

                }
                ret += " + ";
            }
            return ret.TrimEnd(new Char[] { ' ', '+' });
        }

    }
}
