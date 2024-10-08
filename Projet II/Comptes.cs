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
        public int gestionnaire { get; set; }
        public List<decimal> historique { get; set; }
        public static int NbComptes { get; set; }

        public Comptes()
        {
            num = 0;
            Date = DateTime.MinValue;
            solde = 0;
            entrée = 0;
            sortie = 0;
            gestionnaire = 0;
            historique = new List<decimal>();
            NbComptes++;
        }

        public static void LectureComptes(string fcomptes, List<Comptes> cpts, List<ComptesClots> cpClot, List<Gestionnaires> listeGestionnaires)
        {
            using (StreamReader sr = new StreamReader(fcomptes))
            {
                string ligne = sr.ReadLine();
                string[] val;

                while (ligne != null)
                {
                    try
                    {
                        val = ligne.Split(';');
                        if (Tools.VerifComptes(val))
                        {
                            bool entierNum = int.TryParse(val[0], out int NumCpt);
                            bool dateDate = DateTime.TryParse(val[1], out DateTime Date);
                            string val2 = val[2].Replace(".", ",");
                            bool decimalSolde = decimal.TryParse(val2, out decimal Solde);
                            bool entierEntrée = int.TryParse(val[3], out int Entrée);
                            bool entierSortie = int.TryParse(val[4], out int Sortie);

                            bool CompteExistant = cpts.Any(c => c.num == NumCpt);
                            bool EntréeExistant = listeGestionnaires.Any(e => e.numGest == Entrée);
                            bool SortieExistant = listeGestionnaires.Any(s => s.numGest == Sortie);

                            //Creation d'un compte
                            if (EntréeExistant
                                && !CompteExistant
                                && val[3] != string.Empty | Entrée != 0
                                && val[4] == string.Empty | Sortie == 0)
                            {
                                Comptes.Creation(cpts, NumCpt, Date, Solde, Entrée, Sortie);
                            }

                            //Suppression d'un compte
                            if (SortieExistant
                                && CompteExistant
                                && val[3] == string.Empty | Entrée == 0
                                && val[4] != string.Empty | Sortie != 0)
                            {
                                ComptesClots.Cloture(cpts, cpClot, NumCpt, Date, Solde, Entrée, Sortie);
                            }

                            //Cession d'un compte
                            if (CompteExistant
                                && EntréeExistant
                                && SortieExistant
                                && val[3] != string.Empty | Entrée != 0
                                && val[4] != string.Empty | Sortie != 0)
                            {
                                bool CompteCede = cpts.Any(c => c.gestionnaire == Entrée);

                                if (CompteCede) //si le gestionnaire du compte a cedé existe
                                {
                                    Comptes.Cession(cpts, NumCpt, Sortie);
                                }
                            }                         
                        }                     
                        ligne = sr.ReadLine();                   
                    }
                    catch (IndexOutOfRangeException)
                    {
                        ligne = sr.ReadLine();
                        continue;
                    }
                }
            }
        }

        public static void Creation(List<Comptes> cpts, int NumCpt, DateTime Date, decimal Solde, int Entrée, int Sortie)
        {
            Comptes compte = new Comptes();
            cpts.Add(compte);
            compte.num = NumCpt;
            compte.Date = Date;
            compte.solde = Solde;
            compte.entrée = Entrée;
            compte.gestionnaire = Entrée;
        }

        public static void Cession(List<Comptes> cpts, int NumCpt, int Sortie)
        {
            Comptes CompteEchange = cpts.Find(c => c.num == NumCpt);
            CompteEchange.gestionnaire = Sortie;
        }
    }

    public class ComptesClots : Comptes
    {
        public DateTime DateClot { get; set; }

        public ComptesClots()
        {
            DateClot = DateTime.MinValue;
            NbComptes--;
        }

        public static void Cloture(List<Comptes> cpts, List<ComptesClots> cpClot, int NumCpt, DateTime Date, decimal Solde, int Entrée, int Sortie)
        {
            Comptes CompteCloture = cpts.Find(c => c.num == NumCpt);
            cpts.Remove(CompteCloture);
            ComptesClots CompteClot = new ComptesClots();
            cpClot.Add(CompteClot);
            CompteClot.num = CompteCloture.num;
            CompteClot.Date = CompteCloture.Date;
            CompteClot.solde = CompteCloture.solde;
            CompteClot.entrée = CompteCloture.entrée;
            CompteClot.historique = CompteCloture.historique;
            CompteClot.sortie = Sortie;
            CompteClot.gestionnaire = Sortie;
            CompteClot.DateClot = Date;
        }
    }
}
