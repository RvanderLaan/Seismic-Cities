using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

    public Vector3 direction;
    public float speed;

   

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // find out soil type
        // get soil type speed coefficient
        
        transform.position = transform.position + direction * speed * Time.deltaTime;
	}


}
