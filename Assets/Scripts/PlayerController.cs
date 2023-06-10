using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;

    [Header("Parameters")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;

    [Range(0, 1)][SerializeField] private float traction;
    [Range(0, 1)][SerializeField] private float airTraction;

    // [Components]
    private Rigidbody2D mRigidbody2D;

    private Vector2 currentVelocity;
    private bool isGrounded;

    private void Awake()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();

        currentVelocity = Vector2.zero;
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        // Check if there is anything underneath the player.
        isGrounded = false;

        foreach (Collider2D c in Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius))
            isGrounded |= c.gameObject != gameObject;
    }

    public void Move(float direction, bool jump, bool dash)
    {
        Vector3 target = mRigidbody2D.velocity;
        target.x = direction * moveSpeed;

        mRigidbody2D.velocity = Vector2.SmoothDamp(mRigidbody2D.velocity, target, ref currentVelocity,
            isGrounded ? 1 - traction : 1 - airTraction);

        if (isGrounded && jump)
        {
            Vector3 velocity = mRigidbody2D.velocity;
            velocity.y = jumpSpeed;

            mRigidbody2D.velocity = velocity;
            isGrounded = false;
        }

        // Flip the player to match the input direction.
        Vector3 localScale = transform.localScale;
        localScale.x = direction != 0 ? Mathf.Sign(direction) : localScale.x;

        transform.localScale = localScale;
    }
}
