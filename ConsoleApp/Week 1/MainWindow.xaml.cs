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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DefaultText.Text = "Hello World"; //default text
            DefaultText.BorderThickness = new Thickness(25);
        }

        private void DefaultText_MouseEnter(object sender, MouseEventArgs e)
        {
            DefaultText.Text = "GET OUT!!!!";
        }

        private void DefaultText_MouseLeave(object sender, MouseEventArgs e)
        {
            DefaultText.Text = "GET IN!!!!";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultText.Text = "HIIIEEEEEIEE!!";
        }
    }
}
