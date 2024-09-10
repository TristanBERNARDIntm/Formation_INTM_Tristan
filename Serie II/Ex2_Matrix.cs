using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_II
{
    public static class Matrix
    {
        public static int[][] BuildingMatrix(int[] leftVector, int[] rightVector)
        {
            //TODO
            return new int[0][];
        }

        public static int[][] Addition(int[][] leftMatrix, int[][] rightMatrix)
        {
            //TODO
            return new int[0][];
        }

        public static int[][] Substraction(int[][] leftMatrix, int[][] rightMatrix)
        {
            //TODO
            return new int[0][];
        }

        public static int[][] Multiplication(int[][] leftMatrix, int[][] rightMatrix)
        {
            //TODO
            return new int[0][];
        }

        public static void DisplayMatrix(int[][] matrix)
        {
            string s = string.Empty;
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                {
                    s += matrix[i][j].ToString().PadLeft(5) + " ";
                }
                s += Environment.NewLine;
            }
            Console.WriteLine(s);
        }
    }
}
