using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

    public Vector3 direction;
    public float speed;
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + direction * speed * Time.deltaTime;

        // float width = (maxspeed - speed) / maxspeed;
        // speed -= Time.time * constant;
	}


}
