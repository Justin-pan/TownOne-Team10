using UnityEngine;

public class KillPlane : MonoBehaviour
{
    [SerializeField] private float climbRate;

    private void Update()
    {
        transform.position += climbRate * Time.deltaTime * Vector3.up;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            GameManager.Instance.KillPlayer(player);
            

        }
    }
}
