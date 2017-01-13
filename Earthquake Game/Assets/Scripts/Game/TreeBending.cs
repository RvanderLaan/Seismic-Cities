using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBending : MonoBehaviour {

    private Animation anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
        anim["Bending"].speed = Random.Range(0.8f, 1.2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
