using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelName : MonoBehaviour {

    public float waitTime = 3, transTime = 2;
    private float startTime;

    private Text text;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        text = GetComponent<Text>();

        text.text = GameObject.Find("_GM").GetComponent<LevelManager>().getLevelData().name;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > startTime + waitTime) {
            float alpha = 1 - (Time.time - (startTime + waitTime)) / transTime;
            Color c = text.color;
            c.a = alpha;
            text.color = c;

            if (Time.time > startTime + waitTime + transTime)
                gameObject.SetActive(false);
        }
	}
}
