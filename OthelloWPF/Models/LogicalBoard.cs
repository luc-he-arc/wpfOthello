using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWPF.Models
{
    class LogicalBoard
    {
        private int[,] values;
        public int[,] Values => values;

        public enum PawnState { White = 0, Black = 1, Empty = -1 };
        public const int Size = 8;

        public LogicalBoard()
        {
            values = new int[Size, Size];

            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    Values[i, j] = (int)PawnState.Empty;
                }
            }
        }

        public void updateBoard(int[,] newValues)
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    Values[i, j] = newValues[i, j];
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