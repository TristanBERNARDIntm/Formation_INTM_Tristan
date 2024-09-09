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
            if (b == 0)
            {
                result = "Opération invalide.";
            }        
            else
            {
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
            Console.WriteLine($"{a} {operation} {b} = {result}");
        }

    public static void IntegerDivision(int a, int b)
        {
            int q = 0;
            int r = 0;

            if (b == 0)
            {
                Console.WriteLine($"{a} : {b} = Opération invalide");
            }
            else
            {
                q = a / b;
                r = a % b;
                switch (r)
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
        if (b < 0)
            {
                Console.WriteLine($"{a} ^ {b} = Opération invalide");
            }
        else if (b == 0)
            {
                p = 1;
                Console.WriteLine($"{a} ^ {b} = {p}");
            }
        else
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
