using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// observable collections
using System.Collections.ObjectModel;

// debug output
using System.Diagnostics;

// timer, sleep
using System.Threading;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

// hi res timer
//using PrecisionTimers;

// Rectangle
// Must update References manually
using System.Drawing;

// INotifyPropertyChanged
using System.ComponentModel;

namespace BouncingBall
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
        //private TimerQueueTimer.WaitOrTimerDelegate _ballTimerCallbackDelegate;
        //private TimerQueueTimer.WaitOrTimerDelegate _paddleTimerCallbackDelegate;
        private Thread _threadA = null;
        private Thread _threadB = null;
        private bool _threadAIsSuspended = false;
        private bool _threadBIsSuspended = false;
        private bool _isThreadARunning = false;
        private bool _isThreadBRunning = false;
        //private TimerQueueTimer _ballHiResTimer;
        //private TimerQueueTimer _paddleHiResTimer;
        private double _ballXMove = 1;
        private double _ballYMove = 1;
        System.Drawing.Rectangle _ballRectangle;
        System.Drawing.Rectangle _paddleRectangle;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        private bool _moveBall = false;
        public bool MoveBall
        {
            get { return _moveBall; }
            set { _moveBall = value; }
        }

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
            if (_threadA == null)
            {
                _threadAIsSuspended = false;
                _threadA = new Thread(new ThreadStart(ThreadAFunction));
                _isThreadARunning = true;
                _threadA.Start();
            }
            if (_threadB == null)
            {
                _threadBIsSuspended = false;
                _threadB = new Thread(new ThreadStart(ThreadBFunction));
                _isThreadBRunning = true;
                _threadB.Start();
            }

        }
        /// <summary>
        /// Thread A
        /// </summary>
        /// 
        void ThreadAFunction()
        {
            try
            {
                while (_isThreadARunning)
                {
                    Thread.Sleep(2);
                    //if (_threadAIsSuspended) continue;

                    // below are all the things this thread needs to do
                    if (!_moveBall)
                        continue;

                    ballCanvasLeft += _ballXMove;
                    ballCanvasTop += _ballYMove;

                    // check to see if ball has it the left or right side of the drawing element
                    if ((ballCanvasLeft + BallWidth >= _windowWidth) ||
                        (ballCanvasLeft <= 0))
                        _ballXMove = -_ballXMove;


                    // check to see if ball has it the top of the drawing element
                    if (ballCanvasTop <= 0)
                        _ballYMove = -_ballYMove;

                    if (ballCanvasTop + BallWidth >= _windowHeight)
                    {
                        // we hit bottom. stop moving the ball
                        _moveBall = false;
                    }

                    // see if we hit the paddle
                    _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
                    if (_ballRectangle.IntersectsWith(_paddleRectangle))
                    {
                        // hit paddle. reverse direction in Y direction
                        _ballYMove = -_ballYMove;

                        // move the ball away from the paddle so we don't intersect next time around and
                        // get stick in a loop where the ball is bouncing repeatedly on the paddle
                        ballCanvasTop += 2 * _ballYMove;

                        // add move the ball in X some small random value so that ball is not traveling in the same 
                        // pattern
                        ballCanvasLeft += _randomNumber.Next(5);
                    }

                }
            }
            catch (ThreadAbortException)
            {
                Debug.Write("Thread A Aborted\n");
            }
        }

        /// <summary>
        /// Thread B
        /// </summary>
        void ThreadBFunction()
        {
            try
            {
                while (_isThreadBRunning)
                {
                    Thread.Sleep(2);
                    //if (_threadBIsSuspended) continue;

                    if (_movepaddleLeft && paddleCanvasLeft > 0)
                        paddleCanvasLeft -= 2;
                    else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                        paddleCanvasLeft += 2;

                    _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);


                    // done in callback. OK to delete timer
                    _threadBIsSuspended = true;
                }
            }
            catch (ThreadAbortException)
            {
                Debug.Write("Thread B Aborted\n");
            }
        }

        public void CleanUp()
        {
            if (_threadA != null && _threadA.IsAlive)
                _threadA.Abort();
            if (_threadB != null && _threadB.IsAlive)
                _threadB.Abort();
        }


        public void SetStartPosition()
        {
            
            BallHeight = 50;
            BallWidth = 50;
            paddleWidth = 120;
            paddleHeight = 10;

            ballCanvasLeft = _windowWidth/2 - BallWidth/2;
            ballCanvasTop = _windowHeight/5;
           
            _moveBall = false;

            paddleCanvasLeft = _windowWidth / 2 - paddleWidth / 2;
            paddleCanvasTop = _windowHeight - paddleHeight;
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

    }
}
