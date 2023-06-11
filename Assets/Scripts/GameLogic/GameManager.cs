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

    private bool roundFinished = false;

    private GameState gameState = GameState.CLIMBING;

    [SerializeField]
    private List<Player> players;

    [SerializeField]
    private List<Perk> perks;

    [SerializeField]
    private Selection selection;

    [SerializeField]
    private PlaceableSelection placeableSelection;

    private Dictionary<Player, int> points;

    private Queue<Player> playerPointOrder;

    [SerializeField]
    private List<Player> finishOrder;

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

    public void FinishPlayer(Player player)
    {
        if (!finishOrder.Contains(player) && gameState == GameState.CLIMBING)
        {
            Debug.Log("Player " + player.PlayerID + " finished");
            finishOrder.Add(player);
        }

        if (finishOrder.Count == players.Count && !roundFinished)
        {
            foreach (Player p in players)
            {
                points[p] += (int) p.PlayerMaxHeight;
                
            }

            List<KeyValuePair<Player, int>> sortedList = points.OrderByDescending(x => x.Value).ToList();

            
            foreach (KeyValuePair<Player, int> pair in sortedList)
            {
                playerPointOrder.Enqueue(pair.Key);
            }

            gameState = GameState.PERK;
            selection.StartSelection();
            roundFinished = true;
        }
    }

    public void StartBuilding()
    {
        placeableSelection.StartSelection();
    }

    public void StartClimbing()
    {

    }

    // Attempts to place the given placeable with its bottom-left square at the originPosition given in game coordinates
    // (rawOriginPosition after snapping). Returns the success of this action.
    public bool TryPlace(Placeable placeable, Vector3 rawOriginPosition)
    {
        Vector3 originPosition = SnapToGamePosition(rawOriginPosition);

        if (!placeable.IsPlacementValid(originPosition, placedPlaceables, gamePositionPlaceableDic))
        {
            return false;
        }

        GameObject newPlacedGameObject = Instantiate(placeable.gameObject);
        Placeable newPlacedPlaceable = newPlacedGameObject.GetComponent<Placeable>();

        newPlacedPlaceable.SetOriginPosition(originPosition);
        newPlacedGameObject.transform.parent = placeablesRoot.transform;
        newPlacedGameObject.transform.position = newPlacedPlaceable.GetCenterInWorldCoordinates();
        placedPlaceables.Add(newPlacedPlaceable);
        foreach (Vector3 pos in newPlacedPlaceable.GetSpaceTakenGameCoordinates())
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

    public List<Placeable> GetPlacedPlaceables()
    {
        return placedPlaceables;
    }

    public Dictionary<Vector3, Placeable> GetGamePositionPlaceableDic()
    {
        return gamePositionPlaceableDic;
    }


    public GameState GameState
    {
        get => gameState;
        set => gameState = value;
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

    // EO Getters and Setters ===========================
}
