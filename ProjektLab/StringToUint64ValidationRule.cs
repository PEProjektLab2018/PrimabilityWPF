using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjektLab
{
    public class StringToUint64ValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ulong test;

            if(value.ToString() == "" || ulong.TryParse(value.ToString(), out test))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Kérlek érvényes számot adj meg!");
        }
    }
}
