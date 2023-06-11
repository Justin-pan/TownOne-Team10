using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int m_MaxHealth;
    [SerializeField] private int maxHealth;

    public int PlayerID { get; set; }

    private PlayerState m_PlayerState;

    // [Components]
    private Rigidbody2D mRigidbody2D;

    private PlayerController m_PlayerController;

    private int currentHealth;

    private Animator m_Animator;

    private bool m_WasDashReady;

    private bool isDead = false;

    private bool stillHurt = false;

    private void Awake()
    {
        m_PlayerState = PlayerState.Idle;

        currentHealth = maxHealth;
        mRigidbody2D = GetComponent<Rigidbody2D>();
        m_PlayerController = GetComponent<PlayerController>();
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
    }

    public void Update()
    {
        // TODO
        m_PlayerState = m_PlayerController.CurrentState();
        m_PlayerState = (stillHurt) ? PlayerState.Hurt : m_PlayerState;
        if (currentHealth <= 0)
            m_PlayerState = PlayerState.Dead;
        switch(m_PlayerState)
        {
            case PlayerState.Idle:
                // TODO implement Idle
                break;
            case PlayerState.Moving:
                // TODO implement Moving
                break;
           case PlayerState.Jumping:
                // TODO implement Jumping
                break;
            case PlayerState.Hurt:
                // TODO implement Hurt
                break;
            case PlayerState.Dashing:
                // TODO implement Dashing
                break;
            case PlayerState.Falling:
                // TODO
                break;
            case PlayerState.Dead:
                // TODO
                break;
        }
    }

    public void OnHit(Hit hit)
    {
        currentHealth = Mathf.Clamp(currentHealth - hit.Damage, 0, maxHealth);
        mRigidbody2D.AddForce(hit.Knockback);
        stillHurt = true;
        Invoke("KnockBackDelay", 0.3f);
    }

    private void KnockBackDelay()
    {
        stillHurt = false;
    }
}

// Enum for Player States
public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Hurt,
    Dashing,
    Falling,
    Dead
}
