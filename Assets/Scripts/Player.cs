using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int maxHealth;

    public int PlayerID { get; set; }

    // [Components]
    private Rigidbody2D mRigidbody2D;

    private int currentHealth;

    private void Awake()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
    }

    public void OnHit(Hit hit)
    {
        currentHealth = Mathf.Clamp(currentHealth - hit.Damage, 0, maxHealth);
        mRigidbody2D.AddForce(hit.Knockback, ForceMode2D.Impulse);
    }
}
