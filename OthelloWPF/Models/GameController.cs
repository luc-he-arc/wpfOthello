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
            return game.PlayMove(column, line);
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

        public bool IsWhiteTurn()
        {
            return game.IsWhiteTurn;
        }

        //*     Saving      *//

        public void SaveGame(string fileName)
        {
            Tools.SerializeObject(game, fileName);
        }

        public void LoadGame(string path)
        {
            game = Tools.DeSerializeObject<Game>(path);
        }
    }
}
