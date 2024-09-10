using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class Factorial
    {
        public static int Factorial_(int n)
        {
            int factorielle = n;
            if (n <= 1)
            {
                return 1;
            }
            else if (n < 0)
            {
                return -1;
            }
            else
            {
                for (int i = (n-1); i >= 1; i--)
                {
                    factorielle *= i;
                }
                return factorielle;
            }
        }
        public static int FactorialRecursive(int n)
        {
            if (n <= 1)
            {
                return 1;
            }
            else if (n < 0)
            {
                return -1;
            }
            else
            {
                return n * FactorialRecursive(n - 1);
            }
        }
    }
}
