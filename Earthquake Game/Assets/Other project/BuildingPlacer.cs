using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
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

        if (Input.GetMouseButtonDown(0))
            clickPosition = Input.mousePosition;
        if (Input.GetMouseButtonUp(1))
            stopPreview();

        if (preview) {
            // Input mouse position to world space
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            mousePos.y = Screen.height; // Only x pos matters
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // Raycast downwards to 2d collider
            RaycastHit2D hit = Physics2D.Raycast(worldPos, -Vector2.up);
            if (hit.collider != null && (hit.collider.gameObject.tag == "Surface" || hit.collider.gameObject.tag == "Border")) {
                // Preview building on collision location
                Vector2 hitPoint = hit.point;
                hitPoint.y += size.y / 2;
                Debug.Log(size);
                previewInstance.transform.position = hitPoint;
                previewSR.color = goodColor;

                // Place prefab when release
                if (!mouseMoved() && Input.GetMouseButtonUp(0)) {
                    GameObject instance = GameObject.Instantiate(previewPrefab, buildingContainer.transform);
                    instance.transform.position = hitPoint;

                    audioSource.pitch = Random.Range(0.5f, 1.5f);
                    audioSource.clip = place;
                    audioSource.Play();
                }

            } else {
                previewSR.color = badColor;
            }
        }
        
    }
}
