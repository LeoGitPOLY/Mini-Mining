using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocTreuilScript : MonoBehaviour, IblocDigable
{
    private Cell localCell;
    private CaracterControler2D controler2D;
    private bool hasBeenStarted = false;

    private void startWhenNeeded(Cell cellDig)
    {
        if (!hasBeenStarted)
        {
            localCell = cellDig;
            controler2D = GameObject.Find("Player").GetComponent<CaracterControler2D>();
            hasBeenStarted = true;
        }
    }
    public bool isDiging(Cell cellDig, float frameRate)
    {
        bool isDestroy = false;

        startWhenNeeded(cellDig);

        if (cellDig.getPositionCoin() != EventsOnChains.instance.TreuilActif || !EventsOnChains.instance.isOnChainsMine)
        {
            if (localCell.currentHealth <= 0)
            {
                if (localCell.isBuyable)
                    ScoreManager.instance.addBlocCargo(localCell.typeCell);
                digDestroy();
                isDestroy = true;
            }
            else
            {
                localCell.currentHealth -= 0.05 * ImprouvementManager.instance.DigStats[7] * frameRate;
            }
        }
        return isDestroy;
    }

    public void stopDiging()
    {
        if (localCell != null)
        {
            localCell.currentHealth = localCell.solidite;
        }
    }
    public void digDestroy()
    {
        Vector2Int pos = Utils.PositionInt(transform);

        //Score Modification
        ScoreManager.instance.removeTreuilList(pos);

        //Modifie la grille
        if (Utils.getTypeFromXY(pos.x, pos.y - 1) == (int)EnumCell.Rien)
            Utils.setTypeFromXY(pos.x, pos.y, (int)EnumCell.Platefrom);
        else
            Utils.setTypeFromXY(pos.x, pos.y, (int)EnumCell.Rien);

        //Remove Chain last:
        PlayerManager.instance.lastChains[Mathf.Abs(pos.y)] -= 1;
    }

}
