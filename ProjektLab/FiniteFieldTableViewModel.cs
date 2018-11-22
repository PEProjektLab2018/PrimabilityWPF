using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClsPolinom;
using System.Collections.ObjectModel;

namespace ProjektLab
{
    public class FiniteFieldTableViewModel
    {
        public class Row
        {
            public List<ClsPolinom.Polinom> Polinoms { get; set; }
            public ClsPolinom.Polinom Label { get; set; }

            public Row(int count)
            {
                Polinoms = new List<ClsPolinom.Polinom>(count);
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
    }
}
