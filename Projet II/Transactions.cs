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

        public Transactions(bool isOK = false)
        {
            id = 0;
            montant = 0; 
            expediteur = 0;
            destinataire = 0;
            NombreTrans++;
        }     
        //récupération des variables du fichier Transactions dans une liste de Transactions
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
                    ligne = sr.ReadLine();
                }
            }
        }

        public static void Dépot(decimal montant, Comptes CDes)
        {
            CDes.solde += montant;
        }

        public static bool Retrait(decimal montant, Comptes CExp)
        {
            if (VerifSolde(montant, CExp))
            {
                CExp.solde -= montant;
                return true;
            }
            return false;
        }

        public static bool Virement(decimal montant, Comptes CExp, Gestionnaires GExp, Comptes CDes, Gestionnaires GDes)
        {
            if (VerifSolde(montant,CExp))
            {
                if (GExp.typeGest == "Particulier" && GExp.numGest != GDes.numGest)
                {
                    decimal fraisP = (decimal)0.01 * montant;
                    CExp.solde -= montant - fraisP;
                    CDes.solde += montant;
                    GExp.FraisGest.Add(fraisP);
                    return true;
                }
                else if (GExp.typeGest == "Entreprise" && GExp.numGest != GDes.numGest)
                {
                    decimal fraisF = 10;
                    CExp.solde -= montant - fraisF;
                    CDes.solde += montant;
                    GExp.FraisGest.Add(fraisF);
                    return true;
                }
                else
                {
                    return false;
                }
            }
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
                    if (somme <= 1000) return false;
                    else return true;
                }
                else if (compte.historique.Count == NbTransac)
                {
                    compte.historique.RemoveAt(0);
                    compte.historique.Add(montant);
                    somme = compte.historique.Sum();
                    if (somme <= 1000) return false;
                    else return true;
                }
                else return true;
            }
            catch(IndexOutOfRangeException)
            {
                return true;
            }
        }

        public static int Traitement(Dictionary<int,string> statuts, List<Transactions> ListeTransactions, List<Comptes> cpts, List<ComptesClots> cpClot, List<Gestionnaires> listeGestionnaires)
        {
            int compteur = 0;
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
                        if (exp != des && !Transactions.VerifMaximum(mtn, CExp, GExp))
                        {
                            Transactions.Virement(mtn, CExp, GExp, CDest, GDest);
                            statuts.Add(idt, "OK");
                            compteur++;
                            continue;
                        }
                    }

                    //au moins un des deux comptes a été fermé
                    else if (cpClot.Count != 0)
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
                                compteur++;
                                continue;
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
                                compteur++;
                                continue;
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
                                compteur++;
                                continue;
                            }
                        }
                    }
                }

                //dépôt
                if (exp == 0 && des > 0)
                {
                    bool CompteDesti = cpts.Any(d => d.num == des);

                    //si le compte destinataire existe et est ouvert
                    if (CompteDesti)
                    {
                        Comptes CDest = cpts.Find(d => d.num == des);
                        Transactions.Dépot(mtn, CDest);
                        statuts.Add(idt, "OK");
                        compteur++;
                        continue;
                    }

                    //si des comptes ont été supprimés
                    else if (cpClot.Count != 0)
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

                            //si la transaction a eu lieu avant la cloture
                            if (DateCloture > dateEffet && dateEffet >= DateCreation)
                            {
                                Transactions.Dépot(mtn, CCDes);
                                statuts.Add(idt, "OK");
                                compteur++;
                                continue;
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
                        int numGestionnaire = CExp.gestionnaire;
                        Gestionnaires gest = listeGestionnaires.Find(g => g.numGest == numGestionnaire);
                        DateTime DateCreation = CExp.Date;

                        //si solde suffisant, plafond non atteint et si la transaction a eu lieu avant la création du compte
                        if (!Transactions.VerifMaximum(mtn, CExp, gest)
                            && DateCreation <= dateEffet)
                        {
                            Transactions.Retrait(mtn, CExp);
                            statuts.Add(idt, "OK");
                            compteur++;
                            continue;
                        }
                    }

                    //si des comptes ont été supprimés et le compte expediteur n'a pas été trouvé
                    else if (cpClot.Count != 0)
                    {
                        //recherche du compte expediteur parmis les comptes supprimés

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
                            if (!Transactions.VerifMaximum(mtn, CCExp, gest)
                                && DateCloture > dateEffet
                                && dateEffet >= DateCreation)
                            {
                                Transactions.Retrait(mtn, CCExp);
                                statuts.Add(idt, "OK");
                                compteur++;
                                continue;
                            }
                        }
                    }                                  
                }
                statuts.Add(idt, "KO");
            }
            return compteur;
        }
    }
}
