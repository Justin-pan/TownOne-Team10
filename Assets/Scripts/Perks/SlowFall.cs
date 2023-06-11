using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFall : Perk
{
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        title = "Slow Fall";
    }

    public override void ApplyEffect(Player player)
    {
        player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<Rigidbody2D>().gravityScale * 0.5f;
        Debug.Log("SlowFall");

    }

    
   
}
