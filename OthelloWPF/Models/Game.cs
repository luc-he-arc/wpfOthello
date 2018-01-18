using System;
using System.Collections.Generic;
using System.Linq;
using static OthelloWPF.Models.LogicalBoard;

namespace OthelloWPF.Models
{
    class Game
    {
        Player whitePlayer;
        Player blackPlayer;

        LogicalBoard board;

        bool isWhiteTurn;
        public bool IsWhiteTurn { get { return isWhiteTurn; } set { isWhiteTurn = value; } }

        //Just for a more readable code
        int black = (int)LogicalBoard.PawnState.Black;
        int white = (int)LogicalBoard.PawnState.White;
        int empty = (int)LogicalBoard.PawnState.Empty;

        public Game(int size, Player player1, Player player2)
        {
            //Init main classes
            whitePlayer = player1;
            blackPlayer = player2;

            board = new LogicalBoard();

            //Add center firsts pawns
            int[,] values = board.Values;
            int center = (int) values.GetLength(0) / 2 - 1;

            board[center, center] = black;
            board[center + 1, center + 1] = black;
            board[center, center + 1] = white;
            board[center + 1, center] = white;

            isWhiteTurn = true;                               //White begin
        }

        public void PlayMove(int column, int line)
        {
            if (GetPossibleMoves(isWhiteTurn).Count > 0)
            {
                board[column, line] = GetColorFromTurn(isWhiteTurn);//Add pawn
                TurnPawns(column, line, isWhiteTurn);               //Turn needed pawns

                UpdateScore();                                      //Update score from board
            }

            isWhiteTurn = !isWhiteTurn;                     //Change turn
        }

        //*      Game      *//

        private void TurnPawns(int column, int line, bool isWhite)
        {
            int playerColor = GetColorFromTurn(isWhite);
            int opponentColor = GetColorFromTurn(!isWhite);

            List<Tuple<int, int>> listPawnToReturn = new List<Tuple<int, int>>();

            listPawnToReturn.AddRange(CheckPawnsInDirection(column, line, playerColor, opponentColor, 1, 0));      //Right
            listPawnToReturn.AddRange(CheckPawnsInDirection(column, line, playerColor, opponentColor, -1, 0));     //Left
            listPawnToReturn.AddRange(CheckPawnsInDirection(column, line, playerColor, opponentColor, 0, 1));      //Up
            listPawnToReturn.AddRange(CheckPawnsInDirection(column, line, playerColor, opponentColor, 0, -1));     //Down

            listPawnToReturn.AddRange(CheckPawnsInDirection(column, line, playerColor, opponentColor, 1, 1));      //Right/Up
            listPawnToReturn.AddRange(CheckPawnsInDirection(column, line, playerColor, opponentColor, -1, -1));    //Left/Down
            listPawnToReturn.AddRange(CheckPawnsInDirection(column, line, playerColor, opponentColor, 1, -1));     //Right/Down
            listPawnToReturn.AddRange(CheckPawnsInDirection(column, line, playerColor, opponentColor, -1, 1));     //Left/Up

            foreach (Tuple<int, int> pawnToReturn in listPawnToReturn)
            {
                board[pawnToReturn.Item1, pawnToReturn.Item2] = playerColor;
            }
        }

        private List<Tuple<int, int>> CheckPawnsInDirection(int column, int line, int playerColor, int opponentColor, int incrementX, int incrementY)
        {
            List<Tuple<int, int>> eventuallyReturned = new List<Tuple<int, int>>();
            int x = column + incrementX;
            int y = line + incrementY;

            bool valid = false;

            while(x >= 0 && x < board.Values.GetLength(0) && y >= 0 && y < board.Values.GetLength(1))
            {
                //We count the pieces when they are in opponent color
                if (board[x, y] == opponentColor)
                {
                    eventuallyReturned.Add(Tuple.Create(x, y));
                }
                else if (board[x, y] == playerColor)//If it's the player color
                {
                    if (eventuallyReturned.Any()) //Oponnent's pawns are between, add eventuallyReturned to listPawnToReturn
                    {
                        valid = true;
                        break;
                    }
                    else //If list is empty, just break
                    {
                        break;
                    }
                }
                else if (board[x, y] == (int)PawnState.Empty)
                {
                    if (eventuallyReturned.Any())   //If list is not empty, clear it because it's invalid
                        eventuallyReturned.Clear(); //May be useless now
                    break;
                }

                //Next case
                x += incrementX;
                y += incrementY;
            }

            if(valid)
                return eventuallyReturned;
            else
                return Enumerable.Empty<Tuple<int, int>>().ToList<Tuple<int, int>>();
        }

