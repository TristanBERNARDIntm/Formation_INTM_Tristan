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
		public int[,] coordonées;
		public bool[] parois;
		public bool visitée;
		public string statut;

		public Cell()
		{
			coordonées = new int[,]{{0},{0}};
			parois = new bool[4] { false, false, false, false };
			visitée = false;
			statut = "simple";
		}
	}
}
