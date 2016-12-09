using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeRotation : MonoBehaviour {

    private Rigidbody rigidBody;
    private Vector3 prevMousePosition;

    public bool horizontal = true, vertical = false;

    public float clickDrag = 5, normalDrag = 2;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        prevMousePosition = Input.mousePosition;
        rigidBody.centerOfMass = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0)) {
            rigidBody.angularDrag = clickDrag;

            Vector3 dMouse = (prevMousePosition - Input.mousePosition);

            Vector3 rotVector = Vector3.zero;
            if (horizontal)
                rotVector.y = dMouse.x;
            if (vertical)
                rotVector.x = -dMouse.y;

            rigidBody.angularVelocity += rotVector * Time.deltaTime;
        } else
            rigidBody.angularDrag = normalDrag;

        prevMousePosition = Input.mousePosition;
	}
}
