using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ScaleOnHover : MonoBehaviour {

    private Vector3 startScale;
    private bool isHover = false;
    public float hoverScale = 1.1f;
    public float speed = 0.5f;


	// Use this for initialization
	void Start () {
        startScale = transform.localScale;

        EventTrigger et = GetComponent<EventTrigger>();

        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => { isHover = true; });
        et.triggers.Add(enterEntry);

        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { isHover = false; });
        et.triggers.Add(exitEntry);
    }

    void Update()
    {
        if (isHover)
            transform.localScale = Vector3.Slerp(transform.localScale, startScale * hoverScale, speed);
        else
            transform.localScale = Vector3.Slerp(transform.localScale, startScale, speed);
    }
}
