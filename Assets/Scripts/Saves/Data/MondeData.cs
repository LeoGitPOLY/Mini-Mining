using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MondeData
{
    public string[,] gridMonde;
    public string[,] gridExploration;

    public MondeData(string[,] tableauMonde, string[,] tableauExploration)
    {
        gridMonde = tableauMonde;
        gridExploration = tableauExploration;
    }

}
