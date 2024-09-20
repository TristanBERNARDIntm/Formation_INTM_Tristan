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
	}
}

