using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWPF.Models
{
    class Board
    {
        //values: 0 for white 1 for black and -1 for empty tiles
        public const int IS_WHITE = 0;
        public const int IS_BLACK = 1;
        public const int IS_EMPTY = -1;

        int[,] values;

        public Board(int size)
        {
            values = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    values[i, j] = -1;
                }
            }
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
