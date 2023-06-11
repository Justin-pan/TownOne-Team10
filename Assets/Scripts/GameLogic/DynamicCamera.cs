using System;
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
    private const float SCALING_SPEED = 3f;
    private const float SHIFT_SPEED = 3f;
    private Vector3 boundOffset = new Vector3(0, 0f, 0);

    private float Y_SCALE_CONST = 1.1f;

    [Header("Placement/Building Params")]
    private const float MAX_ZOOM = 8f;
    private const float panSpeed = 10f;


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
        GameState currentGameState = GameManager.Instance.GameState;
        if (currentGameState != GameState.BUILDING && currentGameState != GameState.PLACING)
        {
            bound = CreateBound();
            SetBound();
            SetCenter();
        } else 
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, MAX_ZOOM, SCALING_SPEED * Time.deltaTime);
            ControlCamera();
        }
    }

    private void ControlCamera()
    {
        float verticalInput = Input.GetAxis("CameraUp");

        Vector3 translation = new Vector3(0f, verticalInput * panSpeed * Time.deltaTime, 0f);

        transform.Translate(translation);
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
