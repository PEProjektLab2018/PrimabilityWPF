using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClsPolinom;

namespace FiniteFieldLibrary
{
    public class FiniteField
    {
        public static List<Polinom> generateMembers(Order order)
        {
            return generateMembers(order.Exponent, order.Mantissa);
        }
        public static List<Polinom> generateMembers(int power, int p)
        {
            List<Polinom> Polinoms = new List<Polinom>();
            
            //Generate Coefficient Matrix
            int[,] coefMatrix = new int[(int)Math.Pow(p, power), power];
            int counter = 0;
            for (int row = 0; row < (int)Math.Pow(p, power); row++)
            {
                for (int column = 0; column < power; column++)
                {
                    if (column == power - 1)
                    {
                        coefMatrix[row, column] = counter % p;
                    } else
                    {
                        coefMatrix[row, column] = (counter % (int)Math.Pow(p, power - column)) / (int)Math.Pow(p, power - column - 1);
                    }

                }
                counter++;
            }

            //Generate polinoms using Coefficient Matrix
            for (int row = 0; row < (int)Math.Pow(p, power); row++)
            {
                List<Monom> Monoms = new List<Monom>();
                for (int column = 0; column < power; column++)
                {
                    if (coefMatrix[row, column] > 0)
                    {
                        Monoms.Add(new Monom(coefMatrix[row, column], "x", (ulong)(power - column - 1)));
                    }
                }
                Polinom Pol = new Polinom(Monoms);
                Polinoms.Add(Pol);
            }
            return Polinoms;
        }

        public static List<Polinom> generateIrreduciblePolinoms(Order order)
        {
            return generateIrreduciblePolinoms(order.Exponent, order.Mantissa);
        }

        public static List<Polinom> generateIrreduciblePolinoms(int power, int p)
        {
            List<Polinom> tmpList = Polinom.getIrreducible(power, p);

            List<Polinom> result = new List<Polinom>();

            // Use only Given exponent polinoms
            foreach (Polinom pol in tmpList)
            {
                if ((int)Polinom.Sorter(pol).List[0].Exponent == power)
                {
                    result.Add(pol);
                }
            }

            return result;
        }
    
        public static async void calculateTables(List<Polinom> columns, Order order, Polinom IrreduciblePolinom, Action<Polinom, int, int> summationCallback, Action<Polinom, int, int> multiplicationCallback, Action finalCallback)
        {
            int calcSize = (int)Math.Pow(order.Mantissa, order.Exponent);
            for (int i = 0; i < calcSize; i++)
            {
                Polinom rowPolinom = columns[i];
                for (int j = 0; j < calcSize; j++)
                {
                    Polinom colPolinom = columns[j];
                    Polinom summationRes = await Task.Run<Polinom>(() =>
                    {
                        return Polinom.calcPolinomToZp((rowPolinom + colPolinom) % IrreduciblePolinom, order.Mantissa);
                    });
                    summationCallback(summationRes, i, j);
                    Polinom multiplicationRes = await Task.Run<Polinom>(() =>
                    {
                        return Polinom.calcPolinomToZp((rowPolinom * colPolinom) % IrreduciblePolinom, order.Mantissa);
                    });
                    multiplicationCallback(summationRes, i, j);
                }
            }

            finalCallback();
        }
    }
}
