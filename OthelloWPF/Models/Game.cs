using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWPF.Models
{
    class Game
    {
        Player player1;
        Player player2;

        Board board;

        public bool whiteTurn { get; set; }

        public Game(int size, Player player1, Player player2)
        {
            this.player1 = player1;
            this.player2 = player2;

            board = new Board(size);

            //White begin
            whiteTurn = true;
        }

        public void PlayMove(int column, int line, bool isWhite)
        {
            board[column, line] = TurnBoolToTurnValue(isWhite);

            //Change turn
            whiteTurn = !isWhite;
        }

        internal int getWhiteScore()
        {
            throw new NotImplementedException();
        }

        internal int getBlackScore()
        {
            throw new NotImplementedException();
        }

        private int TurnBoolToTurnValue(bool isWhiteTurn)
        {
            if (isWhiteTurn)
                return 0;
            else
                return 1;
        }
    }
}
