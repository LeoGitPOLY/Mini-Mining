using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    public CellTheme cells;
    [Space(10)]

    //Gold :
    public int gold;
    [Space(10)]

    //Essence:
    public float fuel;
    [Space(10)]

    //Cargo:
    public List<EnumCell> cargo;
    [Space(10)]

    //Corde:
    public List<Vector2> treuils;
    public int nbCorde; // nb de chaine place dans le monde

    //Blocs:
    public int[] blocUnlock;
    public int[] MysteryUnlock;

    //Mission:
    public int indexMission;

    //Bonus
    [Serializable]
    public class Bonus
    {
        public double taux;
        public int type;

        public Bonus()
        {
            taux = 0;
            type = 0;
        }
        public Bonus(int type_, double taux_)
        {
            taux = taux_;
            type = type_;
        }
    }
    public List<Bonus> bonus;

    //0:version doesn't support teleporter, 1: teleporteur not found, 2: teleporter found
    public int teleporterFind { get; set; }

    //Shop things, that reset when reset game:
    public bool unlimitedFuel;
    public bool digTwiceFast;

    //GameState:
    /*
     * Current time est le temps depuis le début de la partie en cours (Commence au début de partie et quand restart game)
     * - currentTimeFinishGame == 0: Alors la game n'est pas encore terminé
     * - currentTimeFinishGame != 0: Alors la game est terminé, et le résultat est le temps pour finir dans cette manche.
     * - Pour savoir si la partie est entamé, regarder s'il a un timer de disponible et que currentTime != 0
     */
    public float timeFinishGame;
    public bool isPlayerPaid;
    public bool isDeadNotSaveble;



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

    }

    private void Start()
    {
        EventsGameScore.instance.changeGold();
    }

    // GOLD METHODE:
    public void addGold(int newGold)
    {
        gold += newGold;
        EventsGameScore.instance.changeGold();
    }
    public bool haveEnoughtGold(int goldAmount)
    {
        bool enought = true;

        if (goldAmount > gold)
        {
            enought = false;
            UIManagerPop.instance.setnotEnoughtMoney();
        }

        return enought;
    }

    // CARGO METHODE:
    public void addBlocCargo(EnumCell cell)
    {
        ImprouvementManager improuv = ImprouvementManager.instance;
        if (cargo.Count < improuv.CargoStats[improuv.CargoLevel])
        {
            cargo.Add(cell);

            if (SceneManager.GetActiveScene().name == SceneName.SCENE_PRINCIPALE)
                setUnlockBloc((int)cell);

            EventsGameScore.instance.changeCargo();
            UIManagerPop.instance.instantiateNewBloc(cell);
        }

    }
    public void removeBlocCargo(int nombre, EnumCell type)
    {
        int compteur = 0;
        bool haveRemove = false;

        while (compteur < cargo.Count && nombre > 0)
        {
            if (cargo[compteur] == type)
            {
                cargo.RemoveAt(compteur);
                nombre = nombre - 1;
                haveRemove = true;
            }
            else
            {
                compteur++;
            }
        }

        if (haveRemove)
            EventsGameScore.instance.changeCargo();
    }
    public bool isThereBlocCargo(int nombre, EnumCell type)
    {
        int compteur = 0;
        bool isThere = false;

        foreach (EnumCell item in cargo)
        {
            if (item == type)
                compteur++;
        }

        if (compteur >= nombre)
            isThere = true;

        return isThere;
    }
    public void deleteCargo()
    {
        cargo = new List<EnumCell>();
        EventsGameScore.instance.changeCargo();
    }

    //CORDE METHODE:
    public bool isAddTreuil()
    {
        bool addTreuil = false;
        ImprouvementManager improuv = ImprouvementManager.instance;

        if (treuils.Count < improuv.TreuilStats[improuv.ChainsLevel])
            addTreuil = true;

        return addTreuil;
    }
    public bool isAddChain()
    {
        bool returnChain = false;
        ImprouvementManager improuv = ImprouvementManager.instance;

        if (nbCorde < improuv.ChainsStats[improuv.ChainsLevel])
            returnChain = true;

        return returnChain;
    }
    public void addTreuilList(Vector2 position)
    {
        ImprouvementManager improuv = ImprouvementManager.instance;

        if (treuils.Count < improuv.TreuilStats[improuv.ChainsLevel])
        {
            treuils.Add(position);
            EventsGameScore.instance.changeTreuil();
        }
    }
    public void removeTreuilList(Vector2 position)
    {
        bool isThere = false;
        int compteur = 0;

        while (compteur < treuils.Count && !isThere)
        {
            if (treuils[compteur] == position)
            {
                treuils.RemoveAt(compteur);
                RopeManager.removeAllChainsAlways(position, false);
                EventsGameScore.instance.changeTreuil();
                isThere = true;
            }
            compteur++;
        }
    }
    public void addChain()
    {
        ImprouvementManager improuv = ImprouvementManager.instance;

        if (nbCorde < improuv.ChainsStats[improuv.ChainsLevel])
            nbCorde++;
        // If last chains
        if (nbCorde >= improuv.ChainsStats[improuv.ChainsLevel])
            EventsOnChains.instance.EndChainsEvent(true);
    }
    public void removeChain()
    {
        if (nbCorde > 0)
        {
            nbCorde--;
            EventsOnChains.instance.EndChainsEvent(false);
        }

    }
    public void removeAllChains()
    {
        foreach (Vector2 item in treuils)
        {
            RopeManager.removeAllChainsAlways(item, false);
        }
    }

    //ESSENCE METHODE:
    public void removeFuel(float amount)
    {
        if (unlimitedFuel)
            return;
        
        int pourcentage = (int)(fuel / ImprouvementManager.instance.getStats(EnumLevelName.Fuel) * 100);

        if (pourcentage > 0)
        {
            fuel = fuel - amount;
        }
        else
        {
            fuel = 0;
            EventsGameState.instance.dead(typeMort.Fuel);
        }
        EventsGameScore.instance.changeFuel();
    }
    public void refillFuel()
    {
        ImprouvementManager improuv = ImprouvementManager.instance;

        fuel = improuv.FuelStats[improuv.FuelLevel];
        EventsGameScore.instance.changeFuel();
    }
    public void addFuel(float amount)
    {
        float nextTot = fuel + amount;
        float max = ImprouvementManager.instance.getStats(EnumLevelName.Fuel);

        if (nextTot < max)
            fuel = nextTot;
        else
            fuel = max;

        EventsGameScore.instance.changeFuel();
    }

    //MYSTERY METHODE:   
    public void setDiscovery(int state)
    {
        if (MysteryUnlock.Length > state)
        {
            MysteryUnlock[state] = 1;
            UIManagerPop.instance.instantiateNewMysteroid();
        }
    }

    //UNLOCK METHODE:
    public int nbUnlockBloc()
    {
        int nbBlocUnlock = 0;
        foreach (int item in blocUnlock)
        {
            if (item == 1)
                nbBlocUnlock++;
        }

        return nbBlocUnlock;
    }
    public int GetBlockUnLockByIndex(int index)
    {
        int type = 0;
        int indexRestant = index;


        for (int i = 0; i < blocUnlock.Length; i++)
        {
            int item = blocUnlock[i];

            if (indexRestant == 1)
                type = i;
            if (item == 1)
                indexRestant--;
        }

        return type;
    }
    public void setUnlockBloc(int index)
    {
        if (blocUnlock[index] == 0)
        {
            blocUnlock[index] = 1;
            UIManagerPop.instance.instantiateNewType(index);
            EventsGameScore.instance.findNewBlock((EnumCell)index);
        }
    }

    //BONUS METHODE:
    public void setNewBonus(List<Bonus> newBonus)
    {
        bonus = newBonus;
        EventsGameScore.instance.newBonus();
    }

    //SAVE METHODE:
    public void saveScore()
    {
        SaveSystem.saveScore(instance);
    }
    
    public void LoadScore()
    {
        string version_code = VersionManager.VERSION_CODE_MINI_MINING;
        string version_current = VersionManager.instance.currentVersion;

        //Load version (V1) on code version (V2):
        if (version_code == EnumVersion.V2 && version_current == EnumVersion.V1)
        {
            ScoreData dataV1 = SaveSystem.loadScoreV1();
            print(dataV1);
           
            if (dataV1 != null)
                load_V1_On_V2_(dataV1);
            else
                loadNew();
            

        }
        else if(version_code == version_current)
        {
            ScoreDataV2 data = SaveSystem.loadScore();
            
            if (data != null)
                loadCurrentVersion(data);
            else
                loadNew();
        }

    }
    public void loadNew()
    {
        print("loadnew");
        ImprouvementManager improuv = ImprouvementManager.instance;

        gold = 170;
        nbCorde = 0;
        indexMission = 0;

        timeFinishGame = 0;
        teleporterFind = 1;
        isPlayerPaid = false;
        isDeadNotSaveble = false;

        fuel = improuv.FuelStats[improuv.FuelLevel];
        blocUnlock = new int[cells.allCell.Length];
        MysteryUnlock = new int[6];

        digTwiceFast = false;
        unlimitedFuel = false;

        cargo = new List<EnumCell>();
        treuils = new List<Vector2>();
        bonus = new List<Bonus>();
    }
    public void loadDevelloperMode()
    {
        gold = 100000;
        nbCorde = 0;
        indexMission = 0;
        teleporterFind = 2;

        isPlayerPaid = true;
        isDeadNotSaveble = false;

        fuel = 1000;
        blocUnlock = new int[cells.allCell.Length];
        MysteryUnlock = new int[6];
    }
    public void loadRevive()
    {
        isDeadNotSaveble = false;
        isPlayerPaid = false;
        removeAllChains();
        refillFuel();
        
        nbCorde = 0;
    }

    /*
     * LOAD PRIVATE (care about mini mining version) :
     */
    private void loadCurrentVersion(ScoreDataV2 data)
    {
        print("loadcurent");
        gold = data.gold;
        nbCorde = data.nbCorde;

        timeFinishGame = data.timeFinishGame;
        teleporterFind = data.teleporterFind;
        isPlayerPaid = data.isPlayerPaid;
        isDeadNotSaveble = data.isDead;

        digTwiceFast = data.digTwiceFast;
        unlimitedFuel = data.unlimitedFuel;

        fuel = data.essence;
        cargo = data.cargo;
        bonus = data.bonus;

        blocUnlock = data.blockUnlock;
        MysteryUnlock = data.MysteryUnlock;
        indexMission = data.indexMission;

        //Treuil:
        treuils.Clear();
        foreach (float[] item in data.treuils)
        {
            Vector2 vec = new Vector2();
            vec.x = item[0];
            vec.y = item[1];

            treuils.Add(vec);
        }
    }
    /// <summary>
    /// Changement version (V1) on code version (V2):
    /// - Ajout de savoir si le player paid
    /// - Ajout du bonus en payant: dig twice as fast
    /// - Ajout du bonus en payant: unlimited fuel
    /// - Ajout de savoir si teleporter trouve
    /// </summary>
    private void load_V1_On_V2_(ScoreData data)
    {
        print("Load v1 on V2");
        //Load comme avant:
        //<
        gold = data.gold;
        nbCorde = data.nbCorde;
        timeFinishGame = data.timeFinishGame;
        isDeadNotSaveble = data.isDead;

        fuel = data.essence;
        cargo = data.cargo;
        bonus = data.bonus;

        blocUnlock = data.blockUnlock;
        MysteryUnlock = data.MysteryUnlock;
        indexMission = data.indexMission;

        //Treuil:
        treuils.Clear();
        foreach (float[] item in data.treuils)
        {
            Vector2 vec = new Vector2();
            vec.x = item[0];
            vec.y = item[1];

            treuils.Add(vec);
        }
        //>

        //Ajout de shop things, that reset when reset game:
        digTwiceFast = false;
        unlimitedFuel = false;

        //Ajout de isPlayerPaid:
        isPlayerPaid = false;

        //Ajout du teleporter Find: (teleporter pas supporte)
        teleporterFind = 0;
    }
}
