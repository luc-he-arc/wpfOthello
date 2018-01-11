using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            //White begin
            WhiteTurn = true;
        }

        public void PlayMove(int column, int line, bool isWhite)
        {
            //Find code associated
            int colorValue = -1;
            if (isWhite)
                colorValue = 0;
            else
                colorValue = 1;

            //Add pawn
            board[column, line] = colorValue;

            //Calculate other pawns


            //Calculate score
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

            //Change turn
            WhiteTurn = !isWhite;
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
