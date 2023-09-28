using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocTreasureScript : MonoBehaviour, IblocDigable
{
    private Cell localCell;

    private Animator anim;

    private string[] ANIM_NAME = { "TreasureOpenNormal", "TreasureOpenEpic", "TreasureOpenLegendary" };
    private string[] SPRITE_NAME = { "TreasureNormal", "TreasureEpic", "TreasureLegendary" };

    private bool hasBeenStarted = false;
    private bool animDone = false;
    private bool isAnimated = false;
    private void startWhenNeeded(Cell cellDig)
    {
        if (!hasBeenStarted)
        {
            anim = GetComponent<Animator>();
            anim.enabled = true;

            localCell = cellDig;
            hasBeenStarted = true;
        }
    }

    public bool isDiging(Cell cellDig, float frameRate)
    {
        Vector2Int pos = Utils.PositionInt(transform);
        bool isDestroy = false;

        startWhenNeeded(cellDig);

        if (!isAnimated)
        {
            int stateCell = Utils.getStateFromXY(pos.x, pos.y);

            anim.Play(ANIM_NAME[stateCell]);

            float timer = AnimationManager.getTimeStateByName(anim, ANIM_NAME[stateCell]);
            Invoke("AnimDone", timer);
            isAnimated = true;
        }
        if (animDone)
        {
            destroyAfterAnim();
            isDestroy = true;
        }


        return isDestroy;
    }
    public void stopDiging()
    {
        if (localCell != null)
        {
            Vector2Int pos = Utils.PositionInt(transform);
            int stateCell = Utils.getStateFromXY(pos.x, pos.y);

            CancelInvoke();

            anim.Play(SPRITE_NAME[stateCell]);

            isAnimated = false;
            animDone = false;
        }
    }

    private void destroyAfterAnim()
    {
        //Get position coffre:
        Vector2Int pos = Utils.PositionInt(transform);

        //AddGold:
        int stateCoffre = Utils.getStateFromXY(pos.x, pos.y);
        int coins = 0;

        switch (stateCoffre)
        {
            case (int)EnumTypeTreasure.Normal:
                coins = 750;
                break;
            case (int)EnumTypeTreasure.Epic:
                coins = 2000;
                break;
            case (int)EnumTypeTreasure.Legendary:
                coins = 10000;
                break;
            default:
                break;
        }
        ScoreManager.instance.addGold(coins);

        //Remove coffre:
        Utils.setTypeFromXY(pos.x, pos.y, (int)EnumCell.Rien);

        //SetSkinCoffreOuvert:
        int typeDown = Utils.getTypeFromXY(pos.x, pos.y - 1);
        if (typeDown == (int)EnumCell.Stone || typeDown == (int)EnumCell.Corundum || typeDown == (int)EnumCell.Obsidian)
        {
            Utils.setStateFromXY(pos.x, pos.y - 1, 2);
            gridManager.instance.setGridCell(pos.x, pos.y - 1);
        }

        //UI:
        UIManagerPop.instance.instantiateNewCoffre(stateCoffre, coins);
    }

    private void AnimDone()
    {
        animDone = true;
        CancelInvoke();
    }

}
