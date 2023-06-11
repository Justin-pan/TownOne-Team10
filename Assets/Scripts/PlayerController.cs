using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private float minHorizontalDashSpeed;

    [Header("Parameters")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpSpeed;

    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashDuration;
    [SerializeField] public float dashKnockback;

    [Range(0, 1)][SerializeField] public float traction;
    [Range(0, 1)][SerializeField] public float airTraction;

    public bool IsGrounded { get; set; }
    public bool IsDashReady { get; set; }
    public bool IsDashing { get; set; }

    // [Components]
    private Rigidbody2D mRigidbody2D;

    private Vector2 currentVelocity;

    private void Awake()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();

        currentVelocity = Vector2.zero;
        IsGrounded = IsDashReady = IsDashing = false;
    }

    private void Update()
    {
        // Ignore player input while dashing.
        if (IsDashing)
        {
            return;
        }

        HandleMove(Input.GetAxis("Horizontal"), Input.GetButtonDown("Jump"));
        HandleDash(Input.GetAxis("Horizontal"), Input.GetButton("Jump"), Input.GetButtonDown("Dash"));
    }

    private void FixedUpdate()
    {
        IsGrounded = false;

        // The player is grounded if there is anything solid directly below.
        foreach (Collider2D c in Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius))
        {
            if (c.gameObject != gameObject)
            {
                IsGrounded = IsDashReady = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Colliding with anything solid immediately ends the dash.
        if (collision.gameObject.TryGetComponent(out Player player) && IsDashing)
        {
            player.OnHit(new Hit(0, currentVelocity.normalized * dashKnockback));
        }
        IsDashing = false;
    }

    private void HandleMove(float inputDirection, bool inputJump)
    {
        Vector3 target = mRigidbody2D.velocity;
        target.x = inputDirection * moveSpeed;

        mRigidbody2D.velocity = Vector2.SmoothDamp(mRigidbody2D.velocity, target, ref currentVelocity,
            IsGrounded ? 1 - traction : 1 - airTraction);

        if (IsGrounded && inputJump)
        {
            Vector3 velocity = currentVelocity;
            velocity.y = jumpSpeed;

            mRigidbody2D.velocity = velocity;
            IsGrounded = false;
        }
    }

    private void HandleDash(float inputDirection, bool inputJump, bool inputDash)
    {
        if (IsDashReady && inputDash)
        {
            float x = inputDirection > minHorizontalDashSpeed ? 1 : inputDirection < -minHorizontalDashSpeed ? -1 : 0;
            float y = !IsGrounded && inputJump ? 1 : 0;

            Vector2 dashTarget = new Vector2(x, y) * dashSpeed;

            if (dashTarget != Vector2.zero)
            {
                _ = StartCoroutine(DashController(dashTarget));
            }
        }
    }

    private IEnumerator DashController(Vector2 target)
    {
        IsDashReady = false;
        IsDashing = true;

        float gravityScale = mRigidbody2D.gravityScale;
        mRigidbody2D.gravityScale = 0;

        for (float t = 0; IsDashing && t < dashDuration; t += Time.deltaTime)
        {
            mRigidbody2D.velocity = Vector2.Lerp(target, Vector2.zero, t / dashDuration);
            yield return null;
        }

        mRigidbody2D.velocity = Vector2.zero;
        mRigidbody2D.gravityScale = gravityScale;

        IsDashing = false;
    }
}
