using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    private const float aspectRatio = 16f / 9f;

    private Bounds bound;

    private float minimumSize = 4f;
    private float maximumSize = 6f;

    private float bottomMargin = 2f;
    private float sideMargin = 1f;
    private const float SCALING_SPEED = 3f;
    private const float SHIFT_SPEED = 3f;
    private Vector3 boundOffset = new Vector3(0, 0f, 0);

    private float Y_SCALE_CONST = 1.1f;


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
        Vector3 newSize = new Vector3(bound.size.x, bound.size.y + bottomMargin, 0);

        float xScaling = newSize.x * Screen.height / Screen.width;
        float yScaling = newSize.y * Screen.height / Screen.width * Y_SCALE_CONST;
        float targetBound = Mathf.Max(xScaling, yScaling, minimumSize);
        Debug.Log(targetBound);
        if (xScaling < yScaling)
        {
            boundOffset = new Vector3(0, + yScaling / 5, 0);
        }
        else
        {
            boundOffset = new Vector3(0, + (xScaling / 3), 0);
        }
        camera.orthographicSize =  Mathf.Clamp(Mathf.Lerp(camera.orthographicSize, targetBound, SCALING_SPEED * Time.deltaTime), minimumSize, maximumSize);
    }

    public void SetCenter()
    {
        Vector3 targetPosition = bound.center + boundOffset + new Vector3(0, 0, transform.position.z);
        
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPosition.x, SHIFT_SPEED * Time.deltaTime), Mathf.Lerp(transform.position.y, targetPosition.y, SHIFT_SPEED * Time.deltaTime),
            Mathf.Lerp(transform.position.z, targetPosition.z, SHIFT_SPEED * Time.deltaTime));
    }
}