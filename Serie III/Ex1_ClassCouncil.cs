using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_III
{
    public static class ClassCouncil
    {
        public static void SchoolMeans(string input, string output)
        { 
              var reader = new StreamReader(File.OpenRead(input));
              List<string> listA = new List<string>();
              List<string> listB = new List<string>();
              List<string> listC = new List<string>();
              while (!reader.EndOfStream)
              {
                  var line = reader.ReadLine();
                  var values = line.Split(';');

                  listA.Add(values[0]);
                  listB.Add(values[1]);
                  listC.Add(values[2]);

                  foreach (var coloumn1 in listA)
                  {
                      Console.WriteLine(coloumn1);
                  }
                  foreach (var coloumn2 in listA)
                  {
                      Console.WriteLine(coloumn2);
                  }
              }
        }
    }
}
