using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraPoints : Perk
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        title = "Extra points";
    }

    public override void ApplyEffect(Player player)
    {
        //player.AddPoints(200); no point implementation yet
        Debug.Log("ExtraPoints");
    }

}
