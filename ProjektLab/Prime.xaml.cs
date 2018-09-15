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
using System.Diagnostics;

namespace ProjektLab
{
    /// <summary>
    /// Interaction logic for Prime.xaml
    /// </summary>
    public partial class Prime : UserControl
    {
        public MyNumber MyNumber { get; set; }
        public uint Chance { get; set; }
        private static FontAwesome.WPF.FontAwesome SpinningIcon;
        public Prime()
        {
            SpinningIcon = new FontAwesome.WPF.FontAwesome();
            SpinningIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Spinner;
            SpinningIcon.Spin = true;
            InitializeComponent();

            MyNumber = new MyNumber();
            Chance = 10;

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Erastothenes.Content = SpinningIcon;
            Fermat.Content = SpinningIcon;
            SolovayStrassen.Content = SpinningIcon;
            MillerRabin.Content = SpinningIcon;

            Factors Factors = MyNumber.FactorizeNumber();

            TextBlock FactorResultTextBlock = new TextBlock();

            for (int i = 0; i < Factors.List.Count(); i++)
            {
                if (i > 0)
                {
                    FactorResultTextBlock.Inlines.Add(" * ");
                }
                Power Power = Factors.List[i];
                // Add mantissa
                FactorResultTextBlock.Inlines.Add(Convert.ToString(Power.Mantissa));
                // Is exponent is greater than 1, add superscript mantissa
                if (Power.Exponent > 1)
                {
                    FactorResultTextBlock.Inlines.Add(new Run(Convert.ToString(Power.Exponent))
                    {
                        BaselineAlignment = BaselineAlignment.Superscript,
                        FontSize = 10
                    });
                }
            }

            FactorsResult.Content = FactorResultTextBlock;


            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool ErastothenesTest = Tests.Erastothenes(MyNumber.LocalNumber);
            sw.Stop();
            Erastothenes.Content = (ErastothenesTest ? "Prím" : "Nem Prím") + " Számítási idő: " + sw.Elapsed;

            sw.Reset();
            sw.Start();
            bool FermatTest = Tests.Fermat(MyNumber.LocalNumber, Chance);
            sw.Stop();
            Fermat.Content = (FermatTest ? "Prím" : "Nem Prím") + " Számítási idő: " + sw.Elapsed;

            sw.Reset();
            sw.Start();
            bool SolovayStrassenTest = Tests.SolovayStrassen(MyNumber.LocalNumber, Chance);
            sw.Stop();
            SolovayStrassen.Content = (SolovayStrassenTest ? "Prím" : "Nem Prím") + " Számítási idő: " + sw.Elapsed;

            sw.Reset();
            sw.Start();
            bool MillerRabinTest = Tests.MillerRabin(MyNumber.LocalNumber);
            sw.Stop();
            MillerRabin.Content = (SolovayStrassenTest ? "Prím" : "Nem Prím") + " Számítási idő: " + sw.Elapsed;
            sw.Reset();
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
