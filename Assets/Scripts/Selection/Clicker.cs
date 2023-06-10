using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{

    private Perk displayPerk;

    private void OnMouseDown()
    {
        Player player = GameManager.Instance.FinishOrder[0];
        Debug.Log(player);
        Debug.Log(displayPerk);
        player.AddPerk(displayPerk);
        GameManager.Instance.FinishOrder.Remove(player);
        Destroy(gameObject);

    }

    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.gray);
    }

    
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
    }

    public Perk DisplayPerk
    {
        set => displayPerk = value;
        get => displayPerk;
    }

}
