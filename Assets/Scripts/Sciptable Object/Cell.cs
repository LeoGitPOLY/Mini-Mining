using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "New Cell", menuName = "Cell")]
public class Cell : ScriptableObject
{
    public GameObject gameObject;
    SpriteRenderer spriteRenderer;
    public Sprite[] sprite;
   

    public EnumCell typeCell;
    public EnumNivBloc nivBloc;

    public bool isBuyable;
    public int solidite;
    public int valeur;

    public Color color;
    public Sprite lowRezTexture;

    [NonSerialized]
    public double currentHealth;

    //Animation:
    private Animator anim;
    private string currentState;

    public void SetNouvInfo(int X, int Y)
    {
        //Premier, avoir un nouveau gameObject
        gameObject = Instantiate(gameObject);
        gameObject.transform.position = new Vector3(X + 0.5f, Y - 0.5f);

        //Get Components
        //anim = gameObject.GetComponent<Animator>();
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //SetValeurs
        currentHealth = solidite;

        //Fonctions
        setState();
        //setTitle();
    }

    public void setState()
    {
        Vector2Int pos = Utils.PositionInt(gameObject.transform);

        int state = Utils.getStateFromXY(pos.x, pos.y);

        setSprite(state);
    }

    public Vector2 getPositionCoin()
    {
        return new Vector2((int)(gameObject.transform.position.x - 0.5f), (int)(gameObject.transform.position.y + 0.5f));
    }

    public void setSprite(int index)
    {
        if(spriteRenderer == null)
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (index == -1)
            return;

        if (spriteRenderer != null && index >= 0 && index < sprite.Length)
            spriteRenderer.sprite = sprite[index];
    }

    public void ChangeAnimationState(string newState)
    {
        if(anim == null)
            anim = gameObject.GetComponent<Animator>();

        if (anim != null)
        {
            //stop the same animation from interrupting itself
            if (currentState == newState) return;

            //play the animation
            anim.Play(newState);

            //reassign the current state
            currentState = newState;
        }
    }

    public void setAnimationEnable(bool bol)
    {
        if (anim == null)
            anim = gameObject.GetComponent<Animator>();

        if (anim != null)
            anim.enabled = bol;
    }


    //Private Methode:

    private void setTitle()
    {
        Vector2Int pos = Utils.PositionInt(gameObject.transform);

        string strState = Utils.getStateFromXY(pos.x, pos.y).ToString();
        string strType = Utils.getTypeFromXY(pos.x, pos.y).ToString();

        if (strState == "-1" || strType == "-1")
        {
            strType = "";
            strState = "_";
        }
        if (strState == "1")
        {
            strType = strType + "___________________________________";

        }

        gameObject.name = typeCell.ToString() + " (" + strState + strType + ")";
    }
}