        public List<Tuple<int, int>> GetPossibleMoves(bool isWhiteTurn)
        {
            List<Tuple<int, int>> possibleMoves = new List<Tuple<int, int>>();

            for (int i = 0; i < board.Values.GetLength(0); i++)
            {
                for (int j = 0; j < board.Values.GetLength(1); j++)
                {
                    //if (board[i, j] == empty)
                    //{
                        if (IsPlayable(i, j))
                            possibleMoves.Add(new Tuple<int, int>(i, j));
                    //}
                }
            }

            return possibleMoves;
        }

        public bool IsPlayable(int column, int line)
        {
            bool valid = false;

            if (board[column, line] == empty)
            {
                int playerColor = GetColorFromTurn(isWhiteTurn);                        //Lisibilité + réutilisation
                int opponentColor = GetColorFromTurn(!isWhiteTurn);



                //Si une direction est valide, le coup est valide. On vérifie chacunes
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 1, 0))      //Droite
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, -1, 0))     //Gauche
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 0, 1))      //Haut
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 0, -1))     //Bas
                    valid = true;

                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 1, 1))      //Droit/Haut
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, -1, -1))    //Gauche/Bas
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 1, -1))     //Droite/Bas
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, -1, 1))     //Gauche/Haut
                    valid = true;
            }

            return valid;
        }

        private bool CheckPlayableInDirection(int column, int line, int playerColor, int opponentColor, int incrementX, int incrementY)
        {
            bool isPawnInBetween = false;
            bool isValid = false;

            //On va vérifier les cases suivantes à la notre dans la direction donnée
            int x = column + incrementX;
            int y = line + incrementY;


            while (x >= 0 && x < board.Values.GetLength(0) && y >= 0 && y < board.Values.GetLength(1))
            {
                if (board[x, y] == opponentColor)       //Si c'est la couleur du joueur adverse
                {
                    isPawnInBetween = true;             //Il y a eventuellement une piece adverse entre celui qu'on vérifie et une autre du plateau
                }
                else if (board[x, y] == playerColor)    //Si c'est la couleur du joueur courant
                {
                    if (isPawnInBetween)                //Si il y a des pieces adverses entre celle joué et l'actuelle
                    {
                        isValid = true;                 //Le coup est jouable
                        break;
                    }
                    else                                //La liste est vide. rien à retourner entre les 2 pièces
                    {
                        break;
                    }
                }
                else if (board[x, y] == empty)          //Si c'est une case vide
                {
                    break;                              //Le coup est invalide
                }

                x += incrementX;                        //Case suivante
                y += incrementY;
            }

            return isValid;
        }

        //*     Private     *//

        private void UpdateScore()
        {
            whitePlayer.Score = 0;
            blackPlayer.Score = 0;

            int[,] logicalBoard = board.Values;
            for (int i = 0; i < logicalBoard.GetLength(0); i++)
            {
                for (int j = 0; j < logicalBoard.GetLength(1); j++)
                {
                    if (board[i, j] == white)
                        whitePlayer.Score++;
                    else if (board[i, j] == black)
                        blackPlayer.Score++;
                }
            }
        }

        private int GetColorFromTurn(bool isWhiteTurn)
        {
            if (isWhiteTurn)
                return white;
            else
                return black;
        }

        //*      Getters      *//

        public int[,] GetBoard()
        {
            return board.Values;
        }

        public int GetWhiteScore()
        {
            return whitePlayer.Score;
        }

        public int GetBlackScore()
        {
            return blackPlayer.Score;
        }
    }
}