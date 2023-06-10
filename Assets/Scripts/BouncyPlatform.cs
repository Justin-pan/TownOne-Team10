using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    [SerializeField]
    private float m_InitialBounceForce = 100f;
    [SerializeField]
    private float m_BounceForceDecrease = 10f;
    [SerializeField]
    private float m_BounceResetDelay = 2f;

    private float m_CurrentBounceForce;
    private float m_TimeSinceLastBounce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Rigidbody2D playerBody = collision.gameObject.GetComponent<Rigidbody2D>();

            playerBody.AddForce(new Vector2(0f, m_CurrentBounceForce));
            m_CurrentBounceForce = (m_CurrentBounceForce >= 0) ? m_CurrentBounceForce - m_BounceForceDecrease : m_CurrentBounceForce;
            m_TimeSinceLastBounce = 0f;
        }
    }

    private void start()
    {
        m_CurrentBounceForce = m_InitialBounceForce;
    }

    private void update()
    {
        m_TimeSinceLastBounce += Time.deltaTime;

        if (m_TimeSinceLastBounce >= m_BounceResetDelay)
        {
            m_CurrentBounceForce = m_InitialBounceForce;
        }
    }
}
