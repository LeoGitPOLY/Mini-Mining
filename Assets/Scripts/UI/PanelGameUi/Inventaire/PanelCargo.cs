using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelCargo : MonoBehaviour
{
    //Import de Unity
    [Header("Import Unity:")]
    [SerializeField] private TextMeshProUGUI CargoTot;
    [SerializeField] private GameObject papaContener;
    [SerializeField] private CellTheme cells;


    //Prefab:
    [Header("Import Unity:")]
    [SerializeField] private GameObject prefab;

    //Propriété de classe 
    private List<GameObject> allPrefab;
    private List<PrefabBaseManager> scriptPrefabs;

    private int[] Elements;


    public void setPanelCargo()
    {
        createElements();
        InstatiatePrefab();
        setCargoText();
    }
    private void createElements()
    {
        Elements = new int[cells.allCell.Length];

        foreach (Cell cell in cells.allCell)
        {
            EnumCell type = cell.typeCell;
            foreach (EnumCell item in ScoreManager.instance.cargo)
            {
                if (type == item)
                {
                    Elements[(int)type]++;
                }
            }
        }
    }
    private void InstatiatePrefab()
    {

        if (allPrefab != null)
            foreach (GameObject item in allPrefab)
                Destroy(item);

        allPrefab = new List<GameObject>();
        scriptPrefabs = new List<PrefabBaseManager>();

        for (int i = 0; i < Elements.Length; i++)
        {
            if (Elements[i] != 0)
            {
                Cell cell = cells.allCell[i];
                GameObject objectInter = Instantiate(prefab, papaContener.transform);
                PrefabBaseManager scriptInter = objectInter.GetComponent<PrefabBaseManager>();

                allPrefab.Add(objectInter);
                scriptPrefabs.Add(scriptInter);
                scriptInter.setAll(cell.sprite[0], cell.typeCell, MaterielName.getNameMat(cell.typeCell), Elements[i]);
            }
        }
    }

    private void setCargoText()
    {
        ImprouvementManager improuv = ImprouvementManager.instance;
        int max = improuv.CargoStats[improuv.CargoLevel];
        int current = ScoreManager.instance.cargo.Count;

        CargoTot.text = current.ToString() + "/" + max.ToString();
    }
}
