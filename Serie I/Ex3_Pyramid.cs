using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class Pyramid
    {
        public static void PyramidConstruction(int n, bool isSmooth)
        {
            int j = 1;
            int i = 1;
            int r = 0;
            for (i = 1; i <= n; i++)
            {
                r = i % 2;
                for (j = 1 ; j <= n - i ; j++)
                {
                    Console.Write(" ");
                }
                for (j = 1; j <= 2 * i - 1; j++)
                {
                    if (isSmooth == false)
                    {
                        if (r == 0)
                        { Console.Write("-"); }
                        else
                        { Console.Write("+"); }
                    }
                    else
                    {
                        Console.Write("+");
                    }
                }
                Console.Write("\n");
            }
        }
    }
}
