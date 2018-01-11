using System;
using System.Collections.Generic;

namespace OthelloWPF.Models
{
    class Game
    {
        Player whitePlayer;
        Player blackPlayer;

        LogicalBoard board;

        public bool WhiteTurn { get; set; }

        public Game(int size, Player player1, Player player2)
        {
            this.whitePlayer = player1;
            this.blackPlayer = player2;

            board = new LogicalBoard(size);

            //Add center firsts pawns
            int[,] values = board.Values;
            int center = (int) values.GetLength(0) / 2 - 1;

            board[center, center] = LogicalBoard.IS_BLACK;
            board[center + 1, center + 1] = LogicalBoard.IS_BLACK;
            board[center, center + 1] = LogicalBoard.IS_WHITE;
            board[center + 1, center] = LogicalBoard.IS_WHITE;

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
                return LogicalBoard.IS_WHITE;
            else
                return LogicalBoard.IS_BLACK;
        }

        private void CalculateBoardConsequences(int column, int line, bool isWhite)
        {
            int playerColor = CalculateColor(isWhite);
            int opponentColor = CalculateColor(!isWhite);

            List<Tuple<int, int>> listPawnToReturn = new List<Tuple<int, int>>();

            //Right from the clicked square
            for (int i = column+1; i < board.Values.GetLength(0); i++)
            {
                List<Tuple<int, int>> eventuallyReturned = new List<Tuple<int, int>>();

                //We count the pieces when they are in another color
                if (board[i, line] == opponentColor)
                {
                    eventuallyReturned.Add(Tuple.Create(i, line));
                }
                else if (board[i, line] == playerColor)//If it's the color and the list is empty, return, otherwise add eventuallyReturned to listPawnToReturn
                {
                    //if(eventuallyReturned.Any())
                }
            }

            throw new NotImplementedException();
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
                    if (board[i, j] == LogicalBoard.IS_WHITE)
                        whitePlayer.score++;
                    else if (board[i, j] == LogicalBoard.IS_BLACK)
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
