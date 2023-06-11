using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] public int maxHealth;

    public int PlayerID { get; set; }

    private float playerHeight;
    public float PlayerHeight
    {
        get => playerHeight;
        set => playerHeight = value;
    }

    private float playerMaxHeight;

    public float PlayerMaxHeight
    {
        get => playerMaxHeight;
    }

    [SerializeField]  private Transform heightReferencePoint;

    public Transform HeightReferencePoint
    {
        get => heightReferencePoint;
        set => heightReferencePoint = value;
    }

    private PlayerState m_PlayerState;

    // [Components]
    private Rigidbody2D mRigidbody2D;

    private PlayerController m_PlayerController;

    private SpriteRenderer m_SpriteRenderer;

    private Animator m_CurrentAnimation;

    private int currentHealth;

    private List<Perk> perks;

    private Animator m_Animator;

    private bool m_WasDashReady;

    private bool isDead = false;

    private bool stillHurt = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        mRigidbody2D = GetComponent<Rigidbody2D>();
        m_PlayerController = GetComponent<PlayerController>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        mRigidbody2D = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        perks = new List<Perk>();

        m_PlayerState = PlayerState.Idle;
    }

    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
        m_SpriteRenderer.size *= 4.0f;
    }

    public void Update()
    {
        // TODO
        m_PlayerState = CurrentState();
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

        playerHeight = gameObject.transform.position.y - HeightReferencePoint.position.y;
        playerMaxHeight = Mathf.Max(playerHeight, playerMaxHeight);
    }

    public void OnHit(Hit hit)
    {
        currentHealth = Mathf.Clamp(currentHealth - hit.Damage, 0, maxHealth);
        mRigidbody2D.AddForce(hit.Knockback, ForceMode2D.Impulse);
        stillHurt = true;
        Invoke("KnockBackDelay", 0.3f);
    }

    private void KnockBackDelay()
    {
        stillHurt = false;
    }

    public void AddPerk(Perk p)
    {
        perks.Add(p);
        p.ApplyEffect(this);
    }

    public void ResetPlayer()
    {
        isDead = false;
        stillHurt = false;
        playerMaxHeight = 0;
    }
    public PlayerState CurrentState()
    {
        if (m_PlayerController.IsDashing)
        {
            return PlayerState.Dashing;
        }
        else if (!m_PlayerController.IsGrounded && mRigidbody2D.velocity.y > 0)
        {
            return PlayerState.Jumping;
        }
        else if (!m_PlayerController.IsGrounded && mRigidbody2D.velocity.y <= 0)
        {
            return PlayerState.Falling;
        }
        else if (m_PlayerController.IsGrounded && mRigidbody2D.velocity.x == 0)
        {
            return PlayerState.Idle;
        }
        else if (m_PlayerController.IsGrounded && mRigidbody2D.velocity.x != 0)
        {
            return PlayerState.Moving;
        }
        else
        {
            return PlayerState.Idle;
        }
    }
}
