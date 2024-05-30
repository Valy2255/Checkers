using JocDame.Commands;
using JocDame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JocDame.Models;

namespace JocDame.ViewModels
{
    public class SquareViewModel : BaseNotification
    {
        private GameLogic gameLogic;
        private Square genericSquare;
        private ICommand clickPieceCommand;
        private ICommand movePieceCommand;

        public SquareViewModel(Square square, GameLogic gameLogic)
        {
            genericSquare = square;
            this.gameLogic = gameLogic;
        }

        public Square GenericSquare
        {
            get
            {
                return genericSquare;
            }
            set
            {
                genericSquare = value;
                NotifyPropertyChanged("GenericSquare");
            }
        }

        public ICommand ClickPieceCommand
        {
            get
            {
                if (clickPieceCommand == null)
                {
                    clickPieceCommand = new RelayCommand<Square>(gameLogic.ClickPiece);
                }
                return clickPieceCommand;
            }
        }

        public ICommand MovePieceCommand
        {
            get
            {
                if (movePieceCommand == null)
                {
                    movePieceCommand = new RelayCommand<Square>(gameLogic.MovePiece);
                }
                return movePieceCommand;
            }
        }
    }
}
