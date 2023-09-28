using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Depuis la version 2 de Mini Mining, le Playerdata class servira a verifier si le jeu a deja ete joue
 * Il est donc impossible de modifier cette classe, sinon les mises a jours seront completement brise
 * Signe: programmeur imcompetent qui decide au fur et a mesure!
 */
[System.Serializable]
public class PlayerData 
{

    public float[] position;
    public List<int> lastChain;

    public PlayerData(PlayerManager data)
    {
        Vector3 positionInter = data.playerPosition;
        position = new float[2];
        position[0] = positionInter.x;
        position[1] = positionInter.y;

        lastChain = data.lastChains;
    }
}
