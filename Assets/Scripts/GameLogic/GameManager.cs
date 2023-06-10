using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterController2D> players;
    [SerializeField]
    private List<CharacterController2D> finishOrder;

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
        players = new List<CharacterController2D>();
        finishOrder = new List<CharacterController2D>();
    }

    public void AddPlayer(CharacterController2D player)
    {
        players.Add(player);
    }

    public void FinishPlayer(CharacterController2D player)
    {
        finishOrder.Add(player);
    }
}
