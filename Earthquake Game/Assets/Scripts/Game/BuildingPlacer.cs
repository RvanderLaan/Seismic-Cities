using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour {

    public bool preview = false;
    private GameObject previewPrefab;
    private GameObject previewInstance;
    private SpriteRenderer previewSR;
    private Vector3 size;
    public GameObject buildingContainer;

    public Color goodColor, badColor;

    private Vector3 clickPosition;

    public AudioClip select, place;
    private AudioSource audioSource;

    private BudgetManager budgetManager;

    public LayerMask placementMask;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        budgetManager = GetComponent<BudgetManager>();
	}

    public bool mouseMoved() {
        return (clickPosition - Input.mousePosition).sqrMagnitude >= 4;
    }

    public void startPreview(GameObject prefab) {
        GameObject.Destroy(previewInstance);
        previewPrefab = prefab;
        preview = true;
        previewInstance = GameObject.Instantiate(previewPrefab);
        Collider2D c2d = previewInstance.GetComponent<Collider2D>();
        size = c2d.bounds.size;
        c2d.enabled = false;
        previewInstance.GetComponent<Rigidbody2D>().isKinematic = true;
        previewSR = previewInstance.GetComponent<SpriteRenderer>();
        previewSR.color = goodColor;
        previewInstance.tag = "Untagged";

        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.clip = select;
        audioSource.Play();
    }

    public void stopPreview() {
        preview = false;
        GameObject.Destroy(previewInstance);
    }
	
	// Update is called once per frame
	void Update () {
        // On hover, show tooltip with building properties (maybe in separate script)

        // When not using the GUI
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0))
                clickPosition = Input.mousePosition;
            if (Input.GetMouseButtonUp(1))
                stopPreview();
        }

        if (preview) {
            // Input mouse position to world space
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            mousePos.y = Screen.height; // Only x pos matters
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // Raycast downwards to 2d collider
            RaycastHit2D hit = Physics2D.Raycast(worldPos, -Vector2.up, float.MaxValue, placementMask);
            bool allowPlacement = false;
            if (hit.collider != null) {
                // Move preview building to collision location
                Vector2 hitPoint = hit.point;
                hitPoint.y += size.y / 2;
                previewInstance.transform.position = hitPoint;

                // Check if placement is allowed
                allowPlacement = checkAllowPlacement(hit);
                
                // Place prefab when release
                if (!mouseMoved() && Input.GetMouseButtonUp(0)) {
                    if (allowPlacement)
                        placeBuilding(hitPoint);
                }
            }

            // Change preview color
            if (allowPlacement)
                previewSR.color = goodColor;
            else
                previewSR.color = badColor;
        }
        
    }

    void placeBuilding(Vector2 pos) {
        GameObject instance = GameObject.Instantiate(previewPrefab, buildingContainer.transform);
        instance.transform.position = pos;

        audioSource.pitch = Random.Range(0.5f, 1.5f);
        audioSource.clip = place;
        audioSource.Play();

        // TODO: GetComponent is slow, only call when building changes
        BuildingInfo previewInfo = previewPrefab.GetComponent<BuildingInfo>();
        budgetManager.newBuilding(previewInfo.cost, previewInfo.population);
    }

    bool checkAllowPlacement(RaycastHit2D hit) {
        // Only allow angle between -10 and 10 degrees steep
        float angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;
        if (!(angle > 80 && angle < 100))
            return false;

        
        // Check budget
        // TODO: GetComponent is slow, only call when building changes
        BuildingInfo previewInfo = previewPrefab.GetComponent<BuildingInfo>();
        if (!budgetManager.enoughMoney(previewInfo.cost)) {
            budgetManager.notEnoughMoneyMessage();
            return false;
        }

        return true;
    }
}
