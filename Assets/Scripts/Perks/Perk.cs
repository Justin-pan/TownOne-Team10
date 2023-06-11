using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    protected string title;
    protected string description;
    // Start is called before the first frame update
    protected virtual void Start()
    { 
        GameManager.Instance.AddPerk(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
