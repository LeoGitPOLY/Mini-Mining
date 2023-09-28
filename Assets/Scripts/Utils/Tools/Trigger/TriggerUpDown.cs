using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUpDown : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        EventsOnChains.instance.ChainsEventOther(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EventsOnChains.instance.ChainsEventOther(false);
    }
}
