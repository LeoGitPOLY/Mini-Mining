using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ropeCollider : MonoBehaviour
{
    RopeManager rm;

    // Start is called before the first frame update
    void Start()
    {
        rm = GetComponentInParent<RopeManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "chains")
        {
            rm.TriggerEnter2D(other);
            EventsOnChains.instance.ChainsEventMine(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "chains")
        {
            EventsOnChains.instance.ChainsEventMine(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "chains")
        {
            rm.TriggerExit2D(other);
            EventsOnChains.instance.ChainsEventMine(false);
        }
    }

}
