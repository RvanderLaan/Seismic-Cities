using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradePlacer : MonoBehaviour {

    public bool preview = false;
    private GameObject previewPrefab;
    private GameObject previewInstance;
    private Text previewAmountText;
    private SpriteRenderer[] previewSRs;

    public GameObject buildingContainer;
    public Color goodColor, badColor;

    private Vector3 clickPosition;

    public AudioClip select, place;
    private AudioSource audioSource;

    public LayerMask placementMask, terrainMask;

    public GameObject controlInfo;

    // public List<BuildingPlatformController> buildingPlatformControllers;

    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public bool mouseMoved() {
        return (clickPosition - Input.mousePosition).sqrMagnitude >= 4;
    }

    public void startPreview(GameObject prefab, Text amountText) {
        controlInfo.SetActive(true);
        previewAmountText = amountText;

        // Check if all buildings of this type have been placed (directly from GUI text)
        int currentAmount = int.Parse(previewAmountText.text);
        if (currentAmount <= 0)
            return;

        // Destroy old preview and create new one
        GameObject.Destroy(previewInstance);
        previewPrefab = prefab;
        preview = true;
        previewInstance = GameObject.Instantiate(previewPrefab);

        // Deactivate collider and rigidbody of the building blocks
        Collider2D[] colliders = previewInstance.GetComponentsInChildren<BoxCollider2D>();
        Rigidbody2D[] rigBodies = previewInstance.GetComponentsInChildren<Rigidbody2D>();
        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = false;
            rigBodies[i].isKinematic = true;
        }

        // Change color
        previewSRs = previewInstance.GetComponentsInChildren<SpriteRenderer>();
        // Do not shoot damage particles to the preview building
        previewInstance.tag = "Untagged";

        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.clip = select;
        audioSource.Play();
    }

    public void stopPreview() {
        controlInfo.SetActive(false);
        preview = false;
        GameObject.Destroy(previewInstance);
    }

    // Update is called once per frame
    void Update() {
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

            // Raycast downwards to find the terrain and possibly a placement area
            RaycastHit2D terrainHit = Physics2D.Raycast(worldPos, -Vector2.up, float.MaxValue, terrainMask);
            if (terrainHit.collider != null) {
                // Move preview building to collision location
                Vector2 hitPoint = terrainHit.point;
                previewInstance.transform.position = hitPoint;
            }

            RaycastHit2D placementHit = Physics2D.Raycast(worldPos, -Vector2.up, float.MaxValue, placementMask);
            bool allowPlacement = false;
            if (placementHit.collider != null) {
                // Check if placement is allowed
                allowPlacement = checkAllowPlacement(placementHit);

                // Place prefab when release
                if (!mouseMoved() && Input.GetMouseButtonUp(0)) {
                    if (allowPlacement)
                        placeUpgrade(previewInstance.transform.position, placementHit);
                }
            }

            // Change preview color
            Color newColor = badColor;
            if (allowPlacement)
                newColor = goodColor;
            foreach (SpriteRenderer sr in previewSRs)
                sr.color = newColor;
        }

    }

    void placeUpgrade(Vector2 pos, RaycastHit2D hit) {
        GameObject instance = GameObject.Instantiate(previewPrefab, buildingContainer.transform);
        instance.tag = "Building";
        BuildingPlatformController bpc = hit.collider.GetComponent<BuildingPlatformController>();

        bpc.placeUpgrade(instance.GetComponent<Upgrade>());

        // Play the audio
        audioSource.pitch = Random.Range(0.5f, 1.5f);
        audioSource.clip = place;
        audioSource.Play();

        // Decrease amount in GUI
        int newAmount = int.Parse(previewAmountText.text) - 1;
        previewAmountText.text = newAmount + "";

        if (newAmount <= 0)
            stopPreview();
    }

    bool checkAllowPlacement(RaycastHit2D hit) {
        // Only allow angle between -10 and 10 degrees steep
        float angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;
        if (!(angle > 80 && angle < 100))
            return false;

        // Only allow placement in building zones
        if (!hit.collider.tag.Equals("BuildingPlatform"))
            return false;

        // Only one building can be built for each platform
        BuildingPlatformController bpc = hit.collider.GetComponent<BuildingPlatformController>();
        if (!bpc.isBuilt || bpc.isUpgraded)
            return false;

        return true;
    }
}
