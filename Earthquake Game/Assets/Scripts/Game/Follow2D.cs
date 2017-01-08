using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Follows a 2d object (may not work correctly, used by building tooltips to follow the list item locations)
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class Follow2D : MonoBehaviour {

    public RectTransform target;
    public bool lockX = false, lockY = false;

    private Vector2 startPos;
    private RectTransform rectTransform;


	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        startPos = new Vector2(-target.rect.width / 2, 0);
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 globalPos = target.transform.position;
        if (!lockX && !lockY)
            transform.position = startPos + globalPos;
        else if (!lockX)
            transform.position = startPos + new Vector2(globalPos.x, 0);
        else if (!lockY)
            transform.position = startPos + new Vector2(0, globalPos.y);
    }
}
