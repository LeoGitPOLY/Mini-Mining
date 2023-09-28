using UnityEngine;
using System;

public class ListenerMonoBehaviour : MonoBehaviour
{
    public event Action start;
    public event Action enable;

    // Start is called before the first frame update
    void Start()
    {
        start?.Invoke();
    }

    void OnEnable()
    {
        enable?.Invoke();
    }
}
