using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWPF.Models
{
    class Player
    {
        private string name;
        public string Name { get { return name; } set { name = value; } }

        private int score;
        public int Score { get { return score; } set { score = value; } }


        public Player(string name = "Mystery player")
        {
            this.name = name;
            score = 0;
        }
    }
}
