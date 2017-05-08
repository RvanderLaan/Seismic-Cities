using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seismograph : MonoBehaviour {

    public float transitionTime = 8;
    private bool moving = false;

    public RectTransform mover;
    private float startTime;

    private RectTransform wrapper;

	// Use this for initialization
	void Start () {
        wrapper = transform.GetChild(0).GetComponent<RectTransform>();

        EventManager.StartListening("SimulateEarthquake", StartMoving);
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
        {

            float d = 1 - (Time.time - startTime) / transitionTime;
            Debug.Log(d);
            if (d >= 0)
            {
                Vector2 oldSize = mover.sizeDelta;
                oldSize.x = wrapper.rect.width * d;
                mover.sizeDelta = oldSize;
            } else
            {
                moving = false;
                wrapper.gameObject.SetActive(false);
            }
        }
	}

    public void StartMoving()
    {
        wrapper.gameObject.SetActive(true);
        moving = true;
        startTime = Time.time;
    }
}
