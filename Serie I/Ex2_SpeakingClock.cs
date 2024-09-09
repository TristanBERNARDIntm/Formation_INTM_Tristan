using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class SpeakingClock
    {
        public static string GoodDay(int heure)
        {
            string message;
            switch (heure) //détermination de l'heure
            {   
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    message = "Merveilleuse nuit !";
                break;

                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    message = "Bonne matinée !";
                break;

                case 12:
                    message = "Bon appétit !";
                break;

                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    message = "Profitez de votre après-midi !";
                break;

                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                    message = "Passez une bonne soirée !";
                break;

                default:
                    message = "heure non valide";
                break;

            }
            Console.WriteLine($"Il est {heure}h, {message}");
            return string.Empty;
        }
    }
}
