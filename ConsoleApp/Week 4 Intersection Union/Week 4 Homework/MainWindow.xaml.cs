﻿/////////////////////////////////////////////////////////////////////////////////////////////////////
// CSE483 - homework 1
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

namespace Week_4_Homework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model _mymodel;
        public MainWindow()
        {
            InitializeComponent();
            _mymodel = new Model();
            this.DataContext = _mymodel;
            //this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/myapp;component/Images/icon.png")));
        }

       
        private void Update_button_Click(object sender, RoutedEventArgs e)
        {
            //used to do the actual calculations
            _mymodel.Combine();
        }
    }
}
