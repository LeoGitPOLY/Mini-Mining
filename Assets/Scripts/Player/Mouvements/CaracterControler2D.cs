using UnityEngine;
using UnityEngine.Events;

public class CaracterControler2D : MonoBehaviour
{
    [Header("Smoothing:")]
    [Range(0, .7f)] [SerializeField] private float m_MovementSmoothing_Acc = .05f;
    [Range(0, .7f)] [SerializeField] private float m_MovementSmoothing_Decc = .05f;  // How much to smooth out the movement

    [Range(0, .7f)] [SerializeField] private float m_MovementSmoothingUP_DOWN_Acc = .05f;
    [Range(0, .7f)] [SerializeField] private float m_MovementSmoothingUP_DOWN_Decc = .05f;

    [Space()]
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private TriggerUpDown TriggerUpDown;

    public bool m_Grounded;            // Whether or not the player is grounded.

    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private float last_MovementSmoothing_Decc;

    private float lastPosition;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        RopeManager ropeM = GetComponent<RopeManager>();

        EventsOnChains.instance.onChainsEventMine += RopeEventsListener;
        EventsOnChains.instance.onChainsEventOther += RopeEventsListener;

        last_MovementSmoothing_Decc = m_MovementSmoothing_Decc;
    }

    public void Move(float move)
    {

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

            // And then smoothing it out and applying it to the character
            float smoothing;
            smoothing = Mathf.Abs(targetVelocity.x) > Mathf.Abs(m_Rigidbody2D.velocity.x) ? m_MovementSmoothing_Acc : m_MovementSmoothing_Decc;
            smoothing = EventsOnChains.instance.isOnChainsOther ? m_MovementSmoothing_Decc : smoothing;

            if (Mathf.Abs(targetVelocity.x) < Mathf.Abs(m_Rigidbody2D.velocity.x) && EventsOnChains.instance.isOnChainsOther)
                smoothing = 0;

            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, smoothing);

            double differential = System.Math.Round(lastPosition - transform.position.x, 1);

            if (differential != 0)
            {
                PlayerManagerAnim.instance.moveStateAnim = MoveState.Move;
            }
            else
            {
                PlayerManagerAnim.instance.moveStateAnim = MoveState.notMoving;
            }

            if (move > 0)
            {
                MoveState.isMovingLeft = false;
            }
            else if (move < 0)
            {
                MoveState.isMovingLeft = true;
            }
        }

        lastPosition = gameObject.transform.position.x;
    }
    public void StopMovingX()
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(0, m_Rigidbody2D.velocity.y);

        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = targetVelocity;

        lastPosition = gameObject.transform.position.x;
    }

    public void moveUpDown(float move)
    {
        // If the player should jump...
        if (EventsOnChains.instance.isOnChainsMine || EventsOnChains.instance.isOnChainsOther)
        {
            Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, move * 10f);
            // And then smoothing it out and applying it to the character
            float smoothing;
            smoothing = Mathf.Abs(targetVelocity.y) > Mathf.Abs(m_Rigidbody2D.velocity.y) ? m_MovementSmoothingUP_DOWN_Acc : m_MovementSmoothingUP_DOWN_Decc;
            smoothing = EventsOnChains.instance.isOnChainsOther ? m_MovementSmoothingUP_DOWN_Decc : smoothing;

            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, smoothing);
        }
    }

    public void setIsGround(bool isGround)
    {
        m_Grounded = isGround;
    }
    public float getAbsVitesse()
    {
        return Mathf.Abs(m_Rigidbody2D.velocity.x);
    }

    //EVENTS:
    private void RopeEventsListener(bool OnChains)
    {
        if (OnChains)
        {
            m_Rigidbody2D.gravityScale = 0;
        }
        else
        {
            m_Rigidbody2D.gravityScale = 3;
        }
    }
}