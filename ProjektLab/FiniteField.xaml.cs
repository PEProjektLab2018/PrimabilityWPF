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
using System.Threading;

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
        public FiniteFieldTableViewModel MultiplicationTable { get; set; }
        Thread IrreducibleThread { get; set; }

        private static IValueConverter polinomToXmlConverter;

        public FiniteField()
        {
            InitializeComponent();

            MyOrder = new Order(0, 0);

            SummationTable = new FiniteFieldTableViewModel();
            MultiplicationTable = new FiniteFieldTableViewModel();

            polinomToXmlConverter = new PolinomToXamlConverter();

            polinom.ItemsSource = ClsPolinom.Polinom.getIrreducible(MyOrder.Exponent, MyOrder.Mantissa);
            DataContext = this;

            /*
             * TESTING
            ClsPolinom.Polinom a, b, r;
            a = new ClsPolinom.Polinom();
            b = new ClsPolinom.Polinom();
            r = new ClsPolinom.Polinom();

            r.add(new Monom(1, 3)).add(new Monom(1, 1)).add(new Monom(1, 0));

            a.add(new Monom(1, 1)).add(new Monom(1, 0));
            b.add(new Monom(1, 2));
            */
            
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
            getIrreducible();
        }

        private async void getIrreducible()
        {
            IrreducibleThread = null;
            List<ClsPolinom.Polinom> list = await Task.Run<List<ClsPolinom.Polinom>>(() =>
            {
                IrreducibleThread = Thread.CurrentThread;
                return ClsPolinom.Polinom.getIrreducible(MyOrder.Exponent, MyOrder.Mantissa);
            });
            polinom.ItemsSource = list;
            IrreducibleThread = null;
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

            SummationGrid.Columns.Clear();
            SummationTable.Clear(); ;

            MultiplicationGrid.Columns.Clear();
            MultiplicationTable.Clear();

            generateTablesAsync();
        }

        private async void generateTablesAsync()
        {
            await Task.Run(() =>
            {
                ResultGrid.Dispatcher.Invoke(() => { 
                    List<ClsPolinom.Polinom> columns = FiniteFieldLibrary.FiniteField.generateMembers(MyOrder);

                    SummationGrid.Columns.Add(getDataGridTemplateColumn("+", "Label"));
                    MultiplicationGrid.Columns.Add(getDataGridTemplateColumn("*", "Label"));

                    int i = 0;
                    foreach (ClsPolinom.Polinom polinom in columns)
                    {
                        SummationTable.createRow(columns.Count());
                        SummationGrid.Columns.Add(getDataGridTemplateColumn(getPolinomTextBlock(polinom), String.Format("Polinoms[{0}]", i)));
                        SummationTable.Rows[i].Label = polinom;

                        MultiplicationTable.createRow(columns.Count());
                        MultiplicationGrid.Columns.Add(getDataGridTemplateColumn(getPolinomTextBlock(polinom), String.Format("Polinoms[{0}]", i)));
                        MultiplicationTable.Rows[i].Label = polinom;

                        i++;
                    }
                });
            });

            int calcSize = (int)Math.Pow(MyOrder.Mantissa, MyOrder.Exponent);
            for (int i = 0; i < calcSize; i++)
            {
                ClsPolinom.Polinom rowPolinom = SummationTable.Rows[i].Label;
                for (int j = 0; j < calcSize; j++)
                {
                    ClsPolinom.Polinom colPolinom = SummationTable.Rows[j].Label;
                    await Task.Run(() =>
                    {
                        SummationTable.Rows[i].Polinoms[j] = rowPolinom + colPolinom;
                        //SummationTable.Rows[i].Polinoms[j] = ClsPolinom.Polinom.calcPolinomToZp((rowPolinom + colPolinom) % IrreduciblePolinom, MyOrder.Mantissa);
                    });
                    SummationGrid.ItemsSource = null;
                    SummationGrid.ItemsSource = SummationTable.Rows;
                    await Task.Run(() =>
                    {
                        MultiplicationTable.Rows[i].Polinoms[j] = rowPolinom * colPolinom;
                        //MultiplicationTable.Rows[i].Polinoms[j] = ClsPolinom.Polinom.calcPolinomToZp((rowPolinom * colPolinom) % IrreduciblePolinom, MyOrder.Mantissa);
                    });
                    MultiplicationGrid.ItemsSource = null;
                    MultiplicationGrid.ItemsSource = MultiplicationTable.Rows;
                }
            }

            ResultGrid.Visibility = Visibility.Visible;
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

        private static DataGridTemplateColumn getDataGridTemplateColumn(object header, string bindingString)
        {
            DataGridTemplateColumn column = new DataGridTemplateColumn();

            column.Header = header;

            Binding binding = new Binding(bindingString);
            binding.Converter = polinomToXmlConverter;

            FrameworkElementFactory labelFactory = new FrameworkElementFactory(typeof(Label));
            labelFactory.SetBinding(Label.ContentProperty, binding);
            DataTemplate template = new DataTemplate();
            template.VisualTree = labelFactory;

            column.CellTemplate = template;

            return column;
        }
    }
}
