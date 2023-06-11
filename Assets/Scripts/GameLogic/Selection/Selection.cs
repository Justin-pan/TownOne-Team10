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
        UnityEngine.Debug.Log("Finsih");
    }

    private void GenerateSelection()
    {
        p = new HashSet<Perk>();

        System.Random rnd = new System.Random();
        int next = 0;
        UnityEngine.Debug.Log("Perk count is " + perks.Count);
        for (int j = 0; j < players.Count; ++j)
        {
            UnityEngine.Debug.Log("Starting to adding trap setp1");
            next = rnd.Next(perks.Count);
            p.Add(perks[next]);
            perks.Remove(perks[next]);
            UnityEngine.Debug.Log("adding trap" + next);
        }
        UnityEngine.Debug.Log("Finsih generating" + p.Count);
    }
    private void AddToCanvas()
    {

        shift = rectTransform.rect.width / p.Count;
        position = -300f;
        UnityEngine.Debug.Log("getting here");
        
        foreach (Perk perk in p)
        {
            Clicker c = Instantiate(clicker);
            c.transform.SetParent(this.transform);
            c.DisplayPerk = perk;
            //c.transform.localScale = Vector3.one;
            //c.transform.localRotation = Quaternion.Euler(Vector3.zero);
            c.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(position, transform.position.y + offset, 0);
            position += shift;
            UnityEngine.Debug.Log("Creating at " + position);
        }
    }

    public void Update()
    {
        if (GameManager.Instance.FinishOrder.Count == 0 && GameManager.Instance.GameState == GameState.PERK)
        {
            GameManager.Instance.GameState = GameState.BUILDING;
            UnityEngine.Debug.Log("Moving to building");
        }
    }

}