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
    /// 
    enum MY_OPERATION { ADD, SUBTRACT, MULTIPLY, DIVIDE };
    public partial class MainWindow : Window
    {
        float first_Num = 0;
        float second_Num = 0;
        float answer = 0;
        char choice = '!';

        Model _mymodel;

        public MainWindow()
        {
            InitializeComponent();
            _mymodel = new Model();
            this.DataContext = _mymodel;
        }


        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            _mymodel.SetOperation(MY_OPERATION.ADD);
            /*curr_opp.Text = "PLUS";
                choice = '+';*/
        }

        private void minus_Click(object sender, RoutedEventArgs e)
        {
            _mymodel.SetOperation(MY_OPERATION.SUBTRACT);
            /*curr_opp.Text = "MINUS";
                choice = '-';*/
        }

        private void multiply_Click(object sender, RoutedEventArgs e)
        {
            _mymodel.SetOperation(MY_OPERATION.MULTIPLY);
            /*curr_opp.Text = "MULTIPLY";
            choice = '*';*/
        }

        private void divide_Click(object sender, RoutedEventArgs e)
        {
            _mymodel.SetOperation(MY_OPERATION.DIVIDE);
            /*curr_opp.Text = "DIVISION";
            choice = '/';*/
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void equals_Click(object sender, RoutedEventArgs e)
        {
            _mymodel.CalculateResult();

        }
    }
}
