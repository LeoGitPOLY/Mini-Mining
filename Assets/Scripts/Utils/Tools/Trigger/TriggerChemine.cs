using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChemine : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatIsPlayer;
    [SerializeField] private Rigidbody2D body;

    [SerializeField] private Vector3 velocity;
    [SerializeField] private ParticleSystem particules;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (body.velocity.x < 0)
            burstChemine();
    }

    private void burstChemine()
    {
        body.velocity = velocity;
        particules.Play();
    }
}
