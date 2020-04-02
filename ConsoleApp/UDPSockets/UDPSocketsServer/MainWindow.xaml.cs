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

// don't forget to add Reference System.Windows.Forms
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Remoting.Channels;

namespace UDPSockets
{
    public partial class MainWindow : Window
    {
        private Model _model;
        public MainWindow()
        {
            InitializeComponent();
            _model = new Model();
            this.DataContext = _model;

            /*MessagesText.Text = ("UDP Server is Running!!\n" + "UDP Server");
            _dataSocket = new UdpClient(_localPort);
            StartThread();*/
        }

        /*// some data that keeps track of ports and addresses
        private int _localPort = 5000;
        private string _localIPAddress = "127.0.0.1";

        // this is the thread that will run in the background
        // waiting for incomming data
        private Thread _receiveDataThread;

        // this is the UDP socket that will be used to communicate
        // over the network
        private UdpClient _dataSocket;

        *//*public void InitModel()
        {
            _MessagesText.Text = ("UDP Server is Running!!\n", "UDP Server");
            _dataSocket = new UdpClient(_localPort);
            StartThread();
        }*//*
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
                    //MessagesText.Text = "Data Recieved";
                    // convert byte array to a string
                    MessagesText.Text = (DateTime.Now + ": " + System.Text.Encoding.Default.GetString(receiveData) + "\n" + "UDP Server");
                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    MessagesText.Text = (ex.ToString() + "UDP Server");
                    //return;
                }
            }
        }

        public void StartThread()
        {
            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();
        }*/
    }
}
