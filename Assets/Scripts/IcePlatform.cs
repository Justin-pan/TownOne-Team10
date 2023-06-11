using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatform : MonoBehaviour
{

    [SerializeField]
    private float new_traction = 0f;

    private float old_traction = 0.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            old_traction = player.traction;

            player.traction = new_traction;
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            player.traction = old_traction;
        }
    }
}
