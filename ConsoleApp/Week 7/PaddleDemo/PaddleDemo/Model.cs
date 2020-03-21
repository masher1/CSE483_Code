using System;

// observable collections

// debug output

// timer, sleep
using System.Threading;

// Rectangle
// Must update References manually

// INotifyPropertyChanged
using System.ComponentModel;

// Threading.Timer

// Timer.Timer

namespace PaddleDemo
{
    public partial class Model : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static UInt32 _numBalls = 1;
        private UInt32[] _buttonPresses = new UInt32[_numBalls];
        Random _randomNumber = new Random();
        private double _ballXMove = 1;
        private double _ballYMove = 1;
        System.Drawing.Rectangle _ballRectangle;
        System.Drawing.Rectangle _paddleRectangle;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        bool _moveBallLeftClick = false;
        bool _moveBallRightClick = false;
        uint _paddleMoveSize = 10;


        private bool _moveBall = false;
        public bool MoveBall
        {
            get { return _moveBall; }
            set { _moveBall = value; }
        }


#if THREADING_TIMER
        // .NET Threading.Timer
        System.Threading.Timer _paddleTimer;
        // Create a delegate that invokes methods for the timer.
        TimerCallback _paddleTimerCB;
#else
        // .NET Timers.Timer
        System.Timers.Timer _paddleTimer;
#endif

        private double _windowHeight = 100;
        public double WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

        private double _windowWidth = 100;
        public double WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
        }

        public void InitModel()
        {
#if THREADING_TIMER
            // Create an inferred delegate that invokes methods for the timer.
            _paddleTimerCB = paddleTimerCallback;
            // Create a timer that signals the delegate to invoke 
            _paddleTimer = new System.Threading.Timer(_paddleTimerCB, null, 5, 5);
#else
            _paddleTimer = new System.Timers.Timer(5);
            _paddleTimer.Elapsed += new ElapsedEventHandler(paddleTimerHandler);
            _paddleTimer.Start();

#endif

            // how far does the paddle move (pixels)
            _paddleMoveSize = 5;
        }
        private Boolean _leftMouseButtonStatus = false;
        public Boolean leftMouseButtonStatus
        {
            get { return _leftMouseButtonStatus; }
            set
            {
                _moveBallLeftClick = true;

                _leftMouseButtonStatus = value;
                OnPropertyChanged("leftMouseButtonStatus");
            }
        }

        private Boolean _rightMouseButtonStatus = false;
        public Boolean rightMouseButtonStatus
        {
            get { return _rightMouseButtonStatus; }
            set
            {
                _moveBallRightClick = true;
                _rightMouseButtonStatus = value;
                OnPropertyChanged("rightMouseButtonStatus");
            }
        }
        public void CleanUp()
        {
        }


        public void SetStartPosition()
        {
            paddleWidth = 120;
            paddleHeight = 50;
            BallHeight = 50;
            BallWidth = 50;

            ballCanvasLeft = _windowWidth / 2 - BallWidth / 2;
            ballCanvasTop = _windowHeight / 5;

            paddleCanvasLeft = _windowWidth - paddleWidth / 2;
            paddleCanvasTop = _windowHeight / 2 - paddleHeight;
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
        }

        public void MoveLeft(bool move)
        {
            _movepaddleLeft = move;
        }

        public void MoveRight(bool move)
        {
            _movepaddleRight = move;
        }

        public void ClickLeft(bool move)
        {
            _moveBallLeftClick = move;
        }

        public void ClickRight(bool move)
        {
            _moveBallRightClick = move;
        }

#if THREADING_TIMER
        private void paddleTimerCallback(Object stateInfo)
#else
        private void paddleTimerHandler(object source, ElapsedEventArgs e)
#endif
        {
#if !THREADING_TIMER
            Console.WriteLine(e.SignalTime.ToString());
#endif
            if (_movepaddleLeft && paddleCanvasLeft > 0)
                paddleCanvasLeft -= _paddleMoveSize;
            else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                paddleCanvasLeft += _paddleMoveSize;

            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
        }

        private void BallMMTimerCallback(IntPtr pWhat, bool success)
        {

            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
            if (_ballRectangle.IntersectsWith(_paddleRectangle))
            {
                // hit paddle. reverse direction in Y direction
                paddleCanvasLeft -= _paddleMoveSize;


            }

        }

    }
}
