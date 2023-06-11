using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static readonly int GAME_WIDTH = 20; 
    public static readonly int GAME_HEIGHT = 50; // the width and height of the region in which placeables can be placed, in game units
    public const float POINTS_SCREEN_DELAY = 10f;
    public const int WINNING_SCORE = 15;

    public const int KILL_PLANE_OFFSET = 2;

    [SerializeField]
    private KillPlane killPlane;


    private GameState gameState = GameState.CLIMBING;

    [SerializeField]
    private GameObject pointsBackground;

    [SerializeField]
    private GameObject pointsBar;
    [SerializeField]
    private GameObject pointsCanvas;

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
        pointsCanvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        killPlane.transform.localScale = new Vector3(GAME_WIDTH * 2, GAME_HEIGHT, 1);

        Vector2 position = spawnPoint.transform.position;
        position.y -= GAME_HEIGHT / 2 + KILL_PLANE_OFFSET;

        killPlane.transform.position = position;
        killPlane.enabled = true;
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
            deadPlayers.Push(player); player.gameObject.SetActive(false);
            Debug.Log("Player " + player.PlayerID + " killed (KILL PLAYER)");
        }

        if ((deadPlayers.Count + winningPlayers.Count) == players.Count && gameState == GameState.CLIMBING)
        {
            GameState = GameState.POINTS;
            Vector2 position = spawnPoint.transform.position;
            position.y -= GAME_HEIGHT / 2 + KILL_PLANE_OFFSET;

            killPlane.transform.position = position;
            killPlane.enabled = false;
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
                player.gameObject.SetActive(false);
            }
        }

        if ((deadPlayers.Count + winningPlayers.Count) == players.Count && gameState == GameState.CLIMBING)
        {
            GameState = GameState.POINTS;
            Vector2 position = spawnPoint.transform.position;
            position.y -= GAME_HEIGHT / 2 + KILL_PLANE_OFFSET;

            killPlane.transform.position = position;
            killPlane.enabled = false;
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

    

    public void StartClimbing()
    {
        if (players.Count == 0)
        {
            spawnPoint.SpawnPlayers();
        } 
        else
        {
            foreach (Player p in players)
            {
                p.ResetPlayer();
                p.gameObject.SetActive(true);
            }

            spawnPoint.RespawnPlayers();
            killPlane.enabled = true;
        }
    }


    private void StartPoints()
    {
        pointsCanvas.gameObject.SetActive(true);

        for (int i = 0; i < players.Count; ++i)
        {
            GameObject pb = Instantiate(pointsBar);

            pb.transform.SetParent(pointsCanvas.transform, false);

            pb.GetComponent<RectTransform>().anchoredPosition = CalculateSpawnPosition(i);
            PointsBar pointBarObj = pb.GetComponent<PointsBar>();
            pointBarObj.SetText("Player " + (i + 1));

            AssignPoints();
            int newPoints = points[players[i]];

            Debug.Log(newPoints);

            pointBarObj.UpdatePoints(newPoints);

        }
        CalculatePlayerOrder();

        StartCoroutine(WaitForTime());
    }

    private const int SHIFT = 5;

    private Vector2 CalculateSpawnPosition(int index)
    {
        float canvasWidth = pointsBackground.GetComponent<RectTransform>().rect.width;
        float canvasHeight = pointsBackground.GetComponent<RectTransform>().rect.height;

        // Example: Spacing the point bars evenly vertically
        float xPosition = (index * SHIFT - 5) * (canvasWidth / (players.Count + 1));

        return new Vector2(xPosition, 0f);
    }

    public IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(POINTS_SCREEN_DELAY);
        GameState = GameState.PERK;
    }

    private void StartPlacing()
    {
        throw new NotImplementedException();
    }

    private void StartPerk()
    {
        pointsCanvas.gameObject.SetActive(false);
    }

    

    public void StartBuilding()
    {
        placeableSelection.StartSelection();
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
            switch (gameState)
            {
                case GameState.CLIMBING:
                    StartClimbing();
                    break;
                case GameState.POINTS:
                    StartPoints();
                    break;
                case GameState.PERK:
                    StartPerk();
                    break;
                case GameState.BUILDING:
                    StartBuilding();
                    break;
                case GameState.PLACING:
                    StartPlacing();
                    break;
                default:
                    break;

            }
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
