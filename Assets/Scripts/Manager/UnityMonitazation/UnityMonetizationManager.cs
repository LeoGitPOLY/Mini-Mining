using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

//public class UnityMonetizationManager : MonoBehaviour, IUnityAdsListener
public class UnityMonetizationManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
#if UNITY_IOS
    private string gameId = "3955192";
#elif UNITY_ANDROID
    private string gameId = "3955193";
#endif

    private string myPlacementId = "rewardedVideo";
    private bool hasBeenInit = false;
    bool testMode = false;

    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        if (Advertisement.isSupported)
        {
            Debug.Log(Application.platform + " supported by Advertisement");
        }
        Advertisement.Initialize(gameId, testMode, this);
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowRewardedVideo()
    {
        if (!hasBeenInit)
            Initialize();

        Advertisement.Show(myPlacementId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(gameId))
        {
            print("adsReady");
        }
    }


    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(gameId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("AdsDone");
            instancePriceWin.instance.winPriceAds();
        }
        else if (adUnitId.Equals(gameId) && showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
        {
            // Do not reward the user for skipping the ad.
            UIManagerPop.instance.setSomethingWhentWrong();
        }
        else if (adUnitId.Equals(gameId) && showCompletionState.Equals(UnityAdsShowCompletionState.UNKNOWN))
        {
            UIManagerPop.instance.setSomethingWhentWrong();
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        UIManagerPop.instance.setSomethingWhentWrong();
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        UIManagerPop.instance.setSomethingWhentWrong();
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnInitializationComplete() { hasBeenInit = true; }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { hasBeenInit = false; }
}
