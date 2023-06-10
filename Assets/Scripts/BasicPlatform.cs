using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicPlatform : MonoBehaviour
{
    [SerializeField]
    protected Vector2 m_Position = Vector2.zero;
    [SerializeField]
    protected Vector2 m_Size = Vector2.zero;
    [SerializeField]
    protected float m_RotationAngle = 0.0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
