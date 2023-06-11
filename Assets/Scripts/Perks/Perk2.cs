using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk2 : Perk
{
    // Start is called before the first frame update
    void Start()
    {
        title = "Perk2";
        GameManager.Instance.AddPerk(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
