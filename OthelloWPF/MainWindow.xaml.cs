using OthelloWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

                    square.x = i;
                    square.y = j;

                    graphicalBoard.Children.Add(square);


                }
            }
        }

        private void OnBoardClick(object sender, RoutedEventArgs e)
        {
            bool whiteTurn = gameController.WhoseTurn();
            ChessSquareControl square = (ChessSquareControl) sender;

            bool update = gameController.PlayMove(square.x, square.y, whiteTurn);
            if (update)
                UpdateBoard();
        }

        private void UpdateBoard()
        {
            int[,] board = gameController.GetBoard();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    int logicSquare = board[i, j];
                    ChessSquareControl graphicalSquare = GetChessControlFromIndex(i, j);

                    if (logicSquare == LogicalBoard.IS_WHITE)
                        graphicalSquare.SetWhite();
                    else if (logicSquare == LogicalBoard.IS_BLACK)
                        graphicalSquare.SetBlack();
                    else
                        graphicalSquare.setEmpty();
                }
            }
        }

        private ChessSquareControl GetChessControlFromIndex(int col, int row)
        {
            return (ChessSquareControl) graphicalBoard.Children[col * 8 + row];
        }
    }
}
