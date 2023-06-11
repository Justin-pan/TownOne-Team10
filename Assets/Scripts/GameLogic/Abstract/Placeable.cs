using System.Collections.Generic;
using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
    protected int width; // the width, in units, that this placeable takes up
    protected int height; // the height, in units, that this placeable takes up
    // NOTE: width and height are used when checking if a square is occupied, for the purposes of placeable placement. They do NOT
    // necessarily have to correspond with the collider of the prefab.

    private Vector3 originPosition; // the bottom-left corner of this placeable, in game coordinates

    // Sets the width and height
    protected abstract void DefineDimensions();

    protected virtual void Awake()
    {
        DefineDimensions();
        originPosition = new Vector3(0f, 0f, 0f);
    }

    // produces a list containing all the positions (squares) that this placeable takes up, in game coordinates
    public virtual List<Vector3> GetSpaceTakenGameCoordinates()
    {
        List<Vector3> returnVal = new List<Vector3>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                returnVal.Add(new Vector3(originPosition.x + x, originPosition.y + y, 0));
            }
        }

        return returnVal;
    }

    // returns the center of this placeable, in world position. Useful for placing the prefab in the world
    public virtual Vector3 GetCenterInWorldCoordinates()
    {
        return new Vector3(originPosition.x + (width / 2f), originPosition.y + (height / 2f));
    }

    // returns true if the placeable can be placed at the originPosition (the bottom-left square of the placeable),
    // given what placeables have already been placed and which position holds which placeable
    public abstract bool IsPlacementValid(Vector3 originPosition,
        Dictionary<Vector3, Placeable> gamePositionPlaceableDic);

    // given what placeables have already been placed and which position holds which placeable, returns true if this
    // placeable won't intersect with any other placeables if placed at the given originPosition (in game coordinates).
    protected bool IsNotIntersectingOthers(Vector3 originPosition,
        Dictionary<Vector3, Placeable> gamePositionPlaceableDic)
    {
        foreach (Vector3 pos in gamePositionPlaceableDic.Keys)
        {
            if (GetSpaceTakenGameCoordinates().Contains(pos))
            {
                return false;
            }
        }

        return true;
    }

    // returns true if this placeable won't be out of bounds if placed at the given originPosition (in game coordinates).
    protected bool IsNotOutOfBounds(Vector3 originPosition)
    {
        return originPosition.x >= 0 && originPosition.x + width <= GameManager.GAME_WIDTH && originPosition.y >= 0 && originPosition.y + height <= GameManager.GAME_HEIGHT;
    }

    //Getters and Setters ==============================================

    public void SetOriginPosition(Vector3 originPosition)
    {
        this.originPosition = originPosition;
    }

    public Vector3 GetOriginPosition()
    {
        return originPosition;
    }

    //EO Getters and Setters ===========================================
}
