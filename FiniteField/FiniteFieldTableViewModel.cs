﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClsPolinom;

namespace FiniteFieldLibrary
{
    public class FiniteFieldTableViewModel
    {
        public class Row
        {
            public List<Polinom> Polinoms { get; set; }
            public Polinom Label { get; set; }

            public Row(int count)
            {
                Polinoms = new List<Polinom>(count);
                for (int i = 0; i < count; i++)
                {
                    Polinoms.Add(null);
                }
            }

            public override string ToString()
            {
                string res = Label.ToString();

                foreach (Polinom pol in Polinoms)
                {
                    res += ";" + pol.ToString();
                }

                return res.Trim(new Char[] { ';' });
            }
        }

        public List<Row> Rows { get; set; }

        public FiniteFieldTableViewModel()
        {
            Rows = new List<Row>();
        }

        public void createRow(int count)
        {
            Rows.Add(new Row(count));
        }

        public void Clear()
        {
            Rows.Clear();
        }

        public List<string> GetLabels()
        {
            List<string> res = new List<string>(Rows.Count());

            foreach (Row row in Rows)
            {
                res.Add(row.Label.ToString());
            }

            return res;
        }

        public List<Polinom> GetLabelPolinoms()
        {
            List<Polinom> res = new List<Polinom>(Rows.Count());

            foreach (Row row in Rows)
            {
                res.Add(row.Label);
            }

            return res;
        }
    }
}
