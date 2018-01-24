using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWPF.Models
{
    [Serializable]
    public class Player
    {
        private string name;
        public string Name { get { return name; } set { name = value; } }

        private int score;
        public int Score { get { return score; } set { score = value; } }

        public Player() : this("Mystery player")
        {
            //Empty
        }

        public Player(string name)
        {
            this.name = name;
            score = 0;
        }
    }
}
