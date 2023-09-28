using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocMysteryScript : MonoBehaviour, IblocDigable
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private TileTheme spriteMenu;

    private int state;
    private Cell localCell;

    private ContourProgressBar scriptProgressBar;
    private GameObject blocProgressBar;

    private bool hasBeenStarted = false;
    private void startWhenNeeded(Cell cellDig)
    {
        if (!hasBeenStarted)
        {
            localCell = cellDig;

            blocProgressBar = GameObject.Find("ProgressionBarBloc");
            scriptProgressBar = blocProgressBar.GetComponent<ContourProgressBar>();

            hasBeenStarted = true;
        }
    }

    public bool isDiging(Cell cellDig, float frameRate)
    {
        bool isDestroy = false;
        Vector2Int pos = Utils.PositionInt(transform);

        startWhenNeeded(cellDig);


        blocProgressBar.transform.position = new Vector3(pos.x, pos.y);
        scriptProgressBar.setGameObjectVisible(true);

        if (localCell.currentHealth <= 0)
        {
            if (localCell.isBuyable)
                ScoreManager.instance.addBlocCargo(localCell.typeCell);
            digDestroy();
            isDestroy = true;
        }
        else
        {
            scriptProgressBar.maximum = localCell.solidite;
            scriptProgressBar.current = scriptProgressBar.maximum - cellDig.currentHealth;
            localCell.currentHealth -= 0.05 * ImprouvementManager.instance.getStats(EnumLevelName.Dig) * frameRate;
        }

        return isDestroy;
    }

    public void stopDiging()
    {
        if (localCell != null)
        {
            localCell.currentHealth = localCell.solidite;
            scriptProgressBar.setGameObjectVisible(false);
        }
    }

    public void digDestroy()
    {
        Vector2Int pos = Utils.PositionInt(transform);
        state = Utils.getStateFromXY(pos.x, pos.y);

        destroyBloc(pos);
        scriptProgressBar.setGameObjectVisible(false);
    }


    private void destroyBloc(Vector2Int pos)
    {

        int state = Utils.getStateFromXY(pos.x, pos.y);

        Utils.setTypeFromXY(pos.x, pos.y, (int)EnumCell.Rien);
        gridManager.instance.setGridCell(pos.x, pos.y);

        ScoreManager.instance.setDiscovery(state);
    }
}
