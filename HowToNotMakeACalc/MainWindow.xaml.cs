using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace HowToNotMakeACalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        readonly Calculation sc_calc = new Calculation();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button button)
            {
                switch (button.Content)
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "(":
                    case ")":
                    case "+":
                    case "-":
                    case "/":
                    case "*":
                    case "^":
                    case "√":
                    case ",":
                        TextField.Text += button.Content;
                        break;
                    case "Del":
                        if (TextField.Text != "") { 
                        TextField.Text = TextField.Text.Remove(TextField.Text.Length - 1);
                        break;
                        }
                        else
                        {
                            break;
                        }
                    case "Clear":
                        TextField.Text = "";
                        break;
                    case "=":
                    case "Exe":
                        if (TextField.Text == "66")
                        {
                            TextField.Text = "Order Executed";
                            break;
                        }
                        else if (TextField.Text != "") { 
                        TextField.Text = sc_calc.EvaluateExpression(TextField.Text);
                            break;
                        }
                        else { 
                        break;
                        }
                    default:
                        break;
                }
            }

        }

        private void OnChange(object sender, TextChangedEventArgs e)
        {
            if (sc_calc.IsTextAllowed(TextField.Text))
            {
                MessageBox.Show("Invalid Input");
                TextField.Text = "";
            }
            else
            {
                e.Handled = true;
            }

        }
    }
}
