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
using System.Windows.Threading;
using System.Diagnostics;

namespace Homework_3___BrickBreaker{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _model;
        //private bool _leftMouseDown = false;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            stopWatch.Start();
            dispatcherTimer.Start();
            // make it so the user cannot resize the window
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // create an instance of our Model
            _model = new Model();
            _model.WindowHeight = BallCanvas.RenderSize.Height;
            _model.WindowWidth = BallCanvas.RenderSize.Width;
            this.DataContext = _model;
            _model.InitModel();
            _model.SetStartPosition();

            BrickItems.ItemsSource = _model.BrickCollection;
        }

        void dt_Tick(object sender, EventArgs e)
        {
            int seconds = 0;
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                seconds = (ts.Minutes*60) + ts.Seconds;
                currentTime = String.Format("{0:0}", seconds);
                elapsedTimeCounter.Content = currentTime;
            }
        }

        private void KeypadDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _model.MoveLeft(true);
            else if (e.Key == Key.Right)
                _model.MoveRight(true);
            else if (e.Key == Key.S)//start or pause the game
                _model.MoveBall = !_model.MoveBall;
            else if (e.Key == Key.B)
                _model.SetStartPosition();
            else if (e.Key == Key.R){
                stopWatch.Reset();
                elapsedTimeCounter.Content = "0";
                _model.ResetGame();}
            else if (e.Key == Key.E)
                this.Close();
        }

        private void KeypadUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _model.MoveLeft(false);
            else if (e.Key == Key.Right)
                _model.MoveRight(false);
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.CleanUp();
        }
    }
}
