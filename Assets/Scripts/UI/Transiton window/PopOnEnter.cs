using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopOnEnter : MonoBehaviour
{
    [SerializeField] private GameObject prefabPop;
    [SerializeField] private GameObject pngAnim;

    public event Action isOpenEvent;
    public bool isOpen;
    
    //Animation:
    private Animator anim;
    private const string QUIT_ANIM = "QuitZone";
    private const string ENTER_ANIM = "EnterZone";

    //Child Position:
    private string CHILD_NAME = "PopPosition";
    private Vector2 childPopPosition;

    private void Start()
    {
        if (pngAnim != null)
            anim = pngAnim.GetComponent<Animator>();

        childPopPosition = gameObject.transform.Find(CHILD_NAME).transform.position;
        prefabPop.transform.position = Camera.main.WorldToScreenPoint(childPopPosition);

        isOpen = false;
        prefabPop.SetActive(false);
    }
    private void LateUpdate()
    {
        if (isOpen)
            prefabPop.transform.position = Camera.main.WorldToScreenPoint(childPopPosition);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            prefabPop.SetActive(true);
            setAnimEnter();
            isOpenEvent?.Invoke();
            isOpen = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            prefabPop.SetActive(false);
            setAnimQuit();
            isOpen = false;
        }
    }

    private void setAnimEnter()
    {
        if(anim != null)
        {
            anim.Play(ENTER_ANIM);
        }

    }
    private void setAnimQuit()
    {
        if (anim != null)
        {
            anim.Play(QUIT_ANIM);
        }
    }

}
