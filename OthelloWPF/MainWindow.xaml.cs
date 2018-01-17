using OthelloWPF.Models;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace OthelloWPF
{
    public partial class MainWindow : Window
    {
        GameController gameController;
        UniformGrid graphicalBoard;

        public MainWindow()
        {   
            //Logics
            int size = 8;
            Game game = new Game(size, new HumanPlayer(), new HumanPlayer());
            gameController = new GameController(game, graphicalBoard);
            
            //Graphics
            InitializeComponent();      
            InitBoard(size, size); 
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

            UpdateBoard();
        }

        private void OnBoardClick(object sender, RoutedEventArgs e)
        {
            bool whiteTurn = gameController.WhoseTurn();
            ChessSquareControl square = (ChessSquareControl) sender;

            bool update = gameController.PlayMove(square.x, square.y);
            if (update)
                UpdateBoard();
        }

        private void UpdateBoard()
        {
            int[,] board = gameController.GetBoard();

            //For the 2 Dimension LogicalBoard
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    //Update board values
                    int logicSquare = board[j, i];
                    ChessSquareControl graphicalSquare = GetChessControlFromIndex(i, j);

                    if (logicSquare == (int) LogicalBoard.PawnState.White)
                        graphicalSquare.SetWhite();
                    else if (logicSquare == (int) LogicalBoard.PawnState.Black)
                        graphicalSquare.SetBlack();
                    else
                        graphicalSquare.setEmpty();

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
