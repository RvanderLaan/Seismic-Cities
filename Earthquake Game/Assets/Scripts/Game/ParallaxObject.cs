using UnityEngine;
using System.Collections;

public class ParallaxObject : MonoBehaviour {

	Vector3 startPos;

    public Vector3 moveDirection = Vector3.zero;
    public Vector2 levelBounds = new Vector2(-150, 150);

    public bool freezeX = false, freezeY = false;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        // Update if moving is defined
        Vector3 newPos = startPos + moveDirection * Time.deltaTime * 0.1f;
        // Wrap around
        if (newPos.x < levelBounds.x)
            newPos.x = levelBounds.y;
        else if (newPos.x > levelBounds.y)
            newPos.x = levelBounds.x;

        // Only update position if moving
        if (moveDirection.magnitude > 0.001f)
            startPos = newPos;

		// Move background images relative to camera
		float relativePosX = (Camera.main.transform.position.x / transform.position.z);
        float relativePosY = (Camera.main.transform.position.y / transform.position.z);
        Vector3 relNewPos = startPos + new Vector3(Camera.main.transform.position.x - relativePosX, startPos.y - relativePosY, startPos.z);

        // Check if axis is frozen
        transform.position = new Vector3(freezeX ? transform.position.x : relNewPos.x, freezeY ? transform.position.y : relNewPos.y, relNewPos.z);

    }
}
