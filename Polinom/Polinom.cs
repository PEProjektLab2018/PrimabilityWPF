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

        public Polinom(List<Monom> L)
        {
            this.list = L;
        }
        public Polinom(Polinom P)
        {
            this.list = P.list;
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

        public Polinom add(Monom monom)
        {
            Predicate<Monom> predicate = (mon => mon.Variable == monom.Variable && mon.Exponent == monom.Exponent);
            if (this.list.Exists(predicate))
            {
                Monom m = this.list.Find(predicate);
                m.Coefficient += monom.Coefficient;
            }
            else
            {
                this.list.Add(monom);

            }
            return this;
        }

        private static void divider(Polinom Polinom1, Polinom Polinom2, ref Polinom quotient, ref Polinom remainder)
        {
            Polinom dividend = new Polinom((Sorter(Polinom1)).List);
            Polinom divisor = new Polinom(Sorter(Polinom2).List);
            Polinom tempPolinom;

            Monom maxDivisor = new Monom();
            Monom maxDividend = new Monom();
            Monom quotientMonom;
            quotient = new Polinom();
            //divisor's monom with biggest exponent

            while (true)
            {
                foreach (Monom m in divisor.List)
                {
                    maxDivisor = m;
                    break;
                }

                //dividened's monom with biggest exponent
                foreach (Monom m in dividend.List)
                {
                    maxDividend = m;
                    break;
                }

                if (maxDivisor.Exponent <= maxDividend.Exponent)
                {

                    //1. dividing monoms
                    quotientMonom = new Monom(maxDividend.Coefficient / maxDivisor.Coefficient, maxDividend.Variable, maxDividend.Exponent - maxDivisor.Exponent);

                    tempPolinom = new Polinom();
                    //2. multiplying back 
                    foreach (Monom multiple in divisor.List)
                    {

                        tempPolinom.add(new Monom(multiple.Coefficient * quotientMonom.Coefficient, multiple.Variable, multiple.Exponent + quotientMonom.Exponent));
                    }
                    quotient.add(quotientMonom);
                    //3. substracting multiplied polinom from dividend
                    dividend = Sorter(dividend - tempPolinom);
                }
                else { break; }
            }
            remainder = dividend;
        }


        public static Polinom operator +(Polinom Polinom1, Polinom Polinom2)
        {
            Polinom pol = new Polinom();

            var newMonomList = new List<Monom>();

            newMonomList.AddRange(Polinom1.List);
            newMonomList.AddRange(Polinom2.List);

            foreach (Monom m in newMonomList)
            {
                pol.add(m);
            }

            return Sorter(pol);
        }

        public static Polinom operator -(Polinom Polinom1, Polinom Polinom2)
        {
            Polinom Polinom2Neg = new Polinom();

            foreach(Monom m in Polinom2)
            {
                Polinom2Neg.add(new Monom((-1) * m.Coefficient, m.Exponent));
            }

            return Polinom1 + Polinom2Neg;
        }

        public static Polinom operator *(Polinom Polinom1, Polinom Polinom2)
        {
            Polinom pol = new Polinom();
            foreach (Monom m1 in Polinom1.List)
            {
                foreach (Monom m2 in Polinom2.List)
                {
                    if (m1.Variable == m2.Variable)
                    {
                        pol.add(new Monom(m1.Coefficient * m2.Coefficient, m1.Variable, m1.Exponent + m2.Exponent));
                    }
                    else

                    if (m2.Variable == "" && m1.Variable == "")
                    {
                        pol.add(new Monom(m1.Coefficient * m2.Coefficient));
                    }
                    else
                    //m2 is constant
                    if (m2.Variable == "")
                    {
                        pol.add(new Monom(m1.Coefficient * m2.Coefficient, m1.Variable, m1.Exponent));
                    }
                    else
                    //m1 is constant
                    if (m1.Variable == "")
                    {
                        pol.add(new Monom(m1.Coefficient * m2.Coefficient, m2.Variable, m2.Exponent));
                    }
                }

            }
            return pol;
        }

        public static Polinom operator /(Polinom Polinom1, Polinom Polinom2)
        {
            Polinom quotient = new Polinom();
            Polinom remainder = new Polinom();
            divider(Polinom1, Polinom2, ref quotient, ref remainder);
            return quotient;
        }

        public static Polinom operator %(Polinom Polinom1, Polinom Polinom2)
        {
            Polinom quotient = new Polinom();
            Polinom remainder = new Polinom();
            divider(Polinom1, Polinom2, ref quotient, ref remainder);
            return remainder;
        }

        private static int biggestPower(Polinom polinom)
        {
            polinom = Sorter(polinom);
            int exponent = 0;
            foreach (Monom m in polinom.list)
            {
                exponent = (int)m.Exponent;
                break;
            }
            return exponent;
        }

        public static Polinom Pow(Polinom polinom, int power)
        {
            Polinom retPolinom = new Polinom();
            retPolinom.add(new Monom(1));

            for (int i = 1; i <= power; i++)
            {
                retPolinom = retPolinom * polinom;
            }

            return retPolinom;
        }



        public static List<Polinom> getIrreducible(int power, int p)
        {
            List<Polinom> IrreduciblePolinoms = new List<Polinom>();
            List<Polinom> testPolinoms = new List<Polinom>(); ;
            List<Polinom> multiplyPolinoms = new List<Polinom>(); ;
            List<Polinom> candidatePolinoms;
            Polinom PoweredPolinom = new Polinom();
            bool isIrreducible;

            for (int pow = 1; pow <= power; pow++)
            {
                candidatePolinoms = new List<Polinom>();
                candidatePolinoms = GeneratePolinoms(pow, p);
                if (pow == 1)
                {
                    foreach (Polinom pol in candidatePolinoms)
                    {
                        if (MaxMonom(pol).Coefficient == 1)
                        {
                            IrreduciblePolinoms.Add(pol);
                            testPolinoms.Add(pol);
                        }
                    }
                }
                else
                {
                    // testPolinoms = new List<Polinom>();
                    foreach (Polinom poli in IrreduciblePolinoms)
                    {
                        for (int i = 1; i <= pow; i++)
                        {
                            if (biggestPower(calcPolinomToZp(Pow(poli, i), p)) <= pow)
                            {
                                testPolinoms.Add(calcPolinomToZp(Pow(poli, i), p));
                            }
                        }
                    }


                    for (int start = 0; start < testPolinoms.Count - 1; start++)
                    {
                        for (int end = start + 1; end < testPolinoms.Count; end++)
                        {
                            if (biggestPower(calcPolinomToZp(testPolinoms[start] * testPolinoms[end], p)) <= pow)
                            {

                                multiplyPolinoms.Add(calcPolinomToZp(testPolinoms[start] * testPolinoms[end], p));
                            }
                        }

                    }

                    foreach (Polinom m in multiplyPolinoms)
                    {
                        if (biggestPower(m) <= pow)
                        {
                            testPolinoms.Add(m);
                        }
                    }

                    foreach (Polinom candidate in candidatePolinoms)
                    {
                        isIrreducible = true;

                        foreach (Polinom tp in testPolinoms)
                        {
                            if (candidate.ToString() == tp.ToString())
                            {
                                isIrreducible = false;
                                break;
                            }
                        }
                        if (isIrreducible == true)
                        {
                            IrreduciblePolinoms.Add(candidate);
                            testPolinoms.Add(candidate);
                        }

                    }

                }
            }
            return IrreduciblePolinoms;

        }


        public static List<Polinom> GeneratePolinoms(int power, int p)
        {
            List<Polinom> Polinoms = new List<Polinom>();

            int calcValue = (int)Math.Pow(p, power);
            int tempValue;
            List<Monom> Monoms;
            Polinom Pol;
            //Generate Coefficient Matrix

            int[,] coefMatrix = new int[(int)Math.Pow(p, power) * (p - 1), (power + 1)];
            for (int row = 0; row < (int)Math.Pow(p, power) * (p - 1); row++)
            {
                tempValue = calcValue;
                for (int column = power; column >= 0; column--)
                {
                    if (column > 0)
                    {
                        coefMatrix[row, column] = tempValue / ((int)Math.Pow(p, column));
                    }
                    else
                    {
                        coefMatrix[row, column] = tempValue;
                    }
                    tempValue = tempValue - (coefMatrix[row, column] * ((int)Math.Pow(p, column)));
                }
                calcValue++;

            }

            //Generate polinoms using Coefficient Matrix

            for (int row = 0; row < (int)Math.Pow(p, power) * (p - 1); row++)
            {
                Monoms = new List<Monom>();
                for (int column = power; column >= 0; column--)
                {

                    Monoms.Add(new Monom(coefMatrix[row, column], "x", (ulong)column));
                }
                Pol = new Polinom(Monoms);
                if (MaxMonom(Pol).Coefficient == 1) { Polinoms.Add(Pol); }
            }
            return Polinoms;

            //throw new NotImplementedException();

        }


        public static Polinom calcPolinomToZp(Polinom input, int p)
        {
            Polinom retPolinom = new Polinom();

            foreach (Monom m in input.List)
            {
                if (m.Coefficient > 0)
                {
                    retPolinom.add(new Monom(m.Coefficient % p, m.Variable, m.Exponent));
                }
                else if (m.Coefficient < 0)
                {

                    retPolinom.add(new Monom((m.Coefficient % p) + p, m.Variable, m.Exponent));
                }

            }

            return retPolinom;
        }


        public override string ToString()
        {
            //return base.ToString();
            string ret = "";
            int i = 0;
            foreach (Monom m in list)
            {
                if (m.Coefficient != 0)
                {
                    if (i > 0 && m.Coefficient > 0) { ret += "+"; }
                    if (m.Exponent == 0) { ret += m.Coefficient; }
                    else
                    {
                        if (m.Coefficient > 1 || m.Coefficient < -1) { ret += m.Coefficient; }
                        if (m.Coefficient == -1) { ret += "-"; }
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
            if (ret == "")
            {
                ret = "0";
            }
            return ret.Trim(new Char[] { ' ', '+' });
        }

        public static Polinom PolinomFromString(string input)
        {
            Monom m;
            Regex regex;
            Match match;
            Polinom retPolinom = new Polinom();

            //ulong intResult;
            string[] arrinput = input.Split('-', '+');
            for (int i = 0; i < arrinput.Count(); i++)
            {
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
                    else
                    {
                        m.Coefficient = 1;
                        // get exponent
                        match = regex.Match(arrinput[i]);
                        if (match.Success)
                        {
                            m.Exponent = Convert.ToUInt64(match.Value);
                        }
                        else m.Exponent = 1;
                    }

                }
                else
                {
                    m.Coefficient = Convert.ToInt64(arrinput[i]);
                    m.Variable = "x";
                    m.Exponent = 0;

                }

                retPolinom.add(m);

            }
            return retPolinom;
        }

        public static Polinom Sorter(Polinom unsorted)
        {
            List<Monom> sorted = unsorted.List;
            sorted.Sort(delegate (Monom m1, Monom m2)
            {
                return m2.Exponent.CompareTo(m1.Exponent);
            });
            return new Polinom(sorted);
        }

        public static Monom MaxMonom(Polinom pol)
        {
            pol = Sorter(pol);
            Monom retMonom = new Monom();
            if (pol.List.Count > 0)
            {
                foreach (Monom m in pol.List)
                {
                    retMonom = m;
                    break;
                }
            }
            return retMonom;
        }
    }




}
