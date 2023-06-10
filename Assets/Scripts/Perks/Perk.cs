using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    protected string title;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AddPerk(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
