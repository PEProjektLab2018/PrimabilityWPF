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
        public FiniteFieldTableViewModel SummationTable { get; set; }

        public FiniteField()
        {
            InitializeComponent();

            MyOrder = new Order(0, 0);
            SummationTable = new FiniteFieldTableViewModel();

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
            Button.IsEnabled = false;
            ButtonSpinner.Visibility = Visibility.Visible;
            polinom.ItemsSource = ClsPolinom.Polinom.getIrreducible(MyOrder.Exponent, MyOrder.Mantissa);
            ButtonSpinner.Visibility = Visibility.Hidden;
            Button.IsEnabled = true;
        }

        private void polinomDropDownClosed(object sender, EventArgs e)
        {
            TableButton.IsEnabled = true;
        }

        private void generateTables(object sender, RoutedEventArgs e)
        {
            TableButtonSpinner.Visibility = Visibility.Visible;
            TableButton.IsEnabled = false;

            List<ClsPolinom.Polinom> columns = FiniteFieldLibrary.FiniteField.generateMembers(MyOrder);
            ResultGrid.Visibility = Visibility.Visible;
            SummationGrid.Columns.Clear();
            MultiplicationGrid.Columns.Clear();

            DataGridTextColumn plusSign = new DataGridTextColumn();
            plusSign.Header = "+";
            Binding plusBinding = new Binding("Label");
            plusBinding.Converter = new PolinomToXamlConverter();
            plusSign.Binding = plusBinding;
            SummationGrid.Columns.Add(plusSign);
            DataGridTextColumn multiplySign = new DataGridTextColumn();
            multiplySign.Header = "*";
            MultiplicationGrid.Columns.Add(multiplySign);

            int i = 0;
            foreach (ClsPolinom.Polinom polinom in columns)
            {
                SummationTable.createRow(columns.Count());
                DataGridTextColumn columnSum = new DataGridTextColumn();
                columnSum.Header = getPolinomTextBlock(polinom);
                columnSum.Binding = new Binding(String.Format("Polinoms[{0}]", i));
                SummationGrid.Columns.Add(columnSum);
                SummationTable.Rows[i].Label = polinom;

                DataGridTextColumn columnMul = new DataGridTextColumn();
                columnMul.Header = getPolinomTextBlock(polinom);
                MultiplicationGrid.Columns.Add(columnMul);
                i++;
            }

            TableButtonSpinner.Visibility = Visibility.Hidden;
            TableButton.IsEnabled = true;
        }

        private static TextBlock getPolinomTextBlock(ClsPolinom.Polinom polinom)
        {
            TextBlock tbkPolinom = new TextBlock();

            int i = 0;
            foreach (ClsPolinom.Monom m in polinom.List)
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
    }
}
