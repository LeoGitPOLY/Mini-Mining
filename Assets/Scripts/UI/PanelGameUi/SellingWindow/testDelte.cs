using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using static ScoreManager;

public class testDelte : MonoBehaviour, panel
{
    //POUR SELLING PANEL//

    //Prefab
    [Header("Perfab:")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject prefabEmptyCargo;

    //Import de Unity
    [Header("Import Unity:")]
    [SerializeField] private GameObject papaContener;
    [SerializeField] private GameObject checkMark;
    [SerializeField] private CellTheme cells;
    [SerializeField] private TextMeshProUGUI textPrixTot;

    //Color
    [Header("Color")]
    [SerializeField] private Color colorBonus;
    [SerializeField] private Color colorNormal;

    //Propriété de classe 
    private List<GameObject> allPrefab;
    private List<PrefabBaseManager> scriptPrefabs;

    private int[] nb;

    private double totalePrix;
    private bool isBonus;
    private float bonus = 1.75f;

    public void demarer()
    {
        isBonus = false;
        instatiateBuying();
        createSellBase();

        EventsGameScore.instance.OnNewBonus += changeColor;
    }
    private void OnEnable()
    {
        instatiateBuying();
        createSellBase();
        changeColor();
    }
    private void Update()
    {

        int tot = 0;
        foreach (PrefabBaseManager item in scriptPrefabs)
        {
            if (item.IsSelected())
            {
                tot += item.prixTotal;
            }
        }

        int totAfficher = isBonus ? (int)(tot * bonus) : tot;
        textPrixTot.SetText(Transformation.transformMoney(totAfficher) + " $");

        totalePrix = totAfficher;
    }

    private void instatiateBuying()
    {
        nb = new int[cells.allCell.Length];

        foreach (Cell cell in cells.allCell)
        {
            EnumCell type = cell.typeCell;
            foreach (EnumCell item in ScoreManager.instance.cargo)
            {
                if (type == item)
                {
                    nb[(int)type]++;
                }
            }
        }
    }
    private void createSellBase()
    {
        bool empty = true;

        if (allPrefab != null)
            foreach (GameObject item in allPrefab)
                Destroy(item);

        allPrefab = new List<GameObject>();
        scriptPrefabs = new List<PrefabBaseManager>();

        for (int i = 0; i < nb.Length; i++)
        {
            if (nb[i] != 0)
            {
                Cell cell = cells.allCell[i];
                GameObject objectInter = Instantiate(prefab, papaContener.transform);
                PrefabBaseManager scriptInter = objectInter.GetComponent<PrefabBaseManager>();

                allPrefab.Add(objectInter);
                scriptPrefabs.Add(scriptInter);
                scriptInter.setAll(cell.sprite[0], cell.typeCell, MaterielName.getNameMat(cell.typeCell), nb[i], cell.valeur);
                empty = false;
            }
        }

        if (empty)
            allPrefab.Add(Instantiate(prefabEmptyCargo, papaContener.transform));
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

        foreach (PrefabBaseManager script in scriptPrefabs)
        {
            Cell cell = cells.allCell[(int)script.type];
            if (type.Contains((int)cell.typeCell))
            {
                double multiplicateur = taux[type.IndexOf((int)cell.typeCell)];

                script.getPrixByMatTxt().color = colorBonus;
                script.setNouveauPrixByMat(Mathf.RoundToInt((float)(cell.valeur * multiplicateur)));

            }
            else
            {
                script.getPrixByMatTxt().color = colorNormal;
                script.setNouveauPrixByMat(cell.valeur);
                compteur++;
            }
        }
    }
    private void setIsBonus()
    {
        isBonus = true;
        checkMark.SetActive(true);
    }


    //Button methode:
    public void sell()
    {
        int nbElement = scriptPrefabs.Count;
        ScoreManager.instance.addGold((int)totalePrix);

        if (totalePrix != 0)
        {
            isBonus = false;
            checkMark.SetActive(false);
        }
        for (int i = 0; i < nbElement; i++)
        {
            PrefabBaseManager item = scriptPrefabs[i];
            if (item.IsSelected())
            {
                GameObject itemGameObject = allPrefab[i];
                ScoreManager.instance.removeBlocCargo(item.units, item.type);

                allPrefab.Remove(itemGameObject);
                scriptPrefabs.Remove(item);

                Destroy(itemGameObject);

                i--;
                nbElement--;
            }
        }

        if (allPrefab.Count == 0)
            allPrefab.Add(Instantiate(prefabEmptyCargo, papaContener.transform));
    }
    public void selectAll()
    {
        foreach (PrefabBaseManager item in scriptPrefabs)
        {
            item.setSelected(true);
        }
    }
    public void watchAdBonus()
    {
        if (!isBonus)
        {
            instancePriceWin.instance.showRewardedAds(setIsBonus);
        }
    }
}
