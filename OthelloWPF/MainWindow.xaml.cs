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
                    board.Children.Add(square);
                }
            }

            //V1
            /*Grid board = Board;

            for (int i = 0; i < colCount; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = GridLength.Auto;
                new GridLength(0, GridUnitType.Star);
                board.ColumnDefinitions.Add(col);

                for (int j = 0; j < rowCount; j++)
                {
                    RowDefinition row = new RowDefinition();
                    row.Height = GridLength.Auto;
                    board.RowDefinitions.Add(row);
                }
            }

            //https://stackoverflow.com/questions/11701749/append-a-child-to-a-grid-set-its-row-and-column
            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    ChessSquareControl square = new ChessSquareControl();
                    //square.Height = UserControl.
                    //square.Height = GridUnitType.Star;
                    board.Children.Add(square);
                    Grid.SetColumn(square, i);
                    Grid.SetRow(square, j);
                }
            }
            */
        }

        private void ChessSquareControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
