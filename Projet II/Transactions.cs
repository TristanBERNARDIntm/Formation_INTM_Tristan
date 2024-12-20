using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Projet_II
{
    public class Transactions
    {
        public int id { get; set; }
        public DateTime DateEffet { get; set; }
        public decimal montant { get; set; }
        public int expediteur { get; set; }
        public int destinataire { get; set; }
        public static int NombreTrans { get;set; }
        public static int NbTransOK { get;set; }
        public static int NbTransKO { get;set; }
        public static decimal totMontant { get; set; }

        public Transactions()
        {
            id = 0;
            montant = 0; 
            expediteur = 0;
            destinataire = 0;
            NombreTrans++;
        }     
        //r�cup�ration des variables du fichier Transactions dans une liste de Transactions
        public static void LectureTransactions(string ftransactions, List<Transactions> ListeTransactions)
        {
            using (StreamReader sr = new StreamReader(ftransactions))
            {
                string ligne = sr.ReadLine();
                string[] val;
                while (ligne != null)
                {
                    val = ligne.Split(';');
                    bool entier0 = int.TryParse(val[0], out int idTransactions);
                    bool dateDate = DateTime.TryParse(val[1], out DateTime Date);
                    bool decimal1 = decimal.TryParse(val[2], out decimal montant);
                    bool entier2 = int.TryParse(val[3], out int Expediteur);
                    bool entier3 = int.TryParse(val[4], out int Destinataire);
                    bool TransactionExistante = ListeTransactions.Any(c => c.id == idTransactions);

                    //la transaction n'existe pas et les formats des variables sont conformes
                    if (!TransactionExistante && Tools.VerifTransaction(val))                            
                    {
                        Transactions Transaction = new Transactions();
                        ListeTransactions.Add(Transaction);
                        Transaction.id = idTransactions;
                        Transaction.DateEffet = Date;
                        Transaction.montant = montant;
                        Transaction.expediteur = Expediteur;
                        Transaction.destinataire = Destinataire;
                    }

                    //si le num�ro de transaction n'existe pas mais format non conforme
                    else if (!TransactionExistante)
                    {
                        Transactions Transaction = new Transactions();
                        ListeTransactions.Add(Transaction);
                        Transaction.id = idTransactions;
                    }

                    //si le num�ro de transaction existe d�j�
                    else
                    {
                         ligne = sr.ReadLine();
                         continue;
                    }                  
                    ligne = sr.ReadLine();
                }
            }
        }

        public static void D�pot(int idt, decimal montant, Comptes CDes, Dictionary<int, string> statuts)
        {
            CDes.solde += montant;
            statuts.Add(idt, "OK");
            Transactions.totMontant += montant;
            Transactions.NbTransOK++;
        }

        public static bool Retrait(int idt, decimal montant, Comptes CExp, Dictionary<int, string> statuts)
        {
            if (VerifSolde(montant, CExp))
            {
                CExp.solde -= montant;
                statuts.Add(idt, "OK");
                Transactions.totMontant += montant;
                Transactions.NbTransOK++;
                return true;
            }
            statuts.Add(idt, "KO");
            Transactions.NbTransKO++;
            return false;
        }

        public static bool Virement(int idt, decimal montant, Comptes CExp, Gestionnaires GExp, Comptes CDes, Gestionnaires GDes, Dictionary<int, string> statuts)
        {
            if (VerifSolde(montant,CExp))
            {
                if (GExp.typeGest == "Particulier")
                {
                    decimal fraisP = (decimal)0.01 * montant;
                    CExp.solde -= montant - fraisP;
                    CDes.solde += montant;
                    GExp.FraisGest.Add(fraisP);
                    statuts.Add(idt, "OK");
                    Transactions.NbTransOK++;
                    Transactions.totMontant += montant;
                    return true;
                }
                else if (GExp.typeGest == "Entreprise")
                {
                    decimal fraisF = 10;
                    CExp.solde -= montant - fraisF;
                    CDes.solde += montant;
                    GExp.FraisGest.Add(fraisF);
                    statuts.Add(idt, "OK");
                    Transactions.NbTransOK++;
                    Transactions.totMontant += montant;
                    return true;
                }
            }
            statuts.Add(idt, "KO");
            Transactions.NbTransKO++;
            return false;
        }

        public static bool VerifSolde(decimal montant, Comptes CExp)
        {
            return CExp.solde > montant;
        }
    
        public static bool VerifMaximum(decimal montant, Comptes compte, Gestionnaires gest)
        {
            try
            {
                int NbTransac = gest.NbTransGest;
                decimal somme = 0;
                if (compte.historique.Count < NbTransac)
                {
                    compte.historique.Add(montant);
                    somme = compte.historique.Sum();
                    if (somme < 1000) return false;
                    else return true;
                }
                if (compte.historique.Count == NbTransac && NbTransac != 0 && NbTransac != 1)
                {
                    compte.historique.RemoveAt(0);
                    compte.historique.Add(montant);
                    somme = compte.historique.Sum();
                    if (somme < 1000) return false;
                    else return true;
                }
                if (compte.historique.Count == NbTransac && NbTransac == 1)
                {
                    compte.historique.Add(montant);
                    somme = compte.historique.Sum();
                    if (somme < 1000)
                    {
                        compte.historique.RemoveAt(0);
                        return false;
                    }
                    else
                    {
                        compte.historique.RemoveAt(0);
                        return true;
                    }
                }
                if (NbTransac == 0)
                {
                    if (montant < 1000) return false;
                    else return true;
                }
                else return true;
            }
            catch(IndexOutOfRangeException)
            {
                return true;
            }
        }

        public static void Traitement(Dictionary<int,string> statuts, List<Transactions> ListeTransactions, List<Comptes> cpts, List<ComptesClots> cpClot, List<Gestionnaires> listeGestionnaires)
        {
            foreach (Transactions transac in ListeTransactions)
            {
                int idt = transac.id;
                DateTime dateEffet = transac.DateEffet;
                decimal mtn = transac.montant;
                int exp = transac.expediteur;
                int des = transac.destinataire;

                //virement ou pr�levement
                if (exp > 0 && des > 0)
                {
                    bool CompteExped = cpts.Any(e => e.num == exp);
                    bool CompteDesti = cpts.Any(d => d.num == des);

                    //les comptes expediteur et destinataires existent et sont ouverts
                    if (CompteExped && CompteDesti)
                    {
                        Comptes CExp = cpts.Find(e => e.num == exp);
                        Comptes CDest = cpts.Find(d => d.num == des);
                        try
                        {
                            int numGExp = CExp.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
                            int numGDes = CDest.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
                            Gestionnaires GExp = listeGestionnaires.Find(expe => expe.numGest == numGExp);
                            Gestionnaires GDest = listeGestionnaires.Find(dest => dest.numGest == numGDes);
                            //si solde suffisant et plafond non atteint
                            if (exp != des && !Transactions.VerifMaximum(mtn, CExp, GExp))
                            {
                                Transactions.Virement(idt, mtn, CExp, GExp, CDest, GDest, statuts);
                                continue;
                            }
                        }
                        catch(InvalidOperationException)
                        {
                            //*
                        }                     
                    }

                    //au moins un des deux comptes a �t� ferm�
                    else if (cpClot.Count != 0)
                    {
                        bool ExpCloExiste = cpClot.Any(cc => cc.num == exp);
                        bool DesCloExiste = cpClot.Any(cc => cc.num == des);

                        //si le compte destinataire existe et a �t� clotur� et que le compte expediteur existe et est ouverte
                        if (CompteExped && DesCloExiste)
                        {
                            Comptes CExp = cpts.Find(e => e.num == exp);
                            ComptesClots CCDest = cpClot.Find(d => d.num == des);
                            try
                            {
                                int numGExp = CExp.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
                                int numGDes = CCDest.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
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
                                    Transactions.Virement(idt, mtn, CExp, GExp, CCDest, GCDest, statuts);
                                    continue;
                                }
                            }
                            catch(InvalidOperationException)
                            {
                                //*
                            }
                        }

                        //si le compte expediteur existe et est clotur� 
                        if (ExpCloExiste && CompteDesti)
                        {
                            ComptesClots CCExp = cpClot.Find(e => e.num == exp);
                            Comptes CDest = cpts.Find(d => d.num == des);
                            try
                            {
                                int numGExp = CCExp.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
                                int numGDes = CDest.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
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
                                    Transactions.Virement(idt, mtn, CCExp, GCExp, CDest, GDest, statuts);
                                    continue;
                                }
                            }
                            catch(InvalidOperationException)
                            {
                                //*
                            }
                        }

                        //si les comptes expediteur et destinataire existent et sont clotur�s
                        if (ExpCloExiste && DesCloExiste)
                        {
                            ComptesClots CCExp = cpClot.Find(e => e.num == exp);
                            ComptesClots CCDest = cpClot.Find(d => d.num == des);
                            try
                            {
                                int numGExp = CCExp.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
                                int numGDes = CCDest.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;                          
                                Gestionnaires GCExp = listeGestionnaires.Find(exped => exped.numGest == numGExp);
                                Gestionnaires GCDest = listeGestionnaires.Find(desti => desti.numGest == numGDes);
                                DateTime dtOuvExp = CCExp.Date;
                                DateTime dtClotExp = CCExp.DateClot;
                                DateTime dtOuvDest = CCDest.Date;
                                DateTime dtClotDest = CCDest.DateClot;
                                if (exp != des
                                && !Transactions.VerifMaximum(mtn, CCExp, GCExp)
                                && dtOuvExp <= dateEffet
                                && dtClotExp > dateEffet
                                && dtOuvDest <= dateEffet
                                && dtClotDest > dateEffet)
                                {
                                    Transactions.Virement(idt, mtn, CCExp, GCExp, CCDest, GCDest, statuts);
                                    continue;
                                }
                            }
                            catch(InvalidOperationException)
                            {
                                //*
                            }

                            //si solde suffisant, plafond non atteint et si la transaction a eu lieu avant la cloture
                            
                        }
                    }
                }

                //d�p�t
                if (exp == 0 && des > 0)
                {
                    bool CompteDesti = cpts.Any(d => d.num == des);

                    //si le compte destinataire existe et est ouvert
                    if (CompteDesti)
                    {
                        Comptes CDest = cpts.Find(d => d.num == des);
                        Transactions.D�pot(idt, mtn, CDest, statuts);
                        continue;
                    }

                    //si des comptes ont �t� supprim�s
                    else if (cpClot.Count != 0)
                    {
                        bool DesCloExiste = cpClot.Any(cc => cc.num == des);

                        //si le compte destinataire existe et a �t� clotur�
                        if (DesCloExiste)
                        {
                            ComptesClots CCDes = cpClot.Find(e => e.num == des);
                            DateTime DateCloture = CCDes.DateClot;
                            DateTime DateCreation = CCDes.Date;
                            try
                            {
                                int numGestionnaire = CCDes.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
                                Gestionnaires gest = listeGestionnaires.Find(g => g.numGest == numGestionnaire);

                                //si la transaction a eu lieu avant la cloture
                                if (DateCloture > dateEffet && dateEffet >= DateCreation)
                                {
                                    Transactions.D�pot(idt, mtn, CCDes, statuts);
                                    continue;
                                }
                            }
                            catch(InvalidOperationException)
                            {
                                //*
                            }
                        }
                    }
                }

                //retrait
                if (exp > 0 && des == 0)
                {
                    bool CompteExped = cpts.Any(e => e.num == exp);

                    //si le compte expediteur existe
                    if (CompteExped)
                    {
                        Comptes CExp = cpts.Find(e => e.num == exp);
                        try
                        {
                            int numGestionnaire = CExp.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
                            Gestionnaires gest = listeGestionnaires.Find(g => g.numGest == numGestionnaire);
                            DateTime DateCreation = CExp.Date;

                            //si solde suffisant, plafond non atteint et si la transaction a eu lieu avant la cr�ation du compte
                            if (!Transactions.VerifMaximum(mtn, CExp, gest)
                                && DateCreation <= dateEffet)
                            {
                                Transactions.Retrait(idt, mtn, CExp, statuts);
                                continue;
                            }
                        }
                        catch
                        {
                            //*
                        }
                    }

                    //si des comptes ont �t� supprim�s et le compte expediteur n'a pas �t� trouv�
                    else if (cpClot.Count != 0)
                    {
                        //recherche du compte expediteur parmis les comptes supprim�s

                        bool ExpCloExiste = cpClot.Any(ccl => ccl.num == exp);

                        //si le compte existe et a �t� clotur�
                        if (ExpCloExiste)
                        {
                            ComptesClots CCExp = cpClot.Find(e => e.num == exp);
                            DateTime DateCloture = CCExp.DateClot;
                            DateTime DateCreation = CCExp.Date;
                            try
                            {
                                int numGestionnaire = CCExp.gestionnaire.Last(kvp => kvp.Value <= dateEffet).Key;
                                Gestionnaires gest = listeGestionnaires.Find(g => g.numGest == numGestionnaire);

                                //si solde suffisant, plafond non atteint et si la transaction a eu lieu avant la cloture du compte
                                if (!Transactions.VerifMaximum(mtn, CCExp, gest)
                                    && DateCloture > dateEffet
                                    && dateEffet >= DateCreation)
                                {
                                    Transactions.Retrait(idt, mtn, CCExp, statuts);
                                    continue;
                                }
                            }
                            catch
                            {
                                //*
                            }
                        }
                    }
                }
                statuts.Add(idt, "KO");
                Transactions.NbTransKO++;
            }
        }
    }
}
