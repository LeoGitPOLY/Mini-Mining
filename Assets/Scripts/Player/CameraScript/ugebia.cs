using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//SCRIPT PANEL DEATH! (TO MOVE AND RENAME)
public class ugebia : MonoBehaviour
{
    [SerializeField] private int[] prixByNiveau;
    [SerializeField] private Sprite[] imagebyNiveau;
    [SerializeField] private Image imgButtonRevive;
    [SerializeField] private Image imgButtonCargo;

    [Header("Windows:")]
    [SerializeField] private GameObject[] gameObjectsCargo;
    [SerializeField] private GameObject[] gameObjectsRevive;

    private int prixActuel;

    private void OnEnable()
    {
        resetSkinPrixBuy();
    }
    //private methode:
    private void resetSkinPrixBuy()
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

        imgButtonCargo.sprite = imagebyNiveau[index];
        //imgButtonRevive.sprite = imagebyNiveau[index];
        prixActuel = prixByNiveau[index];
    }

    //Button Script:
    public void watchAdsRevive()
    {
        instancePriceWin.instance.showRewardedAds(reviveReward);
    }
    public void buyRevive()
    {
        ScoreManager score = ScoreManager.instance;

        if (score.haveEnoughtGold(prixActuel))
        {
            score.addGold(-prixActuel);
            reviveReward();
        }
    }
    public void restartGame()
    {
        EventsGameState.instance.restart();
    }

    //RewardedMethode:
    public void reviveReward()
    {
        EventsGameState.instance.revive();
    }

    //PUBLIC METHODE TO OPEN AND CLOSE:
    public void setVisible(bool isVisible)
    {
        this.gameObject.SetActive(isVisible);
    }

    //CARGO PAYING:
    public void setVisibleCargoSave()
    {
        foreach (var item in gameObjectsCargo)
        {
            item.SetActive(true);
        }

        foreach (var item in gameObjectsRevive)
        {
            item.SetActive(false);
        }
    }
    public void watchAdsCargo()
    {
        instancePriceWin.instance.showRewardedAds(cargoReward);
    }
    public void cargoReward()
    {
        foreach (GameObject item in gameObjectsCargo)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in gameObjectsRevive)
        {
            item.SetActive(true);
        }

        EventsGameState.instance.revive();
    }
    public void buyCargo()
    {
        ScoreManager score = ScoreManager.instance;

        if (score.haveEnoughtGold(prixActuel))
        {
            score.addGold(-prixActuel);
            cargoReward();
        }
    }
    public void loseCargo()
    {
        ScoreManager.instance.deleteCargo();
        cargoReward();
    }
}
