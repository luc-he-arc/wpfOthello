using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWPF.Models
{
    class Board
    {
        const int IS_WHITE = -1;
        const int IS_EMPTY = 0;
        const int IS_BLACK = 1;
        int[,] values;

        public Board()
        {

        }


        //https://msdn.microsoft.com/en-us/library/ms182152.aspx
        public int this[int index, int index2]
        {
            get
            {
                return values[index, index2];
            }

            set
            {
                values[index, index2] = value;
            }
        }
    }
}
