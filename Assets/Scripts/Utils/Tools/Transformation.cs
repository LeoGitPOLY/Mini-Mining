using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Transformation
{
    private static string[] tabThousand = { "", " k", " M", " G", " T" };
    public static string transformMoney(int money)
    {
        int nbThousand = 0;
        double moneyRestant = money;

        while (moneyRestant > 1000)
        {
            nbThousand++;
            moneyRestant = moneyRestant / 1000;
        }

        if (nbThousand > 0)
        {
            moneyRestant = moneyRestant * 100;
            moneyRestant = (int)moneyRestant;
            moneyRestant = moneyRestant / 100;
        }

        return moneyRestant + tabThousand[nbThousand];
    }
  
    public static string transformTime(int timeSec, bool showHeures = false)
    {
        int nbMinutes = 0;
        int nbHeure = 0;
        double timeRestant = timeSec;
        string strReturn = "";

        while (timeRestant >= 60)
        {
            nbMinutes++;
            timeRestant = timeRestant - 60;
        }

        while (nbMinutes >= 60)
        {
            nbHeure++;
            nbMinutes = nbMinutes - 60;
        }

        if (timeRestant < 10)
            strReturn = nbMinutes + ":0" + timeRestant;
        else
            strReturn = nbMinutes + ":" + timeRestant;


        if (showHeures)
        {
            if (nbMinutes < 10)
                strReturn = nbHeure + ":0"  + strReturn;
            else
                strReturn = nbHeure + ":" + strReturn;
        }
        return strReturn;
    }
}


