﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// observable collections
using System.Collections.ObjectModel;

// Brush
using System.Windows.Media;


namespace Canvas_ObsCollection_Project
{
    public partial class Model : INotifyPropertyChanged
    {
        // provide an observable collections for shapes
        public ObservableCollection<MyShape> RectCollection;
        public ObservableCollection<MyShape> EllipseCollection;
        public event PropertyChangedEventHandler PropertyChanged;
        public void InitModel(UInt32 height, UInt32 width)
        {

            _drawingAreaHeight = height;
            _drawingAreaWidth = width;

            // place them manually at the top of the item collection in the view
            RectCollection = new ObservableCollection<MyShape>();
            EllipseCollection = new ObservableCollection<MyShape>();
            ResetShapes();
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Random randomNumber = new Random(Guid.NewGuid().GetHashCode());
        private UInt32 _drawingAreaHeight = 100;
        private UInt32 _drawingAreaWidth = 100;

        // this property is bound to our view
        private UInt32 _numRects = 100;
        public UInt32 NumRects
        {
            get { return _numRects; }
            set
            {
                _numRects = value;
                OnPropertyChanged("NumRects");
            }
        }

        private void SetRandomRect(MyShape shape)
        {

            shape.Height = randomNumber.Next(10, 50);
            shape.Width = shape.Height;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.

            mySolidColorBrush.Color = Color.FromArgb(255, (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255));
            shape.Fill = mySolidColorBrush;
            shape.CanvasTop = randomNumber.Next(0, (int)(_drawingAreaHeight - shape.Height));
            shape.CanvasLeft = randomNumber.Next(0, (int)(_drawingAreaWidth - shape.Width));
        }

        private void SetRandomEllipse(MyShape shape)
        {

            shape.Height = randomNumber.Next(10, 50);
            shape.Width = randomNumber.Next(10, 50);
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.

            mySolidColorBrush.Color = Color.FromArgb(255, (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255));
            shape.Fill = mySolidColorBrush;
            shape.CanvasTop = randomNumber.Next(0, (int)(_drawingAreaHeight - shape.Height));
            shape.CanvasLeft = randomNumber.Next(0, (int)(_drawingAreaWidth - shape.Width));
        }

        public void ResetShapes()
        {
            RectCollection.Clear();
            EllipseCollection.Clear();

            for (int rects = 0; rects < _numRects; rects++)
            {
                MyShape temp = new MyShape();
                SetRandomRect(temp);
                RectCollection.Add(temp);
                MyShape temp2 = new MyShape();
                SetRandomEllipse(temp2);
                EllipseCollection.Add(temp2);
            }
        }

    }

}
