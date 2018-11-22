using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using ClsPolinom;
using System.Windows.Documents;
using System.Windows;

namespace ProjektLab
{
    public class PolinomToXamlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ClsPolinom.Polinom)
            {
                TextBlock tbkPolinom = new TextBlock();

                int i = 0;
                foreach (ClsPolinom.Monom m in ((ClsPolinom.Polinom)value).List)
                {
                    if (m.Coefficient != 0)
                    {
                        if (i > 0) { tbkPolinom.Inlines.Add("+"); }

                        if ((m.Exponent == 0 && m.Coefficient == 1) || m.Coefficient > 1)
                        {
                            tbkPolinom.Inlines.Add(m.Coefficient.ToString());
                        }

                        if (m.Exponent != 0)
                        {
                            tbkPolinom.Inlines.Add(m.Variable);
                            if (m.Exponent > 1)
                            {
                                Run run = new Run();
                                run.Text = m.Exponent.ToString();
                                run.BaselineAlignment = BaselineAlignment.Superscript;
                                tbkPolinom.Inlines.Add(run);

                            }
                        }
                        i++;
                    }
                }
                if (i == 0)
                {
                    tbkPolinom.Inlines.Add("0");
                }
                return tbkPolinom;
            }

            return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
