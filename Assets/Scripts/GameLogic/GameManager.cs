using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterController2D> players;
    [SerializeField]
    private List<CharacterController2D> finishOrder;

    [SerializeField]
    private GameObject placeablesRoot; // the root object which is to parent all placeables in the scene

    private List<Placeable> placedPlaceables;
    private Dictionary<Placeable, List<Vector3>> placeableGamePositionsDic; // Keys: each placeable currently placed;
                                                                            // Values: the game positions of the squares they
                                                                            //         take up

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

    // Attempts to place the given placeable with its bottom-left square at the originPosition given in game coordinates.
    // Returns the success of this action.
    public bool TryPlace(Placeable placeable, Vector3 originPosition)
    {
        if (!IsPlacementValid(placeable, originPosition))
        {
            return false;
        }

        GameObject newPlacedGameObject = Instantiate(placeable.gameObject);
        newPlacedGameObject.transform.parent = placeablesRoot.transform;
        newPlacedGameObject.transform.position = placeable.GetCenterInWorldCoordinates();
        placeable.SetOriginPosition(originPosition);
        placedPlaceables.Add(placeable);
        placeableGamePositionsDic.Add(placeable, placeable.GetSpaceTakenGameCoordinates());s
        return true;
    }

    // takes in a placeable and the origin position (bottom left square), in game coordinates, that
    // we wish it to be placed at, and returns true if that placement is valid i.e. there are no other placeables currently placed
    // obstructing the placement
    public bool IsPlacementValid(Placeable placeable, Vector3 originPosition)
    {
        List<Vector3> newPlaceablePositions = placeable.GetSpaceTakenGameCoordinates();
        foreach (List<Vector3> takenPositionList in placeableGamePositionsDic.Values)
        {
            foreach (Vector3 takenPosition in takenPositionList)
            {
                if (newPlaceablePositions.Contains(takenPosition))
                {
                    return false;
                }
            }
        }
        return true;
    }

    // returns the world position (i.e. center) of the tile that contains rawPosition
    public static Vector3 SnapToWorldPosition(Vector3 rawPosition)
    {
        Vector3 snappedPosition = new Vector3((float) Math.Floor(rawPosition.x) + 0.5f, (float)Math.Floor(rawPosition.x) + 0.5f, 0);
        return snappedPosition;
    }

    // returns the game position (i.e. bottom-left) of the tile that contains rawPosition
    public static Vector3 SnapToGamePosition(Vector3 rawPosition)
    {
        Vector3 snappedPosition = new Vector3((float)Math.Floor(rawPosition.x), (float)Math.Floor(rawPosition.x), 0);
        return snappedPosition;
    }
}
