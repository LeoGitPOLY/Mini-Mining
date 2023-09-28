using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ScoreManager;

public class testDelta : MonoBehaviour, panel
{
    //POUR PRICE PANEL//

    //Prefab
    [Header("Perfab:")]
    [SerializeField] private GameObject prefabPrix;
    [SerializeField] private GameObject prefabBonus;

    //Import de Unity
    [Header("Import Unity:")]
    [SerializeField] private GameObject papaContenerPrix;
    [SerializeField] private GameObject papaContenerBonus;
    [SerializeField] private GameObject panelsChangeBonus;
    [SerializeField] private CellTheme cells;

    [Header("Color")]
    [SerializeField] private Color colorBonus;
    [SerializeField] private Color colorNormal;

    //Propriété de classe 
    private List<GameObject> allPrefabPrix;
    private List<GameObject> allPrefabBonus;
    private List<PrefabBaseManager> scriptPrefabsPrix;
    private List<Image> images;

    //Time
    [Header("Time:")]
    [SerializeField] private TextMeshProUGUI timetxt;
    [SerializeField] private float time;
    private const string NAME_TIME = "BonusTime";

    //DEBUGGER!!:
    private float timeSinceBug = 0f;

    private bool isWaiting = false;

    public void demarer()
    {
        instatiateBuying();

        instatiateBonusStart();
        setUnlockBloc(EnumCell.Rien);
        changeColor();

        EventsGameScore.instance.OnNewBonus += changeColor;
        EventsGameScore.instance.onFindNewBlock += setUnlockBloc;

        //TROUVER LA TERRE:
        EventsGameScore.instance.onFindNewBlock += instantiateBonusDirt;
    }
    private void Start()
    {
        firstTimePop();
    }

    private void Update()
    {
        if (isWaiting)
        {
            int time = (int)TimeManager.instance.getTimeRemaningByName(NAME_TIME) + 1;
            timetxt.text = Transformation.transformTime(time, false);

            //DEBUGGER!!:
            if(time == 1)
            {
                timeSinceBug += Time.deltaTime;

                if(timeSinceBug > 2)
                {
                    timeSinceBug = 0f;
                    newBonus();
                }
            }
        }
    }

    private void instantiateBonusDirt(EnumCell type)
    {
        if (type == EnumCell.Dirt)
            instantiateBonus();
    }
    private void instatiateBonusStart()
    {
        if (ScoreManager.instance.blocUnlock[(int)EnumCell.Dirt] == 1)
            instantiateBonus();
    }
    private void instatiateBuying()
    {
        //POUR TABLEAU DES PRIX:

        allPrefabPrix = new List<GameObject>();
        scriptPrefabsPrix = new List<PrefabBaseManager>();
        images = new List<Image>();

        foreach (Cell cell in cells.allCell)
        {
            if (cell.isBuyable)
            {
                GameObject inter = Instantiate(prefabPrix, papaContenerPrix.transform);
                PrefabBaseManager script = inter.GetComponent<PrefabBaseManager>();
                Image image = inter.GetComponent<Image>();

                allPrefabPrix.Add(inter);
                scriptPrefabsPrix.Add(script);
                images.Add(image);
            }
        }
    }
    private void setUnlockBloc(EnumCell type)
    {
        int compteur = 0;

        foreach (Cell cell in cells.allCell)
        {
            if (cell.isBuyable)
            {
                if (ScoreManager.instance.blocUnlock[(int)cell.typeCell] != 1)
                {
                    scriptPrefabsPrix[compteur].getImgSkin().color = new Color32(0, 0, 0, 255);
                    scriptPrefabsPrix[compteur].getTypeMatTxt().SetText("???");
                }
                else
                {
                    scriptPrefabsPrix[compteur].getImgSkin().color = new Color32(255, 255, 255, 255);
                    scriptPrefabsPrix[compteur].getImgSkin().sprite = cell.sprite[0];
                    scriptPrefabsPrix[compteur].getPrixByMatTxt().text = cell.valeur.ToString() + "$";
                    scriptPrefabsPrix[compteur].getTypeMatTxt().text = MaterielName.getNameMat(cell.typeCell);

                }
                compteur++;
            }
        }
    }
    private void changeColor()
    {
        List<Bonus> bonus = ScoreManager.instance.bonus;

        List<int> type = new List<int>();
        List<double> taux = new List<double>();
        int compteur = 0;

        foreach (Bonus item in bonus)
        {
            type.Add(item.type);
            taux.Add(item.taux);
        }

        foreach (Cell cell in cells.allCell)
        {
            if (cell.isBuyable)
            {
                if (type.Contains((int)cell.typeCell))
                {
                    double multiplicateur = taux[type.IndexOf((int)cell.typeCell)];

                    scriptPrefabsPrix[compteur].getPrixByMatTxt().color = colorBonus;
                    scriptPrefabsPrix[compteur].getPrixByMatTxt().text = Mathf.RoundToInt((float)(cell.valeur * multiplicateur)) + "$";
                }
                else
                {
                    scriptPrefabsPrix[compteur].getPrixByMatTxt().color = colorNormal;
                    scriptPrefabsPrix[compteur].getPrixByMatTxt().text = cell.valeur + "$";
                }
                compteur++;
            }
        }
    }

