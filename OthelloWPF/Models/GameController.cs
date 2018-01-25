using System;
using System.Windows.Controls.Primitives;

namespace OthelloWPF.Models
{
    class GameController
    {
        Game game;                                  //Logical game holder
        public Game Game { get { return game; } }   
        UniformGrid graphicalBoard;                 //UI Board

        public GameController(Game game, UniformGrid graphicalBoard)
        {
            this.game = game;
            this.graphicalBoard = graphicalBoard;
        }

        //*     Public     *//

        /// <summary>
        /// Called when a user play a move on [column, line] coordinates
        /// </summary>
        /// <param name="column">the x coordinate</param>
        /// <param name="line">the y coordinate</param>
        /// <returns>false if the game is over</returns>
        public bool PlayMove(int column, int line)
        {
            return game.PlayMove(column, line);
        }

        /// <summary>
        /// Ask if [column, line] move is playable
        /// </summary>
        /// <param name="column">the x coordinate</param>
        /// <param name="line">the y coordinate</param>
        /// <returns>true if the move is playable</returns>
        public bool IsPlayable(int column, int line)
        {
            return game.IsPlayable(column, line, game.IsWhiteTurn);
        }

        //*      Getters      *//

        /// <summary>
        /// Get logical board
        /// </summary>
        /// <returns>an int[,] logical board</returns>
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
    }
}
