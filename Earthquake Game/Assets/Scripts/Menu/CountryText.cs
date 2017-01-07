using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryText : MonoBehaviour {

    private Text text;

    public Color normal = Color.white, hover = Color.green;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.color = normal;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseEnter() {
        text.color = hover;

    }

    public void OnMouseExit() {
        text.color = normal;
    }
}
