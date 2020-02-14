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

namespace CombinedLayouts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private enum enumShapeType { ELLIPSE, RECTANGLE };
        private Random randomNumber = new Random(Guid.NewGuid().GetHashCode());
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void setRandomShapes(Canvas e, enumShapeType t, int number)
        {
            double canvasHeight = e.ActualHeight;
            double canvasWidth = e.ActualWidth;

            Shape shape = new Ellipse();

            for (int count = 0; count < number; count++)
            {
                switch (t)
                {
                    case enumShapeType.ELLIPSE:
                        shape = new Ellipse();
                        break;

                    case enumShapeType.RECTANGLE:
                        shape = new Rectangle();
                        break;
                }
                shape.Height = randomNumber.Next(10, 25);
                shape.Width = shape.Height;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.

                mySolidColorBrush.Color = Color.FromArgb(255, (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255));
                shape.Fill = mySolidColorBrush;
                e.Children.Add(shape);
                Canvas.SetLeft(shape, randomNumber.Next(0, (int)(canvasWidth - shape.Width)));
                Canvas.SetTop(shape, randomNumber.Next(0, (int)(canvasHeight - shape.Height)));
            }
        }
        private void ResetAll_Button_Click(object sender, RoutedEventArgs e)
        {
            TopLeftCanvas.Children.Clear();
            setRandomShapes(TopLeftCanvas, enumShapeType.ELLIPSE, 25);
            TopRightCanvas.Children.Clear();
            setRandomShapes(TopRightCanvas, enumShapeType.RECTANGLE, 25);
            BottomRightCanvas.Children.Clear();
            setRandomShapes(BottomRightCanvas, enumShapeType.RECTANGLE, 25);
            BottomLeftCanvas.Children.Clear();
            setRandomShapes(BottomLeftCanvas, enumShapeType.ELLIPSE, 25);
        }

        private void ResetTL_Button_Click(object sender, RoutedEventArgs e)
        {
            TopLeftCanvas.Children.Clear();
            setRandomShapes(TopLeftCanvas, enumShapeType.ELLIPSE, 25);
        }

        private void ResetTR_Button_Click(object sender, RoutedEventArgs e)
        {
            TopRightCanvas.Children.Clear();
            setRandomShapes(TopRightCanvas, enumShapeType.RECTANGLE, 25);
        }

        private void ResetBL_Button_Click(object sender, RoutedEventArgs e)
        {
            BottomRightCanvas.Children.Clear();
            setRandomShapes(BottomRightCanvas, enumShapeType.RECTANGLE, 25);
        }

        private void ResetBR_Button_Click(object sender, RoutedEventArgs e)
        {
            BottomLeftCanvas.Children.Clear();
            setRandomShapes(BottomLeftCanvas, enumShapeType.ELLIPSE, 25);
        }
    }
}
