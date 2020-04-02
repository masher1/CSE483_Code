using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Sockets
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace UDPSockets
{
    class Model : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _MessageText;
        public string MessageText
        {
            get{ return _MessageText; }
            set{
                _MessageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        private String _StatusBox;
        public String StatusBox

        {
            get { return _StatusBox; }
            set
            {
                _StatusBox = value;
                OnPropertyChanged("StatusBox");
            }
        }

        private String _LoopBackBoxText;
        public String LoopBackBoxText

        {
            get { return _LoopBackBoxText; }
            set
            {
                _LoopBackBoxText = value;
                OnPropertyChanged("LoopBackBoxText");
            }
        }

        public Model()
        {
            try
            {
                // set up generic UDP socket and bind to local port
                _dataSocket = new UdpClient ((int) _localPort);

                ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
                _receiveDataThread = new Thread(threadFunction);
                _receiveDataThread.Start();
            }
            catch (Exception ex)
            {
                StatusBox = (ex.ToString());
                return;
            }

        }

        public void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_localIPAddress), (int)_localPort);
            while (true)
            {
                try
                {
                   Byte[] receiveData = _dataSocket.Receive(ref endPoint);

                    LoopBackBoxText = DateTime.Now + ": " + System.Text.Encoding.Default.GetString(receiveData) + "\n";
                }
                catch (SocketException ex)
                {
                    // exiting from the JoystickPositionWindow form
                    StatusBox = (ex.ToString());
                    return;
                }
            }
        }

        // some data that keeps track of ports and addresses
        private int _remotePort = 5000;
        private String _remoteIPAddress = "127.0.0.1";

        private int _localPort = 5001;
        private String _localIPAddress = "127.0.0.1";

        // this is the UDP socket that will be used to communicate
        // over the network
        private UdpClient _dataSocket;

        private Thread _receiveDataThread;
        public void SendMessage()
        {
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(MessageText);
            StatusBox = "";
            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
                StatusBox = ("\"" + MessageText + "\" Successfully Sent Over!");
            }
            catch (SocketException ex)
            {
                StatusBox = ex.ToString();
                return;
            }
        }
    }
}