using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_S1_NSIS001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Donner un entier");
            string a = Console.ReadLine();
            int entier = int.Parse(a);

            while(entier != 0)
            {
                entier -= 2;
                Console.WriteLine(entier);
            }


            Console.ReadKey();


               
        }
    }
}
