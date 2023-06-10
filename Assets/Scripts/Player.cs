using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int maxHealth;

    public int PlayerID { get; set; }

    private int currentHealth;

    private List<Perk> perks;

    private void Awake()
    {
        currentHealth = maxHealth;
        perks = new List<Perk>();
    }

    private void Start()
    {
        // TODO
        GameManager.Instance.AddPlayer(this);

    }

    public void OnHit(Hit hit)
    {
        currentHealth = Mathf.Clamp(currentHealth - hit.damage, 0, maxHealth);
    }

    public void AddPerk(Perk p)
    {
        perks.Add(p);
    }

}
