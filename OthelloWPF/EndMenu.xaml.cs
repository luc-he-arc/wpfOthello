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
    /// Logical interaction for EndMenu.xaml
    /// </summary>
    public partial class EndMenu : Window
    {
        private int finalScoreWhite;
        private int finalScoreBlack;

        /// <summary>
        /// constructor with the last score for the black and white palyer
        /// display score in label meant for that purpose
        /// </summary>
        /// <param name="fScoreWhite"></param>
        /// <param name="fScoreBlack"></param>
        public EndMenu(int fScoreWhite, int fScoreBlack)
        {
            InitializeComponent();
            finalScoreWhite = fScoreWhite;
            finalScoreBlack = fScoreBlack;
            EndMenuScoreWhite.Content = finalScoreWhite;
            EndMenuScoreBlack.Content = finalScoreBlack;
        }

        private void ReStartGame(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }

        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
