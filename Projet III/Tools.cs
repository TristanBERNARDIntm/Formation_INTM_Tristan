using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Projet_III
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
                bool jourV = uint.TryParse(tab[0], out uint jour);
				bool moisV = uint.TryParse(tab[1], out uint mois);
				bool anneeV = uint.TryParse(tab[2], out uint annee);
				if (jourV && moisV && anneeV)
				{
					if (VerifJourDansMois(jour, mois, annee)
						&& date <= dtnow 
						&& !string.IsNullOrWhiteSpace(tab[0])
                        && !string.IsNullOrWhiteSpace(tab[1])
                        && !string.IsNullOrWhiteSpace(tab[2])
						&& date != DateTime.MinValue)
					{
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

		public static bool AnneeBissextile(uint année)
		{
			if (année % 4 == 0 && année % 100 != 0 || année % 400 == 0)
			{
				return true;
			}
			return false;
		}

		public static bool VerifJourDansMois(uint jour, uint mois, uint année)
		{
			if (mois == 1 | mois == 3 | mois == 5 | mois == 7 | mois == 8 | mois == 10 | mois == 12 && jour <= 31) return true;
			if (mois == 4 | mois == 6 | mois == 9 | mois == 11 && jour <= 30) return true;
			if (mois == 2 && AnneeBissextile(année) && jour <= 29) return true;
			if (mois == 2 && !AnneeBissextile(année) && jour <= 28) return true;
			else return false;
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
            bool entierNum = uint.TryParse(val[0], out uint NumCpt);
			bool charType = char.TryParse(val[1], out char type);
            bool dateDate = DateTime.TryParse(val[2], out DateTime Date);
            string val3 = val[3].Replace(".", ",");
            bool decimalSolde = decimal.TryParse(val3, out decimal Solde);
            bool entierAge = uint.TryParse(val[4], out uint Age);
            bool entierEntrée = uint.TryParse(val[5], out uint Entrée);
            bool entierSortie = uint.TryParse(val[6], out uint Sortie);

			if (val.Length == 7
				&& entierNum
				&& charType 
				&& type == 'C' | type == 'J'| type == 'L'| type == 'T'| val[1] == string.Empty
				&& dateDate | val[2] == string.Empty
				&& decimalSolde | val[3] == string.Empty
				&& entierAge & Age >= 8 | val[4] == string.Empty
				&& entierEntrée | val[5] == string.Empty
				&& entierSortie | val[6] == string.Empty
				&& Tools.VerifDate(Date))
			{
                return true;
            }
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

		public static void VerifAge(Comptes compte, char Type, DateTime Date, uint Age)
		{
            TimeSpan TS = DateTime.Now - Date;
			double NbJoursCompte = TS.TotalDays;
			double NbJoursAge = (double)Age * 365.25;
			
            if (Type == 'J' && Age >= 18 | NbJoursCompte + NbJoursAge >= 6574.5)
            {
                compte.type = 'C';
            }
            if (Age >= 8 && Age < 18)
			{
				decimal mtn0 = 10 * Age;
				try { compte.solde.Add(Date, mtn0); }
				catch(ArgumentException)
				{
                    compte.solde[Date] += mtn0;
                }
			}
        }
	}
}

