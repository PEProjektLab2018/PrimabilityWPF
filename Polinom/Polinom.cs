using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;

namespace ClsPolinom
{
    public class Polinom 
    {

        List<Monom> list = new List<Monom>();

        public List<Monom> List { get => list; }

        public Polinom() { this.list = new List<Monom>(); }

        public Polinom(List<Monom> L) {
            this.list = L;
        }

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

      //  IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }


        public Enumerator GetEnumerator() { return new Enumerator(); }
    

    

        public Polinom add(Monom monom) {
            Predicate<Monom> predicate = (mon => mon.Variable == monom.Variable && mon.Exponent==monom.Exponent);
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

                foreach (Monom m1 in Polinom1.List) {
                    added = false;
                    foreach (Monom m2 in Polinom2.List) {
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

            foreach (Monom m1 in Polinom1.List)
            {
                substracted = false;
                foreach (Monom m2 in Polinom2.List)
                {
                    if (m1.Variable == m2.Variable && m1.Exponent == m2.Exponent)
                    {
                        if (m1.Coefficient != m2.Coefficient) { 
                            pol.add(new Monom(m1.Coefficient - m2.Coefficient, m1.Variable, m1.Exponent));
                        }
                        substracted = true;
                    }

                }
                if (substracted == false)
                {
                    pol.add(m1);
                }
            }

            foreach (Monom m2 in Polinom2.List)
            {
                found = false;
                foreach (Monom m1 in Polinom1.List)
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
            foreach (Monom m1 in Polinom1.List)
            {
                foreach (Monom m2 in Polinom2.List)
                {
                    if (m1.Variable == m2.Variable) {
                        pol.add(new Monom(m1.Coefficient * m2.Coefficient, m1.Variable, m1.Exponent + m2.Exponent));
                    } else

                    if (m2.Variable == "" && m1.Variable=="")
                    {
                        pol.add(new Monom(m1.Coefficient * m2.Coefficient));
                    } else
                    //m2 is constant
                    if (m2.Variable == "") {
                        pol.add(new Monom(m1.Coefficient * m2.Coefficient, m1.Variable, m1.Exponent));
                    } else
                    //m1 is constant
                    if (m1.Variable == "")
                    {
                        pol.add(new Monom(m1.Coefficient * m2.Coefficient, m2.Variable, m2.Exponent));
                    }
                }

            }
            return pol;
        }

        public static Polinom operator / (Polinom Polinom1, Polinom Polinom2)
        {
            Polinom dividend = new Polinom((Sorter(Polinom1)).List);
            Polinom divisor = new Polinom(Sorter(Polinom2).List);
            Polinom tempPolinom = new Polinom();
            Monom maxDivisor=new Monom();
            Monom maxDividend = new Monom();
            Monom quotient = new Monom();
            //divisor's monom with biggest exponent
            
            while (true) { 
            foreach (Monom m in divisor.List) {
                maxDivisor = m;
                break;
            }

            //dividened's monom with biggest exponent
            foreach (Monom m in dividend.List)
            {
                maxDividend = m;
                break;
            }

                if (maxDivisor.Exponent < maxDividend.Exponent)
                {

                    //1. divide monoms

                    quotient.Coefficient = maxDividend.Coefficient / maxDivisor.Coefficient;
                    quotient.Variable = maxDividend.Variable;
                    quotient.Exponent = maxDividend.Exponent - maxDivisor.Exponent;

                    foreach (Monom multiple in divisor.List)
                    {
                        tempPolinom.add(new Monom(multiple.Coefficient * quotient.Coefficient, multiple.Variable, multiple.Exponent + maxDivisor.Exponent));
                    }

                    dividend = dividend - tempPolinom;
                }
                else { break; }
            }
            //throw new NotImplementedException();
            
            return dividend;
        }



        public static Polinom operator % (Polinom Polinom1, Polinom Polinom2)
        {
            throw new NotImplementedException();
        }

        public static Polinom[] getIrreducible(int ElementNumber, int power, int modulo) {

            throw new NotImplementedException();

        }

        public static Polinom calcPolinomToZp(Polinom input, int p) {
            Polinom retPolinom = new Polinom();

            foreach (Monom m in input.List) {
                retPolinom.add(new Monom(m.Coefficient % p,m.Variable, m.Exponent));
            }

            return retPolinom;
        }


        public override string ToString()
        {
            //return base.ToString();
            string ret = "";
            int i = 0;
            foreach (Monom m in list) {
               
                if (m.Coefficient != 0)
                {
                    if (i > 0 && m.Coefficient > 0) { ret += "+"; }
                    if (m.Exponent == 0) { ret += m.Coefficient; }
                    else
                    {
                        ret += m.Coefficient;
                        if (m.Exponent == 1)
                        {
                            ret += m.Variable;
                        }
                        else
                        {
                            ret += m.Variable + "^" + m.Exponent;
                        }

                    }


                }
               
                i++;
                
            }
            return ret.TrimEnd(new Char[] { ' ', '+' });
        }

        public static Polinom PolinomFromString(string input) {
            Monom m;
            Regex regex ;
            Match match;
            Polinom retPolinom = new Polinom();
           
            //ulong intResult;
            string[] arrinput = input.Split('-', '+');
            for (int i = 0; i < arrinput.Count(); i++) {
                m = new Monom();
                /*
              five kind of monom could be:
                 1. only coefficient                                              12
                 2. coefficient multiplied by first power of variable              6x
                 3. only first power of variable (without coefficient)              x
                 4. coefficient multiplied by variable powered more than one       8x^2
                 5. only variable powered more than one (without coefficient)       x^2
                 

              */
                // 1. 

                //First, check whether monom has a variable

                if (Regex.IsMatch(arrinput[i], @"[A-Za-z]"))
                {
                    m.Variable = Regex.Match(arrinput[i], @"[A-Za-z]").Value;
                    regex = new Regex(@"([1-9]+)");
                    //get coefficient, if exists
                    if (Regex.IsMatch(arrinput[i], @"^([1-9]+)[A-Za-z]"))
                    {

                        match = regex.Match(arrinput[i]);
                        m.Coefficient = Convert.ToInt64(match.Value);
                        match = match.NextMatch();
                        if (match.Success)
                        {
                            m.Exponent = Convert.ToUInt64(match.Value);
                        }
                        else
                        {
                            m.Exponent = 1;
                        }
                    }
                    else {
                        m.Coefficient = 1;
                        match = regex.Match(arrinput[i]);
                        if (match.Success)
                        {
                            m.Exponent = Convert.ToUInt64(match.Value);
                        }
                        else m.Exponent = 1;
                    }

                }
                else {
                    m.Coefficient = Convert.ToInt64(arrinput[i]);
                    m.Variable = "x";
                    m.Exponent = 0;

                }

                retPolinom.add(m);

                }
            return retPolinom;
            }

        public static Polinom Sorter(Polinom unsorted) {
            List<Monom> sorted = unsorted.List;
            sorted.Sort(delegate (Monom m1, Monom m2)
            {
                return m2.Exponent.CompareTo(m1.Exponent);
            });
            return new Polinom(sorted);
        }
        }
    /*

    private static Monom MaxMonom(Polinom pol) {
        Monom retMonom=new Monom();
        if (pol.List.Count>0) { 
        foreach (Monom m in pol.List) {
                retMonom = m;
                break;
        }
        }
        return retMonom;
    }
    */
}
