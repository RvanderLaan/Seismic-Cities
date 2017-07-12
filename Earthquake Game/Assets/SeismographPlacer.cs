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

	// Use this for initialization
	void Start () {
        GameObject GM = GameObject.Find("_GM");
        currentCount = GM.GetComponent<LevelManager>().getLevelData().seismographAmount;
        text = GetComponentInChildren<Text>();
        text.text = currentCount + "";

        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(StartPlacement);
        terrainLayer = LayerMask.GetMask("Terrain");
    }

    void StartPlacement()
    {
        if (preview == null && !finishedPlacing())
            preview = GameObject.Instantiate(seismogramPrefab, seismogramContainer.transform);
    }

    void CancelPlacement()
    {
        currentCount++;
    }

    void CompletePlacement(Vector3 worldPos)
    {
        currentCount--;
        text.text = currentCount + "";

        GameObject instance = GameObject.Instantiate(seismogramPrefab, seismogramContainer.transform);
        instance.transform.position = worldPos;

        if (finishedPlacing())
            GameObject.Destroy(preview);
        else
            StartPlacement();

    }

	// Update is called once per frame
	void Update () {
		if (preview != null)
        {
            // Move preview to cursor
            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                mouseDownPos = Input.mousePosition;
            }

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

            RaycastHit2D placementHit = Physics2D.Raycast(worldPos, -Vector2.up, float.MaxValue, terrainLayer);
            if (placementHit.collider != null)
            {
                // Place prefab when release
                if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() && (Input.mousePosition - mouseDownPos).sqrMagnitude < 4)
                {
                    CompletePlacement(terrainHit.point);
                }
            }
        }
	}

    public bool finishedPlacing()
    {
        return currentCount == 0;
    }
}
