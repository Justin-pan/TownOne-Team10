using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    private const float aspectRatio = 16f / 9f;

    private Bounds bound;

    private float minimumSize = 1f;

    private float bottomMargin = -2f;
    private float sideMargin = 3f;
    private const float SCALING_SPEED = 3f;
    private const float SHIFT_SPEED = 3f;
    private Vector3 boundOffset = new Vector3(0, 1.5f, 0);

    [SerializeField]
    private new Camera camera;

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
        bound = new Bounds(GameManager.Instance.Players[0].transform.position , Vector3.zero);

        foreach (Player player in GameManager.Instance.Players)
        {
            bound.Encapsulate(player.transform.position);
        }

        return bound;
    }

    public void SetBound()
    {
        Vector3 newSize = new Vector3(sideMargin, bound.size.y + bottomMargin, 0);

        float targetBound = Mathf.Max(newSize.x * aspectRatio, newSize.y * aspectRatio, minimumSize);

        camera.orthographicSize =  Mathf.Lerp(camera.orthographicSize, targetBound, SCALING_SPEED * Time.deltaTime);
    }

    public void SetCenter()
    {
        Vector3 targetPosition = bound.center + boundOffset + new Vector3(0, 0, transform.position.z);
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetPosition.y, SHIFT_SPEED * Time.deltaTime),
            Mathf.Lerp(transform.position.z, targetPosition.z, SHIFT_SPEED * Time.deltaTime));
    }
}
