using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    private const float aspectRatio = 16f / 9f;

    private Bounds bound;

    private float minimumSize = 1f;

    private float bottomMargin = 3f;
    private const float SCALING_SPEED = 3f;
    private const float SHIFT_SPEED = 3f;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        camera.aspect = aspectRatio;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        bound = CreateBound();
        SetBound();
        SetCenter();
    }

    public Bounds CreateBound()
    {
        bound = new Bounds(Vector3.zero, Vector3.zero);

        foreach (CharacterController2D player in GameManager.Instance.Players)
        {
            bound.Encapsulate(player.transform.position);
        }

        return bound;
    }

    public void SetBound()
    {
        Vector3 newSize = bound.size + new Vector3(bottomMargin, bottomMargin, 0);

        float targetBound = Mathf.Max(newSize.x * aspectRatio, newSize.y * aspectRatio, minimumSize);

        camera.orthographicSize =  Mathf.Lerp(camera.orthographicSize, targetBound, SCALING_SPEED * Time.deltaTime);
    }

    public void SetCenter()
    {
        Debug.Log(bound.center);
        Vector3 targetPosition = bound.center + new Vector3(0, 0, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, SHIFT_SPEED * Time.deltaTime);
    }
}
