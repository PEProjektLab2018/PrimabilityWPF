using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prime;

namespace ProjektLab
{
    /// <summary>
    /// Interaction logic for Prime.xaml
    /// </summary>
    public partial class Prime : UserControl
    {
        public MyNumber MyNumber { get; set; }
        public Prime()
        {
            InitializeComponent();

            MyNumber = new MyNumber();

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Factors Factors = MyNumber.FactorizeNumber();

            InlineCollection ResultInlines = FactorsResult.Inlines;
            ResultInlines.Clear();

            for (int i = 0; i < Factors.List.Count(); i++)
            {
                if (i > 0)
                {
                    ResultInlines.Add(" * ");
                }
                Power Power = Factors.List[i];
                // Add mantissa
                ResultInlines.Add(Convert.ToString(Power.Mantissa));
                // Is exponent is greater than 1, add superscript mantissa
                if (Power.Exponent > 1)
                {
                    ResultInlines.Add(new Run(Convert.ToString(Power.Exponent))
                    {
                        BaselineAlignment = BaselineAlignment.Superscript,
                        FontSize = 10
                    });
                }
            }
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (Validation.GetHasError(NumberInput))
            {
                ((Control)sender).ToolTip = e.Error.ErrorContent.ToString();
                Button.IsEnabled = false;
            }
            else
            {
                ((Control)sender).ToolTip = "";
                Button.IsEnabled = true;
            }
        }
    }
}
