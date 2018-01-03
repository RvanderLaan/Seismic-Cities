using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragButton : MonoBehaviour
{
    public GameObject prefab, previewPrefab, placementContainer;
    public Sprite image;
    public float camScrollBorderSize = 64;
    public float scrollSpeed = 64;
    public bool onlyPlaceInZones = false;
    public int currentCount;
    public GameObject undoButtonPrefab;

    private Text text;
    private LayerMask terrainLayer, placementLayer;
    private GameObject preview;
    private Vector3 mouseDownPos;
    private CamMovement camMovement;

    private BuildingZone lastBuildingZone;

    // Use this for initialization
    void Start()
    {
        Button button = GetComponentInChildren<Button>();
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => StartPlacement());
        trigger.triggers.Add(pointerDown);

        Button b = button.GetComponentInChildren<Button>();
        b.GetComponent<Image>().sprite = image;
        text = GetComponentInChildren<Text>();

        terrainLayer = LayerMask.GetMask("Terrain");
        placementLayer = LayerMask.GetMask("Placement");

        camMovement = Camera.main.transform.parent.GetComponent<CamMovement>();
    }

    public void Reset(int number)
    {
        if (text == null)
            Start();
        currentCount = number;
        text.text = currentCount + "";
    }

    void StartPlacement()
    {
        if (preview == null && !finishedPlacing())
        {
            preview = Instantiate(previewPrefab, placementContainer.transform);
            // Disable seimograph and disable button
            //preview.transform.GetChild(1).gameObject.SetActive(false);
            //preview.transform.GetChild(2).gameObject.SetActive(false);
            camMovement.detectClicks = false;
        }
    }

    void CancelPlacement()
    {
        Destroy(preview);
        camMovement.detectClicks = true;
    }

    void CompletePlacement(Vector3 worldPos)
    {
        currentCount--;
        text.text = currentCount + "";

        GameObject instance;
        if (onlyPlaceInZones)
        {
            instance = Instantiate(prefab, lastBuildingZone.transform, true);
            instance.tag = "Building";
            lastBuildingZone.place(prefab.GetComponent<Building>());
            instance.transform.position = worldPos;
        }
        else
        {
            instance = Instantiate(prefab, placementContainer.transform);
            instance.transform.position = worldPos;
        }



        // Disable seismogram
        //instance.transform.GetChild(1).gameObject.SetActive(false);

        // Add undo logic
        Vector3 undoPosition = instance.transform.position + new Vector3(0, -1, -1);
        GameObject undoButton = Instantiate(undoButtonPrefab, instance.transform);
        undoButton.transform.position = undoPosition;
        undoButton.GetComponentInChildren<Button>().onClick.AddListener(() => {
            Destroy(instance);
            currentCount += 1;
            text.text = currentCount + "";
        });

        Destroy(preview);
        camMovement.detectClicks = true;
    }

    bool isOnBuildingZone(Vector3 worldPos)
    {
        RaycastHit2D placementHit = Physics2D.Raycast(worldPos, -Vector2.up, float.MaxValue, placementLayer);
        if (placementHit.collider != null) 
        {
            // Only allow placement in building zones
            if (!placementHit.collider.tag.Equals("BuildingPlatform"))
                return false;
            // Only one building can be built for each platform
            lastBuildingZone = placementHit.collider.GetComponent<BuildingZone>();
            if (lastBuildingZone.isBuilt)
                return false;
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (preview != null)
        {
            mouseDownPos = Input.mousePosition;

            // Input mouse position to world space
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            mousePos.y = Screen.height; // Only x pos matters
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            bool allowPlacement = true;

            // Raycast downwards to find the terrain and possibly a placement area
            RaycastHit2D terrainHit = Physics2D.Raycast(worldPos, -Vector2.up, float.MaxValue, terrainLayer);
            if (terrainHit.collider != null)
            {
                // Move preview building to collision location
                Vector2 hitPoint = terrainHit.point;
                preview.transform.position = hitPoint;

                if (onlyPlaceInZones && !isOnBuildingZone(hitPoint))
                    allowPlacement = false;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (allowPlacement && terrainHit.collider != null && !EventSystem.current.IsPointerOverGameObject() && (Input.mousePosition - mouseDownPos).sqrMagnitude < 4)
                    CompletePlacement(terrainHit.point);
                else
                    CancelPlacement();
            }

            // Scroll screen if at border
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (mouseDownPos.x < camScrollBorderSize)
                    camMovement.transform.Translate(scrollSpeed * Vector3.left * Time.deltaTime);
                else if (mouseDownPos.x > Screen.width - camScrollBorderSize)
                    camMovement.transform.Translate(scrollSpeed * Vector3.right * Time.deltaTime);
            }
        }
    }

    public bool finishedPlacing()
    {
        return currentCount == 0;
    }
}
