using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class Euclide
    {
        public static int Pgcd(int a, int b)
        {
            int q, r;
            if (b == 0)
            {
                return -1;
            }

            q = a / b;
            r = a % b;
            if (r == 0)
            {
                return q;
            }
            else
            {
                a = b;
                b = r;
            }
          
        }

        public static int PgcdRecursive(int a, int b)
        {
            //TODO
            return -1;
        }
    }
}
