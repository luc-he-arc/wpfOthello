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
        GameController gameController;              //Holder for contextual datas  
        UniformGrid graphicalBoard;                 //The window graphical board

        private DispatcherTimer updateTimeTimer;    //clock who provides the tick to update time
        private Stopwatch stopwatch;                //timer to calculate the elasped time of a player in a turn

        private const int SIZE = 8;                 //Const for the board size

        ///**       Bindings        **//

        private int scoreWhite = 0;                 //Holder score for the binding
        public int ScoreWhite {
            get
            {
                return scoreWhite;
            }
            set
            {
                scoreWhite = value;
                OnPropertyChanged("ScoreWhite");    //Call the binding update
            }
        }

        private int scoreBlack = 0;                 //Holder score for the binding
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

        /// <summary>
        /// Binding for the score to the window.
        /// Ask an update for the proprety specified
        /// </summary>
        /// <param name="propertyName">the proprety you want to update</param>
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

        /// <summary>
        /// Init all window informations:
        /// - Player names
        /// - Start update score timer
        /// </summary>
        private void InitGraphics()
        {
            InitializeComponent();
            InitBoard(SIZE, SIZE);

            NamePlayer1.Content = gameController.Game.WhitePlayer.Name;
            NamePlayer2.Content = gameController.Game.BlackPlayer.Name;
            TimerJ1.Content = "5:00";
            TimerJ2.Content = "5:00";

            //Init updateTimer
            updateTimeTimer = CreateOneSecondTimer();
            updateTimeTimer.Start();
            stopwatch = new Stopwatch();
            stopwatch.Start();

            UpdateInterface();
        }

        /// <summary>
        /// initialization of the graphical board
        /// </summary>
        /// <param name="column">size x</param>
        /// <param name="line">size y</param>
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

        /// <summary>
        /// Called when the user click on a ChessSquareControl
        /// (a case of the board)
        /// </summary>
        /// <param name="sender">the object who send the signal (ChessSquareControl)</param>
        /// <param name="e">args passed from the signal. Unused here</param>
        private void OnBoardClick(object sender, RoutedEventArgs e)
        {
            ChessSquareControl square = (ChessSquareControl) sender;

            bool gameContinue = gameController.PlayMove(square.x, square.y);
            
            UpdateInterface();

            if (!gameContinue)
            {
                //No one can play, game is over
                ShowEndMenu(false, false);
            }
        }

        /// <summary>
        /// Update all window informations
        /// </summary>
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

        /// <summary>
        /// Update the board UI depends on the logical board 
        /// </summary>
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

        /// <summary>
        /// Calculate and return the graphical board case
        /// depending on col and row provided
        /// </summary>
        /// <param name="col">the x coordinate</param>
        /// <param name="row">the y coordinate</param>
        /// <returns></returns>
        private ChessSquareControl GetChessControlFromIndex(int col, int row)
        {
            return (ChessSquareControl) graphicalBoard.Children[col * 8 + row];
        }

        // Try to adapt the position of the save button and exit button ... not working as good as we think
        private void AdaptButtonPosition(object sender, SizeChangedEventArgs e)
        {
            double temp = this.Height - (this.Height / 2) + 20;
            ExitButtonMainWindow.Margin = new Thickness(0, temp, 0, 0);
            SaveButtonMainWindow.Margin = new Thickness(0, temp, 0, 0);
        }

        /// <summary>
        /// Exit method.
        /// Ask user if he want to save
        /// </summary>
        /// <param name="sender">reference of the object who send the signal</param>
        /// <param name="e">args from the signal</param>
        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Do you want to exit without saving?", "Exiting", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        /// <summary>
        /// Method for save the game with a SaveFileDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCurrentGame(object sender, RoutedEventArgs e)
        {
            // Create dialog and ask for location for saving
            SaveFileDialog dlg = new SaveFileDialog();
            Nullable<bool> result = dlg.ShowDialog();

            if (result.HasValue)
            {
                string fileNameSave = dlg.FileName;         //Get back filename
                if(fileNameSave == "") {
                    // nothin, continue the game
                } else {
                    Tools.SerializeObjectBinary(gameController.Game, fileNameSave);//SerializeObject(gameController.Game, fileNameSave);      //Save by serialize
                }
            }
            else
                MessageBox.Show("Error while trying to save datas");
        }
      
        /// <summary>
        /// Juste create a one second update DispatcherTimer.
        /// timer will call OnTimedEvent
        /// </summary>
        /// <returns>a new one second upate DispatcherTimer who call "OnTimedEvent" function</returns>
        private DispatcherTimer CreateOneSecondTimer()
        {
            // Create a timer with a 1 second interval.
            DispatcherTimer timer = new DispatcherTimer();

            // Hook up the Elapsed event for the timer. 
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += OnTimedEvent;

            return timer;
        }

        /// <summary>
        /// Update tick.
        /// manages the event of time, calculate and update time labels
        /// </summary>
        /// <param name="source">reference of the object who send the tick. Unused</param>
        /// <param name="e">args from the signal. Unused</param>
        private void OnTimedEvent(Object source, EventArgs e)
        {
            long elapsed = stopwatch.ElapsedMilliseconds;   //Get time elapsed
            stopwatch.Restart();                            //Reset stopwatch to 0

            if (gameController.IsWhiteTurn())
            {
                //Update whitePlayer time and update label
                gameController.Game.WhitePlayer.LeftTimeMillis -= elapsed;
                TimeSpan timeleft = TimeSpan.FromMilliseconds(gameController.Game.WhitePlayer.LeftTimeMillis);
                
                TimerJ2.Content = timeleft.ToString(@"mm\:ss");

                if (gameController.Game.WhitePlayer.LeftTimeMillis <= 0)
                    ShowEndMenu(true, false);
            }
            else
            {
                //Update blackPlayer time and update label
                gameController.Game.BlackPlayer.LeftTimeMillis -= elapsed;
                TimeSpan timeleft = TimeSpan.FromMilliseconds(gameController.Game.BlackPlayer.LeftTimeMillis);

                TimerJ1.Content = timeleft.ToString(@"mm\:ss");

                if (gameController.Game.BlackPlayer.LeftTimeMillis <= 0)
                    ShowEndMenu(false, true);
            }
        }

        /// <summary>
        /// Stop update timer and show end menu windows
        /// </summary>
        /// <param name="whiteTimeOver">true if whiteplayer exceeded time provided</param>
        /// <param name="blacTimeOver">true if blackplayer exceeded time provided</param>
        private void ShowEndMenu(bool whiteTimeOver, bool blacTimeOver)
        {
            //Stop the update timer
            updateTimeTimer.Stop();

            //Which time is over
            if (whiteTimeOver)
                ScoreWhite = 0;
            else if (blacTimeOver)
                ScoreBlack = 0;

            //Change window to endmenu
            EndMenu endmenu = new EndMenu(ScoreWhite, ScoreBlack);
            endmenu.Show();
            this.Close();
        }
    }
}