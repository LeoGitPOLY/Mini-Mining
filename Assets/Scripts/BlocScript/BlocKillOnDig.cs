using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocKillOnDig : MonoBehaviour, IblocDigable
{
    private Cell localCell;
    private bool hasBeenStarted = false;

    private void startWhenNeeded(Cell cellDig)
    {
        if (!hasBeenStarted)
        {
            localCell = cellDig;
            hasBeenStarted = true;
        }
    }

    public bool isDiging(Cell cellDig, float frameRate)
    {
        bool isDestroy = false;

        startWhenNeeded(cellDig);

        if(localCell.currentHealth <= 0)
        {
            digDestroy();
            isDestroy = true;
        }
        else
        {
            localCell.currentHealth -= 0.5 * ImprouvementManager.instance.getStats(EnumLevelName.Dig) * frameRate;
        }
       
        return isDestroy;
    }

    private void digDestroy()
    {
        EventsGameState.instance.dead(typeMort.Cursed);
    }

    public void stopDiging()
    {
        if (localCell != null)
        {
            localCell.currentHealth = localCell.solidite;
        }
    }
}
