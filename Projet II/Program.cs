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

            //chemin d'accès fichiers
            string fcomptes = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Comptes.csv";
            string ftransactions = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Transactions.csv";
            string fstatuts = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\StatutsTransactions.csv";
            string fgest = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Gestionnaires.csv";
            string fresult = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Resultats.txt";

            //compteurs 
            int cptok = 0;
            int cptko = 0;
            int comptescréés = 0;
            List<decimal> totMontant = new List<decimal>();

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

            //recupération du fichier comptes dans une liste d'objets
            //transfert des comptes supprimés dans une seconde liste d'objets
            List<Comptes> cpts = new List<Comptes>();
            List<ComptesClots> cpClot = new List<ComptesClots>();
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
                            && decimalSolde | val[2] == string.Empty
                            && entierEntrée | val[3] == string.Empty
                            && entierSortie | val[4] == string.Empty)
                        {
                            if (NumCpt > 0)
                            {
                                foreach (Gestionnaires g in listeGestionnaires)
                                {
                                    bool CompteExistant = cpts.Any(c => c.num == NumCpt);
                                    bool EntréeExistant = listeGestionnaires.Any(e => e.numGest == Entrée);
                                    bool SortieExistant = listeGestionnaires.Any(s => s.numGest == Sortie);

                                    //Creation d'un compte
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
                                        comptescréés++;
                                        break;
                                    }

                                    //Suppression d'un compte
                                    if (SortieExistant                                          
                                        && CompteExistant 
                                        && val[3] == string.Empty 
                                        | Entrée == 0 
                                        && val[4] != string.Empty 
                                        | Sortie != 0)
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
                                        break;
                                    }

                                    //Cession d'un compte
                                    if (CompteExistant                                                 
                                        && EntréeExistant 
                                        && SortieExistant
                                        && val[3] != string.Empty | Entrée != 0
                                        && val[4] != string.Empty | Sortie != 0)
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

                            //numéro de compte négatif
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

                        //la transaction n'existe pas et les formats des variables sont conformes
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

                        //si le numéro de transaction n'existe pas mais format non conforme
                        else if (!TransactionExistante)
                        {
                            Transactions Transaction = new Transactions();
                            ListeTransactions.Add(Transaction);
                            Transaction.id = idTransactions;
                        }

                        //si le numéro de transaction existe déjà
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
                DateTime dateEffet = transac.DateEffet;
                decimal mtn = transac.montant;
                int exp = transac.expediteur;
                int des = transac.destinataire;

                //virement ou prélevement
                if (exp > 0 && des > 0) 
                {
                    foreach (Comptes c in cpts)
                    {
                        bool CompteExped = cpts.Any(e => e.num == exp);
                        bool CompteDesti = cpts.Any(d => d.num == des);

                        //les comptes expediteur et destinataires existent et sont ouverts
                        if (CompteExped && CompteDesti) 
                        {
                            Comptes CExp = cpts.Find(e => e.num == exp);
                            Comptes CDest = cpts.Find(d => d.num == des);
                            int numGExp = CExp.gestionnaire;
                            int numGDes = CDest.gestionnaire;
                            Gestionnaires GExp = listeGestionnaires.Find(expe => expe.numGest == numGExp);
                            Gestionnaires GDest = listeGestionnaires.Find(dest => dest.numGest == numGDes);

                            //si solde suffisant et plafond non atteint
                            if (exp != des
                                && !Transactions.VerifMaximum(mtn, CExp, GExp))
                            {
                                Transactions.Virement(mtn, CExp, GExp, CDest, GDest);
                                statuts.Add(idt, "OK");
                                cptok++;
                                break; ;
                            }
                            else
                            {
                                statuts.Add(idt, "KO");
                                cptko++;
                                break;
                            }
                        }

                        //au moins un des deux comptes a été fermé
                        else if (cpClot.Count != 0)
                        {
                            foreach (ComptesClots cclot in cpClot)
                            {
                                bool ExpCloExiste = cpClot.Any(cc => cc.num == exp);
                                bool DesCloExiste = cpClot.Any(cc => cc.num == des);

                                //si le compte destinataire existe et a été cloturé et que le compte expediteur existe et est ouverte
                                if (CompteExped && DesCloExiste)
                                {
                                    Comptes CExp = cpts.Find(e => e.num == exp);
                                    ComptesClots CCDest = cpClot.Find(d => d.num == des);
                                    int numGExp = CExp.gestionnaire;
                                    int numGDes = CCDest.gestionnaire;
                                    Gestionnaires GExp = listeGestionnaires.Find(ex => ex.numGest == numGExp);
                                    Gestionnaires GCDest = listeGestionnaires.Find(de => de.numGest == numGDes);
                                    DateTime dtClotDest = CCDest.DateClot;
                                    DateTime dtOuvExp = CExp.Date;

                                    //si solde siffusant, plafond non atteint et si la transaction a eu lieu avant la cloture
                                    if (exp != des
                                        && !Transactions.VerifMaximum(mtn, CExp, GExp)
                                        && dtOuvExp <= dateEffet
                                        && dtClotDest > dateEffet)
                                    {
                                        Transactions.Virement(mtn, CExp, GExp, CCDest, GCDest);
                                        statuts.Add(idt, "OK");
                                        totMontant.Add(mtn);
                                        cptok++;
                                        break;
                                    }
                                    //si elle a eu lieu après = impossible
                                    else
                                    {
                                        statuts.Add(idt, "KO");
                                        cptko++;
                                        break;
                                    }
                                }

                                //si le compte expediteur existe et est cloturé 
                                if (ExpCloExiste && CompteDesti)
                                {
                                    ComptesClots CCExp = cpClot.Find(e => e.num == exp);
                                    Comptes CDest = cpts.Find(d => d.num == des);
                                    int numGExp = CCExp.gestionnaire;
                                    int numGDes = CDest.gestionnaire;
                                    Gestionnaires GCExp = listeGestionnaires.Find(exped => exped.numGest == numGExp);
                                    Gestionnaires GDest = listeGestionnaires.Find(desti => desti.numGest == numGDes);
                                    DateTime dtOuvDest = CDest.Date;
                                    DateTime dtClotDest = CCExp.DateClot;

                                    //si solde suffisant, plafond non atteint et si la transaction a eu lieu avant la cloture
                                    if (exp != des
                                        && !Transactions.VerifMaximum(mtn, CCExp, GCExp)
                                        && dtOuvDest <= dateEffet
                                        && dtClotDest > dateEffet)
                                    {
                                        Transactions.Virement(mtn, CCExp, GCExp, CDest, GDest);
                                        statuts.Add(idt, "OK");
                                        totMontant.Add(mtn);
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

                                //si les comptes expediteur et destinataire existent et sont cloturés
                                if (ExpCloExiste && DesCloExiste)
                                {
                                    ComptesClots CCExp = cpClot.Find(e => e.num == exp);
                                    ComptesClots CCDest = cpClot.Find(d => d.num == des);
                                    int numGExp = CCExp.gestionnaire;
                                    int numGDes = CCDest.gestionnaire;
                                    Gestionnaires GCExp = listeGestionnaires.Find(exped => exped.numGest == numGExp);
                                    Gestionnaires GCDest = listeGestionnaires.Find(desti => desti.numGest == numGDes);
                                    DateTime dtOuvExp = CCExp.Date;
                                    DateTime dtClotExp = CCExp.DateClot;
                                    DateTime dtOuvDest = CCDest.Date;
                                    DateTime dtClotDest = CCDest.DateClot;

                                    //si solde suffisant, plafond non atteint et si la transaction a eu lieu avant la cloture
                                    if (exp != des
                                        && !Transactions.VerifMaximum(mtn, CCExp, GCExp)
                                        && dtOuvExp <= dateEffet
                                        && dtClotExp > dateEffet
                                        && dtOuvDest <= dateEffet
                                        && dtClotDest > dateEffet)
                                    {
                                        Transactions.Virement(mtn, CCExp, GCExp, CCDest, GCDest);
                                        statuts.Add(idt, "OK");
                                        totMontant.Add(mtn);
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

                                //si les comptes n'ont jamais existés
                                else
                                {
                                    statuts.Add(idt, "KO");
                                    cptko++;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }

                //dépôt
                if (exp == 0 && des > 0)
                {
                    foreach (Comptes c in cpts)
                    {
                        bool CompteDesti = cpts.Any(d => d.num == des);

                        //si le compte destinataire existe et est ouvert
                        if (CompteDesti)
                        {
                            Comptes CDest = cpts.Find(d => d.num == des);
                            Transactions.Dépot(mtn, CDest);
                            statuts.Add(idt, "OK");
                            totMontant.Add(mtn);
                            cptok++;
                            break;
                        }

                        //si des comptes ont été supprimés
                        else if (cpClot.Count != 0)
                        {
                            //recherche du compte destinaire parmis ceux cloturés
                            foreach (Comptes cclot in cpClot)
                            {
                                bool DesCloExiste = cpClot.Any(cc => cc.num == des);

                                //si le compte destinataire existe et a été cloturé
                                if (DesCloExiste)
                                {
                                    ComptesClots CCDes = cpClot.Find(e => e.num == des);
                                    DateTime DateCloture = CCDes.DateClot;
                                    DateTime DateCreation = CCDes.Date;
                                    int numGestionnaire = CCDes.gestionnaire;
                                    Gestionnaires gest = listeGestionnaires.Find(g => g.numGest == numGestionnaire);

                                    //si solde suffisant, plafond non atteint et si la transaction a eu lieu avant la cloture
                                    if  (!Transactions.VerifMaximum(mtn, CCDes, gest)
                                        && DateCloture > dateEffet
                                        && dateEffet >= DateCreation)
                                    {
                                        Transactions.Retrait(mtn, CCDes);
                                        statuts.Add(idt, "OK");
                                        totMontant.Add(mtn);
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
                        }

                        //si le compte n'existe pas et n'a pas été cloturé
                        else
                        {
                            statuts.Add(idt, "KO");
                            cptko++;
                            break;
                        }
                        break;
                    }
                }

                //retrait
                if (exp > 0 && des == 0)
                {
                    foreach (Comptes c in cpts)
                    {
                        bool CompteExped = cpts.Any(e => e.num == exp);
                        
                        //si le compte expediteur existe
                        if (CompteExped)
                        {
                            Comptes CExp = cpts.Find(e => e.num == exp);
                            int numGestionnaire = CExp.gestionnaire;
                            Gestionnaires gest = listeGestionnaires.Find(g => g.numGest == numGestionnaire);
                            DateTime DateCreation = CExp.Date;

                            //si solde suffisant, plafond non atteint et si la transaction a eu lieu avant la création du compte
                            if (!Transactions.VerifMaximum(mtn, CExp, gest)
                                && DateCreation <= dateEffet)
                            {
                                Transactions.Retrait(mtn, CExp);
                                statuts.Add(idt, "OK");
                                totMontant.Add(mtn);
                                cptok++;
                                break ;
                            }
                            else
                            {
                                statuts.Add(idt, "KO");
                                cptko++;
                                break;
                            }
                        }

                        //si des comptes ont été supprimés et le compte expediteur n'a pas été trouvé
                        else if (cpClot.Count != 0)
                        {
                            //recherche du compte expediteur parmis les comptes supprimés
                            foreach (ComptesClots cc in cpClot)
                            {
                                bool ExpCloExiste = cpClot.Any(ccl => ccl.num == exp);

                                //si le compte existe et a été cloturé
                                if (ExpCloExiste)
                                {
                                    ComptesClots CCExp = cpClot.Find(e => e.num == exp);
                                    DateTime DateCloture = CCExp.DateClot;
                                    DateTime DateCreation = CCExp.Date;
                                    int numGestionnaire = CCExp.gestionnaire;
                                    Gestionnaires gest = listeGestionnaires.Find(g => g.numGest == numGestionnaire);

                                    //si solde suffisant, plafond non atteint et si la transaction a eu lieu avant la cloture du compte
                                    if  (!Transactions.VerifMaximum(mtn, CCExp, gest)
                                        && DateCloture > dateEffet
                                        && dateEffet >= DateCreation)
                                    {
                                        Transactions.Retrait(mtn, CCExp);
                                        statuts.Add(idt, "OK");
                                        totMontant.Add(mtn);
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
                            break;
                        }

                        //si le compte n'existe pas et n'a pas été cloturé
                        else
                        {
                            statuts.Add(idt, "KO");
                            cptko++;
                            break;
                        }
                    }
                }

                //erreur
                if(exp == 0 && des == 0)
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
                sw.WriteLine($"Nombre de comptes : {comptescréés}");
                sw.WriteLine($"Nombre de transactions : {Transactions.NombreTrans}");
                sw.WriteLine($"Nombre de réussites : {cptok}");
                sw.WriteLine($"Nombre d'échecs : {cptko}");
                decimal totreussites = totMontant.Sum();
                sw.WriteLine($"Montant total des réussites : {totreussites} euros");
                sw.WriteLine();
                sw.WriteLine($"Frais de gestions :");
                foreach(Gestionnaires gestionnaire in listeGestionnaires)
                {
                    decimal totalfrais = gestionnaire.FraisGest.Sum();
                    sw.WriteLine($"{gestionnaire.numGest} : {totalfrais} euros");
                }
            }    
        }
    }
}
