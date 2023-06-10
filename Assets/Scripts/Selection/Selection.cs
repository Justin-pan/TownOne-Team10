using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class Selection : MonoBehaviour
{
    private GameManager gm;
    private List<Player> players;
    private List<Perk> perks;
    private HashSet<Perk> p;
    private float position;
    private float shift;
    private Vector3 canvasPos;
    

    [SerializeField]
    private Clicker clicker;
    [SerializeField]
    private Transform transform;
   
   

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        perks = gm.Perks;
        players = gm.Players;
        this.GenerateSelection();
        this.AddToCanvas();
        UnityEngine.Debug.Log("Finsih");
    }

    private void GenerateSelection()
    {
        p = new HashSet<Perk>();

        System.Random rnd = new System.Random();
        int next = 0;
        UnityEngine.Debug.Log("Starting to adding trap" + players.Count);
        for (int j = 0; j < players.Count; j++)
        {
            UnityEngine.Debug.Log("Starting to adding trap setp 1");
            next = rnd.Next(perks.Count); 
            while (p.Contains(perks[next]))
            {
                UnityEngine.Debug.Log("Starting to adding trap whileloop");
                next = rnd.Next(perks.Count);
            }
            p.Add(perks[next]);
            UnityEngine.Debug.Log("adding trap");
        }
        UnityEngine.Debug.Log("Finsih generating" + p.Count);
    }



    private void AddToCanvas()
    {
        shift = 5f;
        position = 0f;
        UnityEngine.Debug.Log("getting here");
        
        foreach (Perk perk in p)
        {
            Clicker c = Instantiate(clicker);
            c.DisplayPerk = perk;
            //c.transform.localScale = Vector3.one;
            //c.transform.localRotation = Quaternion.Euler(Vector3.zero);
            c.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(position, transform.position.y, 0);
            position += shift;
            UnityEngine.Debug.Log("Creating at " + position);
        }
    }
   
}