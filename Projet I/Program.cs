using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_I
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string comptes = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet I\bin\Debug\Comptes.csv";
            string transactions = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet I\bin\Debug\Transactions.csv";
            string status = @"C:\Users\FORMATION\source\repos\Formation_INTM_Tristan\Projet I\bin\Debug\StatutsTransactions.csv";

            Console.WriteLine("Fichier Comptes");
            using (StreamReader sr = new StreamReader(comptes))
            {
                string line = sr.ReadLine();
                string[] val;
                Dictionary<int,int> cpts = new Dictionary<int,int>();
                
                while (line != null)
                {
                  //  Console.WriteLine(line);
                    
                    val = line.Split(';');
                   
                        bool entierKey = int.TryParse(val[0], out int output);
                        bool entierValue = int.TryParse(val[1], out int output);
                        if (entierKey && entierValue)
                        {
                            cpts.Add(int.Parse(val[0]), int.Parse(val[1]));
                        }
                        else
                        {
                            continue;
                        }
                    
                    line = sr.ReadLine();
                }
                foreach (KeyValuePair<int,int> kv in cpts)
                {
                    Console.WriteLine(kv.Key + ";" + kv.Value);
                }
            }
            Console.WriteLine("Fichier Transactions");
            using (StreamReader sr = new StreamReader(transactions))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    Console.WriteLine(line);
                    line = sr.ReadLine();
                }
            }
            Console.WriteLine("Fichier Statuts");
            using (StreamReader sr = new StreamReader(status))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    Console.WriteLine(line);
                    line = sr.ReadLine();
                }
            }
            Console.ReadLine();
        }
    }
}
