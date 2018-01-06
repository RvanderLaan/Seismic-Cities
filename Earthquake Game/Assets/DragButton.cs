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

    private SpriteRenderer[] previewSRs;
    public Color goodColor, badColor;

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

        text.text = currentCount + "";
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
            previewSRs = preview.GetComponentsInChildren<SpriteRenderer>();
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
            EventManager.TriggerEvent("BuildingPlace");
            instance = Instantiate(prefab, placementContainer.transform, true);
            instance.tag = "Building";
            lastBuildingZone.place(instance.GetComponent<Building>());
            instance.transform.position = worldPos;
        }
        else
        {
            EventManager.TriggerEvent("SeismographPlace");
            instance = Instantiate(prefab, placementContainer.transform);
            instance.transform.position = worldPos;
        }

        /**
         * Todo: 
         */

        // Disable seismogram
        //instance.transform.GetChild(1).gameObject.SetActive(false);

        // Add undo logic
        Vector3 undoPosition = instance.transform.position + new Vector3(0, -5, -3);
        GameObject undoButton = Instantiate(undoButtonPrefab, instance.transform);
        undoButton.transform.position = undoPosition;
        SetGlobalScale(undoButton.transform, Vector3.one * .3f);
        undoButton.GetComponentInChildren<Button>().onClick.AddListener(() => {
            Destroy(instance);
            currentCount += 1;
            text.text = currentCount + "";
            if (lastBuildingZone != null)
                lastBuildingZone.undoPlacement();
        });
        EventManager.StartListening("Mode:Simulation", () => {
            if (undoButton != null) Destroy(undoButton.gameObject);
        });

        Destroy(preview);
        camMovement.detectClicks = true;
    }

    public static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
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

            if (onlyPlaceInZones)
            {
                // Change preview color
                Color newColor = allowPlacement ? goodColor : badColor;
                foreach (SpriteRenderer sr in previewSRs)
                    sr.color = newColor;
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
                Vector3 dir = Vector2.zero;
                if (mouseDownPos.x < camScrollBorderSize)
                    dir = Vector3.left;
                else if (mouseDownPos.x > Screen.width - camScrollBorderSize)
                    dir = Vector3.right;

                camMovement.transform.position = camMovement.clampToLimits(camMovement.transform.position + Time.deltaTime * scrollSpeed * dir);
            }
        }
    }

    public bool finishedPlacing()
    {
        return currentCount == 0;
    }
}
