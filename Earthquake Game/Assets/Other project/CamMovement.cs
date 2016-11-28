using UnityEngine;
using System.Collections;

public class CamMovement : MonoBehaviour {

    // Positioning/mouse stuff
    private Vector3 previousMousePosition;
    private bool mouseDown = false;
    private Vector3 mouseDownPos = Vector3.zero;
    private bool movedSinceMouseDown = false;
    public float keyMoveSpeed = 1f;

    // Zoom stuff
    public float minScale = 0.1f;
    public float maxScale = 10f;
    public float scrollSpeed = 0.1f;
    // Smooth zoom stuff
    private float zoomTarget;
    public float zoomSmoothness = 0.1f;
    private float zoomVelocity;


    public GameObject target;

    private SeismicWaveEffect effect;

    // Use this for initialization
    void Start () {
        previousMousePosition = Input.mousePosition;
        effect = Camera.main.GetComponent<SeismicWaveEffect>();
        zoomTarget = Camera.main.orthographicSize;
	}

    // Update is called once per frame
    void Update () {
        // Init stuff before rest of update
        if (!mouseDown && Input.GetMouseButtonDown(0)) {
            mouseDown = true;
            mouseDownPos = Input.mousePosition;
            movedSinceMouseDown = false;

            // Reset mouse position after clicking when coming outside of the screen, else it jumps when panning the first time
            if (previousMousePosition.x < 0 || previousMousePosition.x > Screen.width || previousMousePosition.y < 0 || previousMousePosition.y > Screen.height)
                previousMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) {
            mouseDown = false;
        }
        if (mouseDown && !movedSinceMouseDown && (mouseDownPos - Input.mousePosition).sqrMagnitude > 1)
            movedSinceMouseDown = true;


        
        // Zoom
        if (Input.mouseScrollDelta.y != 0) {
            float scale = Camera.main.orthographicSize - Input.mouseScrollDelta.y * scrollSpeed;
            zoomTarget = Mathf.Clamp(scale, minScale, maxScale);
        }
        // Camera.main.orthographicSize = Mathf.Lerp(zoomSource, zoomTarget, dZoom);
        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, zoomTarget, ref zoomVelocity, zoomSmoothness);
        

        // Pan
        Vector3 pos = transform.position;
        if (Input.GetMouseButton(0)) {
            Vector3 dMouse = previousMousePosition - Input.mousePosition;
            dMouse.x *= Camera.main.orthographicSize * 2 * Camera.main.aspect / Screen.width;
            dMouse.y *= Camera.main.orthographicSize * 2 / Screen.height;
            pos += dMouse;
            
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            pos.x -= Time.deltaTime * keyMoveSpeed;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            pos.x += Time.deltaTime * keyMoveSpeed;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            pos.y -= Time.deltaTime * keyMoveSpeed;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            pos.y += Time.deltaTime * keyMoveSpeed;
        transform.position = pos;

        // Place marker
        if (Input.GetMouseButtonUp(0) && !movedSinceMouseDown) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            targetPos.z = -0.01f;
            target.transform.position = targetPos;

            effect.reset();
        }
        previousMousePosition = Input.mousePosition;
    }
}
