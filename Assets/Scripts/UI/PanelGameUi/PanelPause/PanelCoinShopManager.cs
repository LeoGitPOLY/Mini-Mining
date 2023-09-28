using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCoinShopManager : MonoBehaviour
{
    [SerializeField] private int[] amountMoneyBuy;
    [SerializeField] private int[] amountAdsBuy;

    private EasyComponentsGetter getter;
    private int currentAdMoney;
    private bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        getter = GetComponent<EasyComponentsGetter>();
        EventsGameScore.instance.onChangeGold += resetTextMoney;

        getter.getTxt(0).SetText(Transformation.transformMoney(ScoreManager.instance.gold));
        setPrixCoins();
        setBuyOtherConsumable();

        isStart = true;
    }
    private void OnEnable()
    {
        if (isStart)
        {
            setPrixCoins();
            setBuyOtherConsumable();
        }
    }
    private void setPrixCoins()
    {
        //Set AdsCoins
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

        currentAdMoney = amountAdsBuy[index];
        getter.getTxt(1).SetText("x " + currentAdMoney);


        //Set MoneyReal:
        getter.getTxt(2).SetText("x " + amountMoneyBuy[0]);
        getter.getTxt(3).SetText("x " + amountMoneyBuy[1]);
        getter.getTxt(4).SetText("x " + amountMoneyBuy[2]);

    }
    private void resetTextMoney()
    {
        getter.getTxt(0).SetText(Transformation.transformMoney(ScoreManager.instance.gold));
    }

    public void setBuyOtherConsumable()
    {
        getter.getGameObject(0).SetActive(ScoreManager.instance.digTwiceFast);
        getter.getGameObject(1).SetActive(ScoreManager.instance.unlimitedFuel);
    }
    public void clickWatchAdsCoinsShop()
    {
        instancePriceWin.instance.showRewardedAds(winGoldAds);
    }
    public void clickDoubleDig()
    {
        if (!ScoreManager.instance.digTwiceFast)
            IAPManager.instance.buyDoubleDig();
    }
    public void clickUnlimitedFuel()
    {
        if (!ScoreManager.instance.unlimitedFuel)
            IAPManager.instance.buyUnlimitedFuel();
    }
    public void click099Coin()
    {
        instancePriceWin.instance.InAppPurchasingPriceGold(amountMoneyBuy[0]);
        IAPManager.instance.buyCoin099();
    }
    public void click199Coin()
    {
        instancePriceWin.instance.InAppPurchasingPriceGold(amountMoneyBuy[1]);
        IAPManager.instance.buyCoin199();
    }
    public void click399Coin()
    {
        instancePriceWin.instance.InAppPurchasingPriceGold(amountMoneyBuy[2]);
        IAPManager.instance.buyCoin399();
    }

    private void winGoldAds()
    {
        ScoreManager.instance.addGold(currentAdMoney);
    }
}
