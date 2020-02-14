using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media;
using MyIntegerSet;

namespace Week_4_Homework
{
    class Model : INotifyPropertyChanged
    {

        IntegerSet _set1 = new IntegerSet();
        IntegerSet _set2 = new IntegerSet();

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
            /*Set1Input = "1,2,3,4,5";
            Set2Input = "3,2,5,8,6";
            UnionText = "union";
            IntersectionText = "intersection";
            ErrorText = "Error";*/
        }

        public void Combine()
        {
            _set1.Clear();
            _set2.Clear();

            try
            {
                string[] array1 = Set1Input.Split(',');
                string[] array2 = Set2Input.Split(',');
                
                int[] arrayUno = new int[array1.Length];
                int[] arrayDos = new int[array2.Length];

                for (int i = 0; i < arrayUno.Length; i++) { 
                    arrayUno[i] = Int32.Parse(array1[i]);
                }
                for (int i = 0; i < arrayDos.Length; i++)
                {
                    arrayDos[i] = Int32.Parse(array2[i]);
                }

                ErrorText = "";
                UnionText = "";
                IntersectionText = "";

                foreach (uint i in arrayUno)
                {
                    try
                    {
                       /* if (i == null)
                        {
                            throw new System.ArgumentException("Parameter cannot be null", "Set 1");
                            return;
                        }*/
                        //uint j = uint.Parse(i);
                        _set1.InsertElement(i);
                        ErrorText = "";
                    }
                    catch (Exception e)
                    {
                        ErrorText = e.Message;
                        UnionText = "";
                        IntersectionText = "";
                    }
                }
                foreach (uint i in arrayDos)
                {
                    try
                    {
                        /*if (i == "")
                        {
                            throw new System.ArgumentException("Parameter cannot be null", "Set 2");
                            return;
                        }*/
                        //uint j = uint.Parse(i);
                        _set2.InsertElement(i);
                        ErrorText = "";
                    }
                    catch (Exception e)
                    {
                        ErrorText += "\n" + e.Message;
                        UnionText = "";
                        IntersectionText = "";
                    }
                }
            }
            catch (Exception e)
            {
                ErrorText = e.Message;
                UnionText = "";
                IntersectionText = "";
            }

            if (ErrorText == "") {
                IntersectionText = "";
                UnionText = "";
                IntegerSet intersectSet = _set1.Intersection(_set2);
                IntegerSet unionSet = _set1.Union(_set2);
                UnionText = unionSet.ToString();
                IntersectionText = intersectSet.ToString();
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
