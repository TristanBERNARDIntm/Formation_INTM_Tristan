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
        private readonly bool[,,,] _open;
        private readonly bool _visited;
        private readonly string _status;
        private readonly int[,] _taille;

        public Labyrinthe()
        {
            //jsp
        }

        public bool IsOpen(int i, int j, int w)
        {
            return _cell[i,j].parois[w];
        }

        private bool IsMazeStart(int i, int j)
        {
            if (_cell[i,j].statut == "entrée") return true;
            return false;
        }

        private bool IsMazeEnd(int i, int j)
        {
            if (_cell[i,j].statut == "sortie") return true;
            return false;
        }

        public void Open(int i, int j, int w)
        {
            //todo
        }

        private List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
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

        private void Generate(KeyValuePair<int,int> kvp)
        {
            //todo
        }
    }
}
