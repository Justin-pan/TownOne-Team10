using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private RuntimeAnimatorController m_AnimatorController;

    [Header("Parameters")]
    [SerializeField] public int maxHealth;

    public int PlayerID { get; set; }

    // [Components]
    private Rigidbody2D mRigidbody2D;
    private Animator m_CurrentAnimation;

    private int currentHealth;
    private List<Perk> perks;

    private PlayerState m_PlayerState;

    private void Awake()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        perks = new List<Perk>();

        m_PlayerState = PlayerState.Idle;
    }

    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
    }

    public void Update()
    {
        // TODO
        switch (m_PlayerState)
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
        mRigidbody2D.AddForce(hit.Knockback, ForceMode2D.Impulse);

        m_PlayerState = PlayerState.Hurt;
    }

    public void AddPerk(Perk p)
    {
        perks.Add(p);
    }
}
