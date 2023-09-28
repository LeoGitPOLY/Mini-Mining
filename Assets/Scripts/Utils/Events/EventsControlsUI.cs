using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventsControlsUI : MonoBehaviour
{
    public static EventsControlsUI instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // MOVEMENTS EVENTS ------------------------
    public event Action<float> onChangeMovHoriz;
    public void ChangeMovHoriz(float movHoriz)
    {
        onChangeMovHoriz?.Invoke(movHoriz);
    }

    public event Action<float> onChangeMovVerti;
    public void ChangeMovVerti(float movVerti)
    {
        onChangeMovVerti?.Invoke(movVerti);
    }


    // DIG EVENTS ------------------------------
    public event Action<int> onChangeDigUpDown;
    public void ChangeDigUpDown(int digUpDown)
    {
        onChangeDigUpDown?.Invoke(digUpDown);
    }

    public event Action<int> onChangeDigLeftRight;
    public void ChangeDigLeftRight(int digLeftRight)
    {
        onChangeDigLeftRight?.Invoke(digLeftRight);
    }

    
    // OTHER EVENTS -----------------------------
    public event Action onDoubleTap;
    public void DoubleTap()
    {
        onDoubleTap?.Invoke();
    }
}
