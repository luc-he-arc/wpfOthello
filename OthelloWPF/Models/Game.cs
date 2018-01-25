using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using static OthelloWPF.Models.LogicalBoard;

namespace OthelloWPF.Models
{
    [Serializable]
    public class Game
    {
        //Players
        Player whitePlayer;         
        public Player WhitePlayer { get {return whitePlayer;} set { whitePlayer = value; } }

        Player blackPlayer;
        public Player BlackPlayer { get { return blackPlayer; } set { blackPlayer = value; } }

        //Logical board
        LogicalBoard board;
        public LogicalBoard Board { get { return board; } set { board = value; } }

        //Indicate the current player turn, true if white
        bool isWhiteTurn;
        public bool IsWhiteTurn { get { return isWhiteTurn; } set { isWhiteTurn = value; } }

        //Just for a more readable code
        int black = (int) LogicalBoard.PawnState.Black;
        int white = (int) LogicalBoard.PawnState.White;
        int empty = (int) LogicalBoard.PawnState.Empty;

        /// <summary>
        /// Used of serialization
        /// </summary>
        public Game() : this(8, new Player(), new Player())
        {
            //Empty
        } 

        //**     Main     **/

        /// <summary>
        /// Create a brand new game
        /// </summary>
        /// <param name="size">size of the logical board</param>
        /// <param name="player1">player1 Player object</param>
        /// <param name="player2">player2 Player object</param>
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

            UpdateScore();

            isWhiteTurn = false;                               //Black begin
        }

        /// <summary>
        /// Apply logic when a move is played
        /// </summary>
        /// <param name="column">move x coordinate</param>
        /// <param name="line">move y coordinate</param>
        /// <returns>false if the game is over</returns>
        public bool PlayMove(int column, int line)
        {
            board[column, line] = GetColorFromTurn(isWhiteTurn);//Add pawn
            TurnPawns(column, line, isWhiteTurn);               //Turn needed pawns

            UpdateScore();                                      //Update score from board

            if (GetPossibleMoves(!isWhiteTurn).Count > 0)       //Does next player can play?
            {
                isWhiteTurn = !isWhiteTurn;                     //Change turn
            }
            else
            {
                Console.WriteLine((!isWhiteTurn ? "Blanc" : "Noir") + " Passe son tour");

                //Check if this player can move after
                if (GetPossibleMoves(isWhiteTurn).Count <= 0)
                {
                    //No one can play. Game is over
                    Console.WriteLine((isWhiteTurn ? "Blanc" : "Noir") + " Passe son tour");
                    Console.WriteLine("Fin de la partie");

                    return false;
                }
            }

            return true;
        }

        //*      Privates       *//

        /// <summary>
        /// Calculate the consequences of a move.
        /// Turn needed pawns.
        /// </summary>
        /// <param name="column">move x coordinate</param>
        /// <param name="line">move y coordinate</param>
        /// <param name="isWhite">current player turn</param>
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

        /// <summary>
        /// Check pawns to return in specified direction.
        /// if incrementX=1 and icrementY=1, will check diagonal Up/Right.
        /// </summary>
        /// <param name="column">move x coordinate</param>
        /// <param name="line">move y coordinate</param>
        /// <param name="playerColor">color of the current player</param>
        /// <param name="opponentColor">color of the current opponent player</param>
        /// <param name="incrementX">direction increment x</param>
        /// <param name="incrementY">direction increment y</param>
        /// <returns>a list of the pawns to return</returns>
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

        /// <summary>
        /// Get all possible moves with the current logical board and the given turn
        /// </summary>
        /// <param name="isWhiteTurn">current player turn</param>
        /// <returns>all possible moves</returns>
        private List<Tuple<int, int>> GetPossibleMoves(bool isWhiteTurn)
        {
            List<Tuple<int, int>> possibleMoves = new List<Tuple<int, int>>();

            for (int i = 0; i < board.Values.GetLength(0); i++)
            {
                for (int j = 0; j < board.Values.GetLength(1); j++)
                {
                    if (IsPlayable(i, j, isWhiteTurn))
                        possibleMoves.Add(new Tuple<int, int>(i, j));
                }
            }

            return possibleMoves;
        }

        /// <summary>
        /// Check if a pawn is playable in one direction
        /// if incrementX=1 and icrementY=1, will check diagonal Up/Right.
        /// </summary>
        /// <param name="column">move x coordinate</param>
        /// <param name="line">move y coordinate</param>
        /// <param name="playerColor">color of the current player</param>
        /// <param name="opponentColor">color of the current opponent player</param>
        /// <param name="incrementX">direction increment x</param>
        /// <param name="incrementY">direction increment y</param>
        /// <returns>true if the direction is playable</returns>
        private bool CheckPlayableInDirection(int column, int line, int playerColor, int opponentColor, int incrementX, int incrementY)
        {
            bool isPawnInBetween = false;
            bool isValid = false;

            //We will check the following boxes to ours in the given direction
            int x = column + incrementX;
            int y = line + incrementY;


            while (x >= 0 && x < board.Values.GetLength(0) && y >= 0 && y < board.Values.GetLength(1))
            {
                if (board[x, y] == opponentColor)       //If it is the color of the opposing player
                {
                    isPawnInBetween = true;             //There is possibly an enemy piece between the one we check and another of the board
                }
                else if (board[x, y] == playerColor)    //If it's the color of the current player
                {
                    if (isPawnInBetween)                //If there are opposing pieces between the one played and the current one
                    {
                        isValid = true;                 //The play is playable
                        break;
                    }
                    else                                //The list is empty. nothing to return between the 2 chips
                    {
                        break;
                    }
                }
                else if (board[x, y] == empty)          //If it's an empty case
                {
                    break;                              //The play is not palyable
                }

                x += incrementX;                        //next case
                y += incrementY;
            }

            return isValid;
        }

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

        /// <summary>
        /// Check if the [column, line] move is playable on logical board
        /// </summary>
        /// <param name="column">x coordinate</param>
        /// <param name="line">y coordinate</param>
        /// <param name="isWhite">current player turn</param>
        /// <returns></returns>
        public bool IsPlayable(int column, int line, bool isWhite)
        {
            bool valid = false;

            if (board[column, line] == empty)
            {
                int playerColor = GetColorFromTurn(isWhite);
                int opponentColor = GetColorFromTurn(!isWhite);

                //If a direction is valid, the move is valid. We check each
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 1, 0))      //Right
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, -1, 0))     //Left
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 0, 1))      //Up
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 0, -1))     //Down
                    valid = true;

                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 1, 1))      //Right/Up
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, -1, -1))    //Left/Down
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, 1, -1))     //Right/Down
                    valid = true;
                if (CheckPlayableInDirection(column, line, playerColor, opponentColor, -1, 1))     //Left/Up
                    valid = true;
            }

            return valid;
        }

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