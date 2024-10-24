﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Projet_II
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //chemin d'accès fichiers
            string fcomptes = @"C:\Users\Formation\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Comptes.txt";
            string ftransactions = @"C:\Users\Formation\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Transactions.txt";
            string fstatuts = @"C:\Users\Formation\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\StatutsTransactions.txt";
            string fgest = @"C:\Users\Formation\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Gestionnaires.txt"; 
            string fresult = @"C:\Users\Formation\source\repos\Formation_INTM_Tristan\Projet II\bin\Debug\Resultats.txt";

            // recupération du fichier gestionnaires dans une liste d'objet
            List<Gestionnaires> listeGestionnaires = new List<Gestionnaires>();
            Gestionnaires.LectureGestionnaire(fgest, listeGestionnaires);
           
            //recupération du fichier comptes dans une liste d'objets
            //transfert des comptes supprimés dans une seconde liste d'objets
            List<Comptes> cpts = new List<Comptes>();
            List<ComptesClots> cpClot = new List<ComptesClots>();
            Comptes.LectureComptes(fcomptes, cpts, cpClot, listeGestionnaires);

            //récupération du fichier transactions dans une liste d'objet
            List<Transactions> ListeTransactions = new List<Transactions>();
            Transactions.LectureTransactions(ftransactions, ListeTransactions);

            //traitement des transactions
            Dictionary<int,string> statuts = new Dictionary<int,string>();
            Transactions.Traitement(statuts, ListeTransactions, cpts, cpClot, listeGestionnaires);          

            //écriture des statuts de transactions
            using (StreamWriter sw = new StreamWriter(fstatuts))
            {
                foreach(KeyValuePair<int,string> ligne in statuts)
                {
                    sw.WriteLine($"{ligne.Key};{ligne.Value}");
                }
            }

            //écriture du résultat
            using (StreamWriter sw = new StreamWriter(fresult))
            {
                sw.WriteLine("Statistiques :");
                sw.WriteLine($"Nombre de comptes : {Comptes.NbComptes}");
                sw.WriteLine($"Nombre de transactions : {Transactions.NombreTrans}");
                sw.WriteLine($"Nombre de réussites : {Transactions.NbTransOK}");
                sw.WriteLine($"Nombre d'échecs : {Transactions.NbTransKO}");
                sw.WriteLine($"Montant total des réussites : {Transactions.totMontant} euros");
                sw.WriteLine();
                sw.WriteLine($"Frais de gestions :");
                foreach(Gestionnaires gestionnaire in listeGestionnaires)
                {
                    decimal totalfrais = gestionnaire.FraisGest.Sum();
                    sw.WriteLine($"{gestionnaire.numGest} : {totalfrais} euros");
                }
            }    
        }
    }
}

