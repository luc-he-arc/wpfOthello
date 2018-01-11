using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWPF.Models
{
    class Player
    {
        public string name { get; set; }
        public int score { get; set; }

        public Player()
        {
            name = "Paul";
            score = 0;
        }
    }
}
