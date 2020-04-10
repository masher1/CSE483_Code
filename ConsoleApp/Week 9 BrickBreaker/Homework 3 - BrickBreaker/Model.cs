/////////////////////////////////////////////////////////////////////////////////////////////////////
// CSE483 - BrickBreaker
// Author - Malkiel Asher
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

//Sound Library
using System.Media;
using System.IO;
using Microsoft.Win32;

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

        public ObservableCollection<Brick> BrickCollection;
        int _numBricks = randomBrickNums();

        private static int randomBrickNums()
        {
            Random _random = new Random();
            int _randBrickNums = 0;
            _randBrickNums = _random.Next(55, 99);
            return _randBrickNums;
        }

        Rectangle[] _brickRectangles = new Rectangle[4];

        int hitBottom = 0;

        double _brickHeight = 30;
        double _brickWidth = 80;
        
        private static UInt32 _numBalls = 1;
        private UInt32[] _buttonPresses = new UInt32[_numBalls];
        Random _randomNumber = new Random();
        private TimerQueueTimer.WaitOrTimerDelegate _ballTimerCallbackDelegate;
        private TimerQueueTimer.WaitOrTimerDelegate _paddleTimerCallbackDelegate;
        private TimerQueueTimer _ballHiResTimer;
        private TimerQueueTimer _paddleHiResTimer;
        private double _ballXMove = 1;

        internal void ResetGame()
        {
            CleanUp();
            ScoreCounter = 0;
            InitModel();
            SetStartPosition();
            Time = 0;
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

        private int _scoreCounter;
        public int ScoreCounter
        {
            get { return _scoreCounter; }
            set
            {
                _scoreCounter = value;
                OnPropertyChanged("ScoreCounter");
            }
        }

        private Visibility _GameOver;
        public Visibility GameOver
        {
            get { return _GameOver; }
            set
            {
                _GameOver = value;
                OnPropertyChanged("GameOver");
            }
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
            //start playing music at the beginning of the game
            SoundPlayer GameMusic = new SoundPlayer(@"C:\Users\malki\source\CSE483\CSE483_Code\ConsoleApp\Week 9 BrickBreaker\Homework 3 - BrickBreaker\Properties\GameMusic.wav");
            GameMusic.PlayLooping();
            GameOver = Visibility.Hidden;
            // this delegate is needed for the multi media timer defined 
            // in the TimerQueueTimer class.
            _ballTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(BallMMTimerCallback);
            _paddleTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(paddleMMTimerCallback);
            ScoreCounter = 0;
            // create our multi-media timers
            _ballHiResTimer = new TimerQueueTimer();
            hitBottom = 0; // restart the counter for number times we have hit the bottom side of the game
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

            // create brick collection
            // place them manually at the top of the item collection in the view
            BrickCollection = new ObservableCollection<Brick>();
            int counter = 0; //counter for number of rows
            for (int brick = 0; brick < _numBricks; brick++)
            {
                BrickCollection.Add(new Brick()
                {
                    BrickHeight = _brickHeight,
                    BrickWidth = _brickWidth,
                    BrickVisible = System.Windows.Visibility.Visible,
                    BrickName = brick.ToString(),
                });

                if (brick == 0)
                {
                    BrickCollection[brick].BrickCanvasLeft = 0;
                    BrickCollection[brick].BrickCanvasTop = brick * _brickHeight; // offset the bricks from the top
                }
                else if (brick % 11 == 1 && (brick - 1) != 0)//shift down one and reset side
                {
                    BrickCollection[brick].BrickCanvasLeft = BrickCollection[brick - 1].BrickCanvasLeft;
                    BrickCollection[brick].BrickCanvasTop = counter * _brickHeight;
                }
                else if ((BrickCollection[brick - 1].BrickCanvasLeft + 160) < _windowWidth)
                {
                    BrickCollection[brick].BrickCanvasLeft = BrickCollection[brick - 1].BrickCanvasLeft + 80;
                    BrickCollection[brick].BrickCanvasTop = BrickCollection[brick - 1].BrickCanvasTop;
                }
                else if ((BrickCollection[brick - 1].BrickCanvasLeft + 160) > _windowWidth)
                {
                    counter++;
                    BrickCollection[brick].BrickCanvasLeft = 0;
                    BrickCollection[brick].BrickCanvasTop = counter * _brickHeight;
                }
            }
            NETTimerTimerStart(true);
            UpdateRects();
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
                hitBottom++;
                if(hitBottom >= 3)
                {
                    GameOver = Visibility.Visible;
                    //ResetGame();
                    hitBottom = 0;
                }
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

            //see if we hit a brick
            CheckTouch();

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
            _ballHiResTimer.Delete();
            _paddleHiResTimer.Delete();
            ScoreCounter = 0;
            NETTimerTimerStart(false);
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
            ballHeight = 30;
            ballWidth = 30;

            ballCanvasLeft = _windowWidth / 2 - ballWidth / 2;
            ballCanvasTop = _windowHeight / 1.5;

            paddleWidth = 120;
            paddleHeight = 20;

            _moveBall = false;

            paddleCanvasLeft = _windowWidth / 2 - paddleWidth / 2;
            paddleCanvasTop = _windowHeight - paddleHeight;
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);

            UpdateRects();
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

            // did we hit the bottom of the brick
            if (ball.Top == r.Top &&
                r.Height == 1)
                return InterectSide.BOTTOM;

            // did we hit the right of the brick
            if (ball.Left == r.Left &&
                r.Width == 1)
                return InterectSide.RIGHT;

            // did we hit the left of the brick
            if (ball.Left + ball.Width - 1 == r.Left &&
                r.Width == 1)
                return InterectSide.LEFT;

            return InterectSide.NONE;
        }

        private void CheckTouch()
        {
            for (int brick = 0; brick < _numBricks; brick++)
            {
                if (BrickCollection[brick].BrickVisible != Visibility.Collapsed)
                {
                    InterectSide whichSide = IntersectsAt(BrickCollection[brick].BrickRectangle, _ballRectangle);
                    switch (whichSide)
                    {
                        case InterectSide.NONE:
                            break;

                        case InterectSide.TOP:
                            BrickCollection[brick].BrickVisible = Visibility.Collapsed;
                            _ballYMove = -_ballYMove;
                            ScoreCounter += 10;
                            Console.WriteLine("Hit the Top Side of Brick #" + brick + "/" + _numBricks);
                            return;

                        case InterectSide.BOTTOM:
                            BrickCollection[brick].BrickVisible = Visibility.Collapsed;
                            _ballYMove = -_ballYMove;
                            ScoreCounter += 10;
                            Console.WriteLine("Hit the Bottom Side of Brick #" + brick + "/" + _numBricks + "\n Visibility is at: " + BrickCollection[brick].BrickVisible);
                            return;

                        case InterectSide.LEFT:
                            BrickCollection[brick].BrickVisible = Visibility.Collapsed;
                            _ballXMove = -_ballXMove;
                            ScoreCounter += 10;
                            Console.WriteLine("Hit the Left Side of Brick #" + brick + "/" + _numBricks);
                            return;

                        case InterectSide.RIGHT:
                            BrickCollection[brick].BrickVisible = Visibility.Collapsed;
                            _ballXMove = -_ballXMove;
                            ScoreCounter += 10;
                            Console.WriteLine("Hit the Right Side of Brick #" + brick + "/" + _numBricks);
                            return;
                    }
                }
            }
        }

        #region .NET Timer Timer
        bool _netTimerTimerRunning = false;
        // used for measuring the period of the .NET timer timer
        uint NETTimerTimerTicks = 0;
        long NETTimerTimerTotalTime = 0;
        long NETTimerTimerPreviousTime;

        System.Timers.Timer dotNetTimerTimer;

        private uint _time = 0;
        public uint Time
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged("Time"); }
        }
        public bool NETTimerTimerStart(bool startStop)
        {
            if (startStop == true)
            {
                dotNetTimerTimer = new System.Timers.Timer(1000); // hard coded 1 second in this timer
                dotNetTimerTimer.Elapsed += new ElapsedEventHandler(NetTimerTimerHandler);
                dotNetTimerTimer.Start();

            }
            else if (_netTimerTimerRunning)
            {
                dotNetTimerTimer.Stop();
            }

            return true;
        }

        private void NetTimerTimerHandler(object source, ElapsedEventArgs e)
        {
            if (MoveBall) Time++;
        }

        #endregion
    }
}