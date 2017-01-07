using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeightIndicator : MonoBehaviour {

    private Text[] texts;
    public Text textInstance;

    public string[] entries = { "0 m", "10 m", "100 m", "1 km", "10 km", "100 km" };
    public int startIndex = 0;

    public int heightScale = 20;

    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        // Initialize text components with the right value and position
        rectTransform = GetComponent<RectTransform>();
        texts = new Text[entries.Length];
        for (int i = 0; i < texts.Length; i++) {
            texts[i] = GameObject.Instantiate(textInstance);
            texts[i].transform.SetParent(gameObject.transform);
            texts[i].text = entries[i];
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
            texts[i].rectTransform.position = getRelativePosition(i);
        }
    }

    /// <summary>
    /// Calculates the screen position of the text component index to the logarithmic scale position in the game
    /// </summary>
    /// <param name="i">The index of the text components</param>
    /// <returns>The relative position on the screen of the text component</returns>
    private Vector2 getRelativePosition(int i) {
        Vector2 pos = texts[i].rectTransform.position;
        float absoluteHeight = (i - startIndex) * heightScale + Camera.main.transform.position.y - Camera.main.orthographicSize;
        pos.y = absoluteHeight / (Camera.main.orthographicSize) * rectTransform.rect.y;
        return pos;
    }
}
