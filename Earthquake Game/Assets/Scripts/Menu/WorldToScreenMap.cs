using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToScreenMap : MonoBehaviour {

    public GameObject follow;

    private Camera camera;
    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        rectTransform = GetComponent<RectTransform>();

        Update();
    }
	
	// Update is called once per frame
	void Update () {
        /*
        Vector2 viewportPoint = camera.WorldToScreenPoint(follow.transform.position);
        Vector2 screenPosition = new Vector2(viewportPoint.x * rectTransform.sizeDelta.x - rectTransform.sizeDelta.x * 0.5f,
                                             viewportPoint.y * rectTransform.sizeDelta.y - rectTransform.sizeDelta.y * 0.5f);
   
        rectTransform.anchoredPosition = screenPosition;
        */
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(camera, follow.transform.position);
        rectTransform.position = pos;
    }
}
