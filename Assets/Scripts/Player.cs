using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int m_MaxHealth;

    public int PlayerID { get; set; }

    private int m_Health;

    private PlayerState m_PlayerState;

    private void Awake()
    {
        m_Health = m_MaxHealth;
    }

    private void Start()
    {
        // TODO
        m_PlayerState = PlayerState.Idle;
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
            case PlayerState.Hurt:
                // TODO implement Hurt
                break;
            case PlayerState.Jumping:
                // TODO implement Jumping
                break;
        }
    }

    public void OnHit(Hit hit)
    {
        m_PlayerState = PlayerState.Hurt;
        m_Health = Mathf.Clamp(m_Health - hit.damage, 0, m_MaxHealth);
    }
}

// Enum for Player States
public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Hurt
}
