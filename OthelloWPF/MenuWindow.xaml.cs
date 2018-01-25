using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OthelloWPF
{
    /// <summary>
    /// Menu with tree option , start game, load a game, exit the game 
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void OpenMainWindow(object sender, RoutedEventArgs e)
        {
            String name1 = "Player1";
            String name2 = "Player2";

            if (NamePlayer1.Text != "")
                name1 = NamePlayer1.Text;
            if (NamePlayer2.Text != "")
                name2 = NamePlayer2.Text;

            MainWindow mainW = new MainWindow(name1, name2);
            mainW.Show();
            this.Close();
        }

        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void LoadSave(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string fileName = dlg.FileName;
                MainWindow mainW = new MainWindow(fileName);
                mainW.Show();
                this.Close();

            }
        }
    }
}
