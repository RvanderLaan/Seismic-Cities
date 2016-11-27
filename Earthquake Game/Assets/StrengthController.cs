using UnityEngine;
using System.Collections;

public class StrengthController : MonoBehaviour {

    private RectTransform parent, self;

    public float period = 2;

    private float startTime;

	// Use this for initialization
	void Start () {
        parent = transform.parent.GetComponent<RectTransform>();
        self = GetComponent<RectTransform>();
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        float dT = ((Time.time - startTime) % period) / period;
        float pos = (Mathf.Sin(2 * Mathf.PI * dT) / 2) * parent.rect.size.y;

        Vector2 oldPos = self.anchoredPosition;
        oldPos.y = pos;
        self.anchoredPosition = oldPos;
	}

    // Returns score for this specific time between 0 and 1 (where 1 is good)
    public float getTimingScore() {
        float dT = ((Time.time - startTime) % period) / period;
        return 1 - Mathf.Abs(Mathf.Sin(2 * Mathf.PI * dT));
    }
}
