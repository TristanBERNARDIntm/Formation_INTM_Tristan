using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class ElementaryOperations
    {
        public static void BasicOperation(int a, int b, char operation)
        {
            int calcul = 0;
            string result;
            //pas de divisons par zéro
            if (b == 0)
            {
                result = "Opération invalide.";
            }   
            //si dénominateur différent de zéro
            else
            {
                //on détermine le type d'opération à effectuer
                switch (operation)
                {
                    case '+' :
                        calcul = a + b;
                        result = calcul.ToString();
                        break;

                    case '-' :
                        calcul = a - b;
                        result = calcul.ToString();
                        break;

                    case '*' :
                        calcul = a * b;
                        result = calcul.ToString();
                        break;  
                        
                    case '/' :
                        calcul = a / b;
                        result = calcul.ToString();
                        break;

                    default :
                        result = "Opération invalide.";
                    break;
                }
            }
            // on écrit le calcul avec le résultat
            Console.WriteLine($"{a} {operation} {b} = {result}");
        }

    public static void IntegerDivision(int a, int b)
        {
            int q = 0;
            int r = 0;
           
            if (b == 0)  // pas de divisions par zéro
            {
                Console.WriteLine($"{a} : {b} = Opération invalide");
            }
            else  // récupération du quotient et du reste
            {
                q = a / b;
                r = a % b;
                switch (r) //si le reste est nul, on ne l'affiche pas, sinon on affiche a = q * b + r
                {
                    case 0:
                    Console.WriteLine($"{a} = {q} * {b}");
                    break;

                    default:
                    Console.WriteLine($"{a} = {q} * {b} + {r}");
                    break;
                }
            }
        }

    public static void Pow(int a, int b)
        {
        int p = a;
        if (b < 0) //si la puissance est négative, on ne traite pas l'opération
            {
                Console.WriteLine($"{a} ^ {b} = Opération invalide");
            }
        else if (b == 0) //si la puissance est à zéro, on retourne 1
            {
                p = 1;
                Console.WriteLine($"{a} ^ {b} = {p}");
            }
        else //calcul de la puissance 
            {
                for (int i = 1 ; i < b ; i++)
                {
                    p = p * a;
                }
                Console.WriteLine($"{a} ^ {b} = {p}");
            }
        }
    }
}
