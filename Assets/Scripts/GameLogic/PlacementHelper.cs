using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementHelper : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public static PlacementHelper Instance { get; private set; }

    private GameObject placementVisualObject; // use this for instantiating the current visual game object
    private GameObject currentPlacementVisualObject = null;
    public bool isPlacing { get; private set; }
    private Placeable currentPlacing = null;

    private void Awake()
    {
        InitializeFields();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    private void Update()
    {
        if (isPlacing)
        {
            UpdateCurrentVisualObject();
            if (Input.GetMouseButton(0))
            {
                HandleMouseDown();
            }
        }
    }

    private void HandleMouseDown()
    {
        if (isPlacing)
        {
            if (currentPlacing.IsPlacementValid(GameManager.SnapToGamePosition(mainCamera.ScreenToWorldPoint(Input.mousePosition)),
            GameManager.Instance.GetPlacedPlaceables(), GameManager.Instance.GetGamePositionPlaceableDic()))  // this is true if the placement at the mouse position
                                                                                                              // (after snapping) is valid
            {
                HandlePlacementSuccess();
            }
            else
            {
                HandlePlacementFailure();
            }
        }
    }


    private void UpdateCurrentVisualObject()
    {
        currentPlacementVisualObject.transform.position = GameManager.SnapToWorldPosition(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        currentPlacementVisualObject.GetComponent<SpriteRenderer>().sprite = currentPlacing.gameObject.GetComponent<SpriteRenderer>().sprite;
        if (currentPlacing.IsPlacementValid(GameManager.SnapToGamePosition(mainCamera.ScreenToWorldPoint(Input.mousePosition)),
            GameManager.Instance.GetPlacedPlaceables(), GameManager.Instance.GetGamePositionPlaceableDic()))  // this is true if the placement at the mouse position
                                                                                                              // (after snapping) is valid
        {
            currentPlacementVisualObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1f, 0.5f, 1f);
        }
        else
        {
            currentPlacementVisualObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 1f);
        }
    }

    private void InitializeFields()
    {
        isPlacing = false;
        placementVisualObject = new GameObject();
        placementVisualObject.AddComponent<SpriteRenderer>();
    }

    public void PlacePlaceable(Placeable placeable)
    {
        if (!isPlacing)
        {
            isPlacing = true;
            currentPlacing = placeable;
            currentPlacementVisualObject = Instantiate<GameObject>(placementVisualObject);
        }
    }
    private void HandlePlacementFailure()
    {
        // TODO: perhaps play a sound here?
        return;
    }

    private void HandlePlacementSuccess()
    {
        if (GameManager.Instance.TryPlace(currentPlacing, mainCamera.ScreenToWorldPoint(Input.mousePosition)))
        {
            isPlacing = false;
            currentPlacementVisualObject = null;
            currentPlacing = null;
        }
        else
        {
            throw new Exception("WHAT THE FUCK WHY DID IT FAIL???");
        }
    }
}
