using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

    public Vector3 direction;
    public float speed;

    private float timeSinceLastCollision = float.MaxValue;
    private bool currentlyInCollision = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + direction * speed * Time.deltaTime;
	}
}
