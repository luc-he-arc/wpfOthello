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
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //TODO en dynamique
            int rowCount = 8;            
            int colCount = rowCount;

            UniformGrid board = Board;
            board.Children.Clear();

            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    ChessSquareControl square = new ChessSquareControl();
                    square.HorizontalAlignment = HorizontalAlignment.Stretch;
                    square.VerticalAlignment = VerticalAlignment.Stretch;
                    square.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(onBoardClick));
                    board.Children.Add(square);
                }
            }
        }

        private void onBoardClick(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Clicked");
        }
    }
}
