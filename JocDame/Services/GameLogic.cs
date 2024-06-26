﻿using JocDame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JocDame.Services
{
    public class GameLogic
    {
        private ObservableCollection<ObservableCollection<Square>> board;
        private PlayerTurn playerTurn;
        private Winner winner;
        public GameLogic(ObservableCollection<ObservableCollection<Square>> board, PlayerTurn playerTurn, Winner winner)
        {
            this.board = board;
            this.playerTurn = playerTurn;
            this.winner = winner;
            this.winner.RedWins = Utility.getScore().RedWins;
            this.winner.WhiteWins = Utility.getScore().WhiteWins;
        }
        #region Logics
        private void SwitchTurns(Square square)
        {
            if (square.Piece.Color == PieceColor.Red)
            {
                Utility.Turn.PlayerColor = PieceColor.White;
                Utility.Turn.TurnImage = Utility.whitePiece;
                playerTurn.PlayerColor = PieceColor.White;
                playerTurn.TurnImage = Utility.whitePiece;
                
            }
            else
            {
                Utility.Turn.PlayerColor = PieceColor.Red;
                Utility.Turn.TurnImage = Utility.redPiece;
                playerTurn.PlayerColor = PieceColor.Red;
                playerTurn.TurnImage = Utility.redPiece;
                
            }
         
        }

        private void FindNeighbours(Square square)
        {
            var neighboursToCheck = new HashSet<Tuple<int, int>>();

            Utility.initializeNeighboursToBeChecked(square, neighboursToCheck);

            foreach (Tuple<int, int> neighbour in neighboursToCheck)
            {
                if (Utility.isInBounds(square.Row + neighbour.Item1, square.Column + neighbour.Item2))
                {
                    if (board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece == null)
                    {
                        if (!Utility.ExtraMove)
                        {
                            Utility.CurrentNeighbours.Add(board[square.Row + neighbour.Item1][square.Column + neighbour.Item2], null);
                        }
                    }
                    else if (Utility.isInBounds(square.Row + neighbour.Item1 * 2, square.Column + neighbour.Item2 * 2) &&
                        board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece.Color != square.Piece.Color &&
                        board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2].Piece == null)
                    {
                        Utility.CurrentNeighbours.Add(board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2], board[square.Row + neighbour.Item1][square.Column + neighbour.Item2]);
                        Utility.ExtraPath = true;
                    }
                }
            }
        }

        private void DisplayRegularMoves(Square square)
        {
            if (Utility.CurrentSquare != square)
            {
                if (Utility.CurrentSquare != null)
                {
                    board[Utility.CurrentSquare.Row][Utility.CurrentSquare.Column].Texture = Utility.redSquare;

                    foreach (Square selectedSquare in Utility.CurrentNeighbours.Keys)
                    {
                        selectedSquare.LegalSquareSymbol = null;
                    }
                    Utility.CurrentNeighbours.Clear();
                }

                FindNeighbours(square);

                if (Utility.ExtraMove && !Utility.ExtraPath)
                {
                    Utility.ExtraMove = false;
                    SwitchTurns(square);
                }
                else
                {

                    foreach (Square neighbour in Utility.CurrentNeighbours.Keys)
                    {
                        board[neighbour.Row][neighbour.Column].LegalSquareSymbol = Utility.hintSquare;
                    }

                    Utility.CurrentSquare = square;
                    Utility.ExtraPath = false;
                }
            }
            else
            {
                board[square.Row][square.Column].Texture = Utility.redSquare;

                foreach (Square selectedSquare in Utility.CurrentNeighbours.Keys)
                {
                    selectedSquare.LegalSquareSymbol = null;
                }
                Utility.CurrentNeighbours.Clear();
                Utility.CurrentSquare = null;
            }
        }
        #endregion
        #region ClickCommands

        public void ResetGame()
        {
            Utility.ResetGame(board);
        }

        public void SaveGame()
        {
            Utility.SaveGame(board);
        }

      

        public void LoadGame()
        {
            Utility.LoadGame(board);
            playerTurn.PlayerColor = Utility.Turn.PlayerColor;
            playerTurn.TurnImage = Utility.Turn.TurnImage;
           

        }

        public void ShowStatistics()
        {
            Winner statistics = Utility.getScore();
            string message = $"Număr de câștigători albi: {statistics.WhiteWins}\n" +
                             $"Număr de câștigători roșii: {statistics.RedWins}\n" +
                             $"Maximul de piese rămase pe tablă: {statistics.MaxPiecesLeft}";
            MessageBox.Show(message, "Statistici Joc Dame");
        }

        public void About()
        {
            Utility.About();
        }
        public void ClickPiece(Square square)
        {
            if ((Utility.Turn.PlayerColor == PieceColor.Red && square.Piece.Color == PieceColor.Red ||
                Utility.Turn.PlayerColor == PieceColor.White && square.Piece.Color == PieceColor.White) &&
                !Utility.ExtraMove)
            {
                DisplayRegularMoves(square);
            }
        }

        public void MovePiece(Square square)
        {
            square.Piece = Utility.CurrentSquare.Piece;
            square.Piece.Square = square;

            if (Utility.CurrentNeighbours[square] != null)
            {
                Utility.CurrentNeighbours[square].Piece = null;
                Utility.ExtraMove = true;
            }
            else
            {
                Utility.ExtraMove = false;
                SwitchTurns(Utility.CurrentSquare);
            }

            board[Utility.CurrentSquare.Row][Utility.CurrentSquare.Column].Texture = Utility.redSquare;

            foreach (Square selectedSquare in Utility.CurrentNeighbours.Keys)
            {
                selectedSquare.LegalSquareSymbol = null;
            }
            Utility.CurrentNeighbours.Clear();
            Utility.CurrentSquare.Piece = null;
            Utility.CurrentSquare = null;

            if (square.Piece.Type == PieceType.Normal)
            {
                if (square.Row == 0 && square.Piece.Color == PieceColor.Red)
                {
                    square.Piece.Type = PieceType.King;
                    square.Piece.Texture = Utility.redKingPiece;
                }
                else if (square.Row == board.Count - 1 && square.Piece.Color == PieceColor.White)
                {
                    square.Piece.Type = PieceType.King;
                    square.Piece.Texture = Utility.whiteKingPiece;
                }
            }

            if (Utility.ExtraMove && Utility.allowMultipleJumps)
            {
                if (playerTurn.TurnImage == Utility.redPiece)
                {
                    Utility.CollectedWhitePieces++;
                }
                if (playerTurn.TurnImage == Utility.whitePiece)
                {
                    Utility.CollectedRedPieces++;
                }
                DisplayRegularMoves(square);
            }


            if (Utility.CollectedRedPieces == 12 || Utility.CollectedWhitePieces == 12)
            {
                GameOver();
            }
        }



        public void GameOver()
        {
            Winner aux = Utility.getScore();
            
            int totalPieces = 24;

            
            int maxPiecesLeft = totalPieces - Utility.CollectedRedPieces - Utility.CollectedWhitePieces;
            if (Utility.CollectedRedPieces == 12)
            {
                aux.WhiteWins++;
            }
            else if (Utility.CollectedWhitePieces == 12)
            {
                aux.RedWins++;
            }

            // Verificați dacă maxPiecesLeft este mai mare decât valoarea curentă și actualizați dacă este necesar
            if (maxPiecesLeft > aux.MaxPiecesLeft)
            {
                aux.MaxPiecesLeft = maxPiecesLeft;
            }

            Utility.writeScore(aux.RedWins, aux.WhiteWins, aux.MaxPiecesLeft); // Actualizați apelul

            winner.RedWins = aux.RedWins;
            winner.WhiteWins = aux.WhiteWins;
            winner.MaxPiecesLeft = aux.MaxPiecesLeft; // Actualizare

            Utility.CollectedRedPieces = 0;
            Utility.CollectedWhitePieces = 0;
            MessageBox.Show("Game over! Check the statistics for results.");
            Utility.ResetGame(board);
        }
        #endregion

    }
}
