using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuelShopManager : MonoBehaviour
{
    [SerializeField] private float prixParLitre;
    [SerializeField] private float[] multAvancemement;
    protected float prixTot;

    //Composnents popCanvas
    [SerializeField] private TextMeshProUGUI prix;

    private void Start()
    {
        GetComponentInChildren<PopOnEnter>().isOpenEvent += CalculeFuel;
    }
    public void CalculeFuel()
    {
        float max = ImprouvementManager.instance.getStats(EnumLevelName.Fuel);
        float current = ScoreManager.instance.fuel;
        int prixAfficher;

        prixTot = (max - current) * prixParLitre * avancementMultiplicateur();

        prixAfficher = (int)prixTot;
        prix.text = prixAfficher.ToString() + "$";
    }
    public void buyFuel()
    {
        int argent = ScoreManager.instance.gold;
        float argentRetire = 0;

        //Recalcule:
        CalculeFuel();

        if (ScoreManager.instance.haveEnoughtGold((int) prixTot))
        {
            ScoreManager.instance.refillFuel();
            argentRetire = prixTot;
        }
        else
        {
            ScoreManager.instance.addFuel((float)(argent / prixParLitre));
            argentRetire = argent;
        }
        

        ScoreManager.instance.addGold(-(int)argentRetire);

        CalculeFuel();

    }

    private float avancementMultiplicateur()
    {
        int[] blc = ScoreManager.instance.blocUnlock;
        int index;

        if (blc[(int)EnumCell.Obsidian] == 1)
        {
            index = 3;
        }
        else if (blc[(int)EnumCell.Corundum] == 1)
        {
            index = 2;
        }
        else if (blc[(int)EnumCell.Stone] == 1)
        {
            index = 1;
        }
        else
        {
            index = 0;
        }
        return multAvancemement[index];
    }
}
