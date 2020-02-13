using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Week_4_Homework
{
    class Model : INotifyPropertyChanged
    {
        private string _Set1Input;

        public string Set1Input
        {
            get { return _Set1Input; }
            set
            {
                _Set1Input = value;
                OnPropertyChanged("Set1Input");
            }
        }

        private string _Set2Input;

        public string Set2Input
        {
            get { return _Set2Input; }
            set
            {
                _Set2Input = value;
                OnPropertyChanged("Set2Input");
            }
        }

        private string _unionText;
        public string UnionText
        {
            get { return _unionText; }
            set
            {
                _unionText = value;
                OnPropertyChanged("UnionText");
            }
        }
        private string _IntersectionText;
        public string IntersectionText
        {
            get { return _IntersectionText; }
            set
            {
                _IntersectionText = value;
                OnPropertyChanged("IntersectionText");
            }
        }

        private string _ErrorText;
        public string ErrorText
        {
            get { return _ErrorText; }
            set
            {
                _ErrorText = value;
                OnPropertyChanged("ErrorText");
            }
        }

        public Model()
        {
            Set1Input = "set1";
            Set2Input = "set2";
            UnionText = "union";
            IntersectionText = "intersection";
            ErrorText = "Error";
        }

        public void Union()
        {
            //combine all of the sets together without repeating
        }

        public void Intersection()
        {
            //find similarities between the two sets
        }


        #region Data Binding Stuff
        // define out property chane event handler, part of the data binding
        public event PropertyChangedEventHandler PropertyChanged;

        //implemets method for data binding to any and all properties
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
