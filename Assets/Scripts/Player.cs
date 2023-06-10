using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int m_MaxHealth;
    //[SerializeField]
    //private Animation m_CurrentAnimation;
    [SerializeField] private int maxHealth;

    public int PlayerID { get; set; }

    // [Components]
    private PlayerController mPlayerController;

    private int currentHealth;

    private PlayerState m_PlayerState;


    private void Awake()
    {
        mPlayerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        // TODO
        m_PlayerState = PlayerState.Idle;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // TODO
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
        mPlayerController.Move(Input.GetAxis("Horizontal"), Input.GetButtonDown("Jump"), Input.GetButtonDown("Fire1"));
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
