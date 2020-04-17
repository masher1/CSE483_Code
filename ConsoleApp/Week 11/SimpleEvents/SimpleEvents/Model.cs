using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// threading
using System.Threading;


namespace SimpleEvents
{
    partial class Model
    {
        public class SimpleEventArgs : EventArgs
        {
            public string message;
            public SimpleEventArgs(string m)
            {
                message = m;
            }
        }

        private Thread _subscriber1Thread;
        private bool _subscriber1ThreadIsRunning = false;
        private Thread _subscriber2Thread;
        private bool _subscriber2ThreadIsRunning = false;
        private Thread _subscriber3Thread;
        private bool _subscriber3ThreadIsRunning = false;

        public delegate void SimpleEventHandler(object sender, SimpleEventArgs e);

        public event SimpleEventHandler MyMessageEvent;
        public SimpleEventHandler handler;

        Random _randomNumber;


        public Model()
        {

            handler = MyMessageEvent;

            _randomNumber = new Random();

            _subscriber1Thread = new Thread(new ThreadStart(Subscriber1ThreadFunction));
            _subscriber1ThreadIsRunning = true;
            _subscriber1Thread.Start();
            _subscriber2Thread = new Thread(new ThreadStart(Subscriber2ThreadFunction));
            _subscriber2ThreadIsRunning = true;
            _subscriber2Thread.Start();
            _subscriber3Thread = new Thread(new ThreadStart(Subscriber3ThreadFunction));
            _subscriber3ThreadIsRunning = true;
            _subscriber3Thread.Start();
        }

        public void Cleanup()
        {
            _subscriber1Thread.Abort();
            _subscriber2Thread.Abort();
            _subscriber3Thread.Abort();
        }

        public void SendButtonClicked()
        {
            if (handler != null)
                handler(this, new SimpleEventArgs(PublisherText));
        }

        void Subscriber1ThreadFunction()
        {
            handler += new SimpleEventHandler(Subscriber1Handler);

            try
            {
                while (_subscriber1ThreadIsRunning == true)
                {
                    Subscriber1Data = _randomNumber.Next(1,100).ToString();
                    Thread.Sleep(_randomNumber.Next(200,500));
                }
            }
            catch(System.Threading.ThreadAbortException)
            {
                Console.WriteLine("Thread 1 is aborted");
            }
        }

        void Subscriber1Handler(object sender, SimpleEventArgs e)
        {
            Subscriber1Text = "Subscriber 1: " + e.message;
        }

        void Subscriber2ThreadFunction()
        {
            handler += new SimpleEventHandler(Subscriber2Handler);

            try
            {
                while (_subscriber2ThreadIsRunning == true)
                {
                    Subscriber2Data = _randomNumber.Next(101,200).ToString();
                    Thread.Sleep(_randomNumber.Next(200, 500));
                }
            }
            catch(System.Threading.ThreadAbortException)
            {
                Console.WriteLine("Thread 2 is aborted");
            }
        }

        void Subscriber2Handler(object sender, SimpleEventArgs e)
        {
            Subscriber2Text = "Subscriber 2: " + e.message;
        }

        void Subscriber3ThreadFunction()
        {
            handler += new SimpleEventHandler(Subscriber3Handler);

            try
            {
                while (_subscriber3ThreadIsRunning == true)
                {
                    Subscriber3Data = _randomNumber.Next(201,300).ToString();
                    Thread.Sleep(_randomNumber.Next(200, 500));
                }
            }
            catch(System.Threading.ThreadAbortException)
            {
                Console.WriteLine("Thread 3 is aborted");
            }
        }

        void Subscriber3Handler(object sender, SimpleEventArgs e)
        {
            Subscriber3Text = "Subscriber 3: " + e.message;
        }

    }
}
