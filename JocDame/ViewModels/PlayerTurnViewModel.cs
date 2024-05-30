using JocDame.Models;
using JocDame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JocDame.ViewModels
{
    public class PlayerTurnViewModel: BaseNotification
    {
        private GameLogic gameLogic;
        private PlayerTurn playerTurn;

        public PlayerTurnViewModel(GameLogic gameLogic, PlayerTurn playerTurn)
        {
            this.gameLogic = gameLogic;
            this.playerTurn = playerTurn;
        }

        public PlayerTurn PlayerTextIcon
        {
            get
            {
                return playerTurn;
            }
            set
            {
                playerTurn = value;
                NotifyPropertyChanged("PlayerTextIcon");
            }
        }
    }
}
