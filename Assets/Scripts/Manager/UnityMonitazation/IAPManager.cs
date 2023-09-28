using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;



public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products

    private string coin099 = "coin099minimining";
    private string coin199 = "coin199minimining";
    private string coin399 = "coin399minimining";

    private string skinShadow = "skinshadowminimining";
    private string skinMini = "skinminiminimining";
    private string skinTank = "skintankminimining";
    private string skinKing = "skinkingminimining";

    private string doubleDig = "doubledigminimining";
    private string unlimitedFuel = "unlimitedfuelminimining";

    private List<string> allProductsCoins;
    private List<string> allProductsSkins;

    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(coin099, ProductType.Consumable);
        builder.AddProduct(coin199, ProductType.Consumable);
        builder.AddProduct(coin399, ProductType.Consumable);

        builder.AddProduct(skinShadow, ProductType.NonConsumable);
        builder.AddProduct(skinMini, ProductType.NonConsumable);
        builder.AddProduct(skinTank, ProductType.NonConsumable);
        builder.AddProduct(skinKing, ProductType.NonConsumable);

        builder.AddProduct(doubleDig, ProductType.Consumable);
        builder.AddProduct(unlimitedFuel, ProductType.Consumable);

        allProductsCoins = new List<string> { coin099, coin199, coin399 };
        allProductsSkins = new List<string> { skinShadow, skinMini, skinTank, skinKing };

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods
    public void buyCoin099()
    {
        BuyProductID(coin099);
    }
    public void buyCoin199()
    {
        BuyProductID(coin199);
    }
    public void buyCoin399()
    {
        BuyProductID(coin399);
    }
    public void buySkin(EnumSkinName skinName)
    {
        switch (skinName)
        {
            case EnumSkinName.SkinShadow:
                BuyProductID(skinShadow);
                break;
            case EnumSkinName.SkinMini:
                BuyProductID(skinMini);
                break;
            case EnumSkinName.SkinTank:
                BuyProductID(skinTank);
                break;
            case EnumSkinName.SkinKing:
                BuyProductID(skinKing);
                break;
            default:
                Debug.LogError("SKIN NON CONSUMABLE!!");
                break;
        }    
    }
    public void buyDoubleDig()
    {
        BuyProductID(doubleDig);
    }
    public void buyUnlimitedFuel()
    {
        BuyProductID(unlimitedFuel);
    }

    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if(allProductsCoins.Contains(args.purchasedProduct.definition.id, StringComparer.Ordinal))
        {
            //WIN CONSUMABLE PRICE: COINS
            instancePriceWin.instance.winCoinsMoney();         
        }
        else if (allProductsSkins.Contains(args.purchasedProduct.definition.id, StringComparer.Ordinal))
        {
            //WIN NON-CONSUMABLE PRICE: SKINS
            if (String.Equals(args.purchasedProduct.definition.id, skinShadow, StringComparison.Ordinal))
            {
                SettingManager.instance.skinsUnlock[(int)EnumSkinName.SkinShadow] = 1;
            }
            else if(String.Equals(args.purchasedProduct.definition.id, skinMini, StringComparison.Ordinal))
            {
                SettingManager.instance.skinsUnlock[(int)EnumSkinName.SkinMini] = 1;
            }
            else if(String.Equals(args.purchasedProduct.definition.id, skinTank, StringComparison.Ordinal))
            {
                SettingManager.instance.skinsUnlock[(int)EnumSkinName.SkinTank] = 1;
            }
            else if (String.Equals(args.purchasedProduct.definition.id, skinKing, StringComparison.Ordinal))
            {
                SettingManager.instance.skinsUnlock[(int)EnumSkinName.SkinKing] = 1;
            }
        }
        else if(String.Equals(args.purchasedProduct.definition.id, doubleDig, StringComparison.Ordinal))
        {
            ScoreManager.instance.digTwiceFast = true;
            ScoreManager.instance.isPlayerPaid = true;
            UIManager.instance.setBuyOtherConsumable();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, unlimitedFuel, StringComparison.Ordinal))
        {
            ScoreManager.instance.refillFuel();
            ScoreManager.instance.unlimitedFuel = true;
            ScoreManager.instance.isPlayerPaid = true;
            UIManager.instance.setBuyOtherConsumable();
        }
        else
        {
            UIManagerPop.instance.setSomethingWhentWrong();
        }
        return PurchaseProcessingResult.Complete;
    }

    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}