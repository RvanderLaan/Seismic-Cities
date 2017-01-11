using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseExplosion : MonoBehaviour {

    public float intensity = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)) {
            Rigidbody2D[] children = GetComponentsInChildren<Rigidbody2D>();
            for (int i = 0; i < children.Length; i++) {
                children[i].bodyType = RigidbodyType2D.Dynamic;
                children[i].AddForce(new Vector2((Random.Range(1, 3) * 2 - 3), 1) * intensity);
            }
        }
	}


}
