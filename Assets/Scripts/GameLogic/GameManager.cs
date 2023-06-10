using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<Player> players;

    public List<Player> Players
    {
        get => players;
    }

    [SerializeField]
    private List<Player> finishOrder;

    public List<Player> FinishOrder
    {
        get => finishOrder;
    }

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager(); 
            }
            return instance;
        }
    }

    public void Awake()
    {
        if (instance == null || instance == this)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private GameManager()
    {
        players = new List<Player>();
        finishOrder = new List<Player>();
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public void FinishPlayer(Player player)
    {
        if (!finishOrder.Contains(player))
        {
            Debug.Log("Player " + player.PlayerID + " finished");
            finishOrder.Add(player);
        }

        if (finishOrder.Count == players.Count)
        {
            Debug.Log("All players finished");
        }
    }
}
