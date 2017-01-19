using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUI : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void show() {
        anim.SetBool("active", true);
    }

    public void hide() {
        anim.SetBool("active", false);
    }
}
