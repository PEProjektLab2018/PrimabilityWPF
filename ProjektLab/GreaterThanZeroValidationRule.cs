using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjektLab
{
    public class GreaterThanZeroValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            uint test;

            if (uint.TryParse(value.ToString(), out test) && test == 0)
            {
                return new ValidationResult(false, "Kérlek 0-nál nagyobb érvényes számot adj meg!");
            }
            return ValidationResult.ValidResult;
        }
    }
}
