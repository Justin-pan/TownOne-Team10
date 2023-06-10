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

    private Vector2 m_TargetVelocity;
    private Vector2 m_Velocity;

    private bool m_IsGrounded;
    private bool m_IsFacingRight;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();

        m_TargetVelocity = Vector2.zero;
        m_Velocity = Vector2.zero;

        m_IsGrounded = false;
        m_IsFacingRight = true;
}

    private void Update()
    {
        float move = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");

        m_TargetVelocity = new(move * m_MoveSpeed, m_Rigidbody2D.velocity.y);

        if (m_IsGrounded && jump)
        {
            m_Rigidbody2D.AddForce(m_JumpSpeed * Vector2.up);
            m_IsGrounded = false;
        }

        if (m_IsFacingRight ? move < 0 : move > 0)
        {
            m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
            m_IsFacingRight = !m_IsFacingRight;
        }
    }

    private void FixedUpdate()
    {
        m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, m_TargetVelocity, ref m_Velocity, 
            1 - (m_IsGrounded ? m_Traction : m_AirTraction));
        
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (m_Collider2D.bounds.extents.y * Vector3.down),
            Vector3.right + (m_GroundCheckHeight * Vector3.up), 0);

        m_IsGrounded = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                m_IsGrounded = true;
            }
        }
    }
}
