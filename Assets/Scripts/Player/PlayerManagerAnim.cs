using UnityEngine;

public static class digState
{
    public const string digNormal = "DigNormal";
    public const string digUp = "DigUP";
    public const string digDown = "DigDOWN";
    public const string notDiging = "NotDig";

    public const int layer = 1;

    public static bool isDIgLeft;
}

public static class MoveState
{
    public const string Move = "MovingState";
    public const string notMoving = "NotMovingState";

    public const int layer = 0;

    public static bool isMovingLeft;

}

public static class BoolNameAnimator
{
    public const string strDiging = "StateDiging";

    public const int intDigNormal = 0;
    public const int intDigUp = 1;
    public const int intDigDown = 2;

}
public class PlayerManagerAnim : MonoBehaviour
{
    public static PlayerManagerAnim instance;

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject dig;
    [SerializeField] private GameObject Entenne;
    [SerializeField] private GameObject Chenille;
    

    [Header("AnimatorController")]
    [SerializeField] private SkinTheme skinToBuy;
    

    private Animator animator;
    private string[] currentState;

    [HideInInspector] public string digStateAnim;
    [HideInInspector] public string moveStateAnim;

    private bool m_DiggingRight;
    private bool m_FacingRight;

    private int lastIntDig = -1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = skinToBuy.allSkin[0].AnimatorControllers; //SAVE THROW EXUCATION = get...

        digStateAnim = digState.notDiging;
        moveStateAnim = MoveState.notMoving;

        m_DiggingRight = true;
        m_FacingRight = true;

        digState.isDIgLeft = false;
        MoveState.isMovingLeft = false;

        currentState = new string[2];
        currentState[0] = "";
        currentState[1] = "";
    }
    private void LateUpdate()
    {
        manageAnim();
    }

    private void manageAnim()
    {
        //SET DIG ANIM:
        ChangeAnimationState(digStateAnim, digState.layer);

        //SET STATE DIG:
        int indexAnim = -1;
        switch (digStateAnim)
        {
            case digState.notDiging:
                indexAnim = BoolNameAnimator.intDigNormal;
                break;
            case digState.digNormal:
                indexAnim = BoolNameAnimator.intDigNormal;
                break;
            case digState.digUp:
                indexAnim = BoolNameAnimator.intDigUp;
                break;
            case digState.digDown:
                indexAnim = BoolNameAnimator.intDigDown;
                break;
            default:
                break;
        }
        if (lastIntDig != indexAnim)
        {
            currentState[MoveState.layer] = "";
            lastIntDig = indexAnim;
        }

        animator.SetInteger(BoolNameAnimator.strDiging, indexAnim);

        //Set MOVE ANIM:
        ChangeAnimationState(moveStateAnim, MoveState.layer);

        if(digStateAnim!= digState.notDiging)
        {
            //Set Directrion Dig:
            if (m_DiggingRight && digState.isDIgLeft)
            {
                // ... flip the dig.
                FlipDig();
            }
            else if (!m_DiggingRight && !digState.isDIgLeft)
            {
                // ... flip the dig.
                FlipDig();
            }
        }       
        //MEME SENS DIRECTION:
        else if(digStateAnim == digState.notDiging && (m_FacingRight != m_DiggingRight))
        {
            // ... flip the dig.
            FlipDig();
        }

        //Set direction autre:
        if (m_FacingRight && MoveState.isMovingLeft)
        {
            // ... flip the player.
            FlipAutre();
        }
        else if (!m_FacingRight && !MoveState.isMovingLeft)
        {
            // ... flip the player.
            FlipAutre();
        }
    }


    private void FlipDig()
    {
        // Switch the way the player is labelled as facing.
        m_DiggingRight = !m_DiggingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theRotationBody = body.transform.eulerAngles;
        //Vector3 theScaleBody = body.transform.localScale;
        Vector3 theScaleDig = dig.transform.localScale;

        theRotationBody = new Vector3(theRotationBody.x, (180 + theRotationBody.y) % 360, theRotationBody.z);
        //theScaleBody.x *= -1;
        theScaleDig.x *= -1;

        body.transform.eulerAngles = theRotationBody;
        //body.transform.localScale = theScaleBody;
        dig.transform.localScale = theScaleDig;
    }

    private void FlipAutre()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScaleEntenne = Entenne.transform.localScale;
        Vector3 theScaleChenille = Chenille.transform.localScale;

        theScaleEntenne.x *= -1;
        theScaleChenille.x *= -1;

        Entenne.transform.localScale = theScaleEntenne;
        Chenille.transform.localScale = theScaleChenille;
    }


    public void ChangeAnimationState(string newState, int layer)
    {
        animator.enabled = true;

        if (animator != null)
        {
            //stop the same animation from interrupting itself
            if (currentState[layer] == newState) return;

            //play the animation
            animator.Play(newState);

            //reassign the current state
            currentState[layer] = newState;
        }
    }

    public void AnimEnable(bool enable)
    {
        animator.enabled = enable;
    }

    public bool getFacingRight()
    {
        return m_FacingRight;
    }

    public void changeAnimatorController(EnumSkinName skinSelected)
    {
        foreach (SkinToBuy item in skinToBuy.allSkin)
        {
            if(item.SkinType == skinSelected)
            {
                animator.runtimeAnimatorController = item.AnimatorControllers;
            }
        }
    }
}
