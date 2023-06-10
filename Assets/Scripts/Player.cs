using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int maxHealth;

    public int PlayerID { get; set; }

    // [Components]
    private PlayerController mPlayerController;

    private int currentHealth;

    private void Awake()
    {
        mPlayerController = GetComponent<PlayerController>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        mPlayerController.Move(Input.GetAxis("Horizontal"), Input.GetButtonDown("Jump"), Input.GetButtonDown("Fire1"));
    }

    public void OnHit(Hit hit)
    {
        currentHealth = Mathf.Clamp(currentHealth - hit.damage, 0, maxHealth);
    }
}
