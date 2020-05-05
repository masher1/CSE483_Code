/////////////////////////////////////////////////////////////////////////////////////////////////////
// CSE483 - Tic Tac Toe
// Author - Malkiel Asher
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

// socket setup window
using SocketSetup;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _model;
        public int _buttonPresses;
        public MainWindow()
        {
            InitializeComponent();

            // make it so the user cannot resize the window
            this.ResizeMode = ResizeMode.NoResize;

            // create an instance of our Model
            _model = new Model();
            this.DataContext = _model;

            // associate ItemControl with collection. this collection
            // contains the tiles we placed in the ItemsControl
            // the data in the Tile Colleciton will be bound to 
            // each of the UI elements on the display
            MyItemsControl.ItemsSource = _model.TileCollection;
        }

        private void SocketSetup_Button_Click(object sender, RoutedEventArgs e)
        {
            // call up socket setup windows to get setup data
            SocketSetupWindow socketSetupWindow = new SocketSetupWindow();
            socketSetupWindow.ShowDialog();

            // set title bar to be unique
            this.Title = this.Title + " " + socketSetupWindow.SocketData.LocalIPString + "@" + socketSetupWindow.SocketData.LocalPort.ToString();

            // update model with setup data
            _model.SetLocalNetworkSettings(socketSetupWindow.SocketData.LocalPort, socketSetupWindow.SocketData.LocalIPString);
            _model.SetRemoteNetworkSettings(socketSetupWindow.SocketData.RemotePort, socketSetupWindow.SocketData.RemoteIPString);

            // initialize model and get the ball rolling
            _model.InitModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // one of the buttons in our collection. need to figure out
            // which one. Since we know the button is part of a collection, we 
            // have a special way that we need to get at its bame

            var selectedButton = e.OriginalSource as FrameworkElement;
            if (selectedButton != null)
            {
                // get the currently selected item in the collection
                // which we know to be a Tile object
                // Tile has a TileName (refer to Tile.cs)
                var currentTile = selectedButton.DataContext as Tile;
                _model.UserSelection(currentTile.TileName);
            }
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            _model.Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.Model_Cleanup();
        }

        private void Signature_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Launch my Github Page
            System.Diagnostics.Process.Start("https://github.com/masher1");
        }
    }
}
