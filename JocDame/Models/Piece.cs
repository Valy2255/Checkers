using JocDame.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace JocDame.Models
{

    public class Piece : INotifyPropertyChanged
    {
        private PieceColor m_color;
        private PieceType m_type;
        private string m_texture;
        private Square m_square;

        public event PropertyChangedEventHandler PropertyChanged;

        public Piece(PieceColor color)
        {
            this.m_color = color;
            m_type = PieceType.Normal;
            if (color == PieceColor.Red)
            {
                m_texture = Utility.redPiece;
            }
            else
            {
                m_texture = Utility.whitePiece;
            }
        }

        public Piece(PieceColor color, PieceType type)
        {
            m_color = color;
            m_type = type;
            if (color == PieceColor.Red)
            {
                m_texture = Utility.redPiece;
            }
            else
            {
                m_texture = Utility.whitePiece;
            }
            if (type == PieceType.King && color == PieceColor.Red)
            {
                m_texture = Utility.redKingPiece;
            }
            if (type == PieceType.King && color == PieceColor.White)
            {
                m_texture = Utility.whiteKingPiece;
            }
        }
        public PieceColor Color
        {
            get
            {
                return m_color;
            }
        }

        public PieceType Type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
                NotifyPropertyChanged("Type");
            }
        }

        public string Texture
        {
            get
            {
                return m_texture;
            }
            set
            {
                m_texture = value;
                NotifyPropertyChanged("Texture");
            }
        }

        public Square Square
        {
            get
            {
                return m_square;
            }
            set
            {
                m_square = value;
                NotifyPropertyChanged("Square");
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

        
        

