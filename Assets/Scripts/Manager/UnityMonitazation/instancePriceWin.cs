using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class instancePriceWin : MonoBehaviour
{
    public static instancePriceWin instance;

    //Ads:
    private UnityMonetizationManager adsManager;
    private Action nextActionAds;

    //In-App purchasing:
    private Action nextActionPurchasing;
    private int goldAmount = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

    }
    private void Start()
    {
        adsManager = gameObject.GetComponent<UnityMonetizationManager>();
    }

    //Set Prices:
    public void showRewardedAds(Action action)
    {
        adsManager.ShowRewardedVideo();
        nextActionAds = action;
    }
    public void InAppPurchasingPrice(Action action)
    {
        nextActionPurchasing = action;
    }

    public void InAppPurchasingPriceGold(int gold)
    {
        goldAmount = gold;
    }


    //Win:
    public void winPriceAds()
    {
        nextActionAds?.Invoke();
    }

    public void winCoinsMoney()
    {
        ScoreManager.instance.addGold(goldAmount);
        ScoreManager.instance.isPlayerPaid = true;
    }
}
