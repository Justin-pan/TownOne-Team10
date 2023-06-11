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
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashKnockback;

    [Range(0, 1)][SerializeField] private float traction;
    [Range(0, 1)][SerializeField] private float airTraction;

    // [Components]
    private Rigidbody2D mRigidbody2D;

    private Vector2 currentVelocity;

    private bool isGrounded;
    private bool isDashing;

    private bool canDash;

    private void Awake()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();

        currentVelocity = Vector2.zero;
        isGrounded = isDashing = canDash = false;
    }

    private void Update()
    {
        // Ignore player input while dashing.
        if (isDashing)
        {
            return;
        }

        HandleMove(Input.GetAxis("Horizontal"), Input.GetButtonDown("Jump"));
        HandleDash(Input.GetAxis("Horizontal"), Input.GetButton("Jump"), Input.GetButtonDown("Dash"));
    }

    private void FixedUpdate()
    {
        isGrounded = false;

        // The player is grounded if there is anything solid directly below.
        foreach (Collider2D c in Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius))
        {
            if (c.gameObject != gameObject)
            {
                isGrounded = canDash = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Colliding with anything solid immediately ends the dash.
        isDashing = false;

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.OnHit(new Hit(0, currentVelocity.normalized * dashKnockback));
        }
    }

    private void HandleMove(float inputDirection, bool inputJump)
    {
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
        }
    }

    private void HandleDash(float inputDirection, bool inputJump, bool inputDash)
    {
        if (canDash && inputDash)
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

    private IEnumerator DashController(Vector2 target)
    {
        canDash = false;
        isDashing = true;

        float gravityScale = mRigidbody2D.gravityScale;
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
}
