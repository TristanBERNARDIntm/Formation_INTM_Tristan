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
	}
}
