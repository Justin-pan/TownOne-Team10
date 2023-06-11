using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableClicker : MonoBehaviour
{
    private Placeable displayPlaceable;

    public Placeable DisplayPlaceable
    {
        get => displayPlaceable;
        set 
        {
            displayPlaceable = value;
            Sprite newSprite = displayPlaceable.gameObject.GetComponent<SpriteRenderer>().sprite;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = newSprite;
        }
    }
    private void OnMouseDown()
    {
        Player placingPlayer = GameManager.Instance.PlayerPointOrder.Dequeue();
        Debug.Log("Player " + placingPlayer.PlayerID + " is placing");
        PlacementHelper.Instance.PlacePlaceable(displayPlaceable);
        Destroy(gameObject);

        if (GameManager.Instance.PlayerPointOrder.Count == 0)
        {
            GameManager.Instance.GameState = GameState.CLIMBING;
            UnityEngine.Debug.Log("Moving to climbing");
            GameManager.Instance.StartClimbing();
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
}
