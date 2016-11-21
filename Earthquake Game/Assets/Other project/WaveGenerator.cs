using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

    public int waveAmount = 128;
    public float startSpeed = .1f;
    public Wave instance;
    private Wave[] waves;

    LineRenderer lr;
    public float lineWidth = 0.05f;

    // Use this for initialization
    void Start() {
        lr = GetComponent<LineRenderer>();
        lr.SetVertexCount(waveAmount + 1);

        waves = new Wave[waveAmount];
        for (int i = 0; i < waveAmount; i++) {
            Wave w = (Wave) GameObject.Instantiate(instance, transform.position, Quaternion.identity);
            w.transform.SetParent(transform);
            w.direction = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / waveAmount), Mathf.Sin(Mathf.PI * 2 * i / waveAmount), 0).normalized;
            w.speed = startSpeed;
            waves[i] = w;
        }

        

	}
	
	// Update is called once per frame
	void Update () {

        lr.SetWidth(lineWidth, lineWidth);
        for (int i = 0; i < waveAmount; i++) {
            lr.SetPosition(i, waves[i].transform.position);
        }
        lr.SetPosition(waveAmount, waves[0].transform.position);
	}
}
