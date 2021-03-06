﻿using System;
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

        private long leftTimeMillis;
        public long LeftTimeMillis { get { return leftTimeMillis; } set { leftTimeMillis = value; } }

        public Player() : this("Mystery player")
        {
            //Empty
        }

        public Player(string name)
        {
            this.name = name;
            score = 0;
            leftTimeMillis = 300000;    //5 min = 5 (min) * 60 (sec) * 1000 (millis) = 300'000 millis
        }
    }
}
