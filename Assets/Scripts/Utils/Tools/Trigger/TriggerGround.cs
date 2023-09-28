using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGround : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private CaracterControler2D controller;

    private bool isGrounded = false;

    private void Start()
    {
        controller.setIsGround(isGrounded);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isGrounded)
            return;

        if ((m_WhatIsGround & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer) 
        { 
            controller.setIsGround(true);
            isGrounded = true;        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isGrounded)
            return;

        if ((m_WhatIsGround & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            controller.setIsGround(false);
            isGrounded = false;
        }
    }
}
