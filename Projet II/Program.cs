using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Projet_II
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fcomptes = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Comptes.csv";
            string ftransactions = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Transactions.csv";
            string fstatuts = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\StatutsTransactions.csv";
            string fgest = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Gestionnaires.csv";
            string fresult = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Resultats.csv";

            // recupération du fichier gestionnaires dans une liste d'objet
            List<Gestionnaires> listeGestionnaires = new List<Gestionnaires>();
            using (StreamReader sr = new StreamReader(fgest))
            {
                string ligne = sr.ReadLine();
                string[] val;

                while (ligne != null)
                {
                    val = ligne.Split(';');
                    bool entier0 = int.TryParse(val[0], out int NumGest);
  
                    if (val.Length == 3 && entier0)
                    {
                        string type = val[1];
                        bool GestionnaireExistant = listeGestionnaires.Any(c => c.numGest == NumGest);
                        bool entier2 = int.TryParse(val[2], out int NTransac);
                        if (!GestionnaireExistant && entier2 && NumGest > 0 && type == "Particulier" | type == "Entreprise")
                        {
                            Gestionnaires gestion = new Gestionnaires();
                            listeGestionnaires.Add(gestion);
                            gestion.numGest = NumGest;
                            gestion.typeGest = type;
                            gestion.NbTransGest = NTransac;
                        }
                        else
                        {
                            ligne = sr.ReadLine();
                            continue;
                        }
                    }
                    ligne = sr.ReadLine();
                }
            }

            // recupération du fichier comptes dans une liste d'objet
            List<Comptes> cpts = new List<Comptes>();
            using (StreamReader sr = new StreamReader(fcomptes))
            {
                string ligne = sr.ReadLine();
                string[] val;

                while (ligne != null)
                {
                    try
                    {
                        val = ligne.Split(';');
                        string val2 = val[2].Replace(".",",");
                        bool entierNum = int.TryParse(val[0], out int NumCpt);
                        bool dateDate = DateTime.TryParse(val[1], out DateTime Date);
                        bool decimalSolde = decimal.TryParse(val2, out decimal Solde);
                        bool entierEntrée = int.TryParse(val[3], out int Entrée);
                        bool entierSortie = int.TryParse(val[4], out int Sortie);

                        if (val.Length == 5
                            && entierNum
                            && dateDate
                            && decimalSolde
                            && entierEntrée
                            && entierSortie
                            || val[2] == string.Empty
                            || val[3] == string.Empty
                            || val[4] == string.Empty)
                        {
                            if (NumCpt > 0)
                            {
                                foreach (Gestionnaires g in listeGestionnaires)
                                {
                                    bool CompteExistant = cpts.Any(c => c.num == NumCpt);
                                    bool EntréeExistant = listeGestionnaires.Any(e => e.numGest == Entrée);
                                    bool SortieExistant = listeGestionnaires.Any(s => s.numGest == Sortie);

                                    if (Tools.VerifDate(Date)
                                        &&EntréeExistant 
                                        && !CompteExistant 
                                        && val[3] != string.Empty 
                                        | Entrée != 0 
                                        && val[4] == string.Empty 
                                        | Sortie == 0)
                                    {
                                        Comptes compte = new Comptes();
                                        cpts.Add(compte);
                                        compte.num = NumCpt;
                                        compte.Date = Date;
                                        compte.solde = Solde;
                                        compte.entrée = Entrée;
                                        compte.gestionnaire = Entrée;
                                        break;
                                    }

                                    if (SortieExistant 
                                        && CompteExistant 
                                        && val[3] == string.Empty 
                                        | Entrée == 0 
                                        && val[4] != string.Empty 
                                        | Sortie != 0)
                                    {
                                        Comptes CompteCloture = cpts.Find(c => c.num == NumCpt);
                                        cpts.Remove(CompteCloture);
                                        break;
                                    }

                                    if (CompteExistant 
                                        && EntréeExistant 
                                        && SortieExistant
                                        && val[3] != string.Empty
                                        | Entrée != 0
                                        && val[4] != string.Empty
                                        | Sortie != 0)
                                    {
                                        bool CompteCede = cpts.Any(c => c.gestionnaire == Entrée);
                                        
                                        if (CompteCede)
                                        {
                                            Comptes CompteEchange = cpts.Find(c => c.num == NumCpt);
                                            CompteEchange.gestionnaire = Sortie;
                                            break;
                                        }
                                    }
                                }
                            }

                            else
                            {
                                ligne = sr.ReadLine();
                                continue;
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

            //récupération du fichier transactions dans une liste d'objet
            List<Transactions> ListeTransactions = new List<Transactions>();
            using (StreamReader sr = new StreamReader(ftransactions))
            {
                string ligne = sr.ReadLine();
                string[] val;
                while (ligne != null)
                {
                    val = ligne.Split(';');
                    bool entier0 = int.TryParse(val[0], out int idTransactions);
                   
                    if (entier0 && val.Length == 5)
                    {
                        bool dateDate = DateTime.TryParse(val[1], out DateTime Date);
                        bool decimal1 = decimal.TryParse(val[2], out decimal montant);
                        bool entier2 = int.TryParse(val[3], out int Expediteur);
                        bool entier3 = int.TryParse(val[4], out int Destinataire);
                        bool TransactionExistante = ListeTransactions.Any(c => c.id == idTransactions);

                        if (!TransactionExistante 
                            && Tools.VerifDate(Date) 
                            && decimal1 
                            && entier2 
                            && entier3 
                            && Destinataire >= 0 
                            && Expediteur >= 0 
                            && montant > 0)
                        {
                            Transactions Transaction = new Transactions();
                            ListeTransactions.Add(Transaction);
                            Transaction.id = idTransactions;
                            Transaction.DateEffet = Date;
                            Transaction.montant = montant;
                            Transaction.expediteur = Expediteur;
                            Transaction.destinataire = Destinataire;
                        }

                        else if (!TransactionExistante)
                        {
                            Transactions Transaction = new Transactions();
                            ListeTransactions.Add(Transaction);
                            Transaction.id = idTransactions;
                        }

                        else
                        {
                            ligne = sr.ReadLine();
                            continue;
                        }
                    }       
                    ligne = sr.ReadLine();
                }
            }

            //traitement des transactions
            Dictionary<int,string> statuts = new Dictionary<int,string>();
            int cptok = 0;
            int cptko = 0;
            foreach (Transactions transac in ListeTransactions)
            {
                int idt = transac.id;
                DateTime dateEffet = transac.DateEffet;
                decimal mtn = transac.montant;
                int exp = transac.expediteur;
                int des = transac.destinataire;

                if (exp > 0 && des > 0)
                {
                    foreach (Comptes c in cpts)
                    {
                    bool CompteExped = cpts.Any(e => e.num == exp);
                    bool CompteDesti = cpts.Any(d => d.num == des);

                        if (CompteExped && CompteDesti)
                        {

                            Comptes CExp = cpts.Find(e => e.num == exp);
                            Comptes CDest = cpts.Find(d => d.num == des);
                            int numGestionnaire = CExp.gestionnaire;
                            Gestionnaires gest = listeGestionnaires.Find(g => g.numGest == numGestionnaire);

                            if (Transactions.Virement(mtn, CExp, CDest) && exp != des && Transactions.VerifMaximum(mtn,CExp,gest))
                            {
                                statuts.Add(idt, "OK");
                                cptok++;
                                break;
                            }
                            else
                            {
                                statuts.Add(idt, "KO");
                                cptko++;
                                break;
                            }
                        }

                        else
                        {
                            statuts.Add(idt, "KO");
                            cptko++;
                            break;
                        }
                    }
                }

                if (exp == 0 && des > 0)
                {
                    foreach (Comptes c in cpts)
                    {
                        bool CompteDesti = cpts.Any(d => d.num == des);

                        if (CompteDesti)
                        {
                            Comptes CDest = cpts.Find(d => d.num == des);
                            Transactions.Dépot(mtn, CDest);
                            statuts.Add(idt, "OK");
                            cptok++;
                            break;
                        }

                        else
                        {
                            statuts.Add(idt, "KO");
                            cptko++;
                            break;
                        }
                    }
                }

                if (exp > 0 && des == 0)
                {
                    foreach (Comptes c in cpts)
                    {
                        bool CompteExped = cpts.Any(e => e.num == exp);

                        if (CompteExped)
                        {
                            Comptes CExp = cpts.Find(e => e.num == exp);
                            int numGestionnaire = CExp.gestionnaire;
                            Gestionnaires gest = listeGestionnaires.Find(g => g.numGest == numGestionnaire);

                            if (Transactions.Retrait(mtn, CExp) && Transactions.VerifMaximum(mtn,CExp,gest))
                            {
                                statuts.Add(idt, "OK");
                                cptok++;
                                break;
                            }

                            else
                            {
                                statuts.Add(idt, "KO");
                                cptko++;
                                break;
                            }
                        }

                        else
                        {
                            statuts.Add(idt, "KO");
                            cptko++;
                            break;
                        }
                    }
                }

                if (exp == 0 && des == 0)
                {
                    statuts.Add(idt, "KO");
                    cptko++;
                }
            }

            //écriture des statuts de transactions
            using (StreamWriter sw = new StreamWriter(fstatuts))
            {
                foreach(KeyValuePair<int,string> ligne in statuts)
                {
                    sw.WriteLine($"{ligne.Key};{ligne.Value}");
                }
            }

            //écriture du résultat
            using (StreamWriter sw = new StreamWriter(fresult))
            {
                sw.WriteLine("Statistiques :");
                sw.WriteLine($"Nombre de comptes : {Comptes.NombreComptes}");
                sw.WriteLine($"Nombre de transactions : {Transactions.NombreTrans}");
                sw.WriteLine($"Nombre de réussites : {cptok}");
                sw.WriteLine($"Nombre d'échecs : {cptko}");
                sw.WriteLine($"Montant total des réussites :  euros");
                sw.WriteLine();
                sw.WriteLine($"Frais de gestions :");
               // foreach()
                //{
               //     sw.WriteLine($"{} : {} euros");
               // }
                
            }
            
        }
    }
}
