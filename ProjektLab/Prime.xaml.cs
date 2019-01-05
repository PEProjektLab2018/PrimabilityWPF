using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
        private List<Thread> ThreadList;

        public MyNumber MyNumber { get; set; }
        public Prime()
        {
            InitializeComponent();
            
            MyNumber = new MyNumber();
            ThreadList = new List<Thread>();

            DataContext = this;
        }

        private void ResetPrimeTests()
        {
            ErastothenesSpinner.Visibility = Visibility.Visible;
            FermatSpinner.Visibility = Visibility.Visible;
            SolovayStrassenSpinner.Visibility = Visibility.Visible;
            MillerRabinSpinner.Visibility = Visibility.Visible;
            NaiveSpinner.Visibility = Visibility.Visible;

            ErastothenesResult.Visibility = Visibility.Hidden;
            FermatResult.Visibility = Visibility.Hidden;
            SolovayStrassenResult.Visibility = Visibility.Hidden;
            MillerRabinResult.Visibility = Visibility.Hidden;
            NaiveResult.Visibility = Visibility.Hidden;
            ErastothenesResult.Text = "";
            FermatResult.Text = "";
            SolovayStrassenResult.Text = "";
            MillerRabinResult.Text = "";
            NaiveResult.Text = "";
            FactorsSpinner.Visibility = Visibility.Visible;
            FactorsResult.Visibility = Visibility.Hidden;
            FactorsResult.Content = "";
            PrimePowerResult.Text = "";
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button.IsEnabled = false;
            lock(ThreadList)
            {
                foreach (Thread Thread in ThreadList)
                {
                    Thread.Interrupt();
                }
                ThreadList.Clear();
            }
            ResetPrimeTests();

            RunTestErastothenes();
            RunTestFermat();
            RunTestSolovayStrassen();
            RunTestMillerRabin();
            RunTestNaive();
            

            RunFactorization();

            Button.IsEnabled = true;
        }

        private async void RunTestErastothenes()
        {
            // 47995852 * 2 - 1 - Magic number - odd numbers in dictionary
            if (MyNumber.LocalNumber > 47995853 * 2 - 1)
            {
                ErastothenesSpinner.Visibility = Visibility.Hidden;
                ErastothenesResult.Text = "A szám túl nagy. Teszt nem elvégezhető";
                ErastothenesResult.Visibility = Visibility.Visible;
                return;
            }
            Stopwatch sw = new Stopwatch();
            Thread Thread = null;
            sw.Start();
            bool Test = await Task.Run<bool>(() => 
            {
                ThreadList.Add(Thread.CurrentThread);
                return Tests.Erastothenes(MyNumber.LocalNumber);
            });
            sw.Stop();
            ErastothenesSpinner.Visibility = Visibility.Hidden;
            ErastothenesResult.Text = (Test ? "Prím" : "Nem Prím") + "\nSzámítási idő: " + sw.Elapsed;
            ErastothenesResult.Visibility = Visibility.Visible;
            ThreadList.Remove(Thread);
        }

        private async void RunTestFermat()
        {
            ulong Chance = 0;
            Stopwatch sw = new Stopwatch();
            Thread Thread = null;
            sw.Start();
            bool Test = await Task.Run<bool>(() => 
            {
                Chance = Tests.generateRandomNumber(MyNumber.LocalNumber - 1, 1);
                Thread = Thread.CurrentThread;
                ThreadList.Add(Thread);
                return Tests.Fermat(MyNumber.LocalNumber, Chance);
            });
            sw.Stop();
            FermatSpinner.Visibility = Visibility.Hidden;
            FermatResult.Text = (Test ? "Valószínű Prím" : "Nem Prím") + "\nPróbálkozások száma: " + Chance + "\nSzámítási idő: " + sw.Elapsed;
            FermatResult.Visibility = Visibility.Visible;
            ThreadList.Remove(Thread);
        }

        private async void RunTestSolovayStrassen()
        {
            ulong Chance = 0;
            Stopwatch sw = new Stopwatch();
            Thread Thread = null;
            sw.Start();
            bool Test = await Task.Run<bool>(() =>
            {
                Chance = Tests.generateRandomNumber(MyNumber.LocalNumber - 1, 1);
                Thread = Thread.CurrentThread;
                ThreadList.Add(Thread);
                return Tests.SolovayStrassen(MyNumber.LocalNumber, Chance);
            });
            sw.Stop();
            SolovayStrassenSpinner.Visibility = Visibility.Hidden;
            SolovayStrassenResult.Text = (Test ? "Valószínű Prím" : "Nem Prím") + "\nRandom teszt-szám: " + Chance + "\nSzámítási idő: " + sw.Elapsed;
            SolovayStrassenResult.Visibility = Visibility.Visible;
            ThreadList.Remove(Thread);
        }

        private async void RunTestMillerRabin()
        {
            Stopwatch sw = new Stopwatch();
            Thread Thread = null;
            sw.Start();
            bool Test = await Task.Run<bool>(() =>
            {
                Thread = Thread.CurrentThread;
                ThreadList.Add(Thread);
                return Tests.MillerRabin(MyNumber.LocalNumber);
            });
            sw.Stop();
            MillerRabinSpinner.Visibility = Visibility.Hidden;
            MillerRabinResult.Text = (Test ? "Prím" : "Nem Prím") + "\nSzámítási idő: " + sw.Elapsed;
            MillerRabinResult.Visibility = Visibility.Visible;
            ThreadList.Remove(Thread);
        }

        private async void RunTestNaive()
        {
            Stopwatch sw = new Stopwatch();
            Thread Thread = null;
            sw.Start();
            bool Test = await Task.Run<bool>(() =>
            {
                Thread = Thread.CurrentThread;
                ThreadList.Add(Thread);
                return Tests.Naive(MyNumber.LocalNumber);
            });
            sw.Stop();
            NaiveSpinner.Visibility = Visibility.Hidden;
            NaiveResult.Text = (Test ? "Prím" : "Nem Prím") + "\nSzámítási idő: " + sw.Elapsed;
            NaiveResult.Visibility = Visibility.Visible;
            ThreadList.Remove(Thread);
        }

        private async void RunFactorization()
        {
            Stopwatch sw = new Stopwatch();
            Thread Thread = null;
            sw.Start();
            Factors Factors = await Task.Run<Factors>(() =>
            {
                Thread = Thread.CurrentThread;
                ThreadList.Add(Thread);
                return MyNumber.FactorizeNumber();
            });
            sw.Stop();

            

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
            FactorResultTextBlock.Inlines.Add(new LineBreak());
            FactorResultTextBlock.Inlines.Add("Számítási idő: " + sw.Elapsed);

            FactorsSpinner.Visibility = Visibility.Hidden;
            FactorsResult.Content = FactorResultTextBlock;
            FactorsResult.Visibility = Visibility.Visible;

            if (Factors.List.Count == 1)
            {
                PrimePowerResult.Text = "Igen";
            }
            else
            {
                PrimePowerResult.Text = "Nem";
            }
            PrimePowerResult.Visibility = Visibility.Visible;
            ThreadList.Remove(Thread);
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
                ((Control)sender).ToolTip = null;
                Button.IsEnabled = true;
            }
        }
    }
}
