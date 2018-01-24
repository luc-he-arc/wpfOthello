using Microsoft.Win32;
using OthelloWPF.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace OthelloWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        GameController gameController;
        UniformGrid graphicalBoard;

        private DispatcherTimer updateTimeTimer;
        private Stopwatch stopwatch;

        private const int SIZE = 8;

        ///**       Bindings        **//
        ///
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

        /// <summary>
        /// New game
        /// </summary>
        /// <param name="namePlayer1">Name of the player1</param>
        /// <param name="namePlayer2">Name of the player2</param>
        public MainWindow(String namePlayer1, String namePlayer2)
        {
            //Logics
            Game game = new Game(SIZE, new Player(namePlayer1), new Player(namePlayer2));
            gameController = new GameController(game, graphicalBoard);

            //Graphics
            InitGraphics();
        }

        /// <summary>
        /// Load a saved game
        /// </summary>
        /// <param name="namePlayer1">Name of the player1</param>
        /// <param name="namePlayer2">Name of the player2</param>
        /// <param name="fileName">The path for the data of the game saved you want to load</param>
        public MainWindow(String fileName)
        {
            //Load game            
            Game game = Tools.DeSerializeObjectBinary<Game>(fileName);//DeSerializeObject<Game>(fileName);
            gameController = new GameController(game, graphicalBoard);

            //Update window datas
            InitGraphics();
        }

        private void InitGraphics()
        {
            InitializeComponent();
            InitBoard(SIZE, SIZE);

            NamePlayer1.Content = gameController.Game.WhitePlayer.Name;
            NamePlayer2.Content = gameController.Game.BlackPlayer.Name;

            //Init updateTimer
            updateTimeTimer = CreateOneSecondTimer();
            updateTimeTimer.Start();
            stopwatch = new Stopwatch();
            stopwatch.Start();

            UpdateInterface();
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
            ChessSquareControl square = (ChessSquareControl) sender;

            bool gameContinue = gameController.PlayMove(square.x, square.y);

            UpdateInterface();

            if (!gameContinue)
            {
                EndMenu endmenu = new EndMenu();
                endmenu.Show();
                this.Close();
            }
        }

        private void UpdateInterface()
        {
            UpdateBoard();

            ScoreWhite = gameController.GetWhiteScore();
            ScoreBlack = gameController.GetBlackScore();

            if (gameController.IsWhiteTurn())
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

        private void UpdateBoard()
        {
            int[,] board = gameController.GetBoard();

            //For the 2 Dimension LogicalBoard
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    int logicSquare = board[j, i];
                    ChessSquareControl graphicalSquare = GetChessControlFromIndex(i, j);

                    //Update board values
                    if (logicSquare == (int)LogicalBoard.PawnState.White)
                        graphicalSquare.SetWhite();
                    else if (logicSquare == (int)LogicalBoard.PawnState.Black)
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

        private void AdaptButtonPosition(object sender, SizeChangedEventArgs e)
        {
            double temp = this.Height - (this.Height / 2) + 80;
            ExitButtonMainWindow.Margin = new Thickness(0, temp, 0, 0);
            SaveButtonMainWindow.Margin = new Thickness(0, temp, 0, 0);
        }

        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Do you want to exit without saving?", "Exiting", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
            
        }

        private void SaveCurrentGame(object sender, RoutedEventArgs e)
        {
            // Create dialog and ask for location for saving
            SaveFileDialog dlg = new SaveFileDialog();
            Nullable<bool> result = dlg.ShowDialog();

            if (result.HasValue)
            {
                string fileNameSave = dlg.FileName;         //Get back filename
                Tools.SerializeObjectBinary(gameController.Game, fileNameSave);//SerializeObject(gameController.Game, fileNameSave);      //Save by serialize
            }
            else
                MessageBox.Show("Error while trying to save datas");
        }

        private DispatcherTimer CreateOneSecondTimer()
        {
            // Create a timer with a 1 second interval.
            DispatcherTimer timer = new DispatcherTimer();

            // Hook up the Elapsed event for the timer. 
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += OnTimedEvent;

            return timer;
        }

        private void OnTimedEvent(Object source, EventArgs e)
        {
            long elapsed = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            //Console.WriteLine(elapsed);
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);

            if (gameController.IsWhiteTurn())
            {
                gameController.Game.WhitePlayer.LeftTimeMillis -= elapsed;
                TimeSpan timeleft = TimeSpan.FromMilliseconds(gameController.Game.WhitePlayer.LeftTimeMillis);
                
                TimerJ2.Content = timeleft.ToString(@"mm\:ss");
            }
            else
            {
                gameController.Game.BlackPlayer.LeftTimeMillis -= elapsed;
                TimeSpan timeleft = TimeSpan.FromMilliseconds(gameController.Game.BlackPlayer.LeftTimeMillis);

                TimerJ1.Content = timeleft.ToString(@"mm\:ss");
            }
        }
    }
}