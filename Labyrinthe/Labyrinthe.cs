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
            for (int i = 0; i < n ; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    _cell[i,j] = new Cell();
                    if (i == 0 | j == 0 | i == n-1 | j == m-1)
                    {
                        _cell[i,j].extremité = true;
                    }
                }
            }
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

        public List<KeyValuePair<int, int>> CloseNeighbors(int i, int j, int n, int m)
        {
            List<KeyValuePair<int, int>> voisins = new List<KeyValuePair<int, int>>();
            if (i > 0) voisins.Add(new KeyValuePair<int, int>(i - 1, j));
            if (i < n - 1) voisins.Add(new KeyValuePair<int, int>(i + 1, j));
            if (j > 0) voisins.Add(new KeyValuePair<int, int>(i, j - 1));
            if (j < m - 1) voisins.Add(new KeyValuePair<int, int>(i, j + 1));
            
            return voisins;
        }

        public void Generate(KeyValuePair<int,int> kvp)
        {
            Random rdm = new Random();
            Stack<KeyValuePair<int, int>> pile = new Stack<KeyValuePair<int, int>>();
            KeyValuePair<int,int> coordCell = new KeyValuePair<int, int>();
            int n = kvp.Key;
            int m = kvp.Value;

            coordCell = RandomCell(n,m);
            pile.Push(new KeyValuePair<int,int>(coordCell.Key,coordCell.Value));
            _cell[coordCell.Key,coordCell.Value].visitée = true;

            while (pile.Count > 0)
            {   
                var current = pile.Peek();
                int i = current.Key;
                int j = current.Value;
                var neighbors = CloseNeighbors(i,j,n,m).Where(a => !_cell[a.Key, a.Value].visitée).ToList();
                if (neighbors.Count != 0)
                {
                    var neighbor = neighbors[rdm.Next(neighbors.Count)];
                    int k = neighbor.Key;
                    int l = neighbor.Value;

                    if (!_cell[k, l].visitée)
                    {
                        OpenWall(i, j, k, l);
                        _cell[k, l].visitée = true;
                        pile.Push(new KeyValuePair<int, int>(k, l));
                    }
                    else
                    {
                        pile.Pop();
                    }
                } 
            }
          //  KeyValuePair<int,int> coordEntree = new KeyValuePair<int, int>(RandomStartEnd(n,m);
          //  _cell[coordEntree.Key,coordEntree.Value].statut = "entrée";
          //  KeyValuePair<int,int> coordSortie = new KeyValuePair<int, int>(RandomStartEnd(n,m);
          //  _cell[coordSortie.Key,coordSortie.Value].statut = "sortie";
        }

        public KeyValuePair<int,int> RandomCell(int n, int m)
        {
            Random rdm = new Random();
            KeyValuePair<int,int> coord = new KeyValuePair<int, int>(rdm.Next(0,n),rdm.Next(0,m));
            return coord;
        }

        public KeyValuePair<int,int> RandomStartEnd(int n, int m)
        {
            Random rdm = new Random();
            KeyValuePair<int,int> coord = new KeyValuePair<int, int>(rdm.Next(0,n),rdm.Next(0,m));
            if (_cell[coord.Key,coord.Value].extremité == true)
            return coord;
            else return RandomStartEnd(n,m);
        }

        public void OpenWall(int i, int j, int k, int l)
        {
            if (k == i + 1 && l == j)
            {
                _cell[k,l].parois[1] = true;
                _cell[i,j].parois[3] = true;
            }
            if (k == i && l == j + 1)
            {
                _cell[k,l].parois[2] = true;
                _cell[i,j].parois[0] = true;
            }
            if (k == i - 1 && l == j)
            {
                _cell[k,l].parois[3] = true;
                _cell[i,j].parois[1] = true;
            }
            if (k == i && l == j - 1)
            {
                _cell[k,l].parois[0] = true;
                _cell[i,j].parois[2] = true;
            }
        }

        public StringBuilder DisplayLine(int n, int lig, int col)
        {
            StringBuilder ligne = new StringBuilder();
            for (int i = 0; i <= col ; i++)
            {
                if (i == 0 && n == 0)
                {
                    ligne.Append("┌─");
                }
                else if (i == col && n == 0)
                {
                    ligne.Append("─┐");
                }
                else if(i==0 && n==lig)
                {
                    ligne.Append("└─");
                }
                else if(i == col && n == lig)
                {
                    ligne.Append("─┘");
                }
                else if(i !=0 && i != col && n == 0 | n == lig )
                {
                    ligne.Append("─");
                }
                else if(i == 0 && n != 0 && n != lig || i == col && n != lig)
                {
                    ligne.Append("│");
                }
            }
            return ligne;
        }

        public List<StringBuilder> Display(int col, int lig)
        {
            List<StringBuilder> display = new List<StringBuilder>();
            for (int j = 0; j <= lig ; j++ )
            {
                display.Add(DisplayLine(j,col,lig));
            }
            return display;
        }
    }
}
