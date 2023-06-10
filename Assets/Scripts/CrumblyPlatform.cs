using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblyPlatform : MonoBehaviour
{
    private bool PlayerLanded = false;

    /* Example crumble code
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CharacterController2D") && !PlayerLanded)
        {
            PlayerLanded = true;
            Invoke()
        }
    }

    private void Crumble()
    {
        Destroy(gameObject);
    }
    */
}
