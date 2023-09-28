using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RopeManager : MonoBehaviour
{
    [SerializeField] private Transform topCheck;
    [SerializeField] private DragMovDig dragMan;

    private PlayerMouvement playerMov;
    private CaracterControler2D controller;

    private bool moving_UpDown;
    private bool isScenePrincipal;
    public bool localDeathByChains;



    private void Awake()
    {
        playerMov = GetComponent<PlayerMouvement>();
        controller = GetComponent<CaracterControler2D>();

        EventsOnChains.instance.isOnChainsMine = false;
        moving_UpDown = false;
    }

    private void Start()
    {
        playerMov.moving_UpDown += setMovingUpDown;
        EventsControlsUI.instance.onDoubleTap += mettreUnTreuil;

        isScenePrincipal = SceneName.SCENE_PRINCIPALE == SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (EventsOnChains.instance.isOnChainsMine)
        {
            mettreUneChaine();
            animationTreuil();
        }
        isToDeep();
    }


    //Trigger methode:
    public void TriggerEnter2D(Collider2D collider)
    {
        //Verification triggerEnter
        RealTriggerEnter2D();

        removeAllChains(collider.transform.position, false);
        FindActifTreuil(collider);
    }
    public void TriggerExit2D(Collider2D other)
    {
        gererTreuilSprite();
    }

    //Real Trigger methode (on and off the long chain):
    public void RealTriggerEnter2D()
    {
        Vector2Int treuil_Actif = FindActifTreuil(transform);
        bool same_treuil_actif = treuil_Actif == EventsOnChains.instance.TreuilActif;
        bool was_out = !EventsOnChains.instance.isOnChainsMine;
        bool isJumping = isJumpingWinch(transform);

        if ((!same_treuil_actif || was_out) && !isJumping)
        {
            //REAL ENTER:
            EventsOnChains.instance.ChainsEventMineReal(treuil_Actif);
        }
    }

    //Treuil:
    public void mettreUnTreuil()
    {
        //Good Place?
        if (controller.m_Grounded)
        {
            //Improuvements manager good?
            if (ScoreManager.instance.isAddTreuil())
            {
                Vector2Int posPlayer = Utils.PositionInt(transform);
                //If there's nothing here?
                int type = Utils.getTypeFromXY(posPlayer.x, posPlayer.y);
                if (type == (int)EnumCell.Rien || type == (int)EnumCell.Platefrom)
                {
                    EventsOnChains.instance.TreuilActif = Utils.PositionInt(transform);

                    Utils.setTypeFromXY(EventsOnChains.instance.TreuilActif.x, EventsOnChains.instance.TreuilActif.y, (int)EnumCell.Treuil);
                    gridManager.instance.setGridCell(EventsOnChains.instance.TreuilActif.x, EventsOnChains.instance.TreuilActif.y);
                    ScoreManager.instance.addTreuilList(EventsOnChains.instance.TreuilActif);

                    if (isScenePrincipal)
                        PlayerManager.instance.lastChains[Mathf.Abs(Utils.PositionInt(transform).y)] += 1;
                }
            }
        }
    }
    private void animationTreuil()
    {
        Cell cellSkin = gridManager.instance.getCellFromXY(EventsOnChains.instance.TreuilActif.x, EventsOnChains.instance.TreuilActif.y);

        if (cellSkin != null)
        {
            if (moving_UpDown)
                cellSkin.setAnimationEnable(true);
            else
                cellSkin.setAnimationEnable(false);
        }
    }
    private void gererTreuilSprite()
    {
        //Stop Animation:
        Cell cellSkin = gridManager.instance.getCellFromXY(EventsOnChains.instance.TreuilActif.x, EventsOnChains.instance.TreuilActif.y);
        if (cellSkin != null)
            cellSkin.setAnimationEnable(false);

        //Set Good skin:
        int cellDown = Utils.getTypeFromXY(EventsOnChains.instance.TreuilActif.x, EventsOnChains.instance.TreuilActif.y - 1);

        if (cellDown != (int)EnumCell.Chain)
        {
            Utils.setStateFromXY(EventsOnChains.instance.TreuilActif.x, EventsOnChains.instance.TreuilActif.y, 0);

            Cell treuil = gridManager.instance.getCellFromXY(EventsOnChains.instance.TreuilActif.x, EventsOnChains.instance.TreuilActif.y);
            if (treuil != null)
                treuil.setSprite(0);
        }
        else
        {
            Utils.setStateFromXY(EventsOnChains.instance.TreuilActif.x, EventsOnChains.instance.TreuilActif.y, 1);
        }

    }
    private void FindActifTreuil(Collider2D collider)
    {
        const int breakAfter = 5;
        Vector2Int colliderPosition = Utils.PositionInt(collider.transform);

        bool cond1 = colliderPosition.x == EventsOnChains.instance.TreuilActif.x;
        bool cond2 = Utils.getTypeFromXY(colliderPosition.x, colliderPosition.y) == (int)EnumCell.Chain && Utils.getStateFromXY(colliderPosition.x, colliderPosition.y) == 1;

        if (cond1 && !cond2)
            return;

        //Gestion bug quand arrive en haut du treuil
        bool isBlocVide = Utils.getTypeFromXY(colliderPosition.x, colliderPosition.y) == (int)EnumCell.Rien;
        bool isTreuilDessous = Utils.getTypeFromXY(colliderPosition.x, colliderPosition.y - 1) == (int)EnumCell.Treuil;

        if (isBlocVide && isTreuilDessous)
        {
            EventsOnChains.instance.TreuilActif = new Vector2Int(colliderPosition.x, colliderPosition.y - 1);
            return;
        }

        bool nouvTreuil = true;
        int index = colliderPosition.y;

        while (nouvTreuil)
        {
            if (Utils.isInGridMonde(colliderPosition.x, index) && Utils.getTypeFromXY(colliderPosition.x, index) == (int)EnumCell.Treuil)
            {
                EventsOnChains.instance.TreuilActif = new Vector2Int(colliderPosition.x, index);
                nouvTreuil = false;
            }
            index++;

            if (index > breakAfter)
            {
                Utils.setTypeFromXY(colliderPosition.x, colliderPosition.y, (int)EnumCell.Rien);
                gridManager.instance.setGridCell(colliderPosition.x, colliderPosition.y);

                nouvTreuil = false;
                print("sauvé!!");
            }
        }
    }
    private Vector2Int FindActifTreuil(Transform playerPosittion)
    {
        //Safety break
        const int breakAfter = 5;
        Vector2Int PlayerPositionInt = Utils.PositionInt(playerPosittion);

        //Gestion bug quand arrive en haut du treuil
        bool isBlocVide = Utils.getTypeFromXY(PlayerPositionInt.x, PlayerPositionInt.y) == (int)EnumCell.Rien;
        bool isTreuilDessous = Utils.getTypeFromXY(PlayerPositionInt.x, PlayerPositionInt.y - 1) == (int)EnumCell.Treuil;

        if (isBlocVide && isTreuilDessous)
        {
            return new Vector2Int(PlayerPositionInt.x, PlayerPositionInt.y - 1);
        }

        int index = PlayerPositionInt.y;
        while (true)
        {
            if (Utils.isInGridMonde(PlayerPositionInt.x, index) && Utils.getTypeFromXY(PlayerPositionInt.x, index) == (int)EnumCell.Treuil)
            {
                return new Vector2Int(PlayerPositionInt.x, index);
            }
            index++;

            if (index > breakAfter)
            {
                Utils.setTypeFromXY(PlayerPositionInt.x, PlayerPositionInt.y, (int)EnumCell.Rien);
                gridManager.instance.setGridCell(PlayerPositionInt.x, PlayerPositionInt.y);

                return EventsOnChains.instance.TreuilActif;
            }
        }
    }

    private bool isJumpingWinch(Transform playerPosittion)
    {
        Vector2Int PlayerPositionInt = Utils.PositionInt(playerPosittion);

        bool isBlocVide = Utils.getTypeFromXY(PlayerPositionInt.x, PlayerPositionInt.y) == (int)EnumCell.Rien;
        bool isTreuilDessous = Utils.getTypeFromXY(PlayerPositionInt.x, PlayerPositionInt.y - 1) == (int)EnumCell.Treuil;
        bool isVitesseXBig = controller.getAbsVitesse() > 1f;

        if (isBlocVide && isTreuilDessous && !isVitesseXBig)
            return true;
        else
            return false;
    }
    //Chains:
    public static void removeAllChains(Vector2 collider, bool resizeAfter = true)
    {
        Vector2Int colliderPosition = Utils.PositionInt(collider);

        if (colliderPosition.x == EventsOnChains.instance.TreuilActif.x)
        {
            return;
        }

        int x_Inter = colliderPosition.x;
        int y_Inter = colliderPosition.y;

        bool encoreChains = true;
        bool haveBeenRemove = false;

        while (encoreChains)
        {
            y_Inter--;
            encoreChains = false;
            if (Utils.getTypeFromXY(x_Inter, y_Inter) == (int)EnumCell.Chain)
            {
                Utils.setTypeFromXY(x_Inter, y_Inter, (int)EnumCell.Rien);
                ScoreManager.instance.removeChain();

                encoreChains = true;
                haveBeenRemove = true;

                if (!resizeAfter)
                    gridManager.instance.setGridCell(x_Inter, y_Inter);

                PlayerManager.instance.lastChains[Mathf.Abs(y_Inter)] -= 1;
            }
        }

        if (haveBeenRemove)
            Utils.setStateFromXY(colliderPosition.x, colliderPosition.y, 0);
    }
    public static void removeAllChainsAlways(Vector2 collider, bool resizeAfter = true)
    {
        int x_Inter = (int)collider.x;
        int y_Inter = (int)collider.y;

        bool encoreChains = true;
        bool haveBeenRemove = false;

        while (encoreChains)
        {
            y_Inter--;
            encoreChains = false;
            if (Utils.getTypeFromXY(x_Inter, y_Inter) == (int)EnumCell.Chain)
            {
                Utils.setTypeFromXY(x_Inter, y_Inter, (int)EnumCell.Rien);
                ScoreManager.instance.removeChain();

                encoreChains = true;
                haveBeenRemove = true;

                if (!resizeAfter)
                    gridManager.instance.setGridCell(x_Inter, y_Inter);

                PlayerManager.instance.lastChains[Mathf.Abs(y_Inter)] -= 1;
                print("remove chains at " + y_Inter);
            }
        }

        if (haveBeenRemove)
            Utils.setStateFromXY((int)collider.x, (int)collider.y, 0);
    }

    private void mettreUneChaine()
    {
        Vector2Int posPlayer = Utils.PositionInt(transform);

        //Good Place?
        if (posPlayer.x == EventsOnChains.instance.TreuilActif.x && posPlayer.y <= EventsOnChains.instance.TreuilActif.y && Utils.isInGridMonde(posPlayer.x, posPlayer.y))
        {
            int typeBloc = Utils.getTypeFromXY(posPlayer.x, posPlayer.y);
            int typeBlocDeeper = Utils.getTypeFromXY(posPlayer.x, posPlayer.y - 1);

            //ADD CHAIN!
            if (typeBloc == (int)EnumCell.Rien || typeBloc == (int)EnumCell.Platefrom)
            {
                //Improuvements manager good?
                if (ScoreManager.instance.isAddChain())
                {
                    //METTRE CHAIN EN BAS:
                    Utils.setTypeFromXY(posPlayer.x, posPlayer.y, (int)EnumCell.Chain);
                    Utils.setStateFromXY(posPlayer.x, posPlayer.y, 1);
                    gridManager.instance.setGridCell(posPlayer.x, posPlayer.y);

                    //Modifier les objects load (chain haut):
                    Utils.setStateFromXY(posPlayer.x, posPlayer.y + 1, 0);
                    Cell cell = gridManager.instance.getCellFromXY(posPlayer.x, posPlayer.y + 1);

                    if (cell != null)
                        cell.setState();

                    //Ajouter le nombre
                    ScoreManager.instance.addChain();

                    //Ajouter lastChain list:
                    if (isScenePrincipal)
                        PlayerManager.instance.lastChains[Mathf.Abs(posPlayer.y)] += 1;
                }
            }

            if ((typeBloc == (int)EnumCell.Chain || typeBloc == (int)EnumCell.Treuil) && typeBlocDeeper == (int)EnumCell.Chain)
            {
                //REMOVE CHAIN!
                if (Utils.getTypeFromXY(posPlayer.x, posPlayer.y - 1) == (int)EnumCell.Chain)
                {
                    //ENLEVER CHAIN EN BAS:
                    Utils.setTypeFromXY(posPlayer.x, posPlayer.y - 1, (int)EnumCell.Rien);
                    gridManager.instance.setGridCell(posPlayer.x, posPlayer.y - 1);

                    //Modifier les objects load(chain haut):
                    if (typeBloc == (int)EnumCell.Chain)
                    {
                        Utils.setStateFromXY(posPlayer.x, posPlayer.y, 1);
                        Cell cell = gridManager.instance.getCellFromXY(posPlayer.x, posPlayer.y);

                        if (cell != null)
                            cell.setState();
                    }

                    ScoreManager.instance.removeChain();

                    //Remove lastChain list:
                    if (isScenePrincipal)
                        PlayerManager.instance.lastChains[Mathf.Abs(posPlayer.y) + 1] -= 1;
                }

            }
        }
    }
    private void setMovingUpDown(bool is_moving)
    {
        moving_UpDown = is_moving;
    }

    //Dead:
    private void isToDeep()
    {
        if (localDeathByChains && isScenePrincipal)
        {
            const int lenghtCheck = 5;
            const int profTeleporteur = 70;
            int posY = (int)topCheck.position.y;
            int offSet = 1;

            if (ScoreManager.instance.treuils.Count == ImprouvementManager.instance.getStats(EnumLevelName.Chains, true))
                offSet = 0;

            for (int i = 0; i < lenghtCheck; i++)
            {
                int index = Mathf.Abs(posY) - i;

                if (index == 0 || index == profTeleporteur || index == profTeleporteur - 1 || index == profTeleporteur - 2)
                    break;

                if (PlayerManager.instance.lastChains[index] == 0)
                {
                    offSet -= 1;
                }

            }

            if (offSet < 0)
            {
                EventsGameState.instance.dead(typeMort.Chains);
            }
        }
    }
}

