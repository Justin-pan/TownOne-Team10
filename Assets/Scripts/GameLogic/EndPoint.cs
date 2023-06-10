using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CharacterController2D player = collider.gameObject.GetComponent<CharacterController2D>();

        if (player != null)
        {
            GameManager.Instance.FinishPlayer(player);
        }
    }
}
