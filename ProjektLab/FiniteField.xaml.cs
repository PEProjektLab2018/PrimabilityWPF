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
using System.IO;

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
                List<ClsPolinom.Polinom> tmpList = ClsPolinom.Polinom.getIrreducible(MyOrder.Exponent, MyOrder.Mantissa);

                List<ClsPolinom.Polinom> result = new List<ClsPolinom.Polinom>();

                // Use only Given exponent polinoms
                foreach (ClsPolinom.Polinom p in tmpList)
                {
                    if((int) ClsPolinom.Polinom.Sorter(p).List[0].Exponent == MyOrder.Exponent)
                    {
                        result.Add(p);
                    }
                }

                return result;
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
            disableExportButtons();
            TableButtonSpinner.Visibility = Visibility.Visible;
            TableButton.IsEnabled = false;

            SummationGrid.ItemsSource = null;
            SummationTable.Clear(); ;
            SummationGrid.Columns.Clear();
            SummationGrid.ItemsSource = SummationTable.Rows;

            MultiplicationGrid.ItemsSource = null;
            MultiplicationTable.Clear();
            MultiplicationGrid.Columns.Clear();
            MultiplicationGrid.ItemsSource = MultiplicationTable.Rows;


            generateTablesAsync();
        }

        private async void generateTablesAsync()
        {
            await Task.Run(() =>
            {
                ResultGrid.Dispatcher.Invoke(() => { 
                    List<ClsPolinom.Polinom> columns = FiniteFieldLibrary.FiniteField.generateMembers(MyOrder);

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
            ResultGrid.Visibility = Visibility.Visible;

            int calcSize = (int)Math.Pow(MyOrder.Mantissa, MyOrder.Exponent);
            for (int i = 0; i < calcSize; i++)
            {
                ClsPolinom.Polinom rowPolinom = SummationTable.Rows[i].Label;
                for (int j = 0; j < calcSize; j++)
                {
                    ClsPolinom.Polinom colPolinom = SummationTable.Rows[j].Label;
                    await Task.Run(() =>
                    {
                        //SummationTable.Rows[i].Polinoms[j] = rowPolinom + colPolinom;
                        SummationTable.Rows[i].Polinoms[j] = ClsPolinom.Polinom.calcPolinomToZp((rowPolinom + colPolinom) % IrreduciblePolinom, MyOrder.Mantissa);
                    });
                    SummationGrid.ItemsSource = null;
                    SummationGrid.ItemsSource = SummationTable.Rows;
                    await Task.Run(() =>
                    {
                        //MultiplicationTable.Rows[i].Polinoms[j] = rowPolinom * colPolinom;
                        MultiplicationTable.Rows[i].Polinoms[j] = ClsPolinom.Polinom.calcPolinomToZp((rowPolinom * colPolinom) % IrreduciblePolinom, MyOrder.Mantissa);
                    });
                    MultiplicationGrid.ItemsSource = null;
                    MultiplicationGrid.ItemsSource = MultiplicationTable.Rows;
                }
            }

            TableButtonSpinner.Visibility = Visibility.Hidden;
            TableButton.IsEnabled = true;
            enableExportButtons();
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

        private void MultiplicationTableExport_Click(object sender, RoutedEventArgs e)
        {
            var csv = new StringBuilder();

            csv.AppendLine("*;" + string.Join(";", MultiplicationTable.GetLabels()));

            for(int i = 0; i < MultiplicationTable.Rows.Count(); i++)
            {
                csv.AppendLine(MultiplicationTable.Rows[i].ToString());
            }

            Microsoft.Win32.SaveFileDialog dialog = getDialog();

            // Show save file dialog box
            Nullable<bool> result = dialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dialog.FileName;

                File.WriteAllText(filename, csv.ToString());
            }
        }

        private void SummationTableExport_Click(object sender, RoutedEventArgs e)
        {
            var csv = new StringBuilder();

            csv.AppendLine("+;" + string.Join(";", SummationTable.GetLabels()));

            for (int i = 0; i < SummationTable.Rows.Count(); i++)
            {
                csv.AppendLine(SummationTable.Rows[i].ToString());
            }

            Microsoft.Win32.SaveFileDialog dialog = getDialog();

            // Show save file dialog box
            Nullable<bool> result = dialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dialog.FileName;

                File.WriteAllText(filename, csv.ToString());
            }
        }

        private Microsoft.Win32.SaveFileDialog getDialog()
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "export";
            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV (.csv)|*.csv";

            return dialog;
        }

        private void disableExportButtons()
        {
            MultiplicationTableExport.Visibility = Visibility.Hidden;
            SummationTableExport.Visibility = Visibility.Hidden;
            MultiplicationTableExport.IsEnabled = false;
            SummationTableExport.IsEnabled = false;
        }

        private void enableExportButtons()
        {
            MultiplicationTableExport.Visibility = Visibility.Visible;
            SummationTableExport.Visibility = Visibility.Visible;
            MultiplicationTableExport.IsEnabled = true;
            SummationTableExport.IsEnabled = true;
        }
    }
}
