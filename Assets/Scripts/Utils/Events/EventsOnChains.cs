using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsOnChains : MonoBehaviour
{
    public static EventsOnChains instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public event Action<bool> onEndChainsEvent;
    public void EndChainsEvent(bool isEnd)
    {
        onEndChainsEvent?.Invoke(isEnd);
    }

    public event Action<bool> onChainsEventMine;
    public void ChainsEventMine(bool isOn)
    {
        isOnChainsMine = isOn;
        onChainsEventMine?.Invoke(isOn);
    }

    public event Action<Vector2Int> onChainsEventMineReal;
    public void ChainsEventMineReal(Vector2Int nextTreuil)
    {
        onChainsEventMineReal?.Invoke(nextTreuil);
    }

    public event Action<bool> onChainsEventOther;
    public void ChainsEventOther(bool isOn)
    {
        isOnChainsOther = isOn;
        onChainsEventOther?.Invoke(isOn);
    }


    //Propriety holder
    public Vector2Int TreuilActif;
    public bool isOnChainsMine;
    public bool isOnChainsOther;

}
