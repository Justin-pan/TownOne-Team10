using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private float m_GroundCheckHeight;

    [Header("Parameters")]
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_DashSpeed;
    [SerializeField] private float m_JumpSpeed;

    [Range(0, 1)][SerializeField] private float m_Traction;
    [Range(0, 1)][SerializeField] private float m_AirTraction;

    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D m_Rigidbody2D;
    private Collider2D m_Collider2D;

    private float m_TargetSpeed;

    private bool m_IsGrounded;
    private bool m_IsFacingRight;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();

        m_TargetSpeed = 0;

        m_IsGrounded = false;
        m_IsFacingRight = true;
}

    private void Update()
    {
        float inputMove = Input.GetAxis("Horizontal");
        bool inputJump = Input.GetButtonDown("Jump");

        m_TargetSpeed = inputMove * m_MoveSpeed;

        if (inputJump && m_IsGrounded)
        {
            // m_Rigidbody2D.velocity = new(m_Rigidbody2D.velocity.y, m_JumpSpeed);

            m_Rigidbody2D.AddForce(m_JumpSpeed * Vector2.up, ForceMode2D.Impulse);
            m_IsGrounded = false;
        }

        // Update the sprite to match the input direction.
        if (m_IsFacingRight ? m_TargetSpeed < 0 : m_TargetSpeed > 0)
        {
            m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
            m_IsFacingRight = !m_IsFacingRight;
        }
    }

    private void FixedUpdate()
    {
        // m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, m_TargetVelocity, ref m_Velocity, 
        //     1 - (m_IsGrounded ? m_Traction : m_AirTraction));
        
        // TODO: Better collision check method?
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (m_Collider2D.bounds.extents.y * Vector3.down),
            0.9f * Vector3.right + (m_GroundCheckHeight * Vector3.up), 0);

        float speed = m_TargetSpeed - m_Rigidbody2D.velocity.x;
        m_Rigidbody2D.AddForce(speed * (m_IsGrounded ? m_Traction : m_AirTraction) * Vector2.right, ForceMode2D.Impulse);

        m_IsGrounded = false;
        foreach (Collider2D collider in colliders)
        {
            m_IsGrounded |= collider.gameObject != gameObject;
        }
    }
}
