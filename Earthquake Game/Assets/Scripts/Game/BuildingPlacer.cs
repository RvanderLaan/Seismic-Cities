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
        if (c2d != null)
        {
            size = c2d.bounds.size;
            c2d.enabled = false;
            previewInstance.GetComponent<Rigidbody2D>().isKinematic = true;
        }

        // Deactivate collider and rigidbody of the building blocks. Later there will be only building blocks.
        Collider2D[] colliders = previewInstance.GetComponentsInChildren<BoxCollider2D>();
        Rigidbody2D[] rigBodies = previewInstance.GetComponentsInChildren<Rigidbody2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Debug.Log("a");
            colliders[i].enabled = false;
            rigBodies[i].isKinematic = true;
        }

        //The container of the building doesn't have the sprite renderer, 
        //every single block of the building has one sprite renderer
        previewSR = previewInstance.GetComponent<SpriteRenderer>();
        if (previewSR != null)
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
                        placeBuilding(hitPoint, hit);
                }
            }

            // Change preview color
            //TODO: don't call GetComponent each time, but call only once. Later thare will be only building blocks
            if (allowPlacement)
            {
                if (previewSR != null)
                    previewSR.color = goodColor;
                else
                {
                    SpriteRenderer[] spriteRenderers = previewInstance.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer sr in spriteRenderers)
                        sr.color = goodColor;
                }
            }
            else
            {
                if (previewSR != null)
                    previewSR.color = badColor;
                else
                {
                    SpriteRenderer[] spriteRenderers = previewInstance.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer sr in spriteRenderers)
                        sr.color = badColor;
                }
            }
        }
        
    }

    void placeBuilding(Vector2 pos, RaycastHit2D hit) {
        GameObject instance = GameObject.Instantiate(previewPrefab, buildingContainer.transform);
        instance.transform.position = pos;

        //Set the fixed joints of the children to the Building Platform
        FixedJoint2D[] childrenJoints = instance.GetComponentsInChildren<FixedJoint2D>();
        foreach (FixedJoint2D f in childrenJoints)
        {
            f.connectedBody = hit.rigidbody;
        }

        // Set the platform as unavailable
        hit.collider.GetComponent<BuildingPlatformController>().isBuilt = true;

        // Play the audio
        audioSource.pitch = Random.Range(0.5f, 1.5f);
        audioSource.clip = place;
        audioSource.Play();

        // TODO: GetComponent is slow, only call when building changes
        BuildingInfo previewInfo = previewPrefab.GetComponent<BuildingInfo>();
        // budgetManager.newBuilding(previewInfo.cost, previewInfo.population);
    }

    bool checkAllowPlacement(RaycastHit2D hit) {
        // Only allow angle between -10 and 10 degrees steep
        float angle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;
        if (!(angle > 80 && angle < 100))
            return false;

        // Check budget
        // TODO: GetComponent is slow, only call when building changes
        BuildingInfo previewInfo = previewPrefab.GetComponent<BuildingInfo>();
        //if (!budgetManager.enoughMoney(previewInfo.cost)) {
        //    budgetManager.notEnoughMoneyMessage();
        //    return false;
        //}

        // Only allow placement in building zones
        if (!hit.collider.tag.Equals("BuildingPlatform"))
            return false;

        // Only one building can be built for each platform
        if (hit.collider.GetComponent<BuildingPlatformController>().isBuilt)
            return false;

        return true;
    }
}
