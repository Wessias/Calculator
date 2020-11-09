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

        Model sc_model = new Model();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
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
                    case "√(":
                    case ".":
                    case "=":
                        TextField.Text += button.Content;
                        break;
                    case "Del":
                        TextField.Text = TextField.Text.Remove(TextField.Text.Length - 1);
                        break;
                    case "Exe":
                        break;
                    default:
                        break;
                }
            }

        }

        private void TextField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = sc_model.IsTextAllowed(e.Text);
        }

        private void TextField_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!sc_model.IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
