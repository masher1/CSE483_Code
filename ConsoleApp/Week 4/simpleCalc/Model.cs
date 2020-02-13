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

        private String _Status_Block;
        public String Status_Block
        {
            get { return _Status_Block; }
            set
            {
                _Status_Block = value;
                OnPropertyChanged("Status_Block");
            }
        }

        private float _resultBox;
        public float resultBox
        {
            get { return _resultBox; }
            set
            {
                try
                {
                    _resultBox = value;
                    Status_Block = "SIKE222";
                    OnPropertyChanged("resultBox");
                }
                catch(Exception e)
                {
                    ErrorPrint();
                }
                
            }
        }

        private float _firstNumBox;
        public float firstNumBox
        {
            get { return _firstNumBox; }
            set
            {
                try
                {
                    _firstNumBox = value;
                    Status_Block = "SIKEEEEE";
                    OnPropertyChanged("firstNumBox");
                }
                catch(Exception e)
                {
                    ErrorPrint();
                }
               
            }
        }
        
        private float _secondNumBox;
        public float secondNumBox
        {
            get { return _secondNumBox; }
            set
            {
                try
                {
                    _secondNumBox = value;
                    Status_Block = "SIKE2222";
                    OnPropertyChanged("secondNumBox");
                }
                catch(Exception e)
                {
                    ErrorPrint();
                }
                
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

        public void ErrorPrint()
        {
            //Status_Block = Brushes.Red;
            Status_Block = "Please enter a valid number!";
        }

        public void CalculateResult()
        {
            resultBox = answer;
            Status_Block = "Success";
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
