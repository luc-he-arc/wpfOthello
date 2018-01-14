using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWPF.Models
{
    class LogicalBoard
    {
        public enum PawnState { White = 0, Black = 1, Empty = -1};

        public int[,] Values { get; }

        public LogicalBoard(int size)
        {
            Values = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Values[i, j] = (int) PawnState.Empty;
                }
            }
        }


        //https://msdn.microsoft.com/en-us/library/ms182152.aspx
        public int this[int index, int index2]
        {
            get
            {
                return Values[index, index2];
            }

            set
            {
                Values[index, index2] = value;
            }
        }
    }
}
