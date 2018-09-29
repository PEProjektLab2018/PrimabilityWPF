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
using Polinom;


namespace ProjektLab
{
    /// <summary>
    /// Interaction logic for Polinom.xaml
    /// </summary>
    public partial class Polinom : UserControl
    {
        public Polinom()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            //tbkPolinom.Text +=btn.Content.ToString();
            tbkPolinom.Inlines.Add(new Run(btn.Content.ToString()));
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e) { }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                                foreach (Inline I in xsquare.Inlines)
                                {

                                    tbkPolinom.Inlines.Add(I);
                                }
                                */

                tbkPolinom.Text += "x";
                Run run = new Run();
                run.Text = "2";
                run.BaselineAlignment = BaselineAlignment.Superscript;
                tbkPolinom.Inlines.Add(run);
            }
            catch (Exception ex) { }



        }

        

        private void btnPol_Click(object sender, RoutedEventArgs e)
        {
            try {

                if (btnPol.Content.ToString() == "Polinom") {
                    btnPol.Content = "Művelet";
                    foreach (Inline il in tbkPolinom.Inlines) {
                        Run run = new Run();
                        TextRange tr = new TextRange(il.ContentStart, il.ContentEnd);
                        run.Text = tr.Text;
                        run.BaselineAlignment = il.BaselineAlignment;        
                         
                        tbkPol1.Inlines.Add(run);
                    }
                
 
                }
            
        }
            catch (Exception ex) { }
        }
    }
}
