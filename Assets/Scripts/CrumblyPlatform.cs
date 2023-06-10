using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblyPlatform : MonoBehaviour
{
    [SerializeField]
    private float m_CrumbleDelay = 2f;
    private bool PlayerLanded = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() && !PlayerLanded)
        {
            PlayerLanded = true;
            Invoke("Crumble", m_CrumbleDelay);
        }
    }

    private void Crumble()
    {
        Destroy(gameObject);
    }
}
