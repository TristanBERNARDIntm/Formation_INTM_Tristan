using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class PrimeNumbers
    {
        static bool IsPrime(int valeur)
        {
            int r = 0;
            
            r = valeur % 2;
            if (valeur == 0 || valeur == 1 || r == 0 && valeur != 2)
            {
                return false;
            }
            else if (valeur == 2)
            { 
                return true;
            }
            else
            {
                double v = Math.Sqrt(valeur);
                int rac = Convert.ToInt32(v);
                for (int i = 3; i <= rac; i++)
                {
                    r = valeur % i;
                    if (r == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static void DisplayPrimes()
        {

            for (int val = 1; val <= 100; val++)
            {
                PrimeNumbers.IsPrime(val);
                if (IsPrime(val) == true)
                {
                    Console.WriteLine($"{val}");
                }
            }      
        }
    }
}
