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
using PrecisionTimers;

// Rectangle
// Must update References manually
using System.Drawing;

// INotifyPropertyChanged
using System.ComponentModel;

// Threading.Timer
//using System.Windows.Threading;

// Timer.Timer
using System.Timers;

namespace Homework_3___BrickBreaker
{
    public static class ExceptionHelper
    {
        public static int LineNumber(this Exception e)
        {

            int linenum = 0;
            try
            {
                //linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));

                //For Localized Visual Studio ... In other languages stack trace  doesn't end with ":Line 12"
                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(' ')));

            }


            catch
            {
                //Stack trace is not available!
            }
            return linenum;
        }
    }// for debugging
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

        uint _pushMove = 20;
        System.Windows.Media.Brush FillColorRed;
        System.Windows.Media.Brush FillColorBlue;

        public ObservableCollection<Brick> BrickCollection;
        int _numBricks = 75;
        Rectangle[] _brickRectangles = new Rectangle[4];
        // note that the brick height, number of brick columns and rows
        // must match our window demensions.
        double _brickHeight = 50;
        double _brickWidth = 120;

        private static UInt32 _numBalls = 1;
        private UInt32[] _buttonPresses = new UInt32[_numBalls];
        Random _randomNumber = new Random();
        private TimerQueueTimer.WaitOrTimerDelegate _ballTimerCallbackDelegate;
        private TimerQueueTimer.WaitOrTimerDelegate _paddleTimerCallbackDelegate;
        //private Thread _threadA = null;
        //private Thread _threadB = null;
        //private bool _threadAIsSuspended = false;
        //private bool _threadBIsSuspended = false;
        //private bool _isThreadARunning = false;
        //private bool _isThreadBRunning = false;
        private TimerQueueTimer _ballHiResTimer;
        private TimerQueueTimer _paddleHiResTimer;
        private double _ballXMove = 1;

        internal void ResetGame()
        {
            SetStartPosition();
            //MainWindow.resetbtn_Click();
        }

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

            // this delegate is needed for the multi media timer defined 
            // in the TimerQueueTimer class.
            _ballTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(BallMMTimerCallback);
            _paddleTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(paddleMMTimerCallback);

            // create our multi-media timers
            _ballHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _ballHiResTimer.Create(1, 1, _ballTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create Ball timer. Error from GetLastError = {0}", ex.Error);
            }

            _paddleHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _paddleHiResTimer.Create(2, 2, _paddleTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create paddle timer. Error from GetLastError = {0}", ex.Error);
            }

            /*if (_threadA == null)
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
            }*/

            // create brick collection
            // place them manually at the top of the item collection in the view
            BrickCollection = new ObservableCollection<Brick>();
            for (int brick = 0; brick < _numBricks; brick++)
            {
                BrickCollection.Add(new Brick()
                {
                    BrickFill = FillColorRed,
                    BrickHeight = _brickHeight,
                    BrickWidth = _brickWidth,
                    BrickVisible = System.Windows.Visibility.Visible,
                    BrickName = brick.ToString(),
                });


                BrickCollection[brick].BrickCanvasLeft = _windowWidth / 2 - _brickWidth / 2;
                BrickCollection[brick].BrickCanvasTop = brick * _brickHeight + 150; // offset the bricks from the top of the screen by a bitg
            }

            UpdateRects();


        }
