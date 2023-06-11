using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnPlatform : Placeable
{
    public override bool IsPlacementValid(Vector3 originPosition, List<Placeable> placedPlaceables, Dictionary<Vector3, Placeable> gamePositionPlaceableDic)
    {
        for (int i = 0; i < width; i++) // this for loop should check whether every block in the bottom row is on top of a platform (blockBelow exact type is platform)
        {
            Placeable blockBelow;
            if (!gamePositionPlaceableDic.TryGetValue(new Vector3(originPosition.x + i, originPosition.y - 1, originPosition.z), out blockBelow))
            {
                return false;
            }

            if (!blockBelow.GetType().Equals(typeof(Platform)))
            {
                return false;
            }
        }

        // if we get here, it should mean every block on the bottom row is on a platform.

        return IsNotIntersectingOthers(originPosition, placedPlaceables, gamePositionPlaceableDic) && IsNotOutOfBounds(originPosition);

    }
}
