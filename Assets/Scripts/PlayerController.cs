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

    private Vector3 m_Velocity;
    private bool m_IsGrounded;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();

        m_Velocity = Vector3.zero;
        m_IsGrounded = false;
    }

    private void FixedUpdate()
    {
        m_IsGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (m_Collider2D.bounds.extents.y * Vector3.down),
            Vector3.right + (m_GroundCheckHeight * Vector3.up), 0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                m_IsGrounded = true;
            }
        }

        Move(Input.GetAxis("Horizontal"), Input.GetButtonDown("Jump"));
    }

    private void Move(float move, bool jump)
    {
        Vector3 targetVelocity = (move * m_MoveSpeed * Vector3.right) + (m_Rigidbody2D.velocity.y * Vector3.up);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity,
            1 - (m_IsGrounded ? m_Traction : m_AirTraction));

        m_SpriteRenderer.flipX = m_Rigidbody2D.velocity.x > 0;

        if (m_IsGrounded && jump)
        {
            m_Rigidbody2D.AddForce(m_JumpSpeed * Vector2.up);
            m_IsGrounded = false;
        }
    }
}
