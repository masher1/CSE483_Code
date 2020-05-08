/////////////////////////////////////////////////////////////////////////////////////////////////////
// CSE483 - Tic Tac Toe
// Author - Malkiel Asher
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Brushes
using System.Windows.Media;

// observable collections
using System.Collections.ObjectModel;

// INotifyPropertyChanged
using System.ComponentModel;

// byte data serialization
using System.Runtime.Serialization.Formatters.Binary;

// threading
using System.Threading;
// memory streams
using System.IO;

// Sockets
using System.Net.Sockets;
using System.Net;

// debug output
using System.Diagnostics;

namespace TicTacToe
{
    class Model : INotifyPropertyChanged
    {

        bool won = false;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Serializable]
        struct GameData
        {
            public int data1;
            public GameData(int p)
            {
                data1 = p;
            }
        }

        private int _data1;
        public int Data1
        {
            get { return _data1; }
            set
            {
                _data1 = value;
                OnPropertyChanged("Data1");
            }
        }

        private bool _sendEnabled;
        public bool SendEnabled
        {
            get { return _sendEnabled; }
            set
            {
                _sendEnabled = value;
                OnPropertyChanged("SendEnabled");
            }
        }

        public ObservableCollection<Tile> TileCollection;
        private static UInt32 _numTiles = 9;
        private UInt32[] _buttonPresses = new UInt32[_numTiles];
       
