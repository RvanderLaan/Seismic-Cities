using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class SoilInfoWindow : MonoBehaviour {

    public TextInserterPro text;
    public VideoPlayerController videoPlayer;

	// Use this for initialization
	void Awake () {
        text = GetComponentInChildren<TextInserterPro>();
        videoPlayer = GetComponent<VideoPlayerController>();
	}
	
	public void showInfo(string type)
    {
        gameObject.SetActive(true);
        text.reset(type.ToString());
    }
}
