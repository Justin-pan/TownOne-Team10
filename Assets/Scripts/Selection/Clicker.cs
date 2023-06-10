using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{

    private Perk perk;

    private void OnMouseDown()
    {
        Debug.Log("click");
        //AddPerkToPlayer(perk);
    }

    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.gray);
    }

    
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
    }

    

}
