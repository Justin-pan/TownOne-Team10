using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int m_MaxHealth;
    private Animator m_CurrentAnimation;
    [SerializeField]
    private RuntimeAnimatorController m_AnimatorController;
    [SerializeField] private int maxHealth;

    public int PlayerID { get; set; }

    private int currentHealth;

    private PlayerState m_PlayerState;


    private void Awake()
    {
        m_PlayerState = PlayerState.Idle;

        currentHealth = maxHealth;
        m_CurrentAnimation = GetComponent<Animator>();
        m_CurrentAnimation.runtimeAnimatorController = m_AnimatorController;
    }

    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
    }

    public void Update()
    {
        // TODO
        if (Input.GetButtonDown("Jump"))
        {
            m_PlayerState = PlayerState.Moving;
        }
        else
        {
            m_PlayerState = PlayerState.Idle;
        }
        switch(m_PlayerState)
        {
            case PlayerState.Idle:
                m_CurrentAnimation.SetBool("Moving", false);
                // TODO implement Idle
                break;
            case PlayerState.Moving:
                m_CurrentAnimation.SetBool("Moving", true);
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
        m_PlayerState = PlayerState.Hurt;
        currentHealth = Mathf.Clamp(currentHealth - hit.damage, 0, maxHealth);
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
