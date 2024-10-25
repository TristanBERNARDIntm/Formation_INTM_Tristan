using System;               
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;                   
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace Labyrinthe
{
    public class Labyrinthe
    {
        private readonly Cell[,] _cell;
                
        public Labyrinthe(int n, int m)
        {
            _cell = new Cell[n,m];
        }

        public bool IsOpen(int i, int j, int w)
        {
            return _cell[i,j].parois[w];
        }

        public bool IsMazeStart(int i, int j)
        {
            if (_cell[i,j].statut == "entrée") return true;
            return false;
        }

        public bool IsMazeEnd(int i, int j)
        {
            if (_cell[i,j].statut == "sortie") return true;
            return false;
        }

        public void Open(int i, int j, int w)
        {
            _cell[i,j].parois[w] = true;
        }

        public List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> voisins = new List<KeyValuePair<int, int>>();
            for (int ii = i - 1; ii <= i + 1; ii++) 
            {
                if (ii != i && ii >= 0)
                {
                    voisins.Add(new KeyValuePair<int, int>(ii, j));
                }
            }
            for (int jj = j - 1; jj <= j + 1; jj++)
            {
                if (jj != j && jj >= 0)
                {
                    voisins.Add(new KeyValuePair<int, int>(i, jj));
                }
            }
            return voisins;
        }

        public void Generate(KeyValuePair<int,int> kvp)
        {
            int n = kvp.Key;
            int m = kvp.Value;
            RandomCell(n,m);
            RandomNeigbour(n,m);
        }

        public void RandomCell(int n, int m)
        {
            Random rdm = new Random();
            int col = rdm.Next(0,n);
            int lig = rdm.Next(0,m);
            int par = rdm.Next(0,3);
            Open(col,lig,par);
        }

        public void RandomNeigbour(int n, int m)
        {
            Random rdm = new Random();
            List<KeyValuePair<int,int>>voisins = CloseNeighbors(n,m);
            KeyValuePair<int,int> voisin = rdm.Next(voisins.Count);
            int nv = voisin.Key;
            int mv = voisin.Value;
            if (_cell[nv,mv].visitée == true && voisins.Count != 0)
            {
                voisins.Remove(voisin);
                RandomNeigbour(n,m);
            }
            if (voisins.Count == 0)
            {
                RandomNeigbour(n-1,m-1);
            }
                

        }
    }
}
