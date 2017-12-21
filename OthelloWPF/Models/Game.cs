using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWPF.Models
{
    class Game
    {
        Player player1;
        Player player2;

        Board board;

        public Game(Player player1, Player player2)
        {
            this.player1 = player1;
            this.player2 = player2;

            board = new Board();
        }

        public void Play()
        {

        }
    }
}
