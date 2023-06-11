using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSelection : MonoBehaviour
{
    private GameManager gm = GameManager.Instance;
    private List<Player> players;
    private List<Placeable> placeable;
    private HashSet<Placeable> p;
    private float position = 0f;
    private float shift;


    [SerializeField]
    private PlaceableClicker placeableClicker;
    [SerializeField]
    RectTransform rectTransform;



    public void StartSelection()
    {
        placeable = new List<Placeable>(GameManager.Instance.PlacedPlaceables);
        players = GameManager.Instance.Players;
        this.GenerateSelection();
        this.AddToCanvas();
    }

    private void GenerateSelection()
    {
        p = new HashSet<Placeable>();

        System.Random rnd = new System.Random();
        int next = 0;
        for (int j = 0; j < players.Count; ++j)
        {
            next = rnd.Next(placeable.Count);
            p.Add(placeable[next]);
            placeable.Remove(placeable[next]);
        }
    }
    private void AddToCanvas()
    {

        shift = rectTransform.rect.width / p.Count;
        position = transform.position.x - (p.Count - 1) * shift / 2;

        foreach (Placeable placeable in p)
        {
            PlaceableClicker c = Instantiate(placeableClicker);
            c.transform.SetParent(this.transform);
            c.DisplayPlaceable = placeable;
            c.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(position, 0, 0);
            position += shift;
        }
    }
}
