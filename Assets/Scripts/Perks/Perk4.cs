using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk4 : Perk
{
    // Start is called before the first frame update
    void Start()
    {
        title = "Perk4";
        GameManager.Instance.AddPerk(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
