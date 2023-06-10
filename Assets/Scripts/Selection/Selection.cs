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
    private float position;
    private float shift;

    [SerializeField]
    private Clicker clicker;
    [SerializeField]
    private Perk nullPerk;
   

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        //perks = gm.Perks
        for (int i = 0; i < 10; i++) {
            perks.Add(nullPerk);
        }
        players = gm.FinishOrder;
        this.GenerateSelection();
        this.AddToCanvas();
    }

    private void GenerateSelection()
    {
         p = new HashSet<Perk>();

        System.Random rnd = new System.Random();
        int next = 0;

        for (int j = 0; j < players.Count; j++)
        {
            next = rnd.Next(perks.Count); 
            while (p.Contains(perks[next]))
            {
            next = rnd.Next(10);
            }
            p.Add(perks[next]);
        }

        Debug.Log(p.Count);
        
    }



    private void AddToCanvas()
    {
        shift = 5f;
        position = 0f;
        foreach (Perk perk in p)
        {
            Clicker c = Instantiate(clicker);
            c.DisplayPerk = perk;
            //c.transform.localScale = Vector3.one;
            //c.transform.localRotation = Quaternion.Euler(Vector3.zero);
            c.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(10, 10, 0);
            position += shift;
        }
    }
   
}