using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static readonly int GAME_WIDTH = 8; 
    public static readonly int GAME_HEIGHT = 20; // the width and height of the region in which placeables can be placed, in game units

    public const int WINNING_SCORE = 15;


    private GameState gameState = GameState.CLIMBING;

    [SerializeField]
    private List<Player> players;

    [SerializeField]
    private List<Perk> perks;

    [SerializeField]
    private Selection selection;

    [SerializeField]
    private PlaceableSelection placeableSelection;

    [SerializeField]
    private SpawnPoint spawnPoint;

    private Dictionary<Player, int> points;

    private Queue<Player> playerPointOrder; //INVARIANT: Only Contains elements during Trap Drafting Phase

    [SerializeField]
    private List<Player> finishOrder; //INVARIANT: Only Contains elements during Perk Phase

    private Queue<Player> winningPlayers; //INVARIANT: Only Contains elements during Climbing Phase

    private Stack<Player> deadPlayers; //INVARIANT: Only Contains elements during Climbing Phase

    [SerializeField]
    private GameObject placeablesRoot; // the root object which is to parent all placeables in the scene
    
    [SerializeField]
    private List<Placeable> placedPlaceables;

    private Dictionary<Vector3, Placeable> gamePositionPlaceableDic;
                                                                      // Keys: positions at which a placeable exists
                                                                      // Values: the placeable at that location

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
        winningPlayers = new Queue<Player>();
        deadPlayers = new Stack<Player>();
        placedPlaceables = new List<Placeable>();
        playerPointOrder = new Queue<Player>();
        points = new Dictionary<Player, int>();
        gamePositionPlaceableDic = new Dictionary<Vector3, Placeable>();
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
        Points.Add(player, 0);
    }

    public void AddPerk(Perk perk)
    {
        perks.Add(perk);
    }

    public void KillPlayer(Player player)
    {
        if (!winningPlayers.Contains(player) && !deadPlayers.Contains(player))
        {
            deadPlayers.Push(player);
        }
    }

    public void FinishPlayer(Player player)
    {
        if (gameState == GameState.CLIMBING)
        {
            Debug.Log("Player " + player.PlayerID + " finished");

            if (!winningPlayers.Contains(player) && !deadPlayers.Contains(player))
            {
                winningPlayers.Enqueue(player);
            }
        }

        if ((deadPlayers.Count + winningPlayers.Count) == players.Count && gameState == GameState.CLIMBING)
        {
            GameState = GameState.POINTS;
            AssignPoints();
            CalculatePlayerOrder();

            GameState = GameState.PERK;
            selection.StartSelection();
        }
    }

    private void CalculatePlayerOrder()
    {
        List<KeyValuePair<Player, int>> sortedList = points.OrderByDescending(x => x.Value).ToList();


        foreach (KeyValuePair<Player, int> pair in sortedList)
        {
            playerPointOrder.Enqueue(pair.Key);
        }
    }

    private void AssignPoints()
    {
        while (winningPlayers.Count != 0)
        {
            Player p = winningPlayers.Dequeue();
            finishOrder.Add(p);
            points[p] += WINNING_SCORE;
        }

        while (deadPlayers.Count != 0)
        {
            finishOrder.Add(deadPlayers.Pop());
        }

        foreach (Player p in players)
        {
            points[p] += (int)p.PlayerMaxHeight; //Tentative Scoring System

        }
    }

    public void StartBuilding()
    {
        placeableSelection.StartSelection();
    }

    public void StartClimbing()
    {
        foreach (Player p in players)
        {
            p.ResetPlayer();
        }

        spawnPoint.RespawnPlayers();
    }

    // Attempts to place the given placeable with its bottom-left square at the originPosition given in game coordinates
    // (rawOriginPosition after snapping). Returns the success of this action.
    public bool TryPlace(Placeable placeable, Vector3 rawOriginPosition)
    {
        Vector3 originPosition = SnapToGamePosition(rawOriginPosition);

        if (!placeable.IsPlacementValid(originPosition, gamePositionPlaceableDic))
        {
            return false;
        }

        GameObject newPlacedGameObject = Instantiate(placeable.gameObject);
        Placeable newPlacedPlaceable = newPlacedGameObject.GetComponent<Placeable>();

        newPlacedPlaceable.SetOriginPosition(originPosition);
        newPlacedGameObject.transform.parent = placeablesRoot.transform;
        newPlacedGameObject.transform.position = newPlacedPlaceable.GetCenterInWorldCoordinates();
        placedPlaceables.Add(newPlacedPlaceable);
        foreach (Vector3 pos in newPlacedPlaceable.GetSpaceTakenGameCoordinates(originPosition))
        {
            gamePositionPlaceableDic.Add(pos, newPlacedPlaceable);
        }
        return true;
    }

    // returns the world position (i.e. center) of the tile that contains rawPosition
    public static Vector3 SnapToWorldPosition(Vector3 rawPosition)
    {
        Vector3 snappedPosition = new Vector3((float) Math.Floor(rawPosition.x) + 0.5f, (float)Math.Floor(rawPosition.y) + 0.5f, 0);
        return snappedPosition;
    }

    // returns the game position (i.e. bottom-left) of the tile that contains rawPosition
    public static Vector3 SnapToGamePosition(Vector3 rawPosition)
    {
        Vector3 snappedPosition = new Vector3((float)Math.Floor(rawPosition.x), (float)Math.Floor(rawPosition.y), 0);
        return snappedPosition;
    }

    // Getters and Setters ==============================

    public Dictionary<Vector3, Placeable> GetGamePositionPlaceableDic()
    {
        return gamePositionPlaceableDic;
    }


    public GameState GameState
    {
        get => gameState;
        set 
        {
            Debug.Log("Switching to " + value + " from " + gameState);
            gameState = value;
        } 
    }

    public List<Perk> Perks
    {
        get => perks;
    }

    public List<Player> Players
    {
        get => players;
    }

    public Dictionary<Player, int> Points
    {
        get => points;
    }

    public Queue<Player> PlayerPointOrder
    {
        get => playerPointOrder;
    }

    public List<Player> FinishOrder
    {
        get => finishOrder;
    }

    public List<Placeable> PlacedPlaceables
    {
        get => placedPlaceables;
        set => placedPlaceables = value;
    }

    public Stack<Player> DeadPlayers
    {
        get => deadPlayers;
    }

    public Queue<Player> WinningPlayers
    {
        get => winningPlayers;
    }

    // EO Getters and Setters ===========================
}
