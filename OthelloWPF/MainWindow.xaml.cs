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
            //Graphics
            InitializeComponent();

            int rowCount = 8;            
            int colCount = rowCount;

            graphicalBoard = Board;
            graphicalBoard.Children.Clear();

            //Logics
            Game game = new Game(rowCount, new HumanPlayer(), new HumanPlayer());
            gameController = new GameController(game, graphicalBoard);

            //Generate board
            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    ChessSquareControl square = new ChessSquareControl
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    square.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(onBoardClick));

                    square.x = i;
                    square.y = j;

                    graphicalBoard.Children.Add(square);
                }
            }
        }

        private void onBoardClick(object sender, RoutedEventArgs e)
        {
            bool whiteTurn = gameController.WhoseTurn();
            ChessSquareControl square = (ChessSquareControl) sender;

            bool update = gameController.PlayMove(square.x, square.y, whiteTurn);

            if (update)
                if (whiteTurn)
                    square.setBlack();
                else
                    square.SetWhite();
        }
    }
}
