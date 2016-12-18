using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothScaling : MonoBehaviour {

    public float minScale = 0.5f, maxScale = 1.5f, scaleCycleTime = 2f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Scale over time
        Vector3 scal = transform.localScale;
        scal.x = scal.y = Mathf.Lerp(minScale, maxScale, Mathf.Sin(Time.time * Mathf.PI * 2 / scaleCycleTime) / 2 + 0.5f);
        transform.localScale = scal;
    }
}
