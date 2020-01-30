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

namespace simpleCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        float first_Num = 0;
        float second_Num = 0;
        float answer = 0;
        char choice = '!';

        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
                curr_opp.Text = "PLUS";
                choice = '+';
        }

        private void minus_Click(object sender, RoutedEventArgs e)
        {
                curr_opp.Text = "MINUS";
                choice = '-';
        }

        private void multiply_Click(object sender, RoutedEventArgs e)
        {
            curr_opp.Text = "MULTIPLY";
            choice = '*';
        }

        private void divide_Click(object sender, RoutedEventArgs e)
        {
            curr_opp.Text = "DIVISION";
            choice = '/';            
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void equals_Click(object sender, RoutedEventArgs e)
        {
        try
            {
                first_Num = float.Parse(firstNum.Text);
                second_Num = float.Parse(secondNum.Text);

                switch (choice)
                {
                    case '+':
                        answer = first_Num + second_Num;
                        break;
                    case '-':
                        answer = first_Num - second_Num;
                        break;
                    case '*':
                        answer = first_Num * second_Num;
                        break;
                    case '/':
                        answer = first_Num / second_Num;
                        break;
                    default:
                        answer = 0;
                        break;
                }
                Status_Block.Background = Brushes.Green;
                Status_Block.Text = "SUCCESS!";
            }
        catch (System.Exception)
            {
                Status_Block.Background = Brushes.Red;
                Status_Block.Text = "Please enter a valid number!";
            }
                Console.WriteLine("Result =" + answer.ToString());
                result.Text = answer.ToString();
        }
    }
}
