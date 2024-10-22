using System;               
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;                   
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace Projet_III
{
    public class Comptes
    {
        public uint num { get; set; }
        public char type { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<DateTime, decimal> solde {  get; set; }
        public uint age { get; set; }
        public uint entrée { get; set; }
        public uint sortie { get; set; } 
        public List<KeyValuePair<uint,DateTime>> gestionnaire { get; set; }
        public List<decimal> historique { get; set; }
        public decimal interets { get; set; }
        public static uint NbComptes { get; set; }

        public Comptes()
        {
            num = 0;
            Date = DateTime.MinValue;
            solde = new Dictionary<DateTime, decimal>();
            age = 18;
            entrée = 0;
            sortie = 0;
            gestionnaire = new List<KeyValuePair<uint, DateTime>>();
            historique = new List<decimal>();
            interets = 0;
            NbComptes++;
        }
        //recup des variables du fichier Comptes dans une liste 
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
                        if (Tools.VerifComptes(val)) //verification de la conformité des variables
                        {
                            bool entierNum = uint.TryParse(val[0], out uint NumCpt);
                            bool charType = char.TryParse(val[1], out char Type);
                            bool dateDate = DateTime.TryParse(val[2], out DateTime Date);
                            string val3 = val[3].Replace(".", ",");
                            bool decimalSolde = decimal.TryParse(val3, out decimal Solde);
                            bool entierAge = uint.TryParse(val[4], out uint Age);
                            bool entierEntrée = uint.TryParse(val[5], out uint Entrée);
                            bool entierSortie = uint.TryParse(val[6], out uint Sortie);
                            bool CompteExistant = cpts.Any(c => c.num == NumCpt);
                            bool EntréeExistant = listeGestionnaires.Any(e => e.numGest == Entrée);
                            bool SortieExistant = listeGestionnaires.Any(s => s.numGest == Sortie);

                            //Creation d'un compte
                            if (EntréeExistant
                                && !CompteExistant
                                && val[5] != string.Empty | Entrée != 0
                                && val[6] == string.Empty | Sortie == 0)
                            {
                                Comptes.Creation(cpts, NumCpt, Type, Date, Solde, Age, Entrée, Sortie); 
                            }

                            //Suppression d'un compte
                            if (SortieExistant
                                && CompteExistant
                                && val[5] == string.Empty | Entrée == 0
                                && val[6] != string.Empty | Sortie != 0)
                            {
                                ComptesClots.Cloture(cpts, cpClot, NumCpt, Type, Date, Solde, Age, Entrée, Sortie); 
                            }

                            //Cession d'un compte
                            if (CompteExistant
                                && EntréeExistant
                                && SortieExistant
                                && val[5] != string.Empty | Entrée != 0
                                && val[6] != string.Empty | Sortie != 0)
                            {
                                foreach(Comptes c in cpts)
                                {
                                    bool GestionnaireExiste = c.gestionnaire.Any(kvp => kvp.Key == Entrée);
                                    if (GestionnaireExiste) //si le gestionnaire du compte a cedé existe
                                    {
                                        Comptes.Cession(cpts, NumCpt, Date, Sortie);
                                        break;
                                    }
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
        //ajout du compte à la liste de comptes actifs
        public static void Creation(List<Comptes> cpts, uint NumCpt, char Type, DateTime Date, decimal Solde, uint Age, uint Entrée, uint Sortie)
        {
            Comptes compte = new Comptes();
            cpts.Add(compte);
            compte.num = NumCpt;
            compte.type = Type;
            compte.Date = Date;
            compte.age = Age;
            try { compte.solde.Add(Date, Solde); }
            catch(ArgumentException)
            {
                compte.solde[Date] += Solde;
            }
            compte.entrée = Entrée;
            compte.gestionnaire.Add(new KeyValuePair<uint,DateTime>(Entrée,Date));
            Tools.VerifAge(compte, Type, Date, Age);
        }
        //changement de gestionnaire dans la liste de comptes actifs
        public static void Cession(List<Comptes> cpts, uint NumCpt, DateTime Date, uint Sortie)
        {
            Comptes CompteEchange = cpts.Find(c => c.num == NumCpt);
            CompteEchange.gestionnaire.Add(new KeyValuePair<uint,DateTime>(Sortie,Date));
        }

        public static void CalculInterets(List<Comptes> cpts, List<ComptesClots> cpClot)
        {
            DateTime DateToday = DateTime.Today;
            int jourNow = DateToday.Day;
            decimal taux = 0;
            foreach (Comptes compte in cpts)
            {
                if (compte.type == 'L') taux = 0.0017m;
                if (compte.type == 'T') taux = 0.0042m;
                int anneeCrea = compte.Date.Year;
                int moisCrea = compte.Date.Month;
                int jourCrea = compte.Date.Day;
                DateTime DT = compte.Date;
       
                if (compte.type == 'L' || compte.type == 'T')
                {
                    if (jourCrea != 1)
                    {
                        int nbjourmois = DateTime.DaysInMonth(anneeCrea, moisCrea);
                        decimal prorata = ((decimal)nbjourmois - (decimal)jourCrea + (decimal)1 ) / (decimal)nbjourmois;
                        DT = new DateTime(anneeCrea, moisCrea, nbjourmois);
                        decimal soldeActuel = compte.solde.Where(k => k.Key < DT).Sum(x => x.Value);
                        compte.interets += soldeActuel * taux * prorata;
                        DT = DT.AddMonths(1);
                    }
                    while (DT <= DateToday)
                    {
                        decimal solde = compte.solde.Where(k => k.Key < DT).Sum(v => v.Value);
                        compte.interets += solde * taux;
                        DT = DT.AddMonths(1);
                    }
                    if (jourNow != 1)
                    {
                        int nbjourmois = DateTime.DaysInMonth(anneeCrea, moisCrea);
                        decimal prorata = (decimal)jourNow / (decimal)nbjourmois;
                        decimal soldeActuel = compte.solde.Where(k => k.Key < DateToday).Sum(x => x.Value);
                        compte.interets += soldeActuel * taux * prorata;
                    }
                }
            }
            foreach (ComptesClots cp in cpClot)
            {
                int anneeCrea = cp.Date.Year;
                int moisCrea = cp.Date.Month;
                int jourCrea = cp.Date.Day;
                int anneeClot = cp.DateClot.Year;
                int moisClot = cp.DateClot.Month;
                int jourClot = cp.DateClot.Day;
                DateTime DT = cp.Date;
                DateTime DateClot = cp.DateClot;
                if (cp.type == 'L') taux = 0.0017m;
                if (cp.type == 'T') taux = 0.0042m;

                if (cp.type == 'L' || cp.type == 'T')
                {
                    if (jourCrea != 1)
                    {
                        int nbjourmois = DateTime.DaysInMonth(anneeCrea, moisCrea);
                        decimal prorata = ((decimal)nbjourmois - (decimal)jourCrea + 1) / (decimal)nbjourmois;
                        DT = new DateTime(anneeCrea, moisCrea, nbjourmois);
                        decimal soldeActuel = cp.solde.Where(k => k.Key < DT).Sum(x => x.Value);
                        cp.interets += soldeActuel * taux * prorata;
                        DT = DT.AddMonths(1);
                    }
                    while (DT <= DateClot)
                    {
                        decimal solde = cp.solde.Where(k => k.Key < DT).Sum(v => v.Value);
                        cp.interets += solde * taux;
                        DT = DT.AddMonths(1);
                    }
                    if (jourNow != 1)
                    {
                        int nbjourmois = DateTime.DaysInMonth(anneeCrea, moisCrea);
                        decimal prorata = (decimal)jourClot / (decimal)nbjourmois;
                        decimal soldeActuel = cp.solde.Where(k => k.Key < DateClot).Sum(x => x.Value);
                        cp.interets += soldeActuel * taux * prorata;
                    }
                }
            }
        }
    }

    public class ComptesClots : Comptes
    {
        public DateTime DateClot { get; set; }

        public ComptesClots()
        {
            DateClot = DateTime.Now;
            NbComptes--;
        }
        //suppression du compte de la liste de comptes actifs et ajout à celle des comptes cloturés
        public static void Cloture(List<Comptes> cpts, List<ComptesClots> cpClot, uint NumCpt, char Type, DateTime Date, decimal Solde, uint Age, uint Entrée, uint Sortie)
        {
            Comptes CompteCloture = cpts.Find(c => c.num == NumCpt);
            cpts.Remove(CompteCloture);
            ComptesClots CompteClot = new ComptesClots();
            cpClot.Add(CompteClot);
            CompteClot.num = CompteCloture.num;
            CompteClot.type = CompteCloture.type;
            CompteClot.Date = CompteCloture.Date;
            CompteClot.solde = CompteCloture.solde;
            CompteClot.age = CompteCloture.age;
            CompteClot.entrée = CompteCloture.entrée;
            CompteClot.historique = CompteCloture.historique;
            CompteClot.sortie = Sortie;
            CompteClot.gestionnaire = CompteCloture.gestionnaire;
            CompteClot.DateClot = Date;
        }
    }
}
