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

        //Just for a more readable code
        int black = (int) LogicalBoard.PawnState.Black; 
        int white = (int) LogicalBoard.PawnState.White;

        public bool WhiteTurn { get; set; }

        public Game(int size, Player player1, Player player2)
        {
            //Init main classes
            whitePlayer = player1;
            blackPlayer = player2;

            board = new LogicalBoard(size);

            //Add center firsts pawns
            int[,] values = board.Values;
            int center = (int) values.GetLength(0) / 2 - 1;

            board[center, center] = black;
            board[center + 1, center + 1] = black;
            board[center, center + 1] = white;
            board[center + 1, center] = white;

            //White begin
            WhiteTurn = true;
        }

        public void PlayMove(int column, int line, bool isWhite)
        {
            //Find code associated
            int playerColor = CalculateColor(isWhite);

            //Add pawn
            board[column, line] = playerColor;

            //Calculate other pawns
            CalculateBoardConsequences(column, line, isWhite);

            //Calculate score
            CalculateScore();

            //Change turn
            WhiteTurn = !isWhite;
        }

        private int CalculateColor(bool isWhite)
        {
            if (isWhite)
                return white;
            else
                return black;
        }

        private void CalculateBoardConsequences(int column, int line, bool isWhite)
        {
            int playerColor = CalculateColor(isWhite);
            int opponentColor = CalculateColor(!isWhite);

            List<Tuple<int, int>> listPawnToReturn = new List<Tuple<int, int>>();

            listPawnToReturn.AddRange(CheckDirection(column, line, playerColor, opponentColor, 1, 0));      //Right
            listPawnToReturn.AddRange(CheckDirection(column, line, playerColor, opponentColor, -1, 0));     //Left
            listPawnToReturn.AddRange(CheckDirection(column, line, playerColor, opponentColor, 0, 1));      //Up
            listPawnToReturn.AddRange(CheckDirection(column, line, playerColor, opponentColor, 0, -1));     //Down

            listPawnToReturn.AddRange(CheckDirection(column, line, playerColor, opponentColor, 1, 1));      //Right/Up
            listPawnToReturn.AddRange(CheckDirection(column, line, playerColor, opponentColor, -1, -1));    //Left/Down
            listPawnToReturn.AddRange(CheckDirection(column, line, playerColor, opponentColor, 1, -1));     //Right/Down
            listPawnToReturn.AddRange(CheckDirection(column, line, playerColor, opponentColor, -1, 1));     //Left/Up

            foreach (Tuple<int, int> pawnToReturn in listPawnToReturn)
            {
                board[pawnToReturn.Item1, pawnToReturn.Item2] = playerColor;
            }
        }

        private List<Tuple<int, int>> CheckDirection(int column, int line, int playerColor, int opponentColor, int incrementX, int incrementY)
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
                    if (eventuallyReturned.Any()) //If list is not empty, clear it because it's invalid
                        eventuallyReturned.Clear();//May be useless now
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

        private void CalculateScore()
        {
            whitePlayer.score = 0;
            blackPlayer.score = 0;

            int[,] logicalBoard = board.Values;
            for (int i = 0; i < logicalBoard.GetLength(0); i++)
            {
                for (int j = 0; j < logicalBoard.GetLength(1); j++)
                {
                    if (board[i, j] == white)
                        whitePlayer.score++;
                    else if (board[i, j] == black)
                        blackPlayer.score++;
                }
            }
        }

        internal int[,] getBoard()
        {
            return board.Values;
        }

        internal int getWhiteScore()
        {
            return whitePlayer.score;
        }

        internal int getBlackScore()
        {
            return blackPlayer.score;
        }
    }
}
