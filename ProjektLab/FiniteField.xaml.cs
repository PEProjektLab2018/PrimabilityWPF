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
using FiniteFieldLibrary;
using ClsPolinom;

namespace ProjektLab
{
    /// <summary>
    /// Interaction logic for FiniteField.xaml
    /// </summary>
    public partial class FiniteField : UserControl
    {
        public Order MyOrder { get; set; }
        public ClsPolinom.Polinom IrreduciblePolinom { get; set; }

        public FiniteField()
        {
            InitializeComponent();

            MyOrder = new Order(0, 0);

            DataContext = this;
            polinom.ItemsSource = ClsPolinom.Polinom.getIrreducible(unchecked((int) MyOrder.Exponent), unchecked((int) MyOrder.Mantissa));
        }

        private void primeError(object sender, ValidationErrorEventArgs e)
        {
            if (Validation.GetHasError(primeInput))
            {
                ((Control)sender).ToolTip = e.Error.ErrorContent.ToString();
            }
            else
            {
                ((Control)sender).ToolTip = "";
            }

            updateButtonState();
        }

        private void powerError(object sender, ValidationErrorEventArgs e)
        {
            if (Validation.GetHasError(powerInput))
            {
                ((Control)sender).ToolTip = e.Error.ErrorContent.ToString();
            }
            else
            {
                ((Control)sender).ToolTip = "";
            }

            updateButtonState();
        }

        private void updateButtonState()
        {
            if (Validation.GetHasError(powerInput) || Validation.GetHasError(primeInput))
            {
                Button.IsEnabled = false;
            }
            else
            {
                Button.IsEnabled = true;
            }
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            polinom.ItemsSource = ClsPolinom.Polinom.getIrreducible(unchecked((int)MyOrder.Exponent), unchecked((int)MyOrder.Mantissa));
        }

        private void generateTables(object sender, RoutedEventArgs e)
        {
            Testing.Text = polinom.SelectedItem.ToString();
        }

        private void polinomDropDownClosed(object sender, EventArgs e)
        {
            TableButton.IsEnabled = true;
        }
    }
}
