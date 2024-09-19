using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace Projet_II
{
    public class Comptes
    {
        public int num { get; set; }
        public DateTime Date { get; set; } 
        public decimal solde { get; set; }
        public int entrée { get; set; }
        public int sortie { get; set; }
        public static int NombreComptes { get; set; }
        public int gestionnaire { get; set; }
        public List<decimal> historique { get; set; }

        public Comptes()
        {
            num = 0;
            Date = DateTime.MinValue;
            solde = 0;
            entrée = 0;
            sortie = 0;
            NombreComptes++;
            gestionnaire = 0;
            historique = new List<decimal>();
        }
    }
}