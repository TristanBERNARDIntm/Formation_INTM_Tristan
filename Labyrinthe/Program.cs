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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Nombre de Colonnes : ");
            int colonne = int.Parse(Console.ReadLine());
            Console.WriteLine("Nombre de Lignes : ");
            int ligne = int.Parse(Console.ReadLine());
            
            Labyrinthe Labyrinthe = new Labyrinthe(colonne,ligne);
            Labyrinthe.Generate(new KeyValuePair<int,int>(colonne,ligne));
            List<StringBuilder> Grille = Labyrinthe.Display(colonne,ligne);
            foreach (var n in Grille)
            {
                Console.WriteLine(n);
            }

            // Keep the console window open
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
