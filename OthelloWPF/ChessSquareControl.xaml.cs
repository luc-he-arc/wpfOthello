﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OthelloWPF
{
    
    public partial class ChessSquareControl : UserControl
    {
        public int x;
        public int y;

        public ChessSquareControl()
        {
            InitializeComponent();
        }

        public void setBlack()
        {
            Image image = Image;
            image.Source = new BitmapImage(new Uri(@"/OthelloWPF;component/Images/black.png", UriKind.Relative));
        }

        public void SetWhite()
        {
            Image image = Image;
            image.Source = new BitmapImage(new Uri(@"/OthelloWPF;component/Images/white.png", UriKind.Relative));
        }
    }
}
