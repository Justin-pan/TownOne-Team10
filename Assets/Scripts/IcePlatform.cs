using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatform : MonoBehaviour
{

    [SerializeField]
    private float m_IceFriction = 0.1f;

    private float m_OldFriction = 0.0f;

    /* example code
    private void OnCollisionEnter2d(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CharacterController2D"))
        {
            Rigidbody2D playerBody = collision.gameObject.GetComponent<Rigidbody2D>();
            m_OldFriction = playerBody.sharedMaterial.friction;

            playerBody.sharedMaterial.friction = m_IceFriction;
        }
    }

    private void OnCollisionExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CharacterController2D"))
        {
            Rigidbody2D playerBody = collision.gameObject.GetComponent<Rigidbody2D>();

            playerBody.sharedMaterial.friction = m_OldFriction;
        }

    }
    */
}
