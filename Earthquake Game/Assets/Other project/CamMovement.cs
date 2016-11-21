using UnityEngine;
using System.Collections;

public class CamMovement : MonoBehaviour {

    private Vector3 previousMousePosition;
    private bool mouseDown = false;
    private Vector3 mouseDownPos = Vector3.zero;
    private bool movedSinceMouseDown = false;

    public GameObject target;

    private SeismicWaveEffect effect;

	// Use this for initialization
	void Start () {
        previousMousePosition = Input.mousePosition;
        effect = Camera.main.GetComponent<SeismicWaveEffect>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!mouseDown && Input.GetMouseButtonDown(0)) {
            mouseDown = true;
            mouseDownPos = Input.mousePosition;
            movedSinceMouseDown = false;
        }
        if (Input.GetMouseButtonUp(0)) {
            mouseDown = false;
        }
        if (mouseDown && !movedSinceMouseDown && (mouseDownPos - Input.mousePosition).sqrMagnitude > 1)
            movedSinceMouseDown = true;


        Vector3 pos = transform.position;
        // Zoom
        if (Input.mouseScrollDelta.y != 0) {
            pos.z += Input.mouseScrollDelta.y;
        }

        // Pan
        if (Input.GetMouseButton(0)) {
            Vector3 dMouse = previousMousePosition - Input.mousePosition;
            dMouse.x /= Screen.width;
            dMouse.y /= Screen.height;
            pos -= dMouse * Camera.main.transform.position.z / 2;
            
        } 
        // Place marker
        else if (Input.GetMouseButtonUp(0) && !movedSinceMouseDown) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            targetPos.z = -0.01f;
            target.transform.position = targetPos;

            effect.reset();
        }

        transform.position = pos;


        previousMousePosition = Input.mousePosition;
	}
}
