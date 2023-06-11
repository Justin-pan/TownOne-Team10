using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class Selection : MonoBehaviour
{
    private GameManager gm = GameManager.Instance;
    private List<Player> players;
    private List<Perk> perks;
    private HashSet<Perk> p;
    private float position = 0f;
    private float shift;
    private float offset = 0f;
    

    [SerializeField]
    private Clicker clicker;
    [SerializeField]
    private Transform transform;
    [SerializeField]
    RectTransform rectTransform;



    public void StartSelection()
    {

        perks = new List<Perk>(GameManager.Instance.Perks);
        players = GameManager.Instance.Players;
        this.GenerateSelection();
        this.AddToCanvas();
    }

    private void GenerateSelection()
    {
        p = new HashSet<Perk>();

        System.Random rnd = new System.Random();
        int next = 0;
        for (int j = 0; j < players.Count; ++j)
        {
            next = rnd.Next(perks.Count);
            p.Add(perks[next]);
            perks.Remove(perks[next]);
        }
    }
    private void AddToCanvas()
    {

        shift = rectTransform.rect.width / p.Count;
        position = transform.position.x - (p.Count - 1) * shift / 2;
        
        foreach (Perk perk in p)
        {
            Clicker c = Instantiate(clicker);
            c.transform.SetParent(this.transform);
            c.DisplayPerk = perk;
            c.DisplayImage();
            //c.transform.localScale = Vector3.one;
            //c.transform.localRotation = Quaternion.Euler(Vector3.zero);
            c.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(position, transform.position.y + offset, 0);
            position += shift;
        }
    }


}