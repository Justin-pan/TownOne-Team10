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
        PlacementHelper.Instance.PlacePlaceable(displayPlaceable);
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
}
