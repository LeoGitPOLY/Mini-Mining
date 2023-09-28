using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlocNormalScript : MonoBehaviour, IblocDigable
{
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
            int multi = ScoreManager.instance.digTwiceFast ? 2 : 1;
            scriptProgressBar.maximum = localCell.solidite;
            scriptProgressBar.current = scriptProgressBar.maximum - cellDig.currentHealth;
            localCell.currentHealth -= 0.05 * ImprouvementManager.instance.getStats(EnumLevelName.Dig) * frameRate * multi;
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
        Utils.setTypeFromXY(pos.x, pos.y, (int)EnumCell.Rien);
        scriptProgressBar.setGameObjectVisible(false);
    }
}
