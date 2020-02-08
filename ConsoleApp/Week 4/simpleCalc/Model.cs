using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media;



namespace simpleCalc
{
    class Model : INotifyPropertyChanged
    {

        public float answer;

        private float _resultBox;

        public float resultBox
        {
            get { return _resultBox; }
            set
            {
                _resultBox = value;
                OnPropertyChanged("resultBox");
            }
        }

        private float _firstNumBox;
        public float firstNumBox
        {
            get { return _firstNumBox; }
            set
            {
                _firstNumBox = value;
                OnPropertyChanged("firstNumBox");
            }
        }
        
        private float _secondNumBox;
        public float secondNumBox
        {
            get { return _secondNumBox; }
            set
            {
                _secondNumBox = value;
                OnPropertyChanged("secondNumBox");
            }
        }

        private String _operationNumBox;
        public String operationNumBox
        {
            get { return _operationNumBox; }
            set
            {
                _operationNumBox = value;
                OnPropertyChanged("operationNumBox");
            }
        }

        public Model()
        {
            
        }

      public void CalculateResult()
        {

            resultBox = answer;
        }

        public void SetOperation(MY_OPERATION op)
        {
            switch (op)
            {
                case MY_OPERATION.ADD:
                    answer = firstNumBox + secondNumBox;
                    operationNumBox = "ADD";
                    break;
                case MY_OPERATION.SUBTRACT:
                    answer = firstNumBox - secondNumBox;
                    operationNumBox = "SUBTRACT";
                    break;
                case MY_OPERATION.MULTIPLY:
                    answer = firstNumBox * secondNumBox;
                    operationNumBox = "MULTIPLY";
                    break;
                case MY_OPERATION.DIVIDE:
                    answer = firstNumBox / secondNumBox;
                    operationNumBox = "DIVIDE";

                    break;
                default:
                    answer = 0;
                    break;
            }
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
