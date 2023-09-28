using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTeleportHihi : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatIsPlayer;
    [SerializeField] private Transform player;
    [SerializeField] private Transform positionTelport;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.position = positionTelport.position;
    }

}
