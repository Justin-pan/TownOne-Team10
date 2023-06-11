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
            Debug.Log("Killed player " + player.PlayerID);
            Debug.Log(GameManager.Instance.WinningPlayers.Count);
            
            Debug.Log(GameManager.Instance.DeadPlayers.Count);
            Debug.Log(GameManager.Instance.FinishOrder.Count);

        }
    }
}
