using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JocDame.ViewModels;

namespace JocDame.Models
{
    public class Winner : BaseNotification
    {
        private int redWins;
        private int whiteWins;
        private int maxPiecesLeft;


        public Winner(int redWins, int whiteWins)
        {
            this.redWins = redWins;
            this.whiteWins = whiteWins;
            this.maxPiecesLeft = 0;
        }

        public int RedWins
        {
            get
            {
                return redWins;
            }
            set
            {
                redWins = value;
                NotifyPropertyChanged("RedWins");
            }
        }

        public int WhiteWins
        {
            get
            {
                return whiteWins;
            }
            set
            {
                whiteWins = value;
                NotifyPropertyChanged("WhiteWins");
            }
        }

        public int MaxPiecesLeft
        {
            get
            {
                return maxPiecesLeft;
            }
            set
            {
                maxPiecesLeft = value;
                NotifyPropertyChanged("MaxPiecesLeft");
            }
        }
    }
}
