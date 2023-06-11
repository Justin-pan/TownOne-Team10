using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{

    public string title;

    protected string description;
    // Start is called before the first frame update
    protected virtual void Start()
    { 
        GameManager.Instance.AddPerk(this);
    }

    public virtual void ApplyEffect(Player player)
    {

    }

    

}
