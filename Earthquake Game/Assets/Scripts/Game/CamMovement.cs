using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CamMovement : MonoBehaviour {

    // Positioning/mouse stuff
    private Vector3 moveTarget;
    private float moveLength = 1.0f;

    private Vector3 previousMousePosition;
    private bool movedSinceMouseDown = false;
    public float keyMoveSpeed = 1f;

    public bool detectClicks = true;

    public Vector4 limits = new Vector4(0, 500, -150, 150);

    // Zoom stuff
    public float minScale = 0.1f;
    public float maxScale = 10f;
    public float scrollSpeed = 0.05f;
    // Smooth zoom stuff
    private float zoomTarget;
    public float zoomSmoothness = 0.1f;
    private float zoomVelocity;

    public AnimationCurve autoTransformCurve;

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
            if (detectClicks && scroll != 0) {
                float scale = Camera.main.orthographicSize + scroll * scrollSpeed;
                zoomTarget = Mathf.Clamp(scale, minScale, maxScale);
            }
            //Camera.main.orthographicSize = Mathf.SmoothStep(zoomSource, zoomTarget, dZoom);
            //Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, zoomTarget, ref zoomVelocity, zoomSmoothness);
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

            Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, zoomTarget, 0.3f);
        }
 
        transform.position = pos;

        previousMousePosition = Input.mousePosition;
    }

    public void moveTo(Vector2 pos)
    {
        moveTarget = new Vector3(pos.x, pos.y, transform.position.z);
        StartCoroutine("moveToRoutine");
    }

    public void zoomTo(float zoom)
    {
        zoomTarget = zoom;
        StartCoroutine("zoomToRoutine");
    }

    IEnumerator moveToRoutine()
    {
        Vector3 moveOrigin = transform.position;
        float moveStart = Time.time;
        Vector3 step = moveTarget - moveOrigin;

        float t = (Time.time - moveStart) / moveLength;
        while (t < 1)
        {

            transform.position = Vector3.Slerp(moveOrigin, moveTarget, t);
            t = (Time.time - moveStart) / moveLength;
            yield return null;
        }
    }

    IEnumerator zoomToRoutine()
    {
        float zoomSource = Camera.main.orthographicSize;
        float start = Time.time;
        float step = zoomTarget - zoomSource;

        float t = (Time.time - start) / moveLength;
        while (t <= 1)
        {
            Camera.main.orthographicSize = zoomSource + step * autoTransformCurve.Evaluate(t); // Mathf.SmoothStep(zoomSource, zoomTarget, t);
            t = (Time.time - start) / moveLength;
            yield return null;
        }
    }
}
