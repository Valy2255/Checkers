using JocDame.Services;
using JocDame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JocDame.Models
{
    public class PlayerTurn : BaseNotification
    {
        private PieceColor color;
        private string image;

        public PlayerTurn(PieceColor color)
        {
            this.color = color;
            loadImages();
        }

        public void loadImages()
        {
            if (color == PieceColor.Red)
            {
                image = Utility.redPiece;
                return;
            }
            image = Utility.whitePiece;
        }

        public PieceColor PlayerColor
        {
            get { return color; }
            set
            {
                color = value;
                NotifyPropertyChanged("PlayerColor");
                NotifyPropertyChanged("TurnText");
            }
        }

        public string TurnImage
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                NotifyPropertyChanged("TurnImage");
            }
        }
        public string TurnText
        {
            get
            {
                return color == PieceColor.Red ? "Red's Turn" : "White's Turn";
            }
            set { }
        }
    }
}
