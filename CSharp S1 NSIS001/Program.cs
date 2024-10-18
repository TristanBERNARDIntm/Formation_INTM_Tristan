using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_S1_NSIS001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<DateTime, decimal> dico = new Dictionary<DateTime, decimal>();
            DateTime min = DateTime.MinValue;
            decimal mtn1 = 10.10m;
            DateTime max = DateTime.MaxValue;
            decimal mtn2 = 89.90m;
            dico.Add(min, mtn1);
            dico.Add(max, mtn2);
            decimal solde = dico.Where(key => key.Key < DateTime.Today).Sum(x => x.Value);
            Console.WriteLine(solde);
           
            DateTime DT = DateTime.Now;
            DT = new DateTime(DT.Year, DT.Month, 1);
            DT = DT.AddMonths(2);
            Console.WriteLine(DT);
            Console.ReadLine();

        }
    }
}
