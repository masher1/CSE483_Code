using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        private string _MessagesText;
        public string MessagesText
        {
            get{ return _MessagesText; }
            set {
                _MessagesText = value;
                OnPropertyChanged("MessagesText");
            }
        }

        // some data that keeps track of ports and addresses
        private int _localPort = 5000;
        private string _localIPAddress = "127.0.0.1";

        // this is the thread that will run in the background
        // waiting for incomming data
        private Thread _receiveDataThread;

        // this is the UDP socket that will be used to communicate
        // over the network
        private UdpClient _dataSocket;


        public Model()
        {
            MessagesText = ("UDP Server is Running!!\n" + "UDP Server");
            _dataSocket = new UdpClient(_localPort);
            StartThread();
        }

        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_localIPAddress), (int)_localPort);
            while (true)
            {
                try
                {
                    // wait for data
                    // this is a blocking call
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);

                    // convert byte array to a string
                    MessagesText = (DateTime.Now + ": " + System.Text.Encoding.Default.GetString(receiveData) + "\n" + "UDP Server");

                    //loop the message back to the sender
                    _dataSocket.Send(receiveData, receiveData.Length, endPoint);
                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed,
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    MessagesText = (ex.ToString() + "UDP Server");
                    return;
                }
            }
        }

        public void StartThread()
        {
            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();
        }
    }
}
