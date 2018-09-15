using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjektLab
{
    public class StringToUintValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            uint test;

            if(value.ToString() == "" || uint.TryParse(value.ToString(), out test))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Kérlek érvényes számot adj meg!");
        }
    }
}
