using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_II
{
    public static class Search
    {
        public static int LinearSearch(int[] tableau, int valeur)
        {
            for (int i = 0 ; i < tableau.Length ; i++)
            {
                if (tableau[i] == valeur)
                {
                    return i;
                }
            }
            return -1;
        }

        public static int BinarySearch(int[] tableau, int valeur)
        {
            int moit = tableau.Length / 2;
            bool trouvé = false;
            while (trouvé == false)
            {
                if (tableau[moit] < valeur)
                {
                    moit += moit/2;
                    continue;
                }
                else if (tableau[moit] > valeur)
                {
                    moit -= moit/2;
                    continue;
                }
                else
                {
                    trouvé = true;
                    return moit;
                }

            }
            return -1;
        }
    }
}
