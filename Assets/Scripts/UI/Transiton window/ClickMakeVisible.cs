using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickMakeVisible : MonoBehaviour
{
    [SerializeField] private GameObject makeVisible;
    [SerializeField] private FuelShopManager shopfuel;
    [SerializeField] private UIManager uiManager;

    void OnMouseUp()
    {
        if (makeVisible != null)
            makeVisible.SetActive(true);

        if (shopfuel != null)
            shopfuel.buyFuel();
    }


}
