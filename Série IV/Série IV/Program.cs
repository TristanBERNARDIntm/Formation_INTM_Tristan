using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Série_IV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Exercice II - Recherche d'un élément
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Exercice II - Bracket Control      ");
            Console.WriteLine("-----------------------------------");

            Console.WriteLine("Contrôle des parenthèses");
            string[] texte = new string[]
            {
             "Ceci est un texte test",
             "(",
             "{",
             "[",
             "[]",
             "{}",
             "[)",
             "[}]",
             "()"
            };

            foreach (string phrase in texte)
            {
                BracketsControl.BracketsControls(phrase);
                if (BracketsControl.BracketsControls(phrase) == false)
                {
                    Console.WriteLine($"{phrase} : KO");
                }
                else
                {
                    Console.WriteLine($"{phrase} : OK");
                }
            }

            #endregion

            #region Exercice III - Liste des contacts téképhoniques

            Console.WriteLine("------------------------------------");
            Console.WriteLine("Exercice III - Annuaire Téléphonique");
            Console.WriteLine("------------------------------------");

            Dictionary<string, string> annuaire = new Dictionary<string, string>   
            {
                {"0606060606","Mado la Niçoise"},
                {"0000000000","Anonymous"},
                {"0660037646","Tristan Bernard"},
                {"01555555555","Bill Gates"},
                {"Vice Versa","0123456789" },
                {"18","oskour les pompiers"}
            };

            #endregion

            Console.WriteLine("----------------------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
