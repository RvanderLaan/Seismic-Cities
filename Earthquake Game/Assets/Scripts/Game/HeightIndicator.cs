using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeightIndicator : MonoBehaviour {

    private Text[] texts;
    public Text textInstance;

    [Range(0.0f, 1.0f)]
    public float smoothFactor = 0.9f;

    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        // Initialize text components with the right value and position
        rectTransform = GetComponent<RectTransform>();
        string[] values = { "100 km", "10 km", "1 km", "100 m", "10 m", "0 m",
                            "10 m", "100 m", "1 km", "10 km", "100 km"};
        texts = new Text[values.Length];
        for (int i = 0; i < texts.Length; i++) {
            texts[i] = GameObject.Instantiate(textInstance);
            texts[i].transform.SetParent(gameObject.transform);
            texts[i].text = values[i];
            texts[i].rectTransform.localPosition = new Vector2(rectTransform.sizeDelta.x / 2 + 4, 0);
            texts[i].rectTransform.position = getRelativePosition(i);
        }
            
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        // Note: Orthographic size is half of the vertical camera size
        // This loop positions every text element where its 'absolute' position would be
        for (int i = 0; i < texts.Length; i++) {
            Vector2 relativePos = getRelativePosition(i);

            // Smoothly change position: Interpolete between old position and new position
            // Todo: Even better, slightly overshoot new position
            relativePos.y = (texts[i].rectTransform.position.y * smoothFactor + relativePos.y * (1 - smoothFactor));
            texts[i].rectTransform.position = relativePos;
        }
    }

    /// <summary>
    /// Calculates the screen position of the text component index to the logarithmic scale position in the game
    /// </summary>
    /// <param name="i">The index of the text components</param>
    /// <returns>The relative position on the screen of the text component</returns>
    private Vector2 getRelativePosition(int i) {
        Vector2 pos = texts[i].rectTransform.position;
        float absoluteHeight = (i - Mathf.Ceil(texts.Length / 2)) + Camera.main.transform.position.y - Camera.main.orthographicSize;
        pos.y = absoluteHeight / (Camera.main.orthographicSize) * rectTransform.rect.y;
        return pos;
    }
}
