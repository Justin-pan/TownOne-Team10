using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
   
    private void OnMouseDown()
    {
        Debug.Log("click");
        // something to add perk to player??
    }

    private void OnMouseOver()
    {
        Debug.Log("hover");
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.blue);
    }

    private void OnMouseExit()
    {
        Debug.Log("hover");
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
    }

}
