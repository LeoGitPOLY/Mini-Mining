using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ScoreManager;

[System.Serializable]
public class ScoreDataV2
{
    public int gold;
    public int nbCorde;
    public int indexMission;
    public float timeFinishGame;
    public int teleporterFind;
    public bool isPlayerPaid;
    public bool isDead;

    public bool digTwiceFast;
    public bool unlimitedFuel;

    public float essence;

    public List<EnumCell> cargo;
    public List<Bonus> bonus;
    public List<float[]> treuils;

    public int[] blockUnlock;
    public int[] MysteryUnlock;

    public ScoreDataV2(ScoreManager score)
    {
        gold = score.gold;
        nbCorde = score.nbCorde;
        indexMission = score.indexMission;

        timeFinishGame = score.timeFinishGame;
        teleporterFind = score.teleporterFind;

        isPlayerPaid = score.isPlayerPaid;
        isDead = score.isDeadNotSaveble;

        digTwiceFast = score.digTwiceFast;
        unlimitedFuel = score.unlimitedFuel;

        essence = score.fuel;
        cargo = score.cargo;
        bonus = score.bonus;

        blockUnlock = score.blocUnlock;
        MysteryUnlock = score.MysteryUnlock;

        //Treuil:
        treuils = new List<float[]>();
        foreach (Vector2 item in score.treuils)
        {
            float[] vec = new float[2];
            vec[0] = item.x;
            vec[1] = item.y;

            treuils.Add(vec);
        }
    }

}
[System.Serializable]
public class ScoreData
{
    public int gold;
    public int nbCorde;
    public int indexMission;
    public float timeFinishGame;
    public bool isDead;

    public float essence;

    public List<EnumCell> cargo;
    public List<Bonus> bonus;
    public List<float[]> treuils;

    public int[] blockUnlock;
    public int[] MysteryUnlock;



    public ScoreData(ScoreManager score)
    {
        gold = score.gold;
        nbCorde = score.nbCorde;
        indexMission = score.indexMission;
        timeFinishGame = score.timeFinishGame;
        isDead = score.isDeadNotSaveble;

        essence = score.fuel;
        cargo = score.cargo;
        bonus = score.bonus;

        blockUnlock = score.blocUnlock;
        MysteryUnlock = score.MysteryUnlock;

        //Treuil:
        treuils = new List<float[]>();
        foreach (Vector2 item in score.treuils)
        {
            float[] vec = new float[2];
            vec[0] = item.x;
            vec[1] = item.y;

            treuils.Add(vec);
        }
    }
}


