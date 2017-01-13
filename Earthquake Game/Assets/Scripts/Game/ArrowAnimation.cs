using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour {

    private Renderer rend;
    public float speed = 1;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        rend.material.SetTextureOffset("_MainTex", new Vector2(Time.time * speed, 0));
	}
}
