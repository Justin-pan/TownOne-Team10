using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPlatform : MonoBehaviour
{
    [SerializeField]
    private Placeable type;
    private void Start()
    {
        GameManager.Instance.TryPlace(type, GameManager.SnapToGamePosition(transform.position));
        Destroy(gameObject);
    }
}
