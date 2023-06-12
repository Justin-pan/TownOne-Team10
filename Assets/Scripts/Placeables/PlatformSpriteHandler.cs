using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlatformSpriteHandler : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite lowSprite;
    [SerializeField] private Sprite midSprite;
    [SerializeField] private Sprite highSprite;

    [Header("Heights")]
    [SerializeField] private int low;
    [SerializeField] private int mid;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        int snappedHeight = (int) GameManager.SnapToGamePosition(transform.position).y;
        if (snappedHeight <= low)
        {
            spriteRenderer.sprite = lowSprite;
        } else if (low < snappedHeight && snappedHeight <= mid)
        {
            spriteRenderer.sprite = midSprite;
        } else
        {
            spriteRenderer.sprite = highSprite;
        }
    }

}
