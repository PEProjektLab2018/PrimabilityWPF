﻿using System;
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
        public Prime()
        {
            InitializeComponent();

            MyNumber = new MyNumber();
            Chance = 10;

            DataContext = this;
        }

        private void ResetPrimeTests()
        {
            ErastothenesSpinner.Visibility = Visibility.Visible;
            FermatSpinner.Visibility = Visibility.Visible;
            SolovayStrassenSpinner.Visibility = Visibility.Visible;
            MillerRabinSpinner.Visibility = Visibility.Visible;

            ErastothenesResult.Visibility = Visibility.Hidden;
            FermatResult.Visibility = Visibility.Hidden;
            SolovayStrassenResult.Visibility = Visibility.Hidden;
            MillerRabinResult.Visibility = Visibility.Hidden;
            ErastothenesResult.Text = "";
            FermatResult.Text = "";
            SolovayStrassenResult.Text = "";
            MillerRabinResult.Text = "";

            FactorsSpinner.Visibility = Visibility.Visible;
            FactorsResult.Visibility = Visibility.Hidden;
            FactorsResult.Content = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button.IsEnabled = false;
            ResetPrimeTests();

            RunTestErastothenes();
            RunTestFermat();
            RunTestSolovayStrassen();
            RunTestMillerRabin();
            RunFactorization();

            Button.IsEnabled = true;
        }

        private async void RunTestErastothenes()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool Test = await Task.Run<bool>(() => Tests.Erastothenes(MyNumber.LocalNumber));
            sw.Stop();
            ErastothenesSpinner.Visibility = Visibility.Hidden;
            ErastothenesResult.Text = (Test ? "Prím" : "Nem Prím") + "\nSzámítási idő: " + sw.Elapsed;
            ErastothenesResult.Visibility = Visibility.Visible;
        }

        private async void RunTestFermat()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool Test = await Task.Run<bool>(() => Tests.Fermat(MyNumber.LocalNumber, Chance));
            sw.Stop();
            FermatSpinner.Visibility = Visibility.Hidden;
            FermatResult.Text = (Test ? "Prím" : "Nem Prím") + "\nSzámítási idő: " + sw.Elapsed;
            FermatResult.Visibility = Visibility.Visible;
        }

        private async void RunTestSolovayStrassen()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool Test = await Task.Run<bool>(() => Tests.SolovayStrassen(MyNumber.LocalNumber, Chance));
            sw.Stop();
            SolovayStrassenSpinner.Visibility = Visibility.Hidden;
            SolovayStrassenResult.Text = (Test ? "Prím" : "Nem Prím") + "\nSzámítási idő: " + sw.Elapsed;
            SolovayStrassenResult.Visibility = Visibility.Visible;
        }

        private async void RunTestMillerRabin()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool Test = await Task.Run<bool>(() => Tests.MillerRabin(MyNumber.LocalNumber));
            sw.Stop();
            MillerRabinSpinner.Visibility = Visibility.Hidden;
            MillerRabinResult.Text = (Test ? "Prím" : "Nem Prím") + "\nSzámítási idő: " + sw.Elapsed;
            MillerRabinResult.Visibility = Visibility.Visible;
        }

        private async void RunFactorization()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Factors Factors = await Task.Run<Factors>(() => MyNumber.FactorizeNumber());
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
