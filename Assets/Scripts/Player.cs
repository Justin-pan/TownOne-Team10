using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] public int maxHealth;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpSpeed;

    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashDuration;
    [SerializeField] public float dashKnockback;

    [Range(0, 1)][SerializeField] public float traction;
    [Range(0, 1)][SerializeField] public float airTraction;
    [SerializeField] public int placeablesPerRound;

    [Header("Technical")]
    [SerializeField] private string inputDirectionPrefix;
    [SerializeField] private string inputJumpPrefix;
    [SerializeField] private string inputDashPrefix;
    [SerializeField] private string inputPlacePrefix;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private float minHorizontalDashSpeed;

    [Header("Other")]
    [SerializeField] private Placeable placeableToPlace;

    private int platformsLeft;

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

    private Vector2 currentVelocity;

    private Transform heightReferencePoint;

    public Transform HeightReferencePoint
    {
        get => heightReferencePoint;
        set => heightReferencePoint = value;
    }

    private PlayerState m_PlayerState;

    // [Components]
    private Rigidbody2D mRigidbody2D;

    private Animator m_CurrentAnimation;

    private int currentHealth;

    private List<Perk> perks;

    private Animator m_Animator;

    private bool isDead = false;

    private bool stillHurt = false;

    private bool isGrounded;
    private bool isDashReady;
    private bool isDashing;

    private bool facingRight;

    private void Awake()
    {
        currentHealth = maxHealth;
        facingRight = false;
        isGrounded = isDashReady = isDashing = false;
        currentVelocity = Vector2.zero;
        perks = new List<Perk>();
        m_PlayerState = PlayerState.Idle;

        mRigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
    }

    public void Update()
    {
        // TODO
        if (!isDashing && !stillHurt && !isDead)
        {
            HandleMove(Input.GetAxis(inputDirectionPrefix + PlayerID),
                Input.GetButtonDown(inputJumpPrefix + PlayerID));
            HandleDash(Input.GetAxis(inputDirectionPrefix + PlayerID),
                Input.GetButton(inputJumpPrefix + PlayerID), Input.GetButton(inputDashPrefix + PlayerID));
            HandlePlace(Input.GetButtonDown(inputPlacePrefix + PlayerID));
        }
        m_PlayerState = (mRigidbody2D.velocity.y < 0) ? PlayerState.Falling : m_PlayerState;
        m_PlayerState = (stillHurt) ? PlayerState.Hurt : m_PlayerState;
        if (currentHealth <= 0)
            m_PlayerState = PlayerState.Dead;
        switch(m_PlayerState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                break;
            case PlayerState.Jumping:
                break;
            case PlayerState.Hurt:
                break;
            case PlayerState.Dashing:
                break;
            case PlayerState.Falling:
                break;
            case PlayerState.Dead:
                break;
        }

        playerHeight = gameObject.transform.position.y - HeightReferencePoint.position.y;
        playerMaxHeight = Mathf.Max(playerHeight, playerMaxHeight);
    }

    private void FixedUpdate()
    {
        isGrounded = false;

        foreach (Collider2D c in Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius))
        {
            if (c.gameObject != gameObject)
            {
                isGrounded = isDashReady = true;
            }
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && collision.gameObject.TryGetComponent(out Player newPlayer))
        {
            newPlayer.OnHit(new Hit(0, mRigidbody2D.velocity));
            isDashing = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    private void HandleMove(float inputDirection, bool inputJump)
    {
        if (inputDirection != 0)
            m_PlayerState = PlayerState.Moving;
        if (inputDirection > 0 && !facingRight)
            Flip();
        else if (inputDirection < 0 && facingRight)
            Flip();

        Vector3 target = mRigidbody2D.velocity;
        target.x = inputDirection * moveSpeed;

        mRigidbody2D.velocity = Vector2.SmoothDamp(mRigidbody2D.velocity, target, ref currentVelocity,
            isGrounded ? 1 - traction : 1 - airTraction);

        if (isGrounded && inputJump)
        {
            Vector3 velocity = currentVelocity;
            velocity.y = jumpSpeed;

            mRigidbody2D.velocity = velocity;
            isGrounded = false;
            m_PlayerState = PlayerState.Jumping;
        }
    }

    private void HandleDash(float inputDirection, bool inputJump, bool inputDash)
    {
        if (isDashReady && inputDash)
        {
            float x = inputDirection > minHorizontalDashSpeed ? 1 : inputDirection < -minHorizontalDashSpeed ? -1 : 0;
            float y = !isGrounded && inputJump ? 1 : 0;

            Vector2 dashTarget = new Vector2(x, y) * dashSpeed;

            if (dashTarget != Vector2.zero)
            {
                _ = StartCoroutine(DashController(dashTarget));
            }
        }
    }

    private void HandlePlace(bool buttonDown)
    {
        if (buttonDown)
        {
            if (platformsLeft <= 0)
            {
                return;
            }
            mRigidbody2D.velocity = Vector2.zero;
            Debug.Log(new Vector3(transform.position.x, groundCheck.position.y - 0.5f, transform.position.z));
            try
            {
                if (GameManager.Instance.TryPlace(placeableToPlace, new Vector3(transform.position.x, groundCheck.position.y - 0.5f, transform.position.z)))
                {
                    platformsLeft -= 1;
                    Debug.Log("Placed!");
                }
            } catch (ArgumentException e)
            {
                // dw everything is fine (that is a lie)
            }
        }
    }

    private IEnumerator DashController(Vector2 target)
    {
        isDashReady = false;
        isDashing = true;
        m_PlayerState = PlayerState.Dashing;

        float gravityScale = 1;
        mRigidbody2D.gravityScale = 0;

        for (float t = 0; isDashing && t < dashDuration; t += Time.deltaTime)
        {
            mRigidbody2D.velocity = Vector2.Lerp(target, Vector2.zero, t / dashDuration);
            yield return null;
        }

        mRigidbody2D.velocity = Vector2.zero;
        mRigidbody2D.gravityScale = gravityScale;

         isDashing = false;
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
        platformsLeft = placeablesPerRound;
    }
}
