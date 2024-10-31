using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace Labyrinthe
{
	public class Cell
	{
		public bool[] parois;
		public bool visitée;
		public string statut; //simple, entrée ou sortie
		public bool extremité;

		public Cell()
		{
			parois = new bool[4] { false, false, false, false }; //0 = haut, 1 = droite, 2 = bas, 3 = gauche
			visitée = false;
			statut = "simple"; 
			extremité = false;
		}
	}
}
