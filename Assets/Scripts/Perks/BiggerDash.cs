using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerDash : Perk
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        title = "Bigger Dash";
    }

    public override void ApplyEffect(Player player)
    {
        player.dashDuration = player.dashDuration * 1.3f;
        Debug.Log("BiggerDash");
    }
}
