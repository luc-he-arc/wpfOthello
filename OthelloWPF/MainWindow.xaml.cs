using OthelloWPF.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System;
using System.Windows.Data;

namespace OthelloWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        GameController gameController;
        UniformGrid graphicalBoard;


        ///**       Bindings        **//
        private int scoreWhite = 0;
        public int ScoreWhite {
            get
            {
                return scoreWhite;
            }
            set
            {
                scoreWhite = value;
                OnPropertyChanged("ScoreWhite");
                
            }
        }

        private int scoreBlack = 0;
        public int ScoreBlack
        {
            get
            {
                return scoreBlack;
            }
            set
            {
                scoreBlack = value;
                OnPropertyChanged("ScoreBlack");

            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //**        Main        **//

        public MainWindow()
        {

            //Logics
            int size = 8;
            Game game = new Game(size, new HumanPlayer(), new HumanPlayer());
            gameController = new GameController(game, graphicalBoard);
            
            //Graphics
            InitializeComponent();
            double temp = this.Height - 400;
            ExitButtonMainWindow.Margin = new Thickness(0,temp, 0, 0);
            InitBoard(size, size);
            
            ScoreWhite = game.GetWhiteScore();
            ScoreBlack = game.GetBlackScore();
        }

        private void InitBoard(int column, int line)
        {
            graphicalBoard = Board;
            graphicalBoard.Children.Clear();

            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < line; j++)
                {
                    ChessSquareControl square = new ChessSquareControl
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    square.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnBoardClick));

                    square.x = j;
                    square.y = i;

                    graphicalBoard.Children.Add(square);
                }
            }

            UpdateBoard(true);

            //Show player turn
        }

        private void OnBoardClick(object sender, RoutedEventArgs e)
        {
            ChessSquareControl square = (ChessSquareControl) sender;

            bool update = gameController.PlayMove(square.x, square.y);
            UpdateBoard(update);

            ScoreWhite = gameController.GetWhiteScore();
            ScoreBlack = gameController.GetBlackScore();

            if (gameController.isWhiteTurn())
            {
                whiteTurn.Visibility = Visibility.Visible;
                blackTurn.Visibility = Visibility.Hidden;
            }
            else
            {
                whiteTurn.Visibility = Visibility.Hidden;
                blackTurn.Visibility = Visibility.Visible;
            }
        }

        private void UpdateBoard(bool update)
        {
            int[,] board = gameController.GetBoard();

            //For the 2 Dimension LogicalBoard
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    int logicSquare = board[j, i];
                    ChessSquareControl graphicalSquare = GetChessControlFromIndex(i, j);

                    if (update) //If there is changes in the logical board
                    {
                        //Update board values
                        if (logicSquare == (int)LogicalBoard.PawnState.White)
                            graphicalSquare.SetWhite();
                        else if (logicSquare == (int)LogicalBoard.PawnState.Black)
                            graphicalSquare.SetBlack();
                        else
                            graphicalSquare.setEmpty();
                    }

                    //Check each cases if they are playable
                    graphicalSquare.IsEnabled = gameController.IsPlayable(j, i);
                }
            }
        }

        private ChessSquareControl GetChessControlFromIndex(int col, int row)
        {
            return (ChessSquareControl) graphicalBoard.Children[col * 8 + row];
        }
    }
}
