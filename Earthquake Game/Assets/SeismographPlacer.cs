using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SeismographPlacer : MonoBehaviour {

    private Text text;

    public GameObject seismogramPrefab, seismogramContainer;

    LayerMask terrainLayer;

    GameObject preview;

    int currentCount;

    Vector3 mouseDownPos;

    CamMovement camMovement;

    public float camScrollBorderSize = 64;
    public float scrollSpeed = 64;

    // Use this for initialization
    void Start () {
        Button button = GetComponentInChildren<Button>();
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => StartPlacement());
        trigger.triggers.Add(pointerDown);

        terrainLayer = LayerMask.GetMask("Terrain");
        text = GetComponentInChildren<Text>();

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
            preview = GameObject.Instantiate(seismogramPrefab, seismogramContainer.transform);
            camMovement.detectClicks = false;
        }
    }

    void CancelPlacement()
    {
        Destroy(preview);
        camMovement.detectClicks = false;
    }

    void CompletePlacement(Vector3 worldPos)
    {
        currentCount--;
        text.text = currentCount + "";

        GameObject instance = GameObject.Instantiate(seismogramPrefab, seismogramContainer.transform);
        instance.transform.position = worldPos;

        Destroy(preview);
        camMovement.detectClicks = true;
    }

	// Update is called once per frame
	void Update () {
		if (preview != null)
        {
             mouseDownPos = Input.mousePosition;

            // Input mouse position to world space
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            mousePos.y = Screen.height; // Only x pos matters
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // Raycast downwards to find the terrain and possibly a placement area
            RaycastHit2D terrainHit = Physics2D.Raycast(worldPos, -Vector2.up, float.MaxValue, terrainLayer);
            if (terrainHit.collider != null)
            {
                // Move preview building to collision location
                Vector2 hitPoint = terrainHit.point;
                preview.transform.position = hitPoint;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (terrainHit.collider != null && !EventSystem.current.IsPointerOverGameObject() && (Input.mousePosition - mouseDownPos).sqrMagnitude < 4)
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