    private void instantiateBonus()
    {
        panelsChangeBonus.SetActive(true);
        isWaiting = TimeManager.instance.commanderMethode(NAME_TIME, newBonus);

        if (!isWaiting)
            newBonus();
        else
            oldBonus();
    }
    private void newBonus()
    {
        double[] stats = RandomFunction.getProbabiloteBonus();
        int nbBlocUnlock = ScoreManager.instance.nbUnlockBloc();

        List<int> typeUsed = new List<int>();
        List<Bonus> interBonus = new List<Bonus>();

        if (allPrefabBonus != null)
            foreach (GameObject item in allPrefabBonus)
            {
                Destroy(item);
            }

        allPrefabBonus = new List<GameObject>();

        for (int i = 0; i < stats.Length; i++)
        {
            if (i < nbBlocUnlock)
            {
                int index = 0;
                bool notThere = true;

                while (notThere)
                {
                    index = Random.Range(1, nbBlocUnlock + 1);
                    if (!typeUsed.Contains(index))
                    {
                        notThere = false;
                        typeUsed.Add(index);
                    }
                }

                int typeCell = ScoreManager.instance.GetBlockUnLockByIndex(index);

                GameObject inter = Instantiate(prefabBonus, papaContenerBonus.transform);
                PrefabBaseManager script = inter.GetComponent<PrefabBaseManager>();
                EasyComponentsGetter getter = inter.GetComponent<EasyComponentsGetter>();
                Cell cell = cells.allCell[typeCell];

                script.getImgSkin().sprite = cell.sprite[0];
                script.getImgSeconde().sprite = getter.getSprite(Random.Range(0, 3));
                script.getTypeMatTxt().text = MaterielName.getNameMat(cell.typeCell);
                script.getOtherText1().text = "x " + stats[i];

                allPrefabBonus.Add(inter);
                interBonus.Add(new Bonus(typeCell, stats[i]));
            }

        }
        ScoreManager.instance.setNewBonus(interBonus);

        isWaiting = true;
        TimeManager.instance.AddTimeRemoveIfThere(NAME_TIME, time, newBonus);
    }
    private void oldBonus()
    {
        List<Bonus> oldBonus = ScoreManager.instance.bonus;
        allPrefabBonus = new List<GameObject>();

        for (int i = 0; i < oldBonus.Count; i++)
        {
            GameObject inter = Instantiate(prefabBonus, papaContenerBonus.transform);
            PrefabBaseManager script = inter.GetComponent<PrefabBaseManager>();
            EasyComponentsGetter getter = inter.GetComponent<EasyComponentsGetter>();
            Cell cell = cells.allCell[oldBonus[i].type];

            script.getImgSkin().sprite = cell.sprite[0];
            script.getImgSeconde().sprite = getter.getSprite(Random.Range(0, 3));
            script.getTypeMatTxt().text = MaterielName.getNameMat(cell.typeCell); ;
            script.getOtherText1().text = "x " + oldBonus[i].taux;

            allPrefabBonus.Add(inter);
        }

    }
    private void firstTimePop()
    {
        if (!SettingManager.instance.firstThingsDone[3])
        {
            SettingManager.instance.firstThingsDone[3] = true;
            UIManagerPop.instance.instantiateSelling();
        }
    }
    //Methode Boutton:
    public void newBonusPressed()
    {
        instancePriceWin.instance.showRewardedAds(newBonus);
    }

}
