using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace Projet_II
{

	public class Gestionnaires
	{
		public int numGest { get; set; }
		public string typeGest { get; set; }
		public int NbTransGest { get; set; }
		public List<decimal> FraisGest { get; set; }

		public Gestionnaires()
		{
			numGest = 0;
			typeGest = string.Empty;
			NbTransGest = 0;
			FraisGest = new List<decimal>();
        }

		public static void LectureGestionnaire(string fgest, List<Gestionnaires> listeGestionnaires)
		{
            using (StreamReader sr = new StreamReader(fgest))
            {
                string ligne = sr.ReadLine();
                string[] val;

                while (ligne != null) 
                {
                    val = ligne.Split(';');  
                    
                    if (Tools.VerifGestionnaire(val, listeGestionnaires))
                    {
                        Gestionnaires gestion = new Gestionnaires();
                        listeGestionnaires.Add(gestion);
                        gestion.numGest = int.Parse(val[0]);
                        gestion.typeGest = val[1];
                        gestion.NbTransGest = int.Parse(val[2]);
                    }
                    ligne = sr.ReadLine();
                }
            }
        }
	}
}

