using JocDame.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JocDame.Services
{
    internal class Utility
    {
        #region constValues

        // image paths
        public const string whitePiece = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\pieceWhite.png";
        public const string redPiece = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\pieceRed.png";
        public const string whiteKingPiece = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\pieceWhiteKing.png";
        public const string redKingPiece = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\pieceRedKing.png";
        public const string whiteSquare = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\squareWhite.png";
        public const string redSquare = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\squareRed.png";
        public const string hintSquare = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\squareHint.png";
        public const string HIGHLIGHT = "null";

        //const values for serialization
        public const char NO_PIECE = 'N';
        public const char WHITE_PIECE = 'W';
        public const char RED_PIECE = 'R';
        public const char RED_KING = 'K';
        public const char WHITE_KING = 'L';
        public const char WHITE_TURN = '2';
        public const char RED_TURN = '1';
        public const char HAD_COMBO = 'H';
        public const char EXTRA_PATH = 'E';

        //board constants   
        public const int boardSize = 8;
        #endregion

        #region staticValues
        public static Square CurrentSquare { get; set; }
        private static Dictionary<Square, Square> currentNeighbours = new Dictionary<Square, Square>();
        private static PlayerTurn turn = new PlayerTurn(PieceColor.Red);
        private static bool extraMove = false;
        private static bool extraPath = false;
        private static int collectedRedPieces = 0;
        private static int collectedWhitePieces = 0;
        public static bool allowMultipleJumps { get; set; } = false;
        #endregion

        public static void toggleMultipleJumps()
        {
            allowMultipleJumps = !allowMultipleJumps;
            SaveMultipleJumpsSetting();
        }

        public static Dictionary<Square, Square> CurrentNeighbours
        {
            get
            {
                return currentNeighbours;
            }
            set
            {
                currentNeighbours = value;
            }
        }

        public static PlayerTurn Turn
        {
            get
            {
                return turn;
            }
            set
            {
                turn = value;
            }
        }

        public static bool ExtraMove
        {
            get
            {
                return extraMove;
            }
            set
            {
                extraMove = value;
            }
        }

        public static bool ExtraPath
        {
            get
            {
                return extraPath;
            }
            set
            {
                extraPath = value;
            }
        }

        public static int CollectedWhitePieces
        {
            get { return collectedWhitePieces; }
            set { collectedWhitePieces = value; }
        }

        public static int CollectedRedPieces
        {
            get { return collectedRedPieces; }
            set { collectedRedPieces = value; }
        }

        #region UtilityMethods
        public static ObservableCollection<ObservableCollection<Square>> initBoard()
        {
            ObservableCollection<ObservableCollection<Square>> board = new ObservableCollection<ObservableCollection<Square>>();
            const int boardSize = 8;

            for (int row = 0; row < boardSize; ++row)
            {
                board.Add(new ObservableCollection<Square>());
                for (int column = 0; column < boardSize; ++column)
                {
                    if ((row + column) % 2 == 0)
                    {
                        board[row].Add(new Square(row, column, SquareShade.Light, null));
                    }
                    else if (row < 3)
                    {
                        board[row].Add(new Square(row, column, SquareShade.Dark, new Piece(PieceColor.White)));
                    }
                    else if (row > 4)
                    {
                        board[row].Add(new Square(row, column, SquareShade.Dark, new Piece(PieceColor.Red)));
                    }
                    else
                    {
                        board[row].Add(new Square(row, column, SquareShade.Dark, null));
                    }
                }
            }

            return board;
        }

        public static void ResetGameBoard(ObservableCollection<ObservableCollection<Square>> squares)
        {
            for (int index1 = 0; index1 < boardSize; index1++)
            {
                for (int index2 = 0; index2 < boardSize; index2++)
                {
                    if ((index1 + index2) % 2 == 0)
                    {
                        squares[index1][index2].Piece = null;
                    }
                    else
                        if (index1 < 3)
                    {
                        squares[index1][index2].Piece = new Piece(PieceColor.White);
                        squares[index1][index2].Piece.Square = squares[index1][index2];
                        //pieces
                    }
                    else
                        if (index1 > 4)
                    {
                        squares[index1][index2].Piece = new Piece(PieceColor.Red);
                        squares[index1][index2].Piece.Square = squares[index1][index2];
                        //pieces
                    }
                    else
                    {
                        squares[index1][index2].Piece = null;
                    }
                }
            }
        }
        public static bool isInBounds(int row, int column)
        {
            return row >= 0 && column >= 0 && row < boardSize && column < boardSize;
        }

        public static void initializeNeighboursToBeChecked(Square square, HashSet<Tuple<int, int>> neighboursToCheck)
        {
            if (square.Piece.Type == PieceType.King)
            {
                neighboursToCheck.Add(new Tuple<int, int>(-1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(-1, 1));
                neighboursToCheck.Add(new Tuple<int, int>(1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(1, 1));
            }
            else if (square.Piece.Color == PieceColor.Red)
            {
                neighboursToCheck.Add(new Tuple<int, int>(-1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(-1, 1));
            }
            else
            {
                neighboursToCheck.Add(new Tuple<int, int>(1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(1, 1));
            }
        }
        #endregion

        #region Serialization
        public static void LoadGame(ObservableCollection<ObservableCollection<Square>> squares)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            bool? answer = openDialog.ShowDialog();

            if (answer == true)
            {
                string path = openDialog.FileName;
                using (var reader = new StreamReader(path))
                {
                    string text;
                    //current
                    if (CurrentSquare != null)
                    {
                        CurrentSquare.Texture = redSquare;
                    }
                    text = reader.ReadLine();
                    if (text != NO_PIECE.ToString())
                    {
                        CurrentSquare = squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])];
                        CurrentSquare.Texture = HIGHLIGHT;
                    }
                    else
                    {
                        CurrentSquare = null;
                    }

                    text = reader.ReadLine();
                    if (text != NO_PIECE.ToString())
                    {
                        ExtraMove = true;
                    }
                    else
                    {
                        ExtraMove = false;
                    }
                    text = reader.ReadLine();
                    if (text != NO_PIECE.ToString())
                    {
                        ExtraPath = true;
                    }
                    else
                    {
                        ExtraPath = false;
                    }
                    //to_do_multi_JUMP
                    text = reader.ReadLine();
                    if (text == RED_TURN.ToString())
                    {
                        Turn.PlayerColor = PieceColor.Red;
                        Turn.TurnImage = redPiece;
                        
                        
                    }
                    else
                    {
                        Turn.PlayerColor = PieceColor.White;
                        Turn.TurnImage = whitePiece;
                        
                    }
                    
                    //board
                    for (int index1 = 0; index1 < boardSize; index1++)
                    {
                        text = reader.ReadLine();
                        for (int index2 = 0; index2 < boardSize; index2++)
                        {
                            squares[index1][index2].LegalSquareSymbol = null;
                            squares[index1][index2].Piece = text[index2] switch
                            {
                                NO_PIECE => null,
                                RED_PIECE => new Piece(PieceColor.Red, PieceType.Normal) { Square = squares[index1][index2] },
                                RED_KING => new Piece(PieceColor.Red, PieceType.King) { Square = squares[index1][index2] },
                                WHITE_PIECE => new Piece(PieceColor.White, PieceType.Normal) { Square = squares[index1][index2] },
                                WHITE_KING => new Piece(PieceColor.White, PieceType.King) { Square = squares[index1][index2] },
                                _ => squares[index1][index2].Piece // if no case matches, keep the original piece
                            };
                        }
                    }


                    foreach (var square in CurrentNeighbours.Keys)
                    {
                        square.LegalSquareSymbol = null;
                    }

                    CurrentNeighbours.Clear();

                    do
                    {
                        text = reader.ReadLine();
                        if (text == "-")
                        {
                            if (text.Length == 1)
                            {
                                break;
                            }
                            CurrentNeighbours.Add(squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])], null);
                        }
                        else
                        {
                            CurrentNeighbours.Add(squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])],
                                squares[(int)char.GetNumericValue(text[2])][(int)char.GetNumericValue(text[3])]);
                            //TO-DO
                        }
                    } while (text != "-");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("CollectedRedPieces:"))
                        {
                            CollectedRedPieces = int.Parse(line.Split(':')[1]);
                        }
                        else if (line.StartsWith("CollectedWhitePieces:"))
                        {
                            CollectedWhitePieces = int.Parse(line.Split(':')[1]);
                        }
                    }
                }
            }
        }

        public static void SaveGame(ObservableCollection<ObservableCollection<Square>> squares)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            bool? answer = saveDialog.ShowDialog();
            if (answer == true)
            {
                var path = saveDialog.FileName;
                using (var writer = new StreamWriter(path))
                {
                    //current
                    if (CurrentSquare != null)
                    {
                        writer.Write(CurrentSquare.Row.ToString() + CurrentSquare.Column.ToString());
                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                    }
                    writer.WriteLine();
                    if (ExtraMove)
                    {
                        writer.Write(HAD_COMBO);
                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                    }
                    writer.WriteLine();
                    if (ExtraPath)
                    {
                        writer.Write(ExtraPath);
                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                    }
                    writer.WriteLine();
                    //TO_DO_MULTI_JUMP
                    if (Turn.PlayerColor.Equals(PieceColor.Red))
                    {
                        writer.Write(RED_TURN);
                    }
                    else
                    {
                        writer.Write(WHITE_TURN);
                    }
                    writer.WriteLine();
                    //board
                    foreach (var line in squares)
                    {
                        foreach (var square in line)
                        {
                            switch (square)
                            {
                                case { } when square.Piece == null:
                                    writer.Write(NO_PIECE);
                                    break;
                                case { } when square.Piece.Color.Equals(PieceColor.Red) && square.Piece.Type == PieceType.Normal:
                                    writer.Write(RED_PIECE);
                                    break;
                                case { } when square.Piece.Color.Equals(PieceColor.White) && square.Piece.Type == PieceType.Normal:
                                    writer.Write(WHITE_PIECE);
                                    break;
                                case { } when square.Piece.Color.Equals(PieceColor.White) && square.Piece.Type == PieceType.King:
                                    writer.Write(WHITE_KING);
                                    break;
                                case { } when square.Piece.Color.Equals(PieceColor.Red) && square.Piece.Type == PieceType.King:
                                    writer.Write(RED_KING);
                                    break;
                                default:
                                    break;
                            }
                        }
                        writer.WriteLine();

                    }

                    foreach (var square in CurrentNeighbours.Keys)
                    {
                        if (CurrentNeighbours[square] == null)
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + NO_PIECE);
                        }
                        else
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + CurrentNeighbours[square].Row.ToString() + CurrentNeighbours[square].Column.ToString());
                        }
                        writer.WriteLine();
                    }
                    writer.Write("-\n");
                    writer.WriteLine($"CollectedRedPieces:{CollectedRedPieces}");
                    writer.WriteLine($"CollectedWhitePieces:{CollectedWhitePieces}");

                }
            }
        }




        public static void ResetGame(ObservableCollection<ObservableCollection<Square>> squares)
        {
            foreach (var square in CurrentNeighbours.Keys)
            {
                square.LegalSquareSymbol = null;
            }

            if (CurrentSquare != null)
            {
                CurrentSquare.Texture = redSquare;
            }

            currentNeighbours.Clear();
            CurrentSquare = null;
            ExtraMove = false;
            ExtraPath = false;
            CollectedWhitePieces = 0;
            CollectedRedPieces = 0;
            Turn.PlayerColor = PieceColor.Red;
            //texture add
            ResetGameBoard(squares);
        
        }

        public static void SaveMultipleJumpsSetting()
        {
            string path = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\gameSettings.txt";
            File.WriteAllText(path, allowMultipleJumps.ToString());
        }

        public static void LoadMultipleJumpsSetting()
        {
            string path = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\gameSettings.txt";
            if (File.Exists(path))
            {
                string settingValue = File.ReadAllText(path).Trim();
                allowMultipleJumps = bool.Parse(settingValue);
            }
        }

        #endregion

        #region Help
        public static void About()
        {
            
            string PATH = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\about.txt";

            using (var reader = new StreamReader(PATH))
            {
                MessageBox.Show(reader.ReadToEnd(), "About", MessageBoxButton.OKCancel);
            }
        }
        #endregion

        #region HandlingFiles
        public static void writeScore(int r, int w, int maxLeft) 
        {
            string PATH = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\winnerText.txt";
            using (var writer = new StreamWriter(PATH, true)) 
            {
                writer.WriteLine($"{r}, {w}, {maxLeft}");
            }
        }

        public static Winner getScore()
        {
            Winner aux = new Winner(0, 0);
            string PATH = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\winnerText.txt";
            List<string> errorMessages = new List<string>();

            if (File.Exists(PATH) && new FileInfo(PATH).Length > 0)
            {
                string[] lines = File.ReadAllLines(PATH);

                foreach (string line in lines)
                {
                    
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var splitted = line.Split(',');
                    if (splitted.Length == 3)
                    {
                        if (int.TryParse(splitted[0].Trim(), out int redWins))
                        {
                            aux.RedWins = redWins;
                        }
                        else
                        {
                            errorMessages.Add($"Error parsing RedWins from line: '{line}'");
                        }

                        if (int.TryParse(splitted[1].Trim(), out int whiteWins))
                        {
                            aux.WhiteWins = whiteWins;
                        }
                        else
                        {
                            errorMessages.Add($"Error parsing WhiteWins from line: '{line}'");
                        }

                        if (int.TryParse(splitted[2].Trim(), out int maxLeft) && maxLeft > aux.MaxPiecesLeft)
                        {
                            aux.MaxPiecesLeft = maxLeft;
                        }
                        else if (!int.TryParse(splitted[2].Trim(), out _))
                        {
                            errorMessages.Add($"Error parsing MaxPiecesLeft from line: '{line}'");
                        }
                    }
                    else
                    {
                        errorMessages.Add($"Incorrect format for line (expected 3 values separated by commas): '{line}'");
                    }
                }
            }

            // Error handling
            if (errorMessages.Any())
            {
                string errorLogPath = "C:\\aFAC\\MAP\\JocDame\\JocDame\\Resources\\errorLog.txt";
                File.AppendAllLines(errorLogPath, errorMessages);
                MessageBox.Show("Some errors occurred while processing the scores. Please check the error log.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return aux;
        }



        #endregion


    }
}
