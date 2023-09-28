using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomFunction
{
    /* 
    
    Rien(0),

    Terre (1), Terre_Grass (2), Coal (3), Copper (4)

    Stone (5), Quartz (6), Pink Gold (7), Gold (8),

    Corundum (9), Silver (10), Emerald (11), Ruby (12),

    Obsidian (13), Black_Onyx (14), Diamond (15),

   */
    static int[,] tabProb =  {
                     /* { 0 , 1 , 2 , 3 , 4 , 5 , 6 , 7 , 8 , 9 , 10, 11, 12, 13, 14, 15}*/
/*1*/                   { 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
/*2*/                   { 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
/*3*/                   { 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
/*4*/                   { 00, 20, 00, 02, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
/*5*/                   { 00, 20, 00, 04, 00, 02, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
/*6*/                   { 00, 20, 00, 06, 01, 05, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
/*7*/                   { 00, 20, 00, 06, 02, 08, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},

/*8*/                   { 00, 16, 00, 08, 04, 40, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
/*9*/                   { 00, 08, 00, 02, 02, 40, 03, 00, 00, 00, 00, 00, 00, 00, 00, 00},
/*10*/                  { 00, 06, 00, 01, 01, 40, 07, 02, 00, 01, 00, 00, 00, 00, 00, 00},
/*11*/                  { 00, 06, 00, 01, 01, 40, 07, 04, 00, 04, 00, 00, 00, 00, 00, 00},
/*12*/                  { 00, 06, 00, 01, 01, 40, 07, 06, 02, 10, 00, 00, 00, 00, 00, 00},
/*13*/                  { 00, 06, 00, 01, 01, 40, 07, 07, 05, 15, 00, 00, 00, 00, 00, 00},

/*14*/                  { 00, 02, 00, 00, 00, 16, 07, 07, 06, 40, 00, 00, 00, 00, 00, 00},
/*15*/                  { 00, 02, 00, 00, 00, 08, 02, 02, 01, 40, 03, 00, 00, 00, 00, 00},
/*16*/                  { 00, 02, 00, 00, 00, 06, 01, 01, 01, 40, 07, 02, 00, 01, 00, 00},
/*17*/                  { 00, 02, 00, 00, 00, 06, 01, 01, 01, 40, 07, 04, 00, 04, 00, 00},
/*18*/                  { 00, 02, 00, 00, 00, 06, 01, 01, 01, 40, 07, 06, 02, 10, 00, 00},
/*19*/                  { 00, 02, 00, 00, 00, 06, 01, 01, 01, 40, 07, 07, 05, 15, 00, 00},

/*20*/                  { 00, 00, 00, 00, 00, 02, 00, 00, 00, 16, 05, 07, 07, 40, 00, 00},
/*21*/                  { 00, 00, 00, 00, 00, 02, 00, 00, 00, 08, 02, 05, 08, 40, 02, 00},
/*22*/                  { 00, 00, 00, 00, 00, 02, 00, 00, 00, 06, 02, 05, 08, 40, 06, 01},

/*23*/                  { 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
/*EXTRA*/               { 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                         };

    static int sumValeur;
    static int[,] tabProb2 =  {
                     /* { 0 , 1 , 2 , 3 , 4 , 5 , 6 , 7 , 8 , 9 , 10, 11, 12, 13, 14, 15}*/
                        { 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01},
                        { 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                        { 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                             };

    //Correspond a la profondeur possible des coffres
    private const int PROFONDEUR_STONE = 20;
    private const int PROFONDEUR_CORUNDUM = 55;
    private const int PROFONDEUR_OBSIDIAN = 90;


    //Tableau Bonus

    static double[] pourcentage = { 1.25, 1.5, 1.75, 2, 2.5, 3 };
    static int[] probaPourcentage = { 28, 23, 19, 15, 8, 3 };


    public static void SetTableau(int index)
    {
        sumValeur = 0;
        for (int i = 0; i < tabProb.GetLength(1); i++)
        {
            sumValeur += tabProb[index, i];

        }
    }

    public static int getNumber(int index)
    {
        SetTableau(index);

        int numberRandom = UnityEngine.Random.Range(1, sumValeur + 1);
        int nbRendu = 1;
        int valeurRetour = tabProb.GetLength(1) + 1;

        for (int k = 0; k < tabProb.GetLength(1); k++)
        {
            if (numberRandom >= nbRendu && numberRandom < nbRendu + tabProb[index, k])
            {
                valeurRetour = k;
            }
            nbRendu += tabProb[index, k];
        }
        return valeurRetour;
    }
    public static List<Vector2Int>[] newPositionMineShaft()
    {
        List<Vector2Int>[] allPositions = new List<Vector2Int>[1];
        const int LONGUEUR_MAX = 50, LONGUEUR_MIN = 40;
        const int ODDS_UP = 5, ODDS_DOWN = 40; // Base sur les odds d'avant
        const int ODDS_SWITCH = 5;
        const int BUFFER_DIST = 7;

        ///int posJ_X = UnityEngine.Random.Range(0, Utils.largeur);
        int posJ_X = UnityEngine.Random.Range(0, 5);
        int posI_Y = UnityEngine.Random.Range(7, PROFONDEUR_STONE + 1);
        int longeur = UnityEngine.Random.Range(LONGUEUR_MIN, LONGUEUR_MAX + 1);
        int direction = UnityEngine.Random.Range(0, 2); ; //0 = droite, 1 = gauche

        if (posJ_X < BUFFER_DIST)
            direction = 0;
        else if (posJ_X > Utils.largeur - BUFFER_DIST)
            direction = 1;


        List<Vector2Int> positionsNiv = new List<Vector2Int>();
        positionsNiv.Add(new Vector2Int(posJ_X, posI_Y));
        Debug.Log("random");

        for (int i = 0; i < longeur; i++)
        {
            int odds = UnityEngine.Random.Range(0, 100);

            if (odds >= 0 && odds < ODDS_UP)
            {
                posI_Y = posI_Y - 1;
                positionsNiv.Add(new Vector2Int(posJ_X, posI_Y));
                positionsNiv.Add(new Vector2Int(posJ_X + 1, posI_Y));
            }
            else if (odds >= ODDS_UP && odds < ODDS_DOWN)
            {
                posI_Y = posI_Y + 1;
                positionsNiv.Add(new Vector2Int(posJ_X, posI_Y));
                positionsNiv.Add(new Vector2Int(posJ_X + 1, posI_Y));
            }
            else
            {
                positionsNiv.Add(new Vector2Int(posJ_X + 1, posI_Y));
            }

            posJ_X++;
        }

        allPositions[0] = positionsNiv;
        return allPositions;
    }

    public static List<Vector2Int>[] newPositionCoffre()
    {
        List<Vector2Int>[] allPositions = new List<Vector2Int>[3];
        List<Vector2Int> positionsNiv;
        int nbTreasure;

        // Stage stone:
        positionsNiv = new List<Vector2Int>();
        nbTreasure = UnityEngine.Random.Range(1, 2);

        positionsNiv.Add(new Vector2Int(7, 3));

        for (int i = 0; i < nbTreasure; i++)
        {
            int posY = UnityEngine.Random.Range(7, PROFONDEUR_STONE);
            int posX = UnityEngine.Random.Range(1, Utils.largeur - 1);
            Vector2Int newPos = new Vector2Int(posX, posY);


            if (!positionsNiv.Contains(newPos))
                positionsNiv.Add(newPos);
            else
                i--;
        }
        allPositions[0] = positionsNiv;

        // Stage corundum:
        positionsNiv = new List<Vector2Int>();
        nbTreasure = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < nbTreasure; i++)
        {
            int posY = UnityEngine.Random.Range(PROFONDEUR_STONE + 1, PROFONDEUR_CORUNDUM);
            int posX = UnityEngine.Random.Range(1, Utils.largeur - 1);

            Vector2Int newPos = new Vector2Int(posX, posY);

            if (!positionsNiv.Contains(newPos))
                positionsNiv.Add(newPos);
            else
                i--;
        }
        allPositions[1] = positionsNiv;

        // Stage obsidian:
        positionsNiv = new List<Vector2Int>();
        nbTreasure = UnityEngine.Random.Range(1, 5);

        for (int i = 0; i < nbTreasure; i++)
        {
            int posY = UnityEngine.Random.Range(PROFONDEUR_CORUNDUM + 1, PROFONDEUR_OBSIDIAN);
            int posX = UnityEngine.Random.Range(1, Utils.largeur - 1);

            Vector2Int newPos = new Vector2Int(posX, posY);

            if (!positionsNiv.Contains(newPos))
                positionsNiv.Add(newPos);
            else
                i--;
        }
        allPositions[2] = positionsNiv;



        return allPositions;
    }

    public static double[] getProbabiloteBonus()
    {
        int nbBonus = UnityEngine.Random.Range(1, 4);
        double[] proba = new double[nbBonus];


        int sumRandom = 0;
        for (int i = 0; i < probaPourcentage.Length; i++)
        {
            sumRandom += probaPourcentage[i];
        }

        for (int j = 0; j < proba.Length; j++)
        {
            int numberRandom = UnityEngine.Random.Range(0, sumRandom);
            int nbRendu = 0;

            for (int k = 0; k < probaPourcentage.Length; k++)
            {
                if (numberRandom >= nbRendu && numberRandom < nbRendu + probaPourcentage[k])
                {
                    proba[j] = pourcentage[k];
                }
                nbRendu += probaPourcentage[k];
            }
        }

        //Mettre en ordre:
        Array.Sort(proba);
        Array.Reverse(proba);

        return proba;
    }

    public static List<Vector2Int> newPositionMysterious()
    {
        List<EnumCell> notPossibleToChange = new List<EnumCell>() { EnumCell.Rien, EnumCell.BedRock, EnumCell.Cursed, EnumCell.Magma, EnumCell.Treasure };
        List<Vector2Int> positionMonde = new List<Vector2Int>();

        int nombre_mystery = ScoreManager.instance.MysteryUnlock.Length;

        for (int nb = 0; nb < nombre_mystery; nb++)
        {
            int i;
            int j;

            while (true)
            {
                i = UnityEngine.Random.Range(0, Utils.gridMonde.GetLength(1));
                j = UnityEngine.Random.Range(5, Utils.gridMonde.GetLength(0) - 15);
                EnumCell type = (EnumCell) Utils.getTypeGrid(j, i);

                if (!positionMonde.Contains(new Vector2Int(j, i)) && !notPossibleToChange.Contains(type))
                {
                    break;
                }
                else
                {
                    Debug.LogError("LES ODSSS AT (i: " + i + " j: " + j + ")" + " Type: " + type );
                }
            }
            positionMonde.Add(new Vector2Int(j, i));
        }

        return positionMonde;
    }
}
