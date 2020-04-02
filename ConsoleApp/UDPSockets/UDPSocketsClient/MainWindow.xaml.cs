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
// Sockets
using System.Net.Sockets;
using System.Net;

// Message Box
// don't forget to add Reference System.Windows.Forms
using System.Windows.Forms;

namespace UDPSockets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Model _model;
        public MainWindow()
        {
            InitializeComponent();

            _model = new Model();
            this.DataContext = _model;

           /* try
            {
                // set up generic UDP socket and bind to local port
                //
                _dataSocket = new UdpClient();
            }
            catch (Exception ex)
            {
                StatusBox.Text = ex.ToString();
                return;
            }*/
        }

/*        // some data that keeps track of ports and addresses
        private  UInt32 _remotePort = 5000;
        private  String _remoteIPAddress = "127.0.0.1";

        // this is the UDP socket that will be used to communicate
        // over the network
         private UdpClient _dataSocket;*/

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SendMessage();
        }
    }
}
