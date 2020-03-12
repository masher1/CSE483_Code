/////////////////////////////////////////////////////////////////////////////////////////////////////
// CSE483 - Tic Tac Toe
// Author - Malkiel Asher
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Brushes
using System.Windows.Media;

// observable collections
using System.Collections.ObjectModel;

// INotifyPropertyChanged
using System.ComponentModel;


// debug output
using System.Diagnostics;

namespace TicTacToe
{
    class Model : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<Tile> TileCollection;
        private static UInt32 _numTiles = 9;
        //public UInt32 _buttonPresses = 0;
        //Random _randomNumber = new Random();
        public Boolean pressed = false;
        public int counter = 0;
        private String _statusText = "";
        public String StatusText

        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }


        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
            TileCollection = new ObservableCollection<Tile>();
            for (int i = 0; i < _numTiles; i++)
            {
                TileCollection.Add(new Tile()
                {
                    TileBrush = Brushes.Black,
                    TileLabel = " ",
                    TileName = i.ToString(),
                    TileBackground = Brushes.Pink
                });
            }
        }

        public Boolean winner = false;
        /// <summary>
        /// processes all buttons. called from view when a button is clicked
        /// </summary>
        /// <param name="buttonSelected"></param>
        /// <returns></returns>
        public void UserSelection(String buttonSelected, int _buttonPresses)
        {
            int index = int.Parse(buttonSelected);
            if (!winner)
            {
                if (_buttonPresses % 2 != 0 && !TileCollection[index].Pressed)
                {
                    TileCollection[index].TileLabel = "X";
                    TileCollection[index].TileBrush = Brushes.Green;
                    TileCollection[index].Pressed = true;

                    StatusText = "User Selected Button " + index.ToString() + "\n" + "It is O's Turn";
                    counter++;
                    if (_buttonPresses > 4)
                        WinnerSelection();
                }
                else if (!TileCollection[index].Pressed)
                {
                    TileCollection[index].TileLabel = "O";
                    TileCollection[index].TileBrush = Brushes.Red;
                    TileCollection[index].Pressed = true;

                    StatusText = "User Selected Button " + index.ToString() + "\n" + "It is X's Turn";
                    counter++;
                    if (_buttonPresses > 4)
                        WinnerSelection();
                }
                else if (TileCollection[index].Pressed)
                {
                    StatusText = "OOPS, this button has alredy been selected!";
                }
            }
        }


        /// <summary>
        /// resets all buttons back to their starting point
        /// </summary>
        /// <param name></param>
        /// <returns></returns>
        public void Clear()
        {
            for (int x = 0; x < _numTiles; x++)
            {
                TileCollection[x].TileBrush = Brushes.Black;
                TileCollection[x].TileLabel = " ";
                TileCollection[x].ButtonPresses = 0;
                TileCollection[x].Pressed = false;
            }
            winner = false;
            counter = 0;
            StatusText = "New Game!";
        }

        /// <summary>
        /// Logic for Winning and Tying 
        /// </summary>
        /// <param name></param>
        /// <returns></returns>
        public Boolean WinnerSelection()
        {
/*            i % 3 = 0,1,2
            i / 3 = 0,1,2*/
            Boolean empty = false;

            if ((TileCollection[0].TileLabel == TileCollection[1].TileLabel && TileCollection[1].TileLabel == TileCollection[2].TileLabel) && (TileCollection[2].TileLabel != " ")){
                winner = true;
                TileCollection[0].TileBrush = Brushes.Navy;
                TileCollection[1].TileBrush = Brushes.Navy;
                TileCollection[2].TileBrush = Brushes.Navy;
                StatusText = TileCollection[0].TileLabel + " won on First Row!\n🤩";
            }
            else if ((TileCollection[3].TileLabel == TileCollection[4].TileLabel && TileCollection[4].TileLabel == TileCollection[5].TileLabel) && (TileCollection[3].TileLabel != " "))
            {
                winner = true;
                TileCollection[3].TileBrush = Brushes.Navy;
                TileCollection[4].TileBrush = Brushes.Navy;
                TileCollection[5].TileBrush = Brushes.Navy;
                StatusText = TileCollection[3].TileLabel + " won on Second Row!\n🤩";
            }
            else if ((TileCollection[6].TileLabel == TileCollection[7].TileLabel && TileCollection[7].TileLabel == TileCollection[8].TileLabel) && (TileCollection[6].TileLabel != " "))
            {
                winner = true;
                TileCollection[6].TileBrush = Brushes.Navy;
                TileCollection[7].TileBrush = Brushes.Navy;
                TileCollection[8].TileBrush = Brushes.Navy;
                StatusText = TileCollection[6].TileLabel + " won on Third Row!\n🤩";
            }
            else if ((TileCollection[0].TileLabel == TileCollection[3].TileLabel && TileCollection[3].TileLabel == TileCollection[6].TileLabel)  && (TileCollection[0].TileLabel != " "))
            {
                winner = true;
                TileCollection[0].TileBrush = Brushes.Navy;
                TileCollection[3].TileBrush = Brushes.Navy;
                TileCollection[6].TileBrush = Brushes.Navy;
                StatusText = TileCollection[0].TileLabel + " won on First Column!\n🤩";
            }
            else if ((TileCollection[1].TileLabel == TileCollection[4].TileLabel && TileCollection[4].TileLabel == TileCollection[7].TileLabel)  && (TileCollection[1].TileLabel != " "))
            {
                winner = true;
                TileCollection[1].TileBrush = Brushes.Navy;
                TileCollection[4].TileBrush = Brushes.Navy;
                TileCollection[7].TileBrush = Brushes.Navy;
                StatusText = TileCollection[1].TileLabel + " won on Second Column!\n🤩";
            }
            else if ((TileCollection[2].TileLabel == TileCollection[5].TileLabel && TileCollection[5].TileLabel == TileCollection[8].TileLabel)  && (TileCollection[2].TileLabel != " "))
            {
                winner = true;
                TileCollection[2].TileBrush = Brushes.Navy;
                TileCollection[7].TileBrush = Brushes.Navy;
                TileCollection[8].TileBrush = Brushes.Navy;
                StatusText = TileCollection[2].TileLabel + " won on Third Column!\n🤩";
            }
            else if ((TileCollection[0].TileLabel == TileCollection[4].TileLabel && TileCollection[4].TileLabel == TileCollection[8].TileLabel)  && (TileCollection[0].TileLabel != " "))
            {
                winner = true;
                TileCollection[0].TileBrush = Brushes.Navy;
                TileCollection[4].TileBrush = Brushes.Navy;
                TileCollection[8].TileBrush = Brushes.Navy;
                StatusText = TileCollection[0].TileLabel + " won on Diagonal Row!\n🤩";
            }
            else if ((TileCollection[2].TileLabel == TileCollection[4].TileLabel && TileCollection[4].TileLabel == TileCollection[6].TileLabel)  && (TileCollection[2].TileLabel != " "))
            {
                winner = true;
                TileCollection[2].TileBrush = Brushes.Navy;
                TileCollection[4].TileBrush = Brushes.Navy;
                TileCollection[6].TileBrush = Brushes.Navy;
                StatusText = TileCollection[2].TileLabel + " won on Inverse Diagonal Row!\n🤩";
            }
            else if(counter==9 && winner == false)
            {
                winner = false;
                for (int x = 0; x < _numTiles; x++)
                {
                    TileCollection[x].TileBrush = Brushes.White;
                }
                StatusText = "It's a Draw\n😥";
            }
            return winner;
        }

    }
}
