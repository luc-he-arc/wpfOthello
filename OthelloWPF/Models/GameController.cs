using System.Windows.Controls.Primitives;

namespace OthelloWPF.Models
{
    class GameController
    {
        Game game;
        UniformGrid graphicalBoard;

        public GameController(Game game, UniformGrid graphicalBoard)
        {
            this.game = game;
            this.graphicalBoard = graphicalBoard;
        }

        //*     Public     *//

        public bool PlayMove(int column, int line)
        {
            bool isWhite = game.IsWhiteTurn;

            if (IsPlayable(column, line))  //Just a control. Maybe remove it
            {
                game.PlayMove(column, line);
                return true;
            }

            return false;
        }

        public bool IsPlayable(int column, int line)
        {
            return game.IsPlayable(column, line);
        }

        //*      Getters      *//

        public int[,] GetBoard()
        {
            return game.GetBoard();
        }

        public int GetBlackScore()
        {
            return game.GetBlackScore();
        }

        public int GetWhiteScore()
        {
            return game.GetWhiteScore();
        }

        public bool WhoseTurn()
        {
            return game.IsWhiteTurn;
        }
    }
}