/*        /// <summary>
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
                    if ((ballCanvasLeft + ballWidth >= _windowWidth) ||
                        (ballCanvasLeft <= 0))
                        _ballXMove = -_ballXMove;


                    // check to see if ball has it the top of the drawing element
                    if (ballCanvasTop <= 0)
                        _ballYMove = -_ballYMove;

                    if (ballCanvasTop + ballWidth >= _windowHeight)
                    {
                        // we hit bottom. stop moving the ball
                        _moveBall = false;
                    }

                    // see if we hit the paddle
                    _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)ballWidth, (int)ballHeight);
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
            catch (Exception e)
            {
                int linenum = e.LineNumber();
                Debug.Write("Thread A Aborted\n", "Line: " + linenum);
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
            catch (Exception e)
            {
                int linenum = e.LineNumber();
                Debug.Write("Thread B Aborted\n", "Line: " + linenum);
            }
        }*/

        private void BallMMTimerCallback(IntPtr pWhat, bool success)
        {
            if (!_moveBall)
                return;

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_ballHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            ballCanvasLeft += _ballXMove;
            ballCanvasTop += _ballYMove;

            // check to see if ball has it the left or right side of the drawing element
            if ((ballCanvasLeft + ballWidth >= _windowWidth) ||
                (ballCanvasLeft <= 0))
                _ballXMove = -_ballXMove;


            // check to see if ball has it the top of the drawing element
            if (ballCanvasTop <= 0)
                _ballYMove = -_ballYMove;

            if (ballCanvasTop + ballWidth >= _windowHeight)
            {
                // we hit bottom. stop moving the ball
                _moveBall = false;
            }

            // see if we hit the paddle
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)ballWidth, (int)ballHeight);
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

            // done in callback. OK to delete timer
            _ballHiResTimer.DoneExecutingCallback();
        }

        private void paddleMMTimerCallback(IntPtr pWhat, bool success)
        {

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_paddleHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            if (_movepaddleLeft && paddleCanvasLeft > 0)
                paddleCanvasLeft -= 2;
            else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                paddleCanvasLeft += 2;

            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);


            // done in callback. OK to delete timer
            _paddleHiResTimer.DoneExecutingCallback();
        }



        public void CleanUp()
        {
            /*if (_threadA != null && _threadA.IsAlive)
                _threadA.Abort();
            if (_threadB != null && _threadB.IsAlive)
                _threadB.Abort();*/
            _ballHiResTimer.Delete();
            _paddleHiResTimer.Delete();
        }

        public void ProcessMouseClick(uint x, uint y)
        {
            ballCanvasLeft = x - ballWidth / 2;
            ballCanvasTop = y - ballHeight / 2;
            UpdateRects();
        }

        public void ProcessMouseDrag(uint x, uint y)
        {
            System.Windows.Point p = new System.Windows.Point();
            p.X = (double)x;
            p.Y = (double)y;
            ballCanvasLeft = x - ballWidth / 2;
            ballCanvasTop = y - ballHeight / 2;
            CheckPush();
            UpdateRects();
        }

        private void UpdateRects()
        {
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)ballWidth, (int)ballHeight);
            for (int brick = 0; brick < _numBricks; brick++)
                BrickCollection[brick].BrickRectangle = new System.Drawing.Rectangle((int)BrickCollection[brick].BrickCanvasLeft,
                    (int)BrickCollection[brick].BrickCanvasTop, (int)BrickCollection[brick].BrickWidth, (int)BrickCollection[brick].BrickHeight);
        }

        public void SetStartPosition()
        {

            ballHeight = 50;
            ballWidth = 50;

            ballCanvasLeft = _windowWidth / 2 - ballWidth / 2;
            ballCanvasTop = _windowHeight / 5;

            UpdateRects();

            paddleWidth = 120;
            paddleHeight = 10;

            ballCanvasLeft = _windowWidth / 2 - ballWidth / 2;
            ballCanvasTop = _windowHeight / 5;

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

        enum InterectSide { NONE, LEFT, RIGHT, TOP, BOTTOM };
        private InterectSide IntersectsAt(Rectangle brick, Rectangle ball)
        {
            if (brick.IntersectsWith(ball) == false)
                return InterectSide.NONE;

            Rectangle r = Rectangle.Intersect(brick, ball);

            // did we hit the top of the brick
            if (ball.Top + ball.Height - 1 == r.Top &&
                r.Height == 1)
                return InterectSide.TOP;

            if (ball.Top == r.Top &&
                r.Height == 1)
                return InterectSide.BOTTOM;

            if (ball.Left == r.Left &&
                r.Width == 1)
                return InterectSide.RIGHT;

            if (ball.Left + ball.Width - 1 == r.Left &&
                r.Width == 1)
                return InterectSide.LEFT;

            return InterectSide.NONE;
        }

        private void CheckPush()
        {
            for (int brick = 0; brick < _numBricks; brick++)
            {
                if (BrickCollection[brick].BrickFill != FillColorRed) continue;

                InterectSide whichSide = IntersectsAt(BrickCollection[brick].BrickRectangle, _ballRectangle);
                switch (whichSide)
                {
                    case InterectSide.NONE:
                        break;

                    case InterectSide.TOP:
                        BrickCollection[brick].BrickCanvasTop += _pushMove;
                        break;

                    case InterectSide.BOTTOM:
                        BrickCollection[brick].BrickCanvasTop -= _pushMove;
                        break;

                    case InterectSide.LEFT:
                        BrickCollection[brick].BrickCanvasLeft += _pushMove;
                        break;

                    case InterectSide.RIGHT:
                        BrickCollection[brick].BrickCanvasLeft -= _pushMove;
                        break;
                }
            }
        }
    }

    /*public partial class Model : INotifyPropertyChanged
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
        private TimerQueueTimer.WaitOrTimerDelegate _ballTimerCallbackDelegate;
        private TimerQueueTimer.WaitOrTimerDelegate _paddleTimerCallbackDelegate;
        private TimerQueueTimer _ballHiResTimer;
        private TimerQueueTimer _paddleHiResTimer;
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
            // this delegate is needed for the multi media timer defined 
            // in the TimerQueueTimer class.
            _ballTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(BallMMTimerCallback);
            _paddleTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(paddleMMTimerCallback);

            // create our multi-media timers
            _ballHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _ballHiResTimer.Create(1, 1, _ballTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create Ball timer. Error from GetLastError = {0}", ex.Error);
            }

            _paddleHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _paddleHiResTimer.Create(2, 2, _paddleTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create paddle timer. Error from GetLastError = {0}", ex.Error);
            }
        }

        public void CleanUp()
        {
            _ballHiResTimer.Delete();
            _paddleHiResTimer.Delete();
        }


        public void SetStartPosition()
        {

            ballHeight = 50;
            ballWidth = 50;
            paddleWidth = 120;
            paddleHeight = 10;

            ballCanvasLeft = _windowWidth / 2 - ballWidth / 2;
            ballCanvasTop = _windowHeight / 5;

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


        private void BallMMTimerCallback(IntPtr pWhat, bool success)
        {

            if (!_moveBall)
                return;

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_ballHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            ballCanvasLeft += _ballXMove;
            ballCanvasTop += _ballYMove;

            // check to see if ball has it the left or right side of the drawing element
            if ((ballCanvasLeft + ballWidth >= _windowWidth) ||
                (ballCanvasLeft <= 0))
                _ballXMove = -_ballXMove;


            // check to see if ball has it the top of the drawing element
            if (ballCanvasTop <= 0)
                _ballYMove = -_ballYMove;

            if (ballCanvasTop + ballWidth >= _windowHeight)
            {
                // we hit bottom. stop moving the ball
                _moveBall = false;
            }

            // see if we hit the paddle
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)ballWidth, (int)ballHeight);
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

            // done in callback. OK to delete timer
            _ballHiResTimer.DoneExecutingCallback();
        }

        private void paddleMMTimerCallback(IntPtr pWhat, bool success)
        {
            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_paddleHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            if (_movepaddleLeft && paddleCanvasLeft > 0)
                paddleCanvasLeft -= 2;
            else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                paddleCanvasLeft += 2;

            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);


            // done in callback. OK to delete timer
            _paddleHiResTimer.DoneExecutingCallback();
        }
    }*/
}
