using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Projet_I
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fcomptes = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet I\bin\Debug\Comptes.csv";
            string ftransactions = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet I\bin\Debug\Transactions.csv";
            string fstatuts = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet I\bin\Debug\StatutsTransactions.csv";

            // recupération du fichier comptes dans une liste d'objet
            List<Comptes> cpts = new List<Comptes>();
            using (StreamReader sr = new StreamReader(fcomptes))
            {
                string ligne = sr.ReadLine();
                string[] val;

                while (ligne != null)
                {
                    val = ligne.Split(';');               

                    bool entierNum = int.TryParse(val[0], out int NumCpt);
                    bool decimalSolde = decimal.TryParse(val[1], out decimal Solde); // pas val[2]  val[1]
                    
                    if (entierNum && decimalSolde || val[1] == string.Empty)
                    {
                        bool CompteExistant = cpts.Any(c => c.num == NumCpt);
                        if (!CompteExistant && NumCpt > 0)
                        {
                            Comptes compte = new Comptes();
                            cpts.Add(compte);
                            compte.num = NumCpt;
                            compte.solde = Solde;
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

            //récupération du fichier transactions dans une liste d'objet
            List<Transactions> ListeTransactions = new List<Transactions>();
            using (StreamReader sr = new StreamReader(ftransactions))
            {
                string ligne = sr.ReadLine();
                string[] val;
                while (ligne != null)
                {
                    val = ligne.Split(';');
                    Transactions Transaction = new Transactions();

                    bool entier0 = int.TryParse(val[0], out int idTransactions);
                    bool decimal1 = decimal.TryParse(val[1], out decimal montant);
                    bool entier2 = int.TryParse(val[2], out int Expediteur);
                    bool entier3 = int.TryParse(val[3], out int Destinataire);

                    if (entier0)
                    {
                        bool TransactionExistante = ListeTransactions.Any(c => c.id == idTransactions);
                        if (!TransactionExistante && decimal1 && entier2 && entier3 && Destinataire >= 0 && Expediteur >= 0 && montant > 0)
                        {
                            ListeTransactions.Add(Transaction);
                            Transaction.id = idTransactions;
                            Transaction.montant = montant;
                            Transaction.expediteur = Expediteur;
                            Transaction.destinataire = Destinataire;
                        }
                        else if (!TransactionExistante)
                        {
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
            foreach (Transactions transac in ListeTransactions)
            {
                int idt = transac.id;
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
                            if (Transactions.Virement(mtn, CExp, CDest) && exp != des)
                            {
                                statuts.Add(idt, "OK");
                                break;
                            }
                            else
                            {
                                statuts.Add(idt, "KO");
                                break;
                            }
                        }
                        else
                        {
                            statuts.Add(idt, "KO");
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
                            break;
                        }
                        else
                        {
                            statuts.Add(idt, "KO");
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
                            if (Transactions.Retrait(mtn, CExp))
                            {
                                statuts.Add(idt, "OK");
                                break;
                            }
                            else
                            {
                                statuts.Add(idt, "KO");
                                break;
                            }
                        }
                        else
                        {
                            statuts.Add(idt, "KO");
                            break;
                        }
                    }
                }
                if (exp == 0 && des == 0)
                {
                    statuts.Add(idt, "KO");
                }

            }

            //écriture des resultats dans le fichiers de sortie
            Console.WriteLine("Fichier Statuts");
            using (StreamWriter sw = new StreamWriter(fstatuts))
            {
                foreach(KeyValuePair<int,string> ligne in statuts)
                {
                    sw.WriteLine($"{ligne.Key};{ligne.Value}");
                }
            }
            Console.WriteLine(Comptes.NombreComptes);
            Console.WriteLine(Transactions.NombreTrans);
            Console.ReadLine();
        }
    }
}
