using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMouvement : MonoBehaviour
{
    [Header("Managers:")]
    [SerializeField] private CaracterControler2D controller;
    [SerializeField] private Joystick joystick;

    [Header("GameObjects:")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject bonderie;

    [Header("Valeurs modifiables:")]
    [SerializeField] [Range(20f, 70f)] private float runSpeed = 40f;
    [SerializeField] [Range(20f, 70f)] private float climbSpeed = 10f;
    [SerializeField] [Range(0f, 1f)] private float xDeadZone;
    [SerializeField] [Range(0f, 5f)] private float attraction;

    //Instance:

    //Propriété de Classe:
    private PlayerManagerAnim anim;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    private bool wasRight = true;

    private Vector3 lastPosition;
    private int deepestPositionY;

    public bool BlockHorizontaleMov = false;
    public bool BlockVerticaleMov = false;

    public bool isLosingFuel = true;

    //Events:
    public event Action<bool> moving_UpDown;

    //MONOBEHAVIOUR METHODE:
    private void Start()
    {
        RopeManager ropeM = GetComponent<RopeManager>();
        anim = GetComponent<PlayerManagerAnim>();

        EventsOnChains.instance.onChainsEventMine += setOnChains;
        EventsOnChains.instance.onChainsEventOther += setOnChains;
        EventsOnChains.instance.onChainsEventMineReal += blockStupidFalling;
        EventsOnChains.instance.onEndChainsEvent += endChains;

        EventsControlsUI.instance.onChangeMovHoriz += MoveEventHorizontale;
        EventsControlsUI.instance.onChangeMovVerti += MoveEventVertical;


        lastPosition = transform.position;
        deepestPositionY = int.MinValue;


    }
    private void FixedUpdate()
    {
        AttractionChain();
        move();
        fuelRemover();

        lastPosition = transform.position;
    }

    //Methode call every FixedUpdate:
    private void move()
    {
        float offset = 0.45f;

        //CURVE TROTTLE: ( y = 0.8 x^2 + 0.2x )
        int signe_hor = horizontalMove < 0 ? -1 : 1;
        int signe_ver = verticalMove < 0 ? -1 : 1;

        horizontalMove = Mathf.Abs(horizontalMove);
        verticalMove = Mathf.Abs(verticalMove);

        horizontalMove = signe_hor * (0.8f * Mathf.Pow(horizontalMove, 2) + 0.2f * horizontalMove);
        verticalMove = signe_ver * (0.8f * Mathf.Pow(verticalMove, 2) + 0.2f * verticalMove);

        //LAST CHAIN
        if (transform.position.y < (deepestPositionY - offset) && verticalMove < 0)
            verticalMove = 0;

        if (BlockVerticaleMov)
            verticalMove = 0;
        if (BlockHorizontaleMov)
            horizontalMove = 0;

        //DeadZone
        if (SettingManager.instance.AssistedChainsControle)
            if (EventsOnChains.instance.isOnChainsMine)
            {
                if (Mathf.Abs(horizontalMove) < xDeadZone)
                    horizontalMove = 0;
            }

        //Remove updown quand descend chaine
        const float horMax = 0.5f;
        if (SettingManager.instance.AssistedChainsControle)
            if (horizontalMove > horMax)
                verticalMove = verticalMove / 4;

        //Reset quand change side
        if ((wasRight && signe_hor == -1 && horizontalMove != 0) || (!wasRight && signe_hor == 1 && horizontalMove != 0))
        {
            controller.StopMovingX();
            horizontalMove = 0;
        }

        wasRight = signe_hor == 1;


        controller.Move(horizontalMove * Time.fixedDeltaTime * runSpeed);
        controller.moveUpDown(verticalMove * Time.fixedDeltaTime * climbSpeed);


        if (verticalMove != 0)
            moving_UpDown?.Invoke(true);
        else
            moving_UpDown?.Invoke(false);
    }
    private void fuelRemover()
    {
        if (SceneManager.GetActiveScene().name != SceneName.SCENE_PRINCIPALE)
            return;

        if (isLosingFuel)
        {
            const float ratioY = 0.3f;
            float fuelX = 0;
            float fuelY = 0;

            if (Math.Abs(horizontalMove) > 0.01f)
                fuelX = 1;
            if (Math.Abs(verticalMove) > 0.01f)
                fuelY = 1;

            ScoreManager.instance.removeFuel(Math.Abs(transform.position.x - lastPosition.x) * fuelX + Math.Abs(transform.position.y - lastPosition.y) * ratioY * fuelY);
        }
    }
    private void AttractionChain()
    {
        if (SettingManager.instance.AssistedChainsControle)
            if (EventsOnChains.instance.isOnChainsMine && Mathf.Abs(horizontalMove) < xDeadZone)
            {
                float posX = transform.position.x;

                posX = LeanSmooth.linear(posX, EventsOnChains.instance.TreuilActif.x + 0.5f, attraction);
                transform.position = new Vector2(posX, transform.position.y);
            }
    }

    //Events methode:
    private void setOnChains(bool is_chains)
    {
        if (is_chains)
        {
            joystick.AxisOptions = AxisOptions.Both;
        }
        else
            joystick.AxisOptions = AxisOptions.Horizontal;
    }

    private void endChains(bool end)
    {
        if (end)
            deepestPositionY = Utils.PositionInt(transform).y;
        else
            deepestPositionY = int.MinValue;
    }
    private void blockStupidFalling(Vector2Int nextPosition)
    {
        if (SettingManager.instance.AssistedChainsControle)
        {
            float posX = nextPosition.x + 0.5f;
            transform.position = new Vector2(posX, transform.position.y);

            horizontalMove = 0;
            verticalMove = 0;

            controller.StopMovingX();

            joystick.resetHorizontalAxe();
            joystick.BlockLeftRight = true;
        }
    }
    private void MoveEventHorizontale(float moveValue)
    {
        horizontalMove = moveValue;
    }
    private void MoveEventVertical(float moveValue)
    {
        verticalMove = moveValue;
    }

    public void setTop()
    {
        Vector2 p = new Vector2(transform.position.x, 0);
        transform.position = p;
    }
}





