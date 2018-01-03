using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CamMovement : MonoBehaviour {

    // Positioning/mouse stuff
    private Vector3 moveTarget;
    private float moveLength = 1.0f;

    private Vector3 previousMousePosition, dragOrigin, dragOriginWorldPosition;
    private bool movedSinceMouseDown = false;
    public float keyMoveSpeed = 1f;

    public bool detectClicks = true;

    public Vector4 limits = new Vector4(0, 500, -150, 150);

    // Zoom stuff
    public float minScale = 0.1f;
    public float maxScale = 10f;
    public float scrollSpeed = 0.05f, pinchSpeed = 1f;
    // Smooth zoom stuff
    private float zoomTarget;
    public float zoomSmoothness = 0.1f;
    private float zoomVelocity;

    public AnimationCurve autoTransformCurve;

    private Vector3 screenVector;

    // Use this for initialization
    void Start () {
        //previousMousePosition = Input.mousePosition;
        zoomTarget = Camera.main.orthographicSize;
        screenVector = new Vector2(1f / Screen.width, 1f / Screen.width);
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

    private bool pinchZoom()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float scale = Camera.main.orthographicSize + deltaMagnitudeDiff * pinchSpeed;
            zoomTarget = Mathf.Clamp(scale, minScale, maxScale);

            dragOriginWorldPosition = transform.position; // Don't jump to previous position when releasing one finger
            dragOrigin = (touchZero.position + touchOne.position) / 2f;
            return true;
        }
        return false;
    }

    private Vector3 pan()
    {
        Vector3 pos = transform.position;
        // Pan using mouse
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            dragOriginWorldPosition = transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            //Vector3 dMouse = previousMousePosition - Input.mousePosition;
            //dMouse.x *= Camera.main.orthographicSize * 2 * Camera.main.aspect / Screen.width;
            //dMouse.y *= Camera.main.orthographicSize * 2 / Screen.height;
            //pos += dMouse;

            Vector3 diff = (Input.mousePosition - dragOrigin);
            diff.Scale(screenVector);
            diff.Scale(new Vector3(Camera.main.orthographicSize * 2 * Camera.main.aspect, Camera.main.orthographicSize * 4));
            pos = dragOriginWorldPosition - diff;
        }
        else
        {
            // Pan using keys
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                pos.x -= Time.deltaTime * keyMoveSpeed;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                pos.x += Time.deltaTime * keyMoveSpeed;
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                pos.y -= Time.deltaTime * keyMoveSpeed;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                pos.y += Time.deltaTime * keyMoveSpeed;
        }
        pos.x = Mathf.Clamp(pos.x, limits.x, limits.y);
        pos.y = Mathf.Clamp(pos.y, limits.z, limits.w);
        return pos;
    }

    // Update is called once per frame
    void Update () {
        // Reset mouse position after clicking when coming outside of the screen, else it jumps when panning the first time
        //if (Input.GetMouseButtonDown(0) && (previousMousePosition.x < 0 || previousMousePosition.x > Screen.width || previousMousePosition.y < 0 || previousMousePosition.y > Screen.height))
        //    previousMousePosition = Input.mousePosition;

       
        Vector3 pos = transform.position;

        // When not using the GUI
        if (detectClicks && !EventSystem.current.IsPointerOverGameObject()) {
            bool pinchZoomed = pinchZoom();
            if (!pinchZoomed)
                transform.position = pan();
        }
        Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, zoomTarget, 0.3f);
        //previousMousePosition = Input.mousePosition;
    }

    public void moveTo(Vector2 pos)
    {
        moveTarget = new Vector3(pos.x, pos.y, transform.position.z);
        StartCoroutine("moveToRoutine");
    }

    public void zoomTo(float zoom, bool normalize = false)
    {
        if (normalize)
            zoom = zoom * (maxScale - minScale) + minScale;
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
