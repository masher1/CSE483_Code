using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
// Message Box in Console app
// Don't forget to add Reference to System.Windows.Forms
//using System.Windows.Forms;

namespace ClientWPF
{
    class Model : INotifyPropertyChanged
    {
      
        private String _messageBox;
        public String MessageBox
        {
            get { return _messageBox; }
            set
            {
                _messageBox = value;

                OnPropertyChanged("MessageBox");
            }
        }

        private string _statusBox;
        public string StatusBox
        {
            get { return _statusBox; }
            set
            {
                _statusBox = value;

                OnPropertyChanged("StatusBox");
            }
        }
        // some data that keeps track of ports and addresses
        private UInt32 _remotePort = 5000;
        private String _remoteIPAddress = "127.0.0.1";

        // this is the UDP socket that will be used to communicate
        // over the network
        private UdpClient _dataSocket;

        void Main(string[] args)
        {
            try
            {
                // set up generic UDP socket and bind to local port
                //
                _dataSocket = new UdpClient();
            }
            catch (Exception ex)
            {
                StatusBox = (ex.ToString());
                return;
            }

           // SendMessage();
        }

        public void SendMessage()
        {
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(MessageBox);

            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException ex)
            {
                StatusBox= (ex.ToString());
               // StatusBox = "Message successfully sent.";
                return;
            }
        }


        #region Data Binding Stuff
        // define our property chage event handler, part of data binding
        public event PropertyChangedEventHandler PropertyChanged;

        // implements method for data binding to any and all properties
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        #endregion
    }
}
