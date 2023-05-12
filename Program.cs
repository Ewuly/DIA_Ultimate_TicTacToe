using System;

namespace TicTacToe
{
    internal class Morpion
    {
        static void Main(string[] args)
        {
            /*for(int i = 1; i < 10; i++)
            {
                Console.WriteLine((i-1) % 3 * 3 + 1);

            }
            Console.ReadLine();*/
            Console.WriteLine("Bienvenue sur le Morpion, choisissez qui commence en premier : ");
            Console.WriteLine("1-Humain");
            Console.WriteLine("2-Ordinateur");
            int mode_de_jeu = Convert.ToInt32(Console.ReadLine());
            Plateau plateau = new Plateau(mode_de_jeu);
            Console.Clear();
            Console.WriteLine("Voici la matrice Morpion : ");
            plateau.AffichePlateau();
            plateau.Fin();

            Console.ReadKey();
        }
    }
}
