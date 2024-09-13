using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Série_IV
{
    public static class BracketsControl
    {
        public static bool BracketsControls(string sentence)
        {
            Dictionary<char, char> paire = new Dictionary<char, char>   //définition d'un dictionnaire avec les parenthèses ouvrantes en indice et les fermantes en valeur associées
            {
                {'(',')'},
                {'[',']'},
                {'{','}'},
            };

            Stack<char> parent = new Stack<char>();                     //création d'une pile qui va stocker les parenthèses
            if (sentence.Count() == 0)                                  //si la pile est vide, aucunes parenthèses, condition OK
            { 
                return true;
            }
            foreach (char i in sentence)
            {
                if(paire.ContainsKey(i))                                //si la phrase contient une parenthèse ouvrante
                {
                    parent.Push(i);                                     //on ajoute la parenthèse ouvrante à la pile
                }
                else if(paire.Values.Contains(i))                       //si la phrase contient une parenthèse fermante (valeur de l'indice dans le dictionnaire)
                {
                    if (parent.Count == 0)                              //si la pile est vide (=parenthèse ouvrante absente) et qu'une parenthèse fermante est trouvée = KO
                    {
                        return false;
                    }
                    var opening = parent.Pop();                         //suppression de la dernière valeure ajoutée à la liste
                    if (paire[opening] != i)                            //si la valeur retournée (dernière valeur de la pile) est d'un indice (parenthès ouvrante) différent de la valeur actuelle, pas la bonne parenthèse fermante
                    {
                        return false;
                    }
                }
            }
            return parent.Count == 0;                                   //si la pile est vide, condition OK (true) sinon condition KO (false)
        }
    }
}