        private int _counter;
        public int counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
                OnPropertyChanged("Data1");
            }
        }


        private String _statusText = "";
        public String StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }

        // this is the UDP socket that will be used to communicate
        // over the network
        UdpClient _dataSocket;

        // some data that keeps track of ports and addresses
        private static UInt32 _localPort;
        private static String _localIPAddress;
        private static UInt32 _remotePort;
        private static String _remoteIPAddress;

        // this is the thread that will run in the background
        // waiting for incomming data
        private Thread _receiveDataThread;

        // this thread is used to synchronize the startup of 
        // two UDP peers
        private Thread _synchWithOtherPlayerThread;
        /// <summary>
        /// this method is called to set this UDP peer's local port and address
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ipAddress"></param>
        public void SetLocalNetworkSettings(UInt32 port, String ipAddress)
        {
            _localPort = port;
            _localIPAddress = ipAddress;
        }
        
        /// <summary>
        /// this method is called to set the remote UDP peer's port and address
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ipAddress"></param>
        public void SetRemoteNetworkSettings(UInt32 port, String ipAddress)
        {
            _remotePort = port;
            _remoteIPAddress = ipAddress;
        }

        /// <summary>
        /// initialize the necessary data, and start the synchronization
        /// thread to wait for the other peer to join
        /// </summary>
        /// <returns></returns>
        public bool InitModel()
        {
            try
            {
                // set up generic UDP socket and bind to local port
                //
                _dataSocket = new UdpClient((int)_localPort);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return false;
            }

            ThreadStart threadFunction;
            threadFunction = new ThreadStart(SynchWithOtherPlayer);
            _synchWithOtherPlayerThread = new Thread(threadFunction);
            StatusText = DateTime.Now + ":" + " Waiting for other UDP peer to join.\n";
            _synchWithOtherPlayerThread.Start();

            return true;
        }

        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
            TileCollection = new ObservableCollection<Tile>();
            for (int i = 0; i < _numTiles; i++)
            {
                TileCollection.Add(new Tile()
                {
                    TileBrush = Brushes.Black,
                    TileLabel = " ",
                    TileName = i.ToString(),
                    TileBackground = Brushes.Pink
                });
            }

            // this disables the Send button initially
            SendEnabled = false;

            // initialize the help test
            StatusText = "Select Socket Setup button to begin.";
        }

        /// <summary>
        /// called when the view is closing to ensure we clean up our socket
        /// if we don't, the application may hang on exit
        /// </summary>
        public void Model_Cleanup()
        {
            // important. Close socket or application will not exit correctly.
            if (_dataSocket != null) _dataSocket.Close();
            if (_receiveDataThread != null) _receiveDataThread.Abort();
        }


        /// <summary>
        /// called to send some data to the other UDP peer
        /// </summary>
        public void SendMessage()
        {
            // data structure used to communicate data with the other player
            GameData gameData;

            // formatter used for serialization of data
            BinaryFormatter formatter = new BinaryFormatter();

            // stream needed for serialization
            MemoryStream stream = new MemoryStream();

            // Byte array needed to send data over a socket
            Byte[] sendBytes;

            gameData.data1 = _data1;

            if (counter < 9 && !won)
            {
                if (!(TileCollection[gameData.data1].TileFlag))
                {
                    if (counter%2 == 0 && _localPort % 2 == 1)
                    {
                        TileCollection[gameData.data1].TileFlag = true;
                        _buttonPresses[gameData.data1]++;
                        TileCollection[gameData.data1].TileLabel = "O";
                        TileCollection[gameData.data1].TileBrush = Brushes.Red;
                        counter++;
                        StatusText = "User Selected Button " + gameData.data1.ToString() + "\n" + "It is X's Turn";
                        WinnerSelection(TileCollection);

                    }

                    if (counter%2 == 1 && _localPort % 2 == 0)
                    {
                        TileCollection[gameData.data1].TileFlag = true;
                        _buttonPresses[gameData.data1]++;
                        TileCollection[gameData.data1].TileLabel = "X";
                        TileCollection[gameData.data1].TileBrush = Brushes.Green;
                        counter++;
                        StatusText = "User Selected Button " + gameData.data1.ToString() + "\n" + "It is O's Turn";
                        WinnerSelection(TileCollection);
                    }
                }
            }

            if (!won && counter == 9)
            {
                StatusText = "Tie: No one won for this game.\n 😥";
            }
            if (won)
            {
                StatusText = StatusText + "\nStart a new game to continue playing.";
            }

            // serialize the gameData structure to a stream
            formatter.Serialize(stream, gameData);

            // retrieve a Byte array from the stream
            sendBytes = stream.ToArray();

            // send the serialized data
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException)
            {
                StatusText = DateTime.Now + ":" + " ERROR: Message not sent!\n";
                return;
            }
        }

        // this is the thread that waits for incoming messages
        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    // wait for data
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);

                    // check to see if this is synchronization data 
                    // ignore it. we should not recieve any sychronization
                    // data here, because synchronization data should have 
                    // been consumed by the SynchWithOtherPlayer thread. but, 
                    // it is possible to get 1 last synchronization byte, which we
                    // want to ignore
                    if (receiveData.Length < 2)
                        continue;

                    GameData gameData;

                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();

                    // deserialize data back into our GameData structure
                    stream = new System.IO.MemoryStream(receiveData);
                    gameData = (GameData)formatter.Deserialize(stream);

                    // update view data through our bound properties
                    if (!won && !TileCollection[gameData.data1].TileFlag)
                    {
                        if (_remotePort%2 == 0 && counter % 2 == 1)
                        {

                            TileCollection[gameData.data1].TileLabel = "X";
                            TileCollection[gameData.data1].TileBrush = Brushes.Green;
                            TileCollection[gameData.data1].TileFlag = true;
                            _buttonPresses[gameData.data1]++;

                            StatusText = "User Selected Button " + gameData.data1.ToString() + "\n" + "It is O's Turn";
                            counter++;
                            WinnerSelection(TileCollection);
                        }
                        else if (_remotePort%2 == 1 && counter % 2 == 0)
                        {
                            TileCollection[gameData.data1].TileLabel = "O";
                            TileCollection[gameData.data1].TileBrush = Brushes.Red;
                            TileCollection[gameData.data1].TileFlag = true;
                            _buttonPresses[gameData.data1]++;

                            StatusText = "User Selected Button " + gameData.data1.ToString() + "\n" + "It is X's Turn";
                            counter++;
                            WinnerSelection(TileCollection);
                        }
                        else if (TileCollection[gameData.data1].TileFlag)
                        {
                            StatusText = "OOPS, this button has already been selected!";
                        }
                    }
                    if (won)
                    {
                        StatusText = StatusText + "\nStart a new game to continue playing.";

                    }
                    else if (!won && counter == 9)
                    {
                        StatusText = "Tie: No one won for this game.\n 😥";
                    }
                    // update status window
                    //StatusText = DateTime.Now + ":" + " New message received.\n";
                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    Console.WriteLine(ex.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }

            }
        }

        /// <summary>
        /// this thread is used at initialization to synchronize with the other
        /// UDP peer
        /// </summary>
        private void SynchWithOtherPlayer()
        {

            // set up socket for sending synch byte to UDP peer
            // we can't use the same socket (i.e. _dataSocket) in the same thread context in this manner
            // so we need to set up a separate socket here
            Byte[] data = new Byte[1];
            IPEndPoint endPointSend = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            IPEndPoint endPointRecieve = new IPEndPoint(IPAddress.Any, 0);

            UdpClient synchSocket = new UdpClient((int)_localPort + 10);

            // set timeout of receive to 1 second
            _dataSocket.Client.ReceiveTimeout = 1000;

            while (true)
            {
                try
                {
                    synchSocket.Send(data, data.Length, endPointSend);
                    _dataSocket.Receive(ref endPointRecieve);

                    // got something, so break out of loop
                    break;
                }
                catch (SocketException ex)
                {
                    // we get an exception if there was a timeout
                    // if we timed out, we just go back and try again
                    if (ex.ErrorCode == (int)SocketError.TimedOut)
                    {
                        Debug.Write(ex.ToString());
                    }
                    else
                    {
                        // we did not time out, but got a really bad 
                        // error
                        synchSocket.Close();
                        StatusText = "Socket exception occurred. Unable to sync with other UDP peer.\n";
                        StatusText += ex.ToString();
                        return;
                    }
                }
                catch (System.ObjectDisposedException ex)
                {
                    // something bad happened. close the socket and return
                    Console.WriteLine(ex.ToString());
                    synchSocket.Close();
                    StatusText = "Error occurred. Unable to sync with other UDP peer.\n";
                    return;
                }

            }

            // send synch byte
            synchSocket.Send(data, data.Length, endPointSend);

            // close the socket we used to send periodic requests to player 2
            synchSocket.Close();

            // reset the timeout for the dataSocket to infinite
            // _dataSocket will be used to recieve data from other UDP peer
            _dataSocket.Client.ReceiveTimeout = 0;

            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();


            // got this far, so we received a response from player 2
            StatusText = "Game has started. Player 1 (O) goes first.";
            SendEnabled = true;
        }


        /// <summary>
        /// processes all buttons. called from view when a button is clicked
        /// </summary>
        /// <param name="buttonSelected"></param>
        /// <returns></returns>
        public void UserSelection(String buttonSelected)
        {
            int index = int.Parse(buttonSelected);
            _data1 = index;
            SendMessage();
        }

        /// <summary>
        /// resets all buttons back to their starting point
        /// </summary>
        /// <param name></param>
        /// <returns></returns>
        public void Clear()
        {
            for (int x = 0; x < _numTiles; x++)
            {
                TileCollection[x].TileBrush = Brushes.Black;
                TileCollection[x].TileLabel = " ";
                _buttonPresses[x] = 0;
                TileCollection[x].TileFlag = false;
            }
            won = false;
            counter = 0;
            StatusText = "New Game! \n It is O's Turn";
        }

        /// <summary>
        /// Logic for Winning and Tying 
        /// </summary>
        /// <param name></param>
        /// <returns></returns>
        public void WinnerSelection(ObservableCollection<Tile> TileCollection)
        {
            String a = TileCollection[0].TileLabel;
            String b = TileCollection[1].TileLabel;
            String c = TileCollection[2].TileLabel;
            String d = TileCollection[3].TileLabel;
            String e = TileCollection[4].TileLabel;
            String f = TileCollection[5].TileLabel;
            String g = TileCollection[6].TileLabel;
            String h = TileCollection[7].TileLabel;
            String i = TileCollection[8].TileLabel;

            if ((a == "O" && e == "O" && i == "O") || (a == "X" && e == "X" && i == "X"))
            {
                DisplayWin(a);
                TileCollection[0].TileBrush = Brushes.Navy;
                TileCollection[4].TileBrush = Brushes.Navy;
                TileCollection[8].TileBrush = Brushes.Navy;
            }
            else if ((c == "O" && e == "O" && g == "O") || (c == "X" && e == "X" && g == "X"))
            {
                DisplayWin(c);
                TileCollection[2].TileBrush = Brushes.Navy;
                TileCollection[4].TileBrush = Brushes.Navy;
                TileCollection[6].TileBrush = Brushes.Navy;
            }
            else if ((a == "O" && b == "O" && c == "O") || (a == "X" && b == "X" && c == "X"))
            {
                DisplayWin(a);
                TileCollection[0].TileBrush = Brushes.Navy;
                TileCollection[1].TileBrush = Brushes.Navy;
                TileCollection[2].TileBrush = Brushes.Navy;
            }
            else if ((d == "O" && e == "O" && f == "O") || (d == "X" && e == "X" && f == "X"))
            {
                DisplayWin(d);
                TileCollection[3].TileBrush = Brushes.Navy;
                TileCollection[4].TileBrush = Brushes.Navy;
                TileCollection[5].TileBrush = Brushes.Navy;
            }
            else if ((g == "O" && h == "O" && i == "O") || (g == "X" && h == "X" && i == "X"))
            {
                DisplayWin(g);
                TileCollection[6].TileBrush = Brushes.Navy;
                TileCollection[7].TileBrush = Brushes.Navy;
                TileCollection[8].TileBrush = Brushes.Navy;
            }
            else if ((a == "O" && d == "O" && g == "O") || (a == "X" && d == "X" && g == "X"))
            {
                DisplayWin(a);
                TileCollection[0].TileBrush = Brushes.Navy;
                TileCollection[3].TileBrush = Brushes.Navy;
                TileCollection[6].TileBrush = Brushes.Navy;
            }
            else if ((b == "O" && e == "O" && h == "O") || (b == "X" && e == "X" && h == "X"))
            {
                DisplayWin(b);
                TileCollection[1].TileBrush = Brushes.Navy;
                TileCollection[4].TileBrush = Brushes.Navy;
                TileCollection[7].TileBrush = Brushes.Navy;
            }
            else if ((c == "O" && f == "O" && i == "O") || (c == "X" && f == "X" && i == "X"))
            {
                DisplayWin(c);
                TileCollection[2].TileBrush = Brushes.Navy;
                TileCollection[5].TileBrush = Brushes.Navy;
                TileCollection[8].TileBrush = Brushes.Navy;
            }
            else if (counter == 9 && !won)
            {
                won = false;
                for (int x = 0; x < _numTiles; x++)
                {
                    TileCollection[x].TileBrush = Brushes.White;
                }
                StatusText = "It's a Draw\n😥";
            }
        }
        public void DisplayWin(String x)
        {
            if (x == "O")
            {
                StatusText = "won: Player 1\n🤩";
            }
            else
            {
                StatusText = "won: Player 2\n🤩";
            }
            counter= 9;
            won = true;
        }

    }
}