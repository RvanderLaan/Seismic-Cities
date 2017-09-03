using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CamMovement : MonoBehaviour {

    // Positioning/mouse stuff
    private Vector3 previousMousePosition;
    private bool movedSinceMouseDown = false;
    public float keyMoveSpeed = 1f;

    public bool detectClicks = true;

    public Vector4 limits = new Vector4(0, 500, -150, 150);

    // Zoom stuff
    public float minScale = 0.1f;
    public float maxScale = 10f;
    public float scrollSpeed = 0.1f;
    // Smooth zoom stuff
    private float zoomTarget;
    public float zoomSmoothness = 0.1f;
    private float zoomVelocity;

    // Use this for initialization
    void Start () {
        previousMousePosition = Input.mousePosition;
        zoomTarget = Camera.main.orthographicSize;
	}

    public void OnGUI() {
        if (!EventSystem.current.IsPointerOverGameObject() && Event.current.type == EventType.ScrollWheel) {
            // do stuff with  Event.current.delta
            // Debug.Log(Event.current.delta);

            float scroll = Event.current.delta.y;
            // Zoom
            if (scroll != 0) {
                float scale = Camera.main.orthographicSize + scroll * scrollSpeed;
                zoomTarget = Mathf.Clamp(scale, minScale, maxScale);
            }
            // Camera.main.orthographicSize = Mathf.Lerp(zoomSource, zoomTarget, dZoom);
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, zoomTarget, ref zoomVelocity, zoomSmoothness);
        }
    }

    // Update is called once per frame
    void Update () {
        // Reset mouse position after clicking when coming outside of the screen, else it jumps when panning the first time
        if (Input.GetMouseButtonDown(0) && (previousMousePosition.x < 0 || previousMousePosition.x > Screen.width || previousMousePosition.y < 0 || previousMousePosition.y > Screen.height))
            previousMousePosition = Input.mousePosition;

       
        Vector3 pos = transform.position;

        // When not using the GUI
        if (detectClicks && !EventSystem.current.IsPointerOverGameObject()) {
            // Pan using mouse
            if (Input.GetMouseButton(0)) {
                Vector3 dMouse = previousMousePosition - Input.mousePosition;
                dMouse.x *= Camera.main.orthographicSize * 2 * Camera.main.aspect / Screen.width;
                dMouse.y *= Camera.main.orthographicSize * 2 / Screen.height;
                pos += dMouse;

            }
        }
       
        // Pan using keys
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            pos.x -= Time.deltaTime * keyMoveSpeed;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            pos.x += Time.deltaTime * keyMoveSpeed;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            pos.y -= Time.deltaTime * keyMoveSpeed;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            pos.y += Time.deltaTime * keyMoveSpeed;
        
        pos.x = Mathf.Clamp(pos.x, limits.x, limits.y);
        pos.y = Mathf.Clamp(pos.y, limits.z, limits.w);

        transform.position = pos;

        previousMousePosition = Input.mousePosition;
    }
}
