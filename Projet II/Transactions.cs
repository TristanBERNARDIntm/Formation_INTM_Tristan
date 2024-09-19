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

        public Transactions()
        {
            id = 0;
            montant = 0; 
            expediteur = 0;
            destinataire = 0;
            NombreTrans++;
        }

        public static void D�pot(decimal montant, Comptes CDes)
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

        public static bool Virement(decimal montant, Comptes CExp, Comptes CDes)
        {
            if (VerifSolde(montant,CExp))
            {
                CExp.solde -= montant;
                CDes.solde += montant;
                return true;
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
                if (compte.historique.Count == 0)
                {
                    compte.historique.Add(montant);
                    somme = compte.historique.Sum();
                    if (somme <= 1000)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (compte.historique.Count >= NbTransac)
                {
                    compte.historique.RemoveAt(0);
                    compte.historique.Add(montant);
                    somme = compte.historique.Sum();
                    if (somme <= 1000)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(IndexOutOfRangeException)
            {
                return false;
            }
        }
    }
}