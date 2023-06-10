using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int maxHealth;

    public int PlayerID { get; set; }

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
    }

    public void OnHit(Hit hit)
    {
        currentHealth = Mathf.Clamp(currentHealth - hit.damage, 0, maxHealth);
    }
}
