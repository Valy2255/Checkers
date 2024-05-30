using JocDame.Models;
using JocDame.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace JocDame.ViewModels
{
    public class GameViewModel
    {
        public ObservableCollection<ObservableCollection<SquareViewModel>> Board { get; set; }
        public GameLogic Logic { get; set; }
        public ButtonInteractionViewModel Interactions { get; set; }

        public WinnerViewModel WinnerViewModel { get; set; }

        public PlayerTurnViewModel PlayerTurnVM { get; set; }

        public string RED_PIECE { get; set; }
        public string WHITE_PIECE { get; set; }

        public GameViewModel()
        {
            ObservableCollection<ObservableCollection<Square>> board = Utility.initBoard();
            Utility.LoadMultipleJumpsSetting();
            PlayerTurn playerTurn = new PlayerTurn(PieceColor.Red);
            Winner winner = new Winner(0, 0);
            Logic = new GameLogic(board, playerTurn, winner);
            PlayerTurnVM = new PlayerTurnViewModel(Logic, playerTurn);
            WinnerViewModel = new WinnerViewModel(Logic, winner);
            Board = CellBoardToCellVMBoard(board);
            Interactions = new ButtonInteractionViewModel(Logic);
            RED_PIECE = Utility.redPiece;
            WHITE_PIECE = Utility.whitePiece;
           
        }


        private ObservableCollection<ObservableCollection<SquareViewModel>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<Square>> board)
        {
            ObservableCollection<ObservableCollection<SquareViewModel>> result = new ObservableCollection<ObservableCollection<SquareViewModel>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<SquareViewModel> line = new ObservableCollection<SquareViewModel>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    Square c = board[i][j];
                    SquareViewModel cellVM = new SquareViewModel(c, Logic);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }

    }
}
