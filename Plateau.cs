using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Plateau
    {
        char symboleH;
        char symboleO;
        int mode_de_jeu;
        char[,] matrice;
        int[] coupPrecedant;
        public Plateau(int mode_de_jeu)
        {
            this.mode_de_jeu = mode_de_jeu;
            if (mode_de_jeu == 1)
            {
                symboleH = 'X';
                symboleO = 'O';
            }
            else
            {
                symboleH = 'O';
                symboleO = 'X';
            }
            this.matrice = new char[9, 9];
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    matrice[i, j] = '*';
                }
            }
            this.coupPrecedant = new int[] { -1, -1 };
        }
        public void AffichePlateau()
        {
            Console.WriteLine("  123   456   789");
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                Console.Write(i+1 + " ");
                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    Console.Write(matrice[i, j]);
                    if((j+1)%3==0)
                    {
                        Console.Write("   ");
                    }
                }
                Console.WriteLine();
                if((i+1)%3==0)
                {
                    Console.WriteLine();
                }
            }
        }
        public void Fin()
        {
            int indice = mode_de_jeu - 1;
            while (victoire() == '*' && matriceCompleteTotal() == false)
            {
                if (indice % 2 == 0)
                {
                    CoupH();
                    indice++;
                    if(victoireLocal()==symboleH)
                    {
                        int ligneCoupPre = (coupPrecedant[0] - 1) / 3 * 3;
                        int colonneCoupPre = (coupPrecedant[1] - 1) / 3 * 3;
                        for(int i=ligneCoupPre;i<ligneCoupPre+3;i++)
                        {
                            for(int j=colonneCoupPre;j<colonneCoupPre+3;j++)
                            {
                                matrice[i, j] = symboleH;
                            }
                        }
                    }
                    Console.Clear();
                    AffichePlateau();
                }
                else
                {
                    if(matriceVide() || matriceCompleteLocal())
                    {
                        CoupO();
                        indice++; 
                    }
                    else
                    {
                        int ligneNouvCoup = (coupPrecedant[0] - 1) % 3 * 3 + 1;
                        int colonneNouvCoup = (coupPrecedant[1] - 1) % 3 * 3 + 1;

                        char[,] minMatrice = new char[3, 3];

                        for (int i = 0; i < minMatrice.GetLength(0); i++)
                        {
                            for (int j = 0; j < minMatrice.GetLength(1); j++)
                            {
                                minMatrice[i, j] = matrice[ligneNouvCoup + i - 1, colonneNouvCoup + j - 1];
                            }
                        }
                        int[] bestMove = CoupIA2(minMatrice);
                        minMatrice[bestMove[0], bestMove[1]] = symboleO;
                        AfficheMiniMatrice(minMatrice);
                        matrice[ligneNouvCoup - 1 + bestMove[0], colonneNouvCoup - 1 + bestMove[1]] = symboleO;
                        indice++;
                        coupPrecedant[0] = ligneNouvCoup + bestMove[0];
                        coupPrecedant[1] = colonneNouvCoup + bestMove[1];
                        
                    }
                    int ligneCoupPre = (coupPrecedant[0] - 1) / 3 * 3;
                    int colonneCoupPre = (coupPrecedant[1] - 1) / 3 * 3;

                    if (victoireLocal2(ligneCoupPre,colonneCoupPre) == symboleO)
                    {

                        for (int i = ligneCoupPre; i < ligneCoupPre + 3; i++)
                        {
                            for (int j = colonneCoupPre; j < colonneCoupPre + 3; j++)
                            {
                                matrice[i, j] = symboleO;
                            }
                        }
                    }
                    Console.ReadLine();
                    Console.Clear();
                    AffichePlateau();
                }
                

            }
            if (victoire() == symboleH)
            {
                Console.WriteLine("Bravo ! Vous avez gagné !");
            }
            else
            {
                if (victoire() == symboleO)
                {
                    Console.WriteLine("Dommage ! Vous avez perdu :(");
                }
                else
                {
                    Console.WriteLine("Match Nul !");
                }
            }
        }
        public int[] CoupIA2(char[,] minMatrice)
        {
            float bestScore = -100000;
            int[] bestMove = new int[2];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //if the spot available
                    if (minMatrice[i, j] == '*')
                    {
                        minMatrice[i, j] = symboleO;
                        float score = minimax(minMatrice, 0, false);
                        minMatrice[i, j] = '*';
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove[0] = i;
                            bestMove[1] = j;
                        }
                    }
                }
            }
            return bestMove;
        }
        public void CoupTest()
        {
            int ligneNouvCoup = (coupPrecedant[0] - 1) % 3 * 3 + 1;
            int colonneNouvCoup = (coupPrecedant[1] - 1) % 3 * 3 + 1;


            int[] bestMove = new int[2];
            //Console.WriteLine(bestMove[0]+bestMove[1]);
            //Console.ReadLine();
            char[,] minMatrice = new char[3, 3];

            for (int i = 0; i < minMatrice.GetLength(0); i++)
            {
                for (int j = 0; j < minMatrice.GetLength(1); j++)
                {
                    minMatrice[i, j] = matrice[ligneNouvCoup + i - 1, colonneNouvCoup + j - 1];
                }
            }
            Morpion1 test = new Morpion1(mode_de_jeu+1, minMatrice);
            int[] bestMove2=test.CoupIA();
            int li = ligneNouvCoup - 1 + bestMove[0];
            int co = colonneNouvCoup - 1 + bestMove[1];
            //Console.WriteLine(li + " " + co);
            //Console.ReadLine();
            matrice[li, co] = symboleO;
            coupPrecedant[0] = ligneNouvCoup + bestMove[0];
            coupPrecedant[1] = colonneNouvCoup + bestMove[1];
            Console.Clear();
            AffichePlateau();
        }
        public void AfficheMiniMatrice(char[,] mat)
        {
            for(int i = 0; i < mat.GetLength(0); i++)
            {
                for(int j = 0; j < mat.GetLength(1); j++)
                {
                    Console.Write(mat[i, j]);
                }
                Console.WriteLine();
            }
            //Console.ReadLine();
        }
        public void CoupIA()
        {
            if (matriceVide() == true)
            {
                matrice[4, 4] = symboleO;
                coupPrecedant[0] = 5;
                coupPrecedant[1] = 5;
            }
            else
            {
                int ligneNouvCoup = (coupPrecedant[0] - 1) % 3 * 3 +1;
                int colonneNouvCoup = (coupPrecedant[1] - 1) % 3 * 3 +1;


                float bestScore = -1000000;
                int[] bestMove = new int[2];
                //Console.WriteLine(bestMove[0]+bestMove[1]);
                //Console.ReadLine();
                char[,] minMatrice=new char[3,3];
       
                for(int i=0; i<minMatrice.GetLength(0); i++)
                {
                    for(int j=0;j<minMatrice.GetLength(1);j++)
                    {
                        minMatrice[i,j]=matrice[ligneNouvCoup+i-1,colonneNouvCoup+j-1];
                    }
                }
                //AfficheMiniMatrice(minMatrice);

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        //if the spot available
                        if (minMatrice[i, j] == '*')
                        {
                            minMatrice[i, j] = symboleO;
                            float score = minimax(minMatrice, 0, false);
                            minMatrice[i, j] = '*';
                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestMove[0] = i;
                                bestMove[1] = j;
                            }
                        }
                    }
                }
                minMatrice[bestMove[0], bestMove[1]] = symboleO;
                //AfficheMiniMatrice(minMatrice);
                //Console.ReadLine();
                //Console.WriteLine(bestScore);
                int li = ligneNouvCoup - 1 + bestMove[0];
                int co = colonneNouvCoup - 1 + bestMove[1];
                //Console.WriteLine(li + " " + co);
                //Console.ReadLine();
                matrice[li,co] = symboleO;
                coupPrecedant[0] = ligneNouvCoup + bestMove[0] ;
                coupPrecedant[1] = colonneNouvCoup + bestMove[1];
            }
            Console.Clear();
            AffichePlateau();
        }
        public char victoireMinMat(char[,] minMatrice)
        {
            char b = '*';
            if (matriceCompleteLocal2(minMatrice))
            {
                b = 'D';
            }
            //Horizontal
            for (int i = 0; i < 3; i++)
            {
                if (minMatrice[i, 0] == minMatrice[i, 1] && minMatrice[i, 1] == minMatrice[i, 2] && minMatrice[i, 0] != '*')
                {
                    b = minMatrice[i, 0];
                }
            }
            //Vertical
            for (int i = 0; i < 3; i++)
            {
                if (minMatrice[0, i] == minMatrice[1, i] && minMatrice[1, i] == minMatrice[2, i] && minMatrice[0, i] != '*')
                {
                    b = minMatrice[0, i];
                }
            }
            //Diagonal
            if (minMatrice[0, 0] == minMatrice[1, 1] && minMatrice[1, 1] == minMatrice[2, 2] && minMatrice[0, 0] != '*')
            {
                b = minMatrice[0, 0];
            }
            if (minMatrice[0, 2] == minMatrice[1, 1] && minMatrice[1, 1] == minMatrice[2, 0] && minMatrice[1, 1] != '*')
            {
                b = minMatrice[0, 2];
            }
            return b;
        }
        public bool matriceCompleteLocal2(char[,] mat)
        {
            bool b = true;
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (mat[i, k] == '*')
                        b = false;
                }
            }
            return b;
        }
        public float minimax(char[,] minMatrice, int depth, bool isMaximazing)
        {
            //AfficheMiniMatrice(minMatrice);
            //Console.WriteLine();
            char result = victoireMinMat(minMatrice);
            if (result != '*')
            {
                return ResulScore(result, depth);
            }



            if (isMaximazing)
            {
                float bestScore = -100000;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        //if the spot available
                        if (minMatrice[i, j] == '*')
                        {
                            minMatrice[i, j] = symboleO;
                            float score = minimax(minMatrice, depth + 1, false);
                            minMatrice[i, j] = '*';

                            if (score > bestScore)
                            {
                                bestScore = score;
                            }
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                float bestScore = 100000;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        //if the spot available
                        if (minMatrice[i, j] == '*')
                        {
                            minMatrice[i, j] = symboleH;
                            float score = minimax(minMatrice, depth + 1, true);
                            minMatrice[i, j] = '*';
                            if (score < bestScore)
                            {
                                bestScore = score;
                            }
                        }
                    }
                }
                return bestScore;
            }


        }


        /*
        char result = victoireLocal();
            if (result != '*')
            {
                return ResulScore(result);
            }
            if (depth <= 10)
            {
                if (isMaximazing)
                {
                    float bestScore = -10000000;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            //if the spot available
                            if (minMatrice[i, j] == '*')
                            {
                                minMatrice[i, j] = symboleO;
                                float score = minimax(minMatrice, depth + 1, false);
                                minMatrice[i, j] = '*';

                                if (score > bestScore)
                                {
                                    bestScore = score;
                                }
                            }
                        }
                    }
                    return bestScore;
                }
                else
                {
                    float bestScore = 100000000;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            //if the spot available
                            if (minMatrice[i, j] == '*')
                            {
                                minMatrice[i, j] = symboleH;
                                float score = minimax(minMatrice, depth + 1, true);
                                minMatrice[i, j] = '*';
                                if (score < bestScore)
                                {
                                    bestScore = score;
                                }
                            }
                        }
                    }
                    return bestScore;
                }
            }
            else
            {
                return ResulScore(result);
            }
        }*/
        public float ResulScore(char c,int depth)
        {
            if (c == symboleO && depth == 0)
            {
                return 10;
            }
            else
            {
                if (c == symboleO && depth == 1)
                {
                    return 7;
                }
                else
                {
                    if (c == symboleO && depth == 2)
                    {
                        return 4;
                    }
                    else
                    {
                        if (c == symboleH)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }

            }
        }
        /*public void CoupIA2()
        {
            if(mode_de_jeu!=1 && matriceVide()==true)
            {
                matrice[0, 0] = symboleO;
            }
            else
            {
                MeilleurCoup(symboleO);
            }
            Console.Clear();
            AffichePlateau();
        }*/
        /*public int MeilleurCoup(char c)
        {
            int score=0;
            int meilleurLigne = -1;
            int meilleurColonne = -1;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(matrice[i, j] == '*')
                    {
                        char[,] copie = matrice;
                        copie[i, j] = c;
                        score = Score(i, j, c);

                        if(score==1)
                        {
                            meilleurLigne=i;
                            meilleurColonne=j;
                        }

                    }
                }
            }
            if(meilleurColonne!=-1 && meilleurColonne!=-1)
            {
                matrice[meilleurLigne, meilleurColonne] = c;
            }
            else
            {
                while (CoupValide(meilleurLigne, meilleurColonne) == false)
                {
                    Random aleatoire = new Random();
                    meilleurLigne = aleatoire.Next(1, 4); // Génère un entier compris entre 1 et 12
                    meilleurColonne = aleatoire.Next(1, 4);
                }
                matrice[meilleurLigne - 1, meilleurColonne - 1] = symboleO;
                Console.Clear();
                AffichePlateau();
            }
            return score;
        }*/
        /*public int Score(int i,int j,char c)
        {
            char[,] copie = matrice;
            copie[i, j] = c;
            if (victoire(c))
                return 1;
            if(matriceComplete(copie) && victoire(c)==false)
                return 0;
            else
            {
                return 0 - MeilleurCoup(symboleH);
            }
        }*/
        public bool matriceVide()
        {
            bool b = true;
            for (int j = 0; j < matrice.GetLength(0); j++)
            {
                for (int i = 0; i < matrice.GetLength(1); i++)
                {
                    if (matrice[i, j] != '*')
                        b = false;
                }
            }
            return b;
        }
        public void CoupO()
        {
            if(matriceVide()==true)
            {
                matrice[4,4] = symboleO;
                coupPrecedant[0] = 5;
                coupPrecedant[1] = 5;
            }
            else
            {
                if(matriceCompleteLocal() != true)
                {
                    int ligneNouvCoup = (coupPrecedant[0] - 1) % 3 * 3 + 1;
                    int colonneNouvCoup = (coupPrecedant[1] - 1) % 3 * 3 + 1;
                    int ligne = -1;
                    int colonne = -1;
                    while (CoupValide(ligne, colonne,false) == false)
                    {

                        Random aleatoire = new Random();
                        ligne = aleatoire.Next(ligneNouvCoup, ligneNouvCoup + 3); // Génère un entier compris entre 1 et 12
                        colonne = aleatoire.Next(colonneNouvCoup, colonneNouvCoup + 3);
                    }
                    matrice[ligne - 1, colonne - 1] = symboleO;
                    coupPrecedant[0] = ligne;
                    coupPrecedant[1] = colonne;
                }
                else
                {
                    int ligne = -1;
                    int colonne = -1;
                    while (CoupValide(ligne, colonne,true) == false)
                    {

                        Random aleatoire = new Random();
                        ligne = aleatoire.Next(1, 10); // Génère un entier compris entre 1 et 12
                        colonne = aleatoire.Next(1, 10);
                    }
                    matrice[ligne - 1, colonne - 1] = symboleO;
                    coupPrecedant[0] = ligne;
                    coupPrecedant[1] = colonne;
                }
            }
            Console.Clear();
            AffichePlateau();
        }

        public void CoupH()
        {
            int ligne = -1;
            int colonne = -1;
            int ligneNouvCoup = (coupPrecedant[0] - 1) % 3 * 3 + 1;
            int colonneNouvCoup = (coupPrecedant[1] - 1) % 3 * 3 + 1;
            if(ligneNouvCoup != -5 && matriceCompleteLocal()!=true)
            {
                while (CoupValide(ligne, colonne,false) == false)
                {
                    Console.WriteLine("Choisissez une ligne (" + ligneNouvCoup + "," + (ligneNouvCoup + 1) + "ou" + (ligneNouvCoup + 2) + ")");
                    ligne = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Choisissez une colonne (" + colonneNouvCoup + "," + (colonneNouvCoup + 1) + "ou" + (colonneNouvCoup + 2) + ")");
                    colonne = Convert.ToInt32(Console.ReadLine());
                    if (CoupValide(ligne, colonne,false) == false)
                    {
                        Console.Clear();
                        Console.WriteLine("Le coup n'est pas valide");
                        AffichePlateau();
                    }
                }
                matrice[ligne - 1, colonne - 1] = symboleH;
                coupPrecedant[0] = ligne;
                coupPrecedant[1] = colonne;
            }
            else
            {
                while (CoupValide(ligne, colonne,true) == false)
                {
                    Console.WriteLine("Choisissez une ligne ( 1 à 9 )");
                    ligne = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Choisissez une colonne ( 1 à 9 )");
                    colonne = Convert.ToInt32(Console.ReadLine());
                    if (CoupValide(ligne, colonne,true) == false)
                    {
                        Console.Clear();
                        Console.WriteLine("Le coup n'est pas valide");
                        AffichePlateau();
                    }
                }
                matrice[ligne - 1, colonne - 1] = symboleH;
                coupPrecedant[0] = ligne;
                coupPrecedant[1] = colonne;
            }
            
            Console.Clear();
            AffichePlateau();

        }
        public bool matriceCompleteTotal()
        {
            bool b = true;
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                for (int k = 0; k < matrice.GetLength(1); k++)
                {
                    if (matrice[i, k] == '*')
                        b = false;
                }
            }
            return b;
        }
        public bool matriceCompleteLocal()
        {
            bool b = true;
            int ligneNouvCoup = (coupPrecedant[0] - 1) % 3 * 3;
            int colonneNouvCoup = (coupPrecedant[1] - 1) % 3 * 3;
            for (int i = ligneNouvCoup; i < ligneNouvCoup + 3; i++)
            {
                for (int j = colonneNouvCoup; j < colonneNouvCoup + 3; j++)
                {
                    if (matrice[i, j] == '*')
                        b = false;
                }
            }
            return b;
        }
        public char victoireLocal()
        {
            int ligneCoupPre = (coupPrecedant[0]-1) / 3 * 3;
            int colonneCoupPre = (coupPrecedant[1]-1) / 3 * 3;
            char b = '*';
            if (matriceCompleteTotal())
            {
                b = 'D';
            }
            
            if(coupPrecedant[0]!=-1)
            {
                //Horizontal
                for (int i = ligneCoupPre; i < ligneCoupPre + 3; i++)
                {
                    if (matrice[i, colonneCoupPre] == matrice[i, colonneCoupPre + 1] && matrice[i, colonneCoupPre + 1] == matrice[i, colonneCoupPre + 2] && matrice[i, colonneCoupPre] != '*')
                    {
                        b = matrice[i, colonneCoupPre];
                    }
                }
                //Vertical
                for (int i = colonneCoupPre; i < colonneCoupPre + 3; i++)
                {
                    if (matrice[ligneCoupPre, i] == matrice[ligneCoupPre + 1, i] && matrice[ligneCoupPre + 1, i] == matrice[ligneCoupPre + 2, i] && matrice[ligneCoupPre, i] != '*')
                    {
                        b = matrice[ligneCoupPre, i];
                    }
                }
                //Diagonal
                if (matrice[ligneCoupPre, colonneCoupPre] == matrice[ligneCoupPre + 1, colonneCoupPre + 1] && matrice[ligneCoupPre + 1, colonneCoupPre + 1] == matrice[ligneCoupPre + 2, colonneCoupPre + 2] && matrice[ligneCoupPre, colonneCoupPre] != '*')
                {
                    b = matrice[ligneCoupPre, colonneCoupPre];
                }
                if (matrice[ligneCoupPre, colonneCoupPre+2] == matrice[ligneCoupPre+1, colonneCoupPre+ 1] && matrice[ligneCoupPre+1, colonneCoupPre+ 1] == matrice[ligneCoupPre+2, colonneCoupPre] && matrice[ligneCoupPre+1, colonneCoupPre+ 1] != '*')
                {
                    b = matrice[ligneCoupPre, colonneCoupPre+2];
                }
            }
            
            return b;
        }
        public char victoireLocal2(int ligne,int colonne)
        {
            char b = '*';
            int ligneCoupPre = ligne;
            int colonneCoupPre = colonne;

            if (matriceCompleteTotal())
            {
                b = 'D';
            }

            //Horizontal
            for (int i = ligne; i < ligne + 3; i++)
            {
                if (matrice[i, colonneCoupPre] == matrice[i, colonneCoupPre + 1] && matrice[i, colonneCoupPre + 1] == matrice[i, colonneCoupPre + 2] && matrice[i, colonneCoupPre] != '*')
                {
                    b = matrice[i, colonneCoupPre];
                }
            }
            //Vertical
            for (int i = colonneCoupPre; i < colonneCoupPre + 3; i++)
            {
                if (matrice[ligneCoupPre, i] == matrice[ligneCoupPre + 1, i] && matrice[ligneCoupPre + 1, i] == matrice[ligneCoupPre + 2, i] && matrice[ligneCoupPre, i] != '*')
                {
                    b = matrice[ligneCoupPre, i];
                }
            }
            //Diagonal
            if (matrice[ligneCoupPre, colonneCoupPre] == matrice[ligneCoupPre + 1, colonneCoupPre + 1] && matrice[ligneCoupPre + 1, colonneCoupPre + 1] == matrice[ligneCoupPre + 2, colonneCoupPre + 2] && matrice[ligneCoupPre, colonneCoupPre] != '*')
            {
                b = matrice[ligneCoupPre, colonneCoupPre];
            }
            if (matrice[ligneCoupPre, colonneCoupPre + 2] == matrice[ligneCoupPre + 1, colonneCoupPre + 1] && matrice[ligneCoupPre + 1, colonneCoupPre + 1] == matrice[ligneCoupPre + 2, colonneCoupPre] && matrice[ligneCoupPre + 1, colonneCoupPre + 1] != '*')
            {
                b = matrice[ligneCoupPre, colonneCoupPre + 2];
            }
            return b;
        }
        public char victoire()
        {
            char b = '*';
            if (matriceCompleteTotal())
            {
                b = 'D';
            }
            //Horizontal
            for(int i = 0; i < matrice.GetLength(0); i=i+3)
            {
                if (victoireLocal2(i, 0) == victoireLocal2(i, 3) && victoireLocal2(i, 3)== victoireLocal2(i, 6) && victoireLocal2(i, 0)!='*')
                {
                    b = victoireLocal2(i, 0);
                }
            }

            //Vertical
            for (int i = 0; i < matrice.GetLength(1); i=i+3)
            {
                if (victoireLocal2(0, i) == victoireLocal2(3, i) && victoireLocal2(3, i)== victoireLocal2(6, i) && victoireLocal2(0, i)!='*')
                {
                    b= victoireLocal2(0, i);
                }
            }

            //Diagonal
            if(victoireLocal2(0,0)== victoireLocal2(3, 3) && victoireLocal2(0, 0)== victoireLocal2(6, 6) && victoireLocal2(0, 0)!='*')
            {
                b = victoireLocal2(0, 0);
            }
            if(victoireLocal2(0, 6)== victoireLocal2(3, 3)&& victoireLocal2(3, 3)== victoireLocal2(6, 0) && victoireLocal2(3, 3)!='*')
            {
                b = victoireLocal2(3, 3);
            }


            return b;
        }

        public int[] MatriceAutorise(int[] coupPrecedant)
        {
            int[] coup = new int[2];
           
            if(matriceCompleteLocal()==false)
            {
                coup[0] = coupPrecedant[0] / 3 * 3;
                coup[1] = coupPrecedant[1] / 3 * 3;
            }
            else
            {
                coup[0] = -1;
                coup[1] = -1;
            }
            return coup;
        }
        public bool CoupValide(int ligne, int colonne,bool matdejarempli)
        {
            int ligneCoupPre = (coupPrecedant[0] - 1) % 3 * 3 + 1;
            int colonneCoupPre = (coupPrecedant[1] - 1) % 3 * 3 + 1;
            bool b = true;
            if (ligne <= 0 || colonne <= 0 || ligne > 9 || colonne > 9)
                return false;
            if(matdejarempli==false)
            {
                if ((ligne < ligneCoupPre || ligne > ligneCoupPre + 3) && ligneCoupPre != -5)
                {
                    return false;
                }
                if ((colonne < colonneCoupPre || colonne > colonneCoupPre + 3) && colonneCoupPre != -5)
                {
                    return false;
                }
            }
            
            if (matrice[ligne - 1, colonne - 1] != '*')
                b = false;

            return b;
        }

    }
}
