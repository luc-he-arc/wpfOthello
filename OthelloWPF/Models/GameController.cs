using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace OthelloWPF.Models
{
    class GameController : IPlayable
    {
        Game game;
        UniformGrid graphicalBoard;

        public GameController(Game game, UniformGrid graphicalBoard)
        {
            this.game = game;
            this.graphicalBoard = graphicalBoard;
        }

        public bool WhoseTurn()
        {
            return game.WhiteTurn;
        }

        //*      IPlayable      *//
        public int[,] GetBoard()
        {
            return game.getBoard();
        }

        public int GetBlackScore()
        {
            return game.getBlackScore();
        }

        public int GetWhiteScore()
        {
            return game.getWhiteScore();
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            //If AI
            throw new NotImplementedException();
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            //TODO
            //throw new NotImplementedException();
            return true;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            if (IsPlayable(column, line, isWhite))
            {
                game.PlayMove(column, line, isWhite);
                return true;
            }

            return false;
        }

        /*  Unused here */

        public string GetName()
        {
            throw new NotImplementedException();
        }
    }
}
