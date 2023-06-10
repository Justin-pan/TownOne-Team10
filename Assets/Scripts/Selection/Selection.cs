using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Selection : MonoBehaviour
{
    private GameManager gm;
    private List<Player> players;
    private List<Perk> perks;
    private HashSet<Perk> p;
    private Transform position;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        players = gm.FinishOrder;
    }

    private void GenerateSelection()
    {
         p = new HashSet<Perk>();

        System.Random rnd = new System.Random();
        int next = 0;

        for (int j = 0; j < players.Count; j++)
        {
            next = rnd.Next(p.Count); 
            while (p.Contains(perks[next]))
            {
            next = rnd.Next(10);
            }
            p.Add(perks[next]);
        }
        
    }



    private void AddToCanvas()
    {
        
        foreach (Perk perk in p)
        {
            //Instantiate(new Clicker(), position);
        }
    }
   
}