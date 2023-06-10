using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk3 : Perk
{
    // Start is called before the first frame update
    void Start()
    {
        title = "Perk3";
        GameManager.Instance.AddPerk(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
