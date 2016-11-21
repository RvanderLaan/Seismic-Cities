using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

    public int waveAmount = 128;
    public float startSpeed = .1f;
    public Wave instance;
    // private Wave[] waves;

    // Use this for initialization
    void Start() {
    }

    void SpawnWave() {
        // waves = new Wave[waveAmount];
        for (int i = 0; i < waveAmount; i++) {
            Wave w = (Wave) GameObject.Instantiate(instance, transform.position, Quaternion.identity);
            w.direction = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / waveAmount), Mathf.Sin(Mathf.PI * 2 * i / waveAmount), 0).normalized;
            w.speed = startSpeed;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space)) {
            SpawnWave();
        }
	}
}
