using UnityEngine;
using UnityEngine.SceneManagement;

public class Dig : MonoBehaviour
{
    //Manager: 
    [SerializeField] private DragMovDig dragMan;

    private float DownUp;
    private float LeftRight;

    //Cell on dig:
    private Vector2 positionCell;
    private Cell cellOnDig;
    private EnumCell typeCell;
    private IblocDigable blocComponents;

    private bool wasDigging;

    //MonoBehaviour Methodes:
    private void Start()
    {
        positionCell = Vector2.zero;
        typeCell = EnumCell.Rien;
        cellOnDig = null;
        blocComponents = null;

        wasDigging = false;
    }

    private void FixedUpdate()
    {
        if (DownUp != 0 || LeftRight != 0)
            FindBlocToDig();
        else if ((DownUp == 0 || LeftRight == 0))
            ChangeOrReleased();

    }

    void Update()
    {
        if (!dragMan.getBlockDig())
        {
            LeftRight = dragMan.dig_horizontale;
            DownUp = dragMan.dig_Vertical;
        }
        else
        {
            LeftRight = 0;
            DownUp = 0;
        }

        setSkinDig();
        //    if (Input.GetKeyDown("s"))
        //        DownUp = -1;

        //    if (Input.GetKeyDown("w"))
        //        DownUp = +1;

        //    if (Input.GetKeyDown("d"))
        //        LeftRight = +1;

        //    if (Input.GetKeyDown("a"))
        //        LeftRight = -1;

        //    if (Input.GetKeyUp("s") || Input.GetKeyUp("w") || Input.GetKeyUp("d") || Input.GetKeyUp("a"))
        //    {
        //        DownUp = 0;
        //        LeftRight = 0;

        //        if (tileOnDig != null)
        //            tileOnDig.curentHealth = tileOnDig.cell.solidite;

        //        ProgressBar.SetActive(false);
        //        script.current = 0;
        //    }

    }

    public void ChangeOrReleased()
    {
        DownUp = 0;
        LeftRight = 0;

        if (blocComponents != null)
            blocComponents.stopDiging();
    }

    private void FindBlocToDig()
    {
        Vector2Int positionBloc = Utils.PositionInt(transform);

        int nouv_X = (int)(positionBloc.x + LeftRight);
        int nouv_Y = (int)(positionBloc.y + DownUp);

        DigFunction(nouv_X, nouv_Y);
    }
    private void setSkinDig()
    {
        //UP and DOWN
        if (DownUp > 0)
            PlayerManagerAnim.instance.digStateAnim = digState.digDown;
        if (DownUp < 0)
            PlayerManagerAnim.instance.digStateAnim = digState.digUp;

        //LEFT and RIGHT
        if (LeftRight > 0)
        {
            PlayerManagerAnim.instance.digStateAnim = digState.digNormal;
            digState.isDIgLeft = false;
        }
        if (LeftRight < 0)
        {
            PlayerManagerAnim.instance.digStateAnim = digState.digNormal;
            digState.isDIgLeft = true;
        }

        //NOTHING DIG
        if (DownUp == 0 && LeftRight == 0)
            PlayerManagerAnim.instance.digStateAnim = digState.notDiging;
    }
    private void setSoundDig()
    {
        if (SceneName.SCENE_PRINCIPALE == SceneManager.GetActiveScene().name)
        {
            if (!wasDigging && (DownUp != 0 || LeftRight != 0))
            {
                FxSoundManager.instance.playClip("DigSound");
                wasDigging = true;
            }

            //NOTHING DIG
            if (DownUp == 0 && LeftRight == 0)
            {
                FxSoundManager.instance.stopSound("DigSound");
                wasDigging = false;
            }

        }

    }


    private void DigFunction(int x, int y)
    {
        bool isDestroy = false;

        if (cellOnDig == null || positionCell != new Vector2(x, y) || (int)typeCell != Utils.getTypeFromXY(x, y))
        {
            ChangeOrReleased();

            cellOnDig = gridManager.instance.getCellFromXY(x, y);
            blocComponents = cellOnDig.gameObject.GetComponent<IblocDigable>();
            positionCell = new Vector2(x, y);
            typeCell = cellOnDig.typeCell;
        }

        int digLevel = ImprouvementManager.instance.DigLevel;

        if ((digLevel == 7 && cellOnDig.nivBloc != EnumNivBloc.indestructible) || digLevel >= 2 * (int)cellOnDig.nivBloc)
        {
            try
            {
                float frameRate = Time.fixedDeltaTime;
                isDestroy = blocComponents.isDiging(cellOnDig, frameRate);
                checkWaitWinch();
            }
            catch
            {
                //CONTRER LE BUG
                changeBlocReset(x, y);
            }
        }
        else
        {
            if (cellOnDig.nivBloc != EnumNivBloc.indestructible)
            {
                string niv;

                niv = "" + (int)cellOnDig.nivBloc * 2;

                if (niv == "8")
                    niv = "Golden";
                UIManagerPop.instance.setLevelRequire(niv.ToString());
            }
        }

        //Permet de ne pas detruire le bloc avec d'en avoir terminé
        if (isDestroy)
        {
            changeBlocReset(x, y);
        }
    }

    private void changeBlocReset(int x, int y)
    {
        cellOnDig = null;
        blocComponents = null;
        gridManager.instance.setGridCell(x, y);
    }

    private void checkWaitWinch()
    {
        if (SettingManager.instance.firstThingsDone[2] == true || EventsOnChains.instance.isOnChainsMine)
            return;
        if (DownUp != -1)
            return;

        UIManagerPop.instance.instantiateWaitWinch();
        DownUp = 0;

    }
}