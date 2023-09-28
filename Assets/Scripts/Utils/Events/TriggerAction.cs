using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerAction : MonoBehaviour
{
    [SerializeField] private UnityEvent actionToTrigger;

    private bool uniqueTime = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (uniqueTime)
        {
            actionToTrigger?.Invoke();
            uniqueTime = false;
        }
    }
}
