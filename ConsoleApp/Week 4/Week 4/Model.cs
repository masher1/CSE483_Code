using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Week_4
{
    class Model : INotifyPropertyChanged
    {
        private string _topBoxText;
        public string TopBoxText
        {
            get { return _topBoxText; }
            set
            {
                _topBoxText = value;
                OnPropertyChanged("TopBoxText");
            }
        }

        private string _bottomBoxText;
        public string BottomBoxText
        {
            get { return _bottomBoxText; }
            set
            {
                _bottomBoxText = value;
                OnPropertyChanged("BottomBoxText");
            }
        }

        public Model()
        {
            TopBoxText = "Put Text Here!";
        }

        public void CopyText()
        {
            BottomBoxText = TopBoxText;
        }


        #region Data Binding Stuff
        // define out property chane event handler, part of the data binding
        public event PropertyChangedEventHandler PropertyChanged;

        //implemets method for data binding to any and all properties
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
