using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class EarthquakeSimulator : MonoBehaviour {

    public float intervalBetweenWaves;
    public int numberOfWaves;
    public float intensityOfWaves;

    public float shakeMagnitude = 0.8f, shakeRoughness = 5, shakeFadeIn = 3, shakeFadeOut = 15;

    public TargetController targetController;

    // Use this for initialization
    void Start () {
		
	}

    public void simulateEarthquake()
    {
        StartCoroutine("SpawnWaves");
    }

    IEnumerator SpawnWaves()
    {
        CameraShakeInstance c = CameraShaker.Instance.StartShake(shakeMagnitude, shakeRoughness, shakeFadeIn);

        for (int i = 0; i < numberOfWaves; i++)
        {
            //spawn a wave
            targetController.createWave(Random.Range(0.5f, 1f));
            yield return new WaitForSeconds(intervalBetweenWaves);
        }
        yield return new WaitForSeconds(5);

        c.StartFadeOut(shakeFadeOut);
    }
}
