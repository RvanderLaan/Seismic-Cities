using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour {

    public float speed = 0.5f;
    public Vector3 axis = Vector3.forward;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.AngleAxis(360 * (Time.time * speed), axis);
	}
}
