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
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController playerBody = collision.gameObject.GetComponent<PlayerController>();
            old_traction = playerBody.traction;

            playerBody.traction = new_traction;
            Debug.Log("set to " + old_traction);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController playerBody = collision.gameObject.GetComponent<PlayerController>();
            playerBody.traction = old_traction;
            Debug.Log("set to " + old_traction);
        }

    }
}
