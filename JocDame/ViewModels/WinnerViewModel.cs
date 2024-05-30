using JocDame.Models;
using JocDame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JocDame.ViewModels
{
    public class WinnerViewModel : BaseNotification
    {
        private GameLogic gameLogic;
        private Winner winner;
            
        public WinnerViewModel(GameLogic gameLogic, Winner winner)
        {
            this.gameLogic = gameLogic;
            this.winner = winner;
        }

        public Winner WinnerPlayer
        {
            get { return winner; }
            set
            {
                winner = value;
                NotifyPropertyChanged("WinnerPlayer");
            }
        }

        
    }
}
