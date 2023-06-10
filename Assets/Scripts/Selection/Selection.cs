using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    private GameManager gm;
    private List<CharacterController2D> players;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        players = gm.FinishOrder;
    }

    private void GenerateSelection()
    {
        System.Random rnd = new System.Random();

        for (int j = 0; j < players.Count; j++)
        {
            Console.WriteLine(rnd.Next(10));//returns random integers < 10
        }
    }
}
