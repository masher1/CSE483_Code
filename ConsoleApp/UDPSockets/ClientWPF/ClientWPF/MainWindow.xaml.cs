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

namespace ClientWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model _myModel; //reference
        public MainWindow()
        {
            InitializeComponent();
            _myModel = new Model(); //creates instance here
            this.DataContext = _myModel; //data context for data binding
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            _myModel.SendMessage();
        }
    }
}
