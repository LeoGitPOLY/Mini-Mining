using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSkinManager : MonoBehaviour, panel
{
    [SerializeField] private SkinTheme skinToBuy;
    [SerializeField] private SaveManager save;


    private EasyComponentsGetter getter;
    private int indexSkinShow;

    private bool isFirstTime = true;

    public void demarer()
    {
        PlayerManagerAnim.instance.changeAnimatorController((EnumSkinName) SettingManager.instance.indexSkinSelected);
    }

    // Start is called before the first frame update
    void Start()
    {
        getter = GetComponent<EasyComponentsGetter>();
        indexSkinShow = 0;

        ResetInfoSkinSelected();
        
        isFirstTime = false;
    }
    private void OnEnable()
    {
        if (!isFirstTime)
            ResetInfoSkinSelected();
    }

    private void ResetInfoSkinSelected()
    {
        SkinToBuy skin = skinToBuy.allSkin[indexSkinShow];
        getter.getTxt(0).SetText(skin.NameSkin);
        getter.getTxt(1).SetText(Transformation.transformMoney(skin.CoinsToBuy));
        getter.getTxt(2).SetText(Transformation.transformMoney(ScoreManager.instance.gold));
        getter.getTxt(3).SetText("$ " + skin.RealMoneyToBuy);
        getter.getTxt(4).SetText("or");

        getter.getImage(0).sprite = skinToBuy.allSkin[indexSkinShow].SpriteToShow;

        if (SettingManager.instance.skinsUnlock[indexSkinShow] == 1)
        {
            getter.setActiveGameObject(0, false);

            if (SettingManager.instance.indexSkinSelected == indexSkinShow)
            {
                getter.setActiveGameObject(2, true);
                getter.setActiveGameObject(1, false);
            }
            else
            {
                getter.setActiveGameObject(1, true);
                getter.setActiveGameObject(2, false);
            }
        }
        else
        {
            getter.setActiveGameObject(0, true);

            //REMOVE SELECTION:
            getter.setActiveGameObject(1, false);
            getter.setActiveGameObject(2, false);
        }

        getter.setActiveGameObject(5, skin.RealMoneyToBuy == 0 ? false : true);
        getter.setActiveGameObject(6, skin.CoinsToBuy == 0 ? false : true);

        if(skin.Description != "")
        {
            getter.setActiveGameObject(5, false);
            getter.setActiveGameObject(6, false);
            getter.getTxt(4).SetText(skin.Description);
        }
    }

    //METHODES CALL BY BUTTON:
    public void buyMoneyGame()
    {
        int moneyAmount = skinToBuy.allSkin[indexSkinShow].CoinsToBuy;

        if (ScoreManager.instance.haveEnoughtGold(moneyAmount))
        {
            EnumSkinName skin = skinToBuy.allSkin[indexSkinShow].SkinType;

            ScoreManager.instance.addGold(-moneyAmount);
            SettingManager.instance.skinsUnlock[(int) skin] = 1;
            ResetInfoSkinSelected();
            save.saveAll();
        }
    }
    public void buyRealMoney()
    {
        EnumSkinName skin = skinToBuy.allSkin[indexSkinShow].SkinType;

        IAPManager.instance.buySkin(skin);
        ResetInfoSkinSelected();
        save.saveAll();
    }
    public void selectSkin()
    {

        EnumSkinName skin = skinToBuy.allSkin[indexSkinShow].SkinType;

        PlayerManagerAnim.instance.changeAnimatorController(skin);
        SettingManager.instance.indexSkinSelected = indexSkinShow;

        ResetInfoSkinSelected();
    }
    public void LeftArrow()
    {
        if (indexSkinShow > 0)
        {
            indexSkinShow--;
            ResetInfoSkinSelected();

            //Make visible both arrows:
            getter.setActiveGameObject(3, true);
            getter.setActiveGameObject(4, true);
        }

        if (indexSkinShow == 0)
        {
            //Hide leftArrow:
            getter.setActiveGameObject(3, false);
        }
    }
    public void RightArrow()
    {
        if (indexSkinShow < skinToBuy.allSkin.Length - 1)
        {
            indexSkinShow++;
            ResetInfoSkinSelected();

            //Make visible both arrows:
            getter.setActiveGameObject(3, true);
            getter.setActiveGameObject(4, true);
        }

        if (indexSkinShow == skinToBuy.allSkin.Length - 1)
        {
            //Hide leftArrow:
            getter.setActiveGameObject(4, false);
        }
    }
}
