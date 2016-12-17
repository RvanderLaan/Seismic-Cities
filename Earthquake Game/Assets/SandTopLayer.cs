using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandTopLayer : MonoBehaviour {

    public float fallDistance = 0.5f;
    public float fallTime = 8f;

    private bool falling = false, finished = false;
    private float startFallTime = 0;



    private Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (falling) {
            float dT = (Time.time - startFallTime) / fallTime;
            Vector3 newPos;
            if (dT > 1) {
                newPos = startPos + Vector3.down * fallDistance;
                falling = false;
                finished = true;
            } else {
                newPos = startPos + Vector3.down * dT * fallDistance;
            }
            transform.position = newPos;
        }
	}

    public void StartFalling() {
        falling = true;
        startFallTime = Time.time;
    }

    void OnTriggerEnter2D(Collider2D c2d) {
        Debug.Log(c2d.gameObject.name);
        if (!finished && c2d.gameObject.tag == "Wave")
            falling = true;
    }
}
