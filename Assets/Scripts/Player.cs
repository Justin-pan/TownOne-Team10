using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int m_MaxHealth;

    public int PlayerID { get; set; }

    private int m_Health;

    private void Awake()
    {
        m_Health = m_MaxHealth;
        
    }

    private void Start()
    {
        GameManager.Instance.AddPlayer(this);
    }

    private void Update()
    {
        // TODO
    }

    public void OnHit(Hit hit)
    {
        m_Health = Mathf.Clamp(m_Health - hit.damage, 0, m_MaxHealth);
    }
}
