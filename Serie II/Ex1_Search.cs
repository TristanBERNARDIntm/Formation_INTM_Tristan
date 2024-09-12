//MAUVAISE VERSION DU CODE, PB PUSH




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
            int d = tableau.Length;
            int moit = tableau.Length / 2;
            int moit2 = moit;
            bool trouvé = false;
            while (trouvé == false)
            {
                if (tableau[moit] < valeur)
                {
                    moit2 = (moit+d)/2;
                }
                else if (tableau[moit] > valeur)
                {
                    d = moit;
                    moit2 = moit / 2;
                }
                else
                {
                    trouvé = true;
                    return moit2;
                }
                if (moit2 == moit)
                {
                    return -1;
                }
                moit = moit2;
            }
            return -1;
        }
    }
}
