using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblyPlatform : MonoBehaviour
{
    [SerializeField]
    private float m_CrumbleDelay = 2f;
    [SerializeField]
    private float m_ReappearDelay = 5f;
    private bool PlayerLanded = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player && !PlayerLanded)
        {
            
            PlayerLanded = true;
            Invoke("Crumble", m_CrumbleDelay);
        }
    }

    private void Crumble()
    {
        gameObject.SetActive(false);
        Invoke("Reappear", m_ReappearDelay);
    }

    private void Reappear()
    {
        gameObject.SetActive(true);
    }
}
