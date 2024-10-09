using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Projet_II
{

	public static class Tools
	{
		//vérification de la conformité de la date
		public static bool VerifDate(DateTime date)
		{
			try
			{
				DateTime dtnow = DateTime.Now;
				string dt = date.ToString();
                string[] tab = dt.Split('/',' ');
                bool jourV = int.TryParse(tab[0], out int jour);
				bool moisV = int.TryParse(tab[1], out int mois);
				bool anneeV = int.TryParse(tab[2], out int annee);
				if (jourV && moisV && anneeV)
				{
					if (jour > 0 
						&& jour < 31 
						&& mois > 0 
						&& mois < 13 
						&& annee > 0 
						&& date <= dtnow 
						&& !string.IsNullOrWhiteSpace(tab[0])
                        && !string.IsNullOrWhiteSpace(tab[1])
                        && !string.IsNullOrWhiteSpace(tab[2]))
					{
						if (date == DateTime.MinValue)
						{
							return false;
						}
						return true;
					}
				}
				return false;
			}
			catch (IndexOutOfRangeException)
			{
				return false;
			}
        }
        //vérification de la conformité des variables du Gestionnaire
        public static bool VerifGestionnaire(string[] val, List<Gestionnaires> listeGestionnaires)
		{
            bool entier0 = int.TryParse(val[0], out int NumGest);
            string type = val[1];
            bool entier2 = int.TryParse(val[2], out int n);
            bool GestionnaireExistant = listeGestionnaires.Any(c => c.numGest == NumGest);

            if (val.Length == 3
				&& entier0
				&& !GestionnaireExistant
				&& entier2
				&& NumGest > 0
				&& type == "Particulier" | type == "Entreprise")
				return true;

            return false;
		}
		//vérification de la conformité des variables du Compte
		public static bool VerifComptes(string[] val)
		{
            bool entierNum = int.TryParse(val[0], out int NumCpt);
            bool dateDate = DateTime.TryParse(val[1], out DateTime Date);
            string val2 = val[2].Replace(".", ",");
            bool decimalSolde = decimal.TryParse(val2, out decimal Solde);
            bool entierEntrée = int.TryParse(val[3], out int Entrée);
            bool entierSortie = int.TryParse(val[4], out int Sortie);

			if (val.Length == 5
				&& entierNum
				&& dateDate
				&& decimalSolde | val[2] == string.Empty
				&& entierEntrée | val[3] == string.Empty
				&& entierSortie | val[4] == string.Empty
				&& NumCpt > 0
				&& Tools.VerifDate(Date)) 
				return true;
			return false;
        }
		//vérification de la conformité des variables de la Transaction
		public static bool VerifTransaction(string[] val)
		{
            bool entier0 = int.TryParse(val[0], out int idTransactions);
            bool dateDate = DateTime.TryParse(val[1], out DateTime Date);
            bool decimal1 = decimal.TryParse(val[2], out decimal montant);
            bool entier2 = int.TryParse(val[3], out int Expediteur);
            bool entier3 = int.TryParse(val[4], out int Destinataire);

			if (entier0 
				&& val.Length == 5
                && Tools.VerifDate(Date)
                && decimal1
                && entier2
                && entier3               
                && montant > 0) return true;			
            return false;
		}
	}
}

