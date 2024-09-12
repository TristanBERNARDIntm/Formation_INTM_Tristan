using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_IV
{
    public static class BracketsControl
    {
        public static bool BracketsControls(string sentence)
        {
            Dictionary<char, char> paire = new Dictionary<char, char>
            {
                {'(',')'},
                {'[',']'},
                {'{','}'},
            };

            var s = new Stack<string>(sentence);
            Stack<char> parent = new Stack<char>();
            if (s.Count() == 0)
            { 
                return true;
            }
            foreach (char i in s)
            {
                if(paire.ContainsKey(i))
                {
                    parent.Push(i);
                }
                else if(paire.Values.Contains(i))
                {
                    if (parent.Count == 0)
                    {
                        return false;
                    }
                    var opening = parent.Pop();
                    if (paire[opening] != i)
                    {
                        return false;
                    }
                }
            }
            return parent.Count == 0;
        }
    }
}
