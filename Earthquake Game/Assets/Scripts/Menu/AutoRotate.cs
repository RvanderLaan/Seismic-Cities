using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoRotate : MonoBehaviour {

    public float speed = 1f;
    public float time = 3f;

    private bool starting = false;

    private Quaternion start, target;

    private float startTime;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (starting) {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, 0.1f);
        } else 
            transform.Rotate(new Vector3(0, Time.deltaTime * speed, 0));
	}

    public void RotateAndStart() {
        starting = true;
        startTime = Time.time;
        start = transform.rotation;
        target = Quaternion.identity;
        
        Invoke("StartGame", time + 1);
        GetComponent<Fader>().Invoke("BeginFade", time);
    }

    private void StartGame() {
        SceneManager.LoadScene("Level");
    }
}
