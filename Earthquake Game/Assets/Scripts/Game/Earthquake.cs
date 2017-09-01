using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Earthquake : MonoBehaviour {

    public EarthquakeData earthquakeData;

    public AudioClip earthquakeSound;

    public TargetController targetController;

    private AudioSource audioSource;

    private GameObject[] targets;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    public void simulateEarthquake(GameObject[] targets)
    {
        this.targets = targets;
        EventManager.TriggerEvent("SimulateEarthquake");
        StartCoroutine("SpawnWaves");

        //audioSource.pitch = Random.Range(0.5f, 1f);
        audioSource.volume = 1;
        audioSource.clip = earthquakeSound;
        audioSource.Play();
    }

    IEnumerator SpawnWaves()
    {
        CameraShakeInstance c = CameraShaker.Instance.StartShake(earthquakeData.shakeMagnitude, earthquakeData.shakeRoughness, earthquakeData.shakeFadeIn);

        for (int i = 0; i < earthquakeData.numberOfWaves; i++)
        {
            //spawn a wave
            targetController.createWave(Random.Range(0.5f, 1f), targets);
            yield return new WaitForSeconds(earthquakeData.intervalBetweenWaves);
        }
        yield return new WaitForSeconds(5);

        c.StartFadeOut(earthquakeData.shakeFadeOut);
    }
}
