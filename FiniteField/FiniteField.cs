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


    }
}
