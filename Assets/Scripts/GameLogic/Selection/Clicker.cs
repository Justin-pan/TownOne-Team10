using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{

    private Perk displayPerk;

    [SerializeField]
    private Sprite SlowFallSprite;
    [SerializeField]
    private Sprite ExtraPointsSprite;
    [SerializeField]
    private Sprite BiggerDashSprite;

    private void OnMouseDown()
    {
        Player player = GameManager.Instance.FinishOrder[0];
        Debug.Log("Player " + player.PlayerID + " is picking a perk");
        player.AddPerk(displayPerk);
        GameManager.Instance.FinishOrder.Remove(player);
        Destroy(gameObject);
        if (GameManager.Instance.FinishOrder.Count == 0 && GameManager.Instance.GameState == GameState.PERK)
        {
            GameManager.Instance.GameState = GameState.BUILDING;
            GameManager.Instance.StartBuilding();
        }

    }

    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.gray);
    }


    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
    }

    public void DisplayImage()
    {
        string perkName = displayPerk.title;
        switch (perkName)
        {
            case "Extra points":
                GetComponent<SpriteRenderer>().sprite = ExtraPointsSprite;
                break;
            case "Bigger Dash":
                GetComponent<SpriteRenderer>().sprite = BiggerDashSprite;
                break;
            case "Slow Fall":
                GetComponent<SpriteRenderer>().sprite = SlowFallSprite;
                break;
            
        }
    }

    public Perk DisplayPerk
    {
        set => displayPerk = value;
        get => displayPerk;
    }

}
