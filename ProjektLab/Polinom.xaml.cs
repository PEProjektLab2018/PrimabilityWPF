using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ClsPolinom;


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
           // DataContext = this;
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
        private void equal_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                ClsPolinom.Polinom polinom=new ClsPolinom.Polinom();
                /*
                string pol="";

                foreach (Inline il in tbkPolinom.Inlines)
                {
                    
                    TextRange tr = new TextRange(il.ContentStart, il.ContentEnd);
                    pol += tr.Text;
                }


                polinom = ClsPolinom.Polinom.PolinomFromString(pol);
               
                */
                /*
                ClsPolinom.Polinom pol1 = new ClsPolinom.Polinom();
                ClsPolinom.Polinom pol2 = new ClsPolinom.Polinom();

                pol1 = ClsPolinom.Polinom.PolinomFromString("5x2+2x+5");
                pol2= ClsPolinom.Polinom.PolinomFromString("x+1");

                ClsPolinom.Polinom Pol3 = new ClsPolinom.Polinom();

                Pol3 = pol1 * pol2;

                ClsPolinom.Polinom Pol4 = new ClsPolinom.Polinom();
                Pol4 = ClsPolinom.Polinom.Sorter(pol2);
                ClsPolinom.Polinom Pol5 = new ClsPolinom.Polinom();

                Pol5 = pol1/ pol2;

                ClsPolinom.Polinom Pol6 = new ClsPolinom.Polinom();
                ClsPolinom.Polinom Pol7 = new ClsPolinom.Polinom();

                Pol6 = ClsPolinom.Polinom.PolinomFromString("19x2+27x+8");
                Pol7 = ClsPolinom.Polinom.calcPolinomToZp(Pol6, 7);
                string s = Pol7.ToString();
                */
                
                List<ClsPolinom.Polinom> irreducible=new List<ClsPolinom.Polinom>();
                //irreducible = ClsPolinom.Polinom.getIrreducible(1, 2);
               //irreducible = ClsPolinom.Polinom.getIrreducible(1, 3);
                //irreducible = ClsPolinom.Polinom.getIrreducible(4, 2);
                irreducible = ClsPolinom.Polinom.getIrreducible(4, 3);

                //ClsPolinom.Polinom pol8 = new ClsPolinom.Polinom();
                //pol8 = ClsPolinom.Polinom.Pow(ClsPolinom.Polinom.PolinomFromString("x+1"), 0);
                //ClsPolinom.Polinom pol9 = new ClsPolinom.Polinom();
                //pol9 = ClsPolinom.Polinom.Pow(ClsPolinom.Polinom.PolinomFromString("x+1"), 1);




            }

            catch (Exception ex) { }
        }
    }

   

}
