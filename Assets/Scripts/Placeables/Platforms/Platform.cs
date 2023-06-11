using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Works for all Platforms (can be placed anywhere that isn't intersecting)
public class Platform : Placeable
{
    public override bool IsPlacementValid(Vector3 originPosition, Dictionary<Vector3, Placeable> gamePositionPlaceableDic)
    {
        return IsNotIntersectingOthers(originPosition, gamePositionPlaceableDic) && IsNotOutOfBounds(originPosition);
    }

    protected override void DefineDimensions()
    {
        width = 1;
        height = 1;
    }
}
