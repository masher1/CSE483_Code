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

namespace Week_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model _mymodel;
        public MainWindow()
        {
            InitializeComponent();

            _mymodel = new Model();
            this.DataContext = _mymodel;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            _mymodel.CopyText();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Top_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                //The following forces a bind to take place without having to do a content switch
                box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                _mymodel.CopyText();
            }
        }
    }
}
