using UnityEngine;
using System.Collections;

public class WaveGeneratorGenerator : MonoBehaviour {

    public WaveGenerator instance;
    public StrengthController sc;
    public float minimumIntervalBetweenWaves = 0.75f;

    private float lastSpawnTime;

	// Use this for initialization
	void Start () {
        lastSpawnTime = -minimumIntervalBetweenWaves;
	}
	
	// Update is called once per frame
	void Update () {
        float now = Time.time;
        if (Input.GetKeyDown(KeyCode.Space) && (now -lastSpawnTime) > minimumIntervalBetweenWaves) {
            WaveGenerator wg = (WaveGenerator) GameObject.Instantiate(instance, transform.position, Quaternion.identity);
            wg.startSpeed = sc.getTimingScore() * wg.startSpeed * .8f + wg.startSpeed * .2f;
            // wg.transform.SetParent(transform);
            lastSpawnTime = now;
         }
    }
}
