using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using ClsPolinom;
using Microsoft.Win32;
using System.IO;

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
            this.KeyDown += new KeyEventHandler(Polinom_KeyDown);
            // DataContext = this;
            isFunction = false;
        }

        private ClsPolinom.Polinom poli1;
        private ClsPolinom.Polinom poli2;
        private ClsPolinom.Polinom resPoli;
        private bool isFunction;
        private char actFunction;
        private int modulo = 0;
        private bool isExponent = false;
        private int exponent = 0;


        private void btnMod_click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                modulo = Convert.ToInt16(tbkPolinom.Text);
                if (modulo < 0) { throw new Exception(); }
                if (modulo != 0) { tbkMod.Text = tbkPolinom.Text; } else { tbkMod.Text = ""; }
                tbkPolinom.Inlines.Clear();
            }
            catch {
                MessageBox.Show("A modulo értékének természetes számnak kell lennie!");
            }
        }


        private void Polinom_KeyDown(object sender, KeyEventArgs e)
        {

            string number = "";
            if ((e.Key >= Key.D0 && e.Key <= Key.D9))
            {

                number = e.Key.ToString().Substring(1);
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                number = e.Key.ToString().Substring(6);
            }
            else if (e.Key == Key.Add)
            {
                number = "+";
            }

            else if (e.Key == Key.Subtract)
            {
                number = "-";
            }
            else if (e.Key == Key.Multiply)
            {
                number = "*";
            }

            else if (e.Key == Key.Divide)
            {
                number = "/";
            }

            tbkPolinom.Inlines.Add(new Run(number));


        }


        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
           
            Run run = new Run();
           
            if (isExponent == true) {
                run.BaselineAlignment = BaselineAlignment.Superscript;
            }
            run.Text = btn.Content.ToString();
            tbkPolinom.Inlines.Add(run);
          


        }

        private void btnFunct_click(object sender, RoutedEventArgs e) {
            Button btn = sender as Button;

            if (isExponent == true) {
                isExponent = false;  

            }
            if (isFunction == true)
            {

                switch (btn.Content.ToString())
                {
                    case "+":
                        actFunction = '+';
                        break;
                    case "-":
                        actFunction = '-';
                        break;
                    case "*":
                        actFunction = '*';
                        break;
                    case "/":
                        actFunction = '/';
                        break;
                    case "%":
                        actFunction = '%';
                        break;
                }
                lbLog.Items.Add(new TextBlock(new Run(actFunction.ToString())));
                tbkPolinom.Inlines.Clear();
                isFunction = false;
            }
            else {
                if (btn.Content.ToString() == "+" || btn.Content.ToString() == "-") {
                    tbkPolinom.Inlines.Add(new Run(btn.Content.ToString()));
                }
            }
        }

       

        private void Button_Click(object sender, RoutedEventArgs e) { }

        private void Button_Click_square(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                                foreach (Inline I in xsquare.Inlines)
                                {

                                    tbkPolinom.Inlines.Add(I);
                                }
                                */
                Run run1 = new Run();
                run1.Text = "x";
                tbkPolinom.Inlines.Add(run1);
                Run r = new Run();
                r.Text = "2";
                r.BaselineAlignment = BaselineAlignment.Superscript;
                tbkPolinom.Inlines.Add(r);
            }
            catch (Exception ex) { }



        }

        private void Button_Click_Cube(object sender, RoutedEventArgs e)
        {
            try
            {

                Run run = new Run();
                run.Text += "x";
                tbkPolinom.Inlines.Add(run);

                Run r2 = new Run();
                r2.Text = "3";
                r2.BaselineAlignment = BaselineAlignment.Superscript;
                tbkPolinom.Inlines.Add(r2);
            }
            catch (Exception ex) { }



        }

        private void Button_Click_Exponent(object sender, RoutedEventArgs e) {
            isExponent = true;
            Run run = new Run(text: "x");
            tbkPolinom.Inlines.Add(run);
     
        }

        private void btnPol_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strPolinom="";
                
                
                
                TextBlock tb = new TextBlock();
                foreach (Inline il in tbkPolinom.Inlines)
                {
                    Run run = new Run();
                    TextRange tr = new TextRange(il.ContentStart, il.ContentEnd);
                    run.Text = tr.Text;
                    strPolinom += tr.Text;
                    run.BaselineAlignment = il.BaselineAlignment;

                    tb.Inlines.Add(run);
                }
                
                if (isFunction == false) {
                    if (modulo == 0)
                    {
                        poli1 = ClsPolinom.Polinom.PolinomFromString(strPolinom);
                    }

                    else {
                        poli1 = ClsPolinom.Polinom.calcPolinomToZp(ClsPolinom.Polinom.PolinomFromString(strPolinom),modulo);
                    }

                }
                isFunction = true;
                lbLog.Items.Add(tb);


                //      }

            }
            catch (Exception ex) { }
        }

        private ClsPolinom.Polinom PolinomFromTextBlock(TextBlock textBlock) {
            ClsPolinom.Polinom retPolinom=new ClsPolinom.Polinom();
            string strPolinom = "";
          
           // isFunction = true;
            TextBlock tb = new TextBlock();
            foreach (Inline il in tbkPolinom.Inlines)
            {
                Run run = new Run();
                TextRange tr = new TextRange(il.ContentStart, il.ContentEnd);
                run.Text = tr.Text;
                strPolinom += tr.Text;
                run.BaselineAlignment = il.BaselineAlignment;

                tb.Inlines.Add(run);
            }
            retPolinom = ClsPolinom.Polinom.PolinomFromString(strPolinom);
            return retPolinom;
        }

        private String stringFromTextBlock(TextBlock textBlock)
        {
          
            string str = "";

            // isFunction = true;
           // TextBlock tb = new TextBlock();
            foreach (Inline il in textBlock.Inlines)
            {
                Run run = new Run();
                TextRange tr = new TextRange(il.ContentStart, il.ContentEnd);
                run.Text = tr.Text;
                run.BaselineAlignment = il.BaselineAlignment;
                if (run.BaselineAlignment == BaselineAlignment.Superscript) {
                    str += "^";
                }
                str += tr.Text;
               
            }
            return str;

        }

        private void btnclipBoard_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ClsPolinom.Polinom polinom = new ClsPolinom.Polinom();
                polinom = ClsPolinom.Polinom.PolinomFromString(Clipboard.GetText());
                polinom = ClsPolinom.Polinom.Sorter(polinom);
            }

            catch  {
                MessageBox.Show("A vágólap üres vagy érvénytelen adatot tartalmaz.","Hiba");
            }

        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClsPolinom.Polinom polinom = new ClsPolinom.Polinom();
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text Files (*.txt) | *.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    polinom = ClsPolinom.Polinom.PolinomFromString(System.IO.File.ReadAllText(openFileDialog.FileName));
                    displayPolinom(polinom);

                }
            }
            catch {
                MessageBox.Show("A file nem tartalmaz polinomként értelmezhető karaktersorozatot.", "Hiba");
            }
        }

        private void displayPolinom(ClsPolinom.Polinom polinom)
        {
            if (polinom.List.Count == 0) { lbLog.Items.Add(new Run("0")); }
            TextBlock tb = new TextBlock();
            // tbkPolinom.Text = "";
            int i = 0;
            foreach (Monom m in polinom.List)
            {
                if (i > 0 && (m.Coefficient > 1 || i<polinom.List.Count-1 )) { tb.Inlines.Add("+"); }
                if ((m.Coefficient == 1 || m.Coefficient==0) && m.Exponent == 0) { tb.Inlines.Add(m.Coefficient.ToString()); }
                    if (m.Coefficient > 1)
                {
                    //tbkPolinom.Text += m.Coefficient;
                    tb.Inlines.Add(m.Coefficient.ToString());
                }
                if (m.Exponent != 0)
                {
                    //tbkPolinom.Text += m.Variable;
                    tb.Inlines.Add(m.Variable);
                    if (m.Exponent > 1)
                    {
                        Run run = new Run();
                        run.Text = m.Exponent.ToString();
                        run.BaselineAlignment = BaselineAlignment.Superscript;
                        tb.Inlines.Add(run);
                      //  StackPanel sp = new StackPanel();
                      //  sp.Children.Add(tb);
                        
                    }
                }
                i++;
            }
            lbLog.Items.Add(tb);

        }

        private void btnEraseLog_Click(object sender, RoutedEventArgs e) {
            lbLog.Items.Clear();
        }

        private void btnCE_click(object sender, RoutedEventArgs e) {
            tbkPolinom.Inlines.Clear();
            isExponent = false;
        }

        private void btnC_click(object sender, RoutedEventArgs e) {
            poli1.List.Clear();
            poli2.List.Clear();
            tbkPolinom.Inlines.Clear();
        }

        private void btnSave_click(object sender, RoutedEventArgs e) {


            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Szövegfájlok (*.txt) | *.txt";
                if (sfd.ShowDialog() == true)
                {
                    StreamWriter sw = new StreamWriter(sfd.FileName);
                    foreach (var i in lbLog.Items)
                    {
                        if (i.GetType().ToString() == "System.Windows.Controls.TextBlock")
                        {
                            string st = stringFromTextBlock((TextBlock)i);
                            sw.WriteLine(st);
                        }


                    }
                    sw.Close();
                }

            }
            catch (Exception ex) {
               MessageBox.Show(ex.Message);
            }

        }

        private void equal_Click(object sender, RoutedEventArgs e)
        {

            try
            {

               
                
                if (poli1 != null) {
                    string strPolinom = "";
                    
                    isFunction = true;
                    TextBlock tb = new TextBlock();
                    foreach (Inline il in tbkPolinom.Inlines)
                    {
                        Run run = new Run();
                        TextRange tr = new TextRange(il.ContentStart, il.ContentEnd);
                        run.Text = tr.Text;
                        strPolinom += tr.Text;
                        run.BaselineAlignment = il.BaselineAlignment;

                        tb.Inlines.Add(run);
                    }

                    if (modulo == 0)
                    {
                        poli2 = ClsPolinom.Polinom.PolinomFromString(strPolinom);
                    }
                    else
                    {
                        poli2 = ClsPolinom.Polinom.calcPolinomToZp(ClsPolinom.Polinom.PolinomFromString(strPolinom), modulo);
                    }
                   
                    lbLog.Items.Add(tb);
                }
                lbLog.Items.Add(new TextBlock(new Run("=")));
                switch (actFunction) {
                    case '+':
                        resPoli = poli1 + poli2;
                        break;
                    case '-':
                        resPoli = poli1 - poli2;
                        break;
                    case '*':
                        resPoli = poli1 * poli2;
                        break;
                    case '/':
                        resPoli = poli1 / poli2;
                        break;

                    case '%':
                        resPoli = poli1 % poli2;
                        break;
                }
                

                if (modulo != 0) { resPoli = ClsPolinom.Polinom.calcPolinomToZp(resPoli, modulo); }
                displayPolinom(resPoli);
                tbkPolinom.Inlines.Clear();
                isFunction = false;
                isExponent = false;
                
            }

            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }



}
