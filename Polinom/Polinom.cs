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
                        pol.add(new Monom(m1.Coefficient - m2.Coefficient, m1.Variable, m1.Exponent));
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

                if (m.Coefficient > 1 || (m.Coefficient==1 && m.Exponent==0)) { ret += m.Coefficient; }
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
                    m.Variable = "";
                    m.Exponent = 0;

                }

                retPolinom.add(m);

                }
            return retPolinom;
            }


        }
    }
