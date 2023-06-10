using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    private GameManager gm;
    private List<Player> players;
    //private List<Perk> perks; perks dont exist yet

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        players = gm.FinishOrder;
    }

    private void GenerateSelection()
    {
        //private HashSet<Perk> p;

        System.Random rnd = new System.Random();
        int next = 0;
        for (int j = 0; j < players.Count; j++)
        {
            //next = rnd.Next(10); //change the 10 to (total # of perks - 1).
            //while (p.Contains(perks[next])) {
            //next = rnd.Next(10);
            //}

            //p.Add(perks[next]);
        }

        //foreach(Perk perk: p){

        //}
        

    }

}