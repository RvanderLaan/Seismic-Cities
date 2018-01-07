using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelName : MonoBehaviour {

    public float waitTime = 3, transTime = 2;
    private float startTime;

    private Text text;
    private Image background;
    private Color initialBackgroundColor;

    private string levelName;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        background = transform.parent.GetComponent<Image>();
        initialBackgroundColor = background.color;
        Init();
    }

    void Init()
    {
        transform.parent.gameObject.SetActive(true);
        GetComponent<TextInserter>().reset(levelName);
        startTime = Time.time;

        Color c = text.color;
        c.a = 1;
        text.color = c;

        background.color = initialBackgroundColor;
    }

    public void StartFade(string levelName)
    {
        this.levelName = levelName;
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.time > startTime + waitTime) {
            float alpha = 1 - (Time.time - (startTime + waitTime)) / transTime;
            Color c = text.color;
            c.a = alpha;
            text.color = c;

            Color bc = initialBackgroundColor;
            bc.a = alpha * initialBackgroundColor.a;
            background.color = bc;

            if (Time.time > startTime + waitTime + transTime)
                transform.parent.gameObject.SetActive(false);
        }
	}
}
