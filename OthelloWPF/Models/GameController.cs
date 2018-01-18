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
            //bool isWhite = game.IsWhiteTurn;

            //if (IsPlayable(column, line))  //Just a control. Maybe remove it
            //{
            game.PlayMove(column, line);
                //return true;
            //}

            return true;
        }

        public bool IsPlayable(int column, int line)
        {
            return game.IsPlayable(column, line, game.IsWhiteTurn);
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

        public bool isWhiteTurn()
        {
            return game.IsWhiteTurn;
        }
    }
}
