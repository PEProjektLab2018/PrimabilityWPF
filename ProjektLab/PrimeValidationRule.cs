using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Prime;

namespace ProjektLab
{
    public class PrimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ulong parsed;
            if (value.ToString() != "" && ulong.TryParse(value.ToString(), out parsed) && Tests.Naive(parsed))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Kérlek prímszámot adj meg!");
        }
    }
}